using System.Collections.Generic;
using System.Linq;
using Framework.Durak.Collections.Extensions;
using Framework.Durak.Datas;
using Framework.Durak.Datas.Extensions;
using Framework.Shared.Cards.Entities;
using Framework.Shared.Collections;
using Framework.Shared.Collections.Extensions;

using UnityEngine;

namespace Framework.Durak.Players.Selectors
{
    public sealed class AiRandomAttacker : AiCardSelector
    {
        private readonly IBoard<Data> board;
        private readonly IMap<ICard, Data> map;
        private readonly IDeck<Data> deck;
        private readonly IHand defer;
        private readonly IHand supper;
        private bool first_t;
        private List<Data> attacking = new List<Data>();
        private List<Data> defending = new List<Data>();
        private List<IHand> hands = new List<IHand>();

        public AiRandomAttacker(IBoard<Data> board, IMap<ICard, Data> map, IDeck<Data> deck, IPlayerQueue<IPlayer> players)
        {
            this.board = board;
            this.map = map;
            this.deck = deck;
            this.defer = players.Defender.Hand;
            this.supper = players.GetNextFrom(players.Defender, andSkip: (players.GetNextFrom(players.Defender) == Current) ? 1 : 0).Hand;
        }
        private void TreeConstruct(int i, int who)
        {
            Current.tree.Add((new Dictionary<Data, int>(), 0));
            foreach (var e in hands[who])
            {
                if (attacking.Contains(e) || defending.Contains(e)) continue;
                if (Validate(who,e))
                {
                    Current.tree.collection[i].Item1[e] = Current.tree.collection.Count;
                    if (who == 0) attacking.Add(e);
                    else defending.Add(e);
                    TreeConstruct(Current.tree.collection.Count, (who + 1) % 2);
                    if (who == 0) attacking.Remove(e);
                    else defending.Remove(e);
                }
            }
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
            if (who==0)
            {
                if (attacking.Count==0)
                {
                    return true;
                }

                if (attacking.Count<6&&ContainsRank(data))
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
        public override ICard GetCard(IReadOnlyList<Data> hand)
        {
            if (board.IsEmpty) first_t = true;
            else first_t = false;
            if (Current.tree.IsEmpty())
            {
                hands = new List<IHand>(){ (IHand)(Current.Hand).Clone(), (IHand)defer.Clone(), (IHand)supper.Clone() };
                TreeConstruct(0, 0);
            }
            if (board.IsEmpty)
            {
                int random = Random.Range(0, hand.Count);

                Data data = hand[random];

                ICard card = map.Get(data);

                return card;
            }

            if (board.IsFull is false)
            {
                foreach (var data in hand)
                {
                    if (board.ContainsRank(data))
                    {
                        ICard card = map.Get(data);

                        return card;
                    }
                }
            }

            return default;
        }
    }
}