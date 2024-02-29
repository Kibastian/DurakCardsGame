using System.Collections.Generic;

using Framework.Durak.Datas;
using Framework.Durak.Datas.Extensions;
using Framework.Shared.Cards.Entities;
using Framework.Shared.Collections;
using Framework.Shared.Collections.Extensions;

using UnityEngine.Assertions;

namespace Framework.Durak.Players.Selectors
{
    public sealed class AiRandomDefender : AiCardSelector
    {
        private readonly IDeck<Data> deck;
        private readonly IBoard<Data> board;
        private readonly IMap<ICard, Data> map;
        IPlayerQueue<IPlayer> players;

        public AiRandomDefender(IDeck<Data> deck, IBoard<Data> board, IMap<ICard, Data> map, IPlayerQueue<IPlayer> players)
        {
            this.deck = deck;
            this.board = board;
            this.map = map;
            this.players = players;
        }

        public override ICard GetCard(IReadOnlyList<Data> hand)
        {
            IPlayer doner = new Player();
            if (players.GetNextFrom(players.Defender) == Current)
            {
                for (int i = 0; i < 3; i++)
                    if (players.GetNextFrom(players.Defender, andSkip: i).Type == PlayerType.Ai && players.GetNextFrom(players.Defender, andSkip: i) != players.Current)
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

            //Data trump = deck.Bottom;

            //Data last = board.Last();

            //Assert.IsFalse(board.IsEmpty, "In defending state the board can't be empty");

            //foreach (var data in hand)
            //{
            //    if (data.CanBeat(last, trump))
            //    {
            //        ICard card = map.Get(data);

            //        return card;
            //    }
            //}

            //return default;
        }
    }
}