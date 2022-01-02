﻿using Framework.Shared.States;

using UnityEngine;

namespace Framework.Durak.States
{
    public abstract class DurakState : MonoBehaviour, IState
    {
        [Space(height: 15), SerializeField] private DurakGameStateMachine machine;

        public virtual void Enter()
        {
            Debug.Log($"{nameof(Enter)}: {GetType().Name}", this);
        }
        public virtual void Exit()
        {
            // Debug.Log($"{nameof(Exit)}: {GetType().Name}", this);
        }

        protected void NextState(DurakGameState state) => machine.Fire(state);
    }
}