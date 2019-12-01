#pragma warning disable 649

using UnityEngine;

namespace Game.AnimationStateBehaviours
{
    public enum State
    {
        Pickup,
        Idle,
        Walking,
        Running,
        Sprinting,
        RunningJump,
        Jumping,
        Falling
    }
    
    public delegate void AnimationStateHandler(Animator animator, State state);
    
    public class AnimationStateEvents : StateMachineBehaviour
    {
        public static event AnimationStateHandler Enter;
        public static event AnimationStateHandler Exit;

        [SerializeField] private State state;

        private bool inState;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
//        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//        {
//        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.IsInTransition(layerIndex) || inState)
                return;
            
            Enter?.Invoke(animator, state);

            inState = true;
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Exit?.Invoke(animator, state);

            inState = false;
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
