
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using Framework.Durak.Datas;
using Framework.Durak.Players;
using Framework.Shared.Collections;
using Framework.Shared.States;
using Framework.Durak.Gameplay;

using UnityEngine;

namespace Framework.Durak.States.Battles
{
    public abstract class BattlePlayerWinnerState : DurakState
    {
        private readonly IBoard<Data> board;
        private readonly IPlayerQueue<IPlayer> queue;
        private readonly IPlaces<Transform> places;
        private readonly ICardDealer dealer;
        protected BattlePlayerWinnerState(IStateMachine<DurakGameState> machine, IBoard<Data> board, IPlayerQueue<IPlayer> queue, IPlaces<Transform> places, ICardDealer dealer)
            : base(machine)
        {
            this.board = board;
            this.queue = queue;
            this.places = places;
            this.dealer = dealer;
        }

        public sealed override async void Enter()
        {
            IReadOnlyList<Data> all = board.All;

            await MoveCards(all);
            board.Clear();
            places.Clear();
            await dealer.DealoCard();

            UpdatePlayerQueue(queue);

            NextState(DurakGameState.BattleEnd);
        }

        protected abstract UniTask MoveCards(IReadOnlyList<Data> datas);
        protected abstract void UpdatePlayerQueue(IPlayerQueue<IPlayer> entity);
    }
}