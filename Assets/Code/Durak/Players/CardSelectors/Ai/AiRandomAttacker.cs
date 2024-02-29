using System.Collections.Generic;
using System.Linq;
using Framework.Durak.Collections.Extensions;
using Framework.Durak.Datas;
using Framework.Durak.Datas.Extensions;
using Framework.Shared.Cards.Entities;
using Framework.Shared.Collections;
using Framework.Shared.Collections.Extensions;

using UnityEngine;
using UnityEngine.Experimental.Playables;

namespace Framework.Durak.Players.Selectors
{
    public sealed class AiRandomAttacker : AiCardSelector
    {
        private readonly IBoard<Data> board;
        private readonly IMap<ICard, Data> map;
        private readonly IDeck<Data> deck;
        private readonly IHand defer;
        private readonly IHand supper;
        IPlayerQueue<IPlayer> players;
        private bool first_t;
        private List<Data> attacking = new List<Data>();
        private List<Data> defending = new List<Data>();
        private List<IHand> hands = new List<IHand>();
        private int passed = 0;
        private int cur = 0;
        private int totalpassed = 0;

        public AiRandomAttacker(IBoard<Data> board, IMap<ICard, Data> map, IDeck<Data> deck, IPlayerQueue<IPlayer> players)
        {
            this.board = board;
            this.map = map;
            this.deck = deck;
            this.players = players;
            this.defer = players.Defender.Hand;
            this.supper = players.GetNextFrom(players.Defender, andSkip: (players.GetNextFrom(players.Defender) == Current) ? 1 : 0).Hand;
        }
        //private void TreeConstruct(int i, int who)
        //{
        //    if (Current.tree.collection.Count>5000)
        //    {
        //        Debug.Log("FUCK");
        //    }
        //    Current.tree.Add((new Dictionary<Data, int>(), 0));
        //    var tmp = passed;
        //    foreach (var e in hands[who])
        //    {
        //        if (attacking.Contains(e) || defending.Contains(e)) continue;
        //        if (Validate(who,e))
        //        {
                    
        //            passed = 0;
        //            Current.tree.collection[i].Item1[e] = Current.tree.collection.Count;
        //            if (who < 2) attacking.Add(e);
        //            else defending.Add(e);
        //            TreeConstruct(Current.tree.collection.Count, (who<2)?2:cur);
        //            if (who < 2) attacking.Remove(e);
        //            else defending.Remove(e);
        //        }
        //    }
        //    passed = tmp;
        //    if (attacking.Count == 0) return;
        //    if (who == 2) return;
        //    passed++;
        //    if (passed == 2)
        //    {
        //        passed--;
        //        return;
        //    }
        //    Current.tree.collection[i].Item1[new Data()] = Current.tree.collection.Count;
        //    cur = (cur + 1) % 2;
        //    TreeConstruct(Current.tree.collection.Count, cur);
        //    passed--;
        //    cur = (cur + 1) % 2;
        //}
        //private bool ContainsRank(Data data)
        //{
        //    foreach (var item in attacking)
        //    {
        //        if (item.EqualRank(data))
        //        {
        //            return true;
        //        }
        //    }
        //    foreach (var item in defending)
        //    {
        //        if (item.EqualRank(data))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        //private bool Validate(int who, Data data)
        //{
        //    if (who<2)
        //    {
        //        if (attacking.Count==0)
        //        {
        //            return true;
        //        }

        //        if (attacking.Count<6&&ContainsRank(data))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    else
        //    {
        //        Data trump = deck.Bottom;

        //        Data last = attacking.Last();
        //        if (data.CanBeat(last, trump)) return true;
        //        else return false;
        //    }
        //}
        public override ICard GetCard(IReadOnlyList<Data> hand)
        {
            //foreach (var e in board.Attacks)
            //    attacking.Add(e);
            //foreach (var e in board.Defends)
            //    defending.Add(e);
            //if (board.Count > 0) passed++;
            IPlayer doner = new Player();
            if (players.GetNextFrom(players.Defender) == Current)
            {
                for (int i = 0; i < 3; i++)
                    if (players.GetNextFrom(players.Defender, andSkip: i).Type == PlayerType.Ai&& players.GetNextFrom(players.Defender, andSkip: i)!=players.Current)
                        doner = players.GetNextFrom(players.Defender, andSkip: i);
            }
            else if (Current.tree.IsEmpty())
            {
                //hands = new List<IHand>(){ (IHand)(Current.Hand).Clone(),  (IHand)supper.Clone(), (IHand)defer.Clone() };
                //TreeConstruct(0, 0);
                Current.tree = new Tree(board, map, deck, players);
                doner = Current;
            }
            else doner = Current;
            var turn = doner.tree.BestTurn();
            if (turn == new Data(-1, -1)) return default;
            else return map.Get(turn);
            //int a = 2;
            //var b = a;
            //a = 3;
            //if (board.IsEmpty)
            //{
            //    int random = Random.Range(0, hand.Count);

            //    Data data = hand[random];

            //    ICard card = map.Get(data);

            //    return card;
            //}

            //if (board.IsFull is false)
            //{
            //    foreach (var data in hand)
            //    {
            //        if (board.ContainsRank(data))
            //        {
            //            ICard card = map.Get(data);

            //            return card;
            //        }
            //    }
            //}

            return default;
        }
    }
}