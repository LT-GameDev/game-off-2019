#pragma warning disable 649

using System;
using System.Collections.Generic;
using Game.AnimationStateBehaviours;
using Game.Models.Animations;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Game.Components.Animations
{
    public class PlayerAnimationControl : AnimationControl<PlayerAnimationContext>
    {
        public event Action BecomeBusy;
        public event Action NoLongerBusy;
        
        private static readonly int locoHash = UnityEngine.Animator.StringToHash("loco");
        private static readonly int walkingHash = UnityEngine.Animator.StringToHash("walking");
        private static readonly int sprintingHash = UnityEngine.Animator.StringToHash("sprinting");
        private static readonly int pickupHash = UnityEngine.Animator.StringToHash("pickup");
        private static readonly int jumpHash = UnityEngine.Animator.StringToHash("jump");

        [SerializeField] private List<State> busyStates;

        private State animationState;
        private bool busy;
        
        private void OnEnable()
        {
            AnimationStateEvents.Enter += OnStateEnter;
        }

        private void OnDisable()
        {
            AnimationStateEvents.Enter -= OnStateEnter;
        }
        
        protected override void DispatchContext()
        {
            Animator.SetBool(locoHash, Context.Loco);
            Animator.SetBool(walkingHash, Context.Walking);
            Animator.SetBool(sprintingHash, Context.Sprinting);

            if (Context.Jump)
            {
                Animator.SetTrigger(jumpHash);
            }

            if (Context.Pickup)
            {
                Animator.SetTrigger(pickupHash);
            }
        }

        private void OnStateEnter(Animator animator, State state)
        {
            if (animator == Animator)
            {
                animationState = state;

                var newBusyState = busyStates.Contains(animationState);

                if (newBusyState && !busy)
                {
                    BecomeBusy?.Invoke();
                }
                else if (busy && !newBusyState)
                {
                    NoLongerBusy?.Invoke();
                }

                busy = newBusyState;
            }
        }
    }
}