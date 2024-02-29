
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using Framework.Durak.Datas;
using Framework.Durak.Players;
using Framework.Durak.Services.Movements;
using Framework.Shared.Collections;
using Framework.Shared.States;


namespace Framework.Durak.States.Battles
{
    public sealed class BattleAttackerWinnerState : BattlePlayerWinnerState
    {
        private readonly IPlayerQueue<IPlayer> queue;
        private readonly IPlayerCardMovement movement;
        private readonly IBoard<Data> board;

        public BattleAttackerWinnerState(IStateMachine<DurakGameState> machine, IPlayerQueue<IPlayer> queue, IBoard<Data> board, IPlayerCardMovement movement)
            : base(machine, board, queue)
        {
            this.queue = queue;
            this.movement = movement;
            this.board = board;
        }

        protected override async UniTask MoveCards(IReadOnlyList<Data> datas)
        {
            IPlayer defender = queue.Defender;

            defender.Hand.AddRange(datas);

            await movement.MoveTo(defender, datas);
            foreach (var e in datas)
                board.AddSeen(e);
        }

        protected override void UpdatePlayerQueue(IPlayerQueue<IPlayer> queue)
        {
            queue.SetAttackerQueue(
                attacker: queue.GetNextFrom(queue.Defender),
                defender: queue.GetNextFrom(queue.Defender, andSkip:+1));
        }
    }
}