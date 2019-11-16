#pragma warning disable 649

using Game.Models.Animations;
using UnityEngine.InputSystem.Utilities;

namespace Game.Components.Animations
{
    public class PlayerAnimationControl : AnimationControl<PlayerAnimationContext>
    {
        private static readonly int locoHash = UnityEngine.Animator.StringToHash("loco");
        private static readonly int walkingHash = UnityEngine.Animator.StringToHash("walking");
        private static readonly int sprintingHash = UnityEngine.Animator.StringToHash("sprinting");
        private static readonly int jumpHash = UnityEngine.Animator.StringToHash("jump");
        
        protected override void DispatchContext()
        {
            Animator.SetBool(locoHash, Context.Loco);
            Animator.SetBool(walkingHash, Context.Walking);
            Animator.SetBool(sprintingHash, Context.Sprinting);

            if (Context.Jump)
            {
                Animator.SetTrigger(jumpHash);
            }
        }
    }
}