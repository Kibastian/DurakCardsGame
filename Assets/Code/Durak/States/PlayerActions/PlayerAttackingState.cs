﻿using Framework.Durak.Datas;
using Framework.Durak.Gameplay.Handlers;
using Framework.Durak.Players;
using Framework.Durak.Players.Selectors;
using Framework.Shared.Collections;
using Framework.Shared.States;
using Framework.Shared.Cards.Entities;
namespace Framework.Durak.States.Actions
{
    public sealed class PlayerAttackingState : PlayerActionState
    {
        protected override DurakGameState AfterCardSelected => DurakGameState.PlayerDefending;
        protected override DurakGameState AfterPass => (cpass>1)?DurakGameState.BattleDefenderWinner:DurakGameState.PlayerAttacking;

        public PlayerAttackingState(IStateMachine<DurakGameState> machine, IDeck<Data> deck, IBoard<Data> board, IPlayerStorage<IPlayer> storage, IPlayerQueue<IPlayer> queue, IReadonlyIndexer<PlayerType, ISelectorsGroup> selectorsIndexer, IAttackerSelectionHandler selection, IMap<ICard, Data> map)
            : base(machine, deck, board, storage, queue, selectorsIndexer, selection, map) { }

        protected override void UpdatePlayerQueue(IPlayerQueue<IPlayer> queue) {
            queue.SetAttackerQueue(
                attacker: (cpass==0)?queue.Attacker:queue.GetNextFrom(queue.Defender, andSkip: (queue.GetNextFrom(queue.Defender)==queue.Attacker)?1:0),
                defender: queue.Defender);
        }

        protected override ICardSelector GetSelector(ISelectorsGroup group) => group.Attacking;
    }
}