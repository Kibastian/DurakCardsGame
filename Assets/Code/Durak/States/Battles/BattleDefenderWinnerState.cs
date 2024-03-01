
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using Framework.Durak.Collections;
using Framework.Durak.Datas;
using Framework.Durak.Players;
using Framework.Durak.Services.Movements;
using Framework.Shared.Collections;
using Framework.Shared.States;
using Framework.Durak.Gameplay;
using UnityEngine;

namespace Framework.Durak.States.Battles
{
    public class BattleDefenderWinnerState : BattlePlayerWinnerState
    {
        private readonly IDiscardPile discardPile;
        private readonly IDiscardPileCardMovement movement;
        private readonly IBoard<Data> board;
        public BattleDefenderWinnerState(IStateMachine<DurakGameState> machine, IPlayerQueue<IPlayer> queue, IBoard<Data> board, IDiscardPile discardPile, IDiscardPileCardMovement movement, IPlaces<Transform> places, ICardDealer dealer)
            : base(machine, board, queue, places, dealer)
        {
            this.discardPile = discardPile;
            this.movement = movement;
            this.board = board;
        }

        protected override UniTask MoveCards(IReadOnlyList<Data> datas)
        {
            discardPile.AddRange(datas);
            foreach (var e in datas)
            {
                board.RemoveSeen(e);
            }
            return movement.MoveTo(datas);

        }
        protected override void UpdatePlayerQueue(IPlayerQueue<IPlayer> queue)
        {
            queue.SetAttackerQueue(
                attacker: queue.Defender,
                defender: queue.GetNextFrom(queue.Defender));
        }
    }
}