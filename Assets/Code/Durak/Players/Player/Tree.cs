
using System;
using System.Collections;
using System.Collections.Generic;       
using Framework.Durak.Datas;
using System.Linq;
using Framework.Durak.Collections.Extensions;
using Framework.Durak.Datas.Extensions;
using Framework.Shared.Cards.Entities;
using Framework.Shared.Collections;
using Framework.Shared.Collections.Extensions;
using System.Diagnostics;

namespace Framework.Durak.Players
{
    [Serializable]
    public class Tree
    {
        private readonly IBoard<Data> board;
        private readonly IMap<ICard, Data> map;
        private readonly IDeck<Data> deck;
        private readonly IHand attacker;
        private readonly IHand defer;
        private readonly IHand supper;
        IPlayerQueue<IPlayer> players;
        List<Data>[] turns = new List<Data>[3] { new List<Data>(), new List<Data>(), new List<Data>() };
        private List<Data> attacking = new List<Data>();
        private List<Data> defending = new List<Data>();
        private List<IHand> hands = new List<IHand>();
        private IHand realHand;
        public int node;
        private int ix;
        private int passed = 0;
        private int cur = 0;
        private bool toss = false;
        public List<Dictionary<Data, int>> collection = new List<Dictionary<Data, int>>();
        public List<double> ans = new List<double>();
        public Tree()
        {

        }
        
        public Tree(IBoard<Data> board, IMap<ICard, Data> map, IDeck<Data> deck, IPlayerQueue<IPlayer> players)
        {
            node = 0;
            this.board = board;
            this.map = map;
            this.deck = deck;
            this.players = players;
            this.attacker = players.Attacker.Hand;
            this.defer = players.Defender.Hand;
            for (int i = 0; i < 3; i++)
                if (players.GetNextFrom(players.Defender, andSkip: i).Type == PlayerType.Real)
                    realHand = players.GetNextFrom(players.Defender, andSkip: i).Hand;
            this.supper = players.GetNextFrom(players.Defender, andSkip: (players.GetNextFrom(players.Defender) == players.Attacker) ? 1 : 0).Hand;
            foreach (var e in board.Attacks)
            {
                attacking.Add(e);
                turns[0].Add(e);
            }
            foreach (var e in board.Defends)
            {
                defending.Add(e);
                turns[2].Add(e);
            }
            //if (board.Count > 0) passed++;
            hands = new List<IHand>() { (IHand)(attacker), (IHand)supper, (IHand)defer };
            for (int i = 0; i < 3; i++)
                if (hands[i] == realHand) ix = i;
            TreeConstruct(0, (players.Current == players.Defender) ? 2 : 0);
        }
        private double Desire(Data card, int who)
        {
            int freq = 1;
            if (card == new Data(-1, -1))
            {
                return 1.75 * Math.Pow(0.978, 18 - deck.Count()) * Math.Pow(0.922, board.Count() - 1);
            }
            if (who==2)
            {
                foreach (var e in board.Seen)
                    if (e.rank == card.rank && e.suit != card.suit) freq++;
            }
            return 4 * Math.Pow(0.921, card.rank + ((card.suit == deck.Bottom.suit) ? 9 : 0)) /freq;
        }
        private Dictionary<Data,double> Pos(List<Data> cards, int who)
        {
            Dictionary<Data, double> pos = new Dictionary<Data, double>();
            foreach (var e in cards)
                pos[e]=Desire(e, who);
            pos[new Data(-1,-1)]=Desire(new Data(-1, -1),who);
            double sum = pos.Values.Sum();
            foreach (var e in cards)
                pos[e] = pos[e] / sum;
            pos[new Data(-1, -1)] = pos[new Data(-1, -1)] / sum;
            return pos;
        }
        public Data BestTurn()
        {
            Data card = new Data(-1, -1);
            double res = -10;
            foreach (var e in collection[node])
            {
                if (ans[e.Value]>res)
                {
                    card = e.Key;
                    res = ans[e.Value];
                }
            }
            return card;
        }
        private double TreeConstruct(int i, int who)
        {
            this.Add(new Dictionary<Data, int>());
            ans.Add(-300.0);
            bool flag = true;
            var tmp = 0;
            if (tmp != passed) tmp++;
            List<Data> temp = new List<Data>();
            foreach (var e in hands[who])
            {
                if (attacking.Contains(e) || defending.Contains(e)) continue;
                else temp.Add(e);
            }
            var possibilities = Pos(temp,who);
            foreach (var e in hands[who])
            {
                if (attacking.Contains(e) || defending.Contains(e)) continue;
                if (Validate(who, e))
                {
                    turns[who].Add(e);
                    passed = 0;
                    this.collection[i][e] = this.collection.Count;
                    if (who < 2) attacking.Add(e);
                    else defending.Add(e);
                    var tump = TreeConstruct(this.collection.Count, ((!toss)&&(who < 2)) ? 2 : cur);
                    if (who < 2) attacking.Remove(e);
                    else defending.Remove(e);
                    turns[who].Remove(e);
                    if (who == ix)
                    {
                        if (flag)
                        {
                            ans[i] = 0.0;
                            flag = false;
                        }
                        ans[i] += tump * possibilities[e];
                    }
                    else ans[i] = Math.Max(ans[i], tump);
                }
            }
            passed = tmp;
            if (attacking.Count == 0)
            {
                if (ans[i] == -300.0)
                {
                    Console.WriteLine("FUCK");
                }
                return ans[i];
            }
            if (who == 2)
            {
                this.collection[i][new Data(-1,-1)] = this.collection.Count;
                toss = true;
                var tump = TreeConstruct(this.collection.Count, cur);
                if (who == ix)
                {
                    if (flag)
                    {
                        ans[i] = 0.0;
                        flag = false;
                    }
                    ans[i] += tump * possibilities[new Data(-1,-1)];
                }
                else ans[i] = Math.Max(ans[i], tump);
                toss = false;
                return ans[i];
            }
            passed++;
            if (passed == 2)
            {
                passed--;
                int idx = 0;
                double s = 1;
                double[] scores = new double[3];
                for (int j = 0; j < 3; j++)
                {
                    scores[j] = Score(j, ref idx);
                    s *= scores[j];
                }
                s /= scores[ix]*scores[ix];
                ans[i] = s;
                return ans[i];
            }
            this.collection[i][new Data(-1,-1)] = this.collection.Count;
            
            cur = (cur + 1) % 2;
            double tamp = TreeConstruct(this.collection.Count, cur);
            if (who == ix)
            {
                if(flag)
                        {
                    ans[i] = 0.0;
                    flag = false;
                }
                ans[i] += tamp * possibilities[new Data(-1, -1)];
            }
            else ans[i] = Math.Max(ans[i], tamp);
            passed--;
            cur = (cur + 1) % 2;
            if (ans[i] == -300.0)
            {
                Console.WriteLine("FUCK");
            }
            return ans[i];
        }
        private double Score(int i, ref int idx)
        {
     
            double sum = 0;
            if (i==2&&toss)
            {
                foreach (var e in hands[i])
                    sum += 4 * Math.Pow(0.921, 17-(e.rank + ((e.suit == deck.Bottom.suit) ? 11 : 0)));
                foreach (var e in attacking)
                    sum += 4 * Math.Pow(0.921, 17 - (e.rank + ((e.suit == deck.Bottom.suit) ? 11 : 0)));
                return sum / (hands[i].Count() + attacking.Count());
            }
            var totake = Math.Min(deck.Count() - idx, 6 - Math.Min(6, hands[i].Count() + ((i == ix) ? board.Count() : 0) - turns[i].Count()));
            var slice = deck.ToList().GetRange(deck.Count()-idx-totake, Math.Min(deck.Count()-idx,6-Math.Min(6, hands[i].Count() + ((i==ix)?board.Count():0) - turns[i].Count())));
            idx += totake;

            foreach (var e in hands[i])
                sum += 4 * Math.Pow(0.921, 17 - (e.rank + ((e.suit == deck.Bottom.suit) ? 11 : 0)));
            foreach (var e in slice)
                sum += 4 * Math.Pow(0.921, 17 - (e.rank + ((e.suit == deck.Bottom.suit) ? 11 : 0)));
            foreach (var e in turns[i])
                sum -= 4 * Math.Pow(0.921, 17 - (e.rank + ((e.suit == deck.Bottom.suit) ? 11 : 0)));
            if (hands[i].Count() - turns[i].Count() + totake < 2) return 1000;
            sum /= Math.Pow((hands[i].Count() - turns[i].Count()+ totake),1-0.028*deck.Count());
            return sum;
        }
        private bool ContainsRank(Data data)
        {
            foreach (var item in attacking)
            {
                if (item.EqualRank(data))
                {
                    return true;
                }
            }
            foreach (var item in defending)
            {
                if (item.EqualRank(data))
                {
                    return true;
                }
            }
            return false;
        }
        private bool Validate(int who, Data data)
        {
            if (who < 2)
            {
                if (attacking.Count == 0)
                {
                    return true;
                }

                if (attacking.Count < 6 && ContainsRank(data))
                {
                    return true;
                }
                return false;
            }
            else
            {
                Data trump = deck.Bottom;

                Data last = attacking.Last();
                if (data.CanBeat(last, trump)) return true;
                else return false;
            }
        }
        

        public void AddRange(List<Dictionary<Data, int>> datas)
        {
            collection.AddRange(datas);
        }

        public void Add(Dictionary<Data, int> datas)
        {
            collection.Add(datas);
        }

        public void Clear()
        {
            collection.Clear();
        }
        public bool IsEmpty()
        {
            return collection.Count == 0;
        }
    }
}