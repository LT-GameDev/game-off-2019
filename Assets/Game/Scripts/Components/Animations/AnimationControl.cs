#pragma warning disable 649

using System;
using UnityEngine;

namespace Game.Components.Animations
{
    public interface IAnimationContext
    {
        void OnUpdated(Action callback);
    }
    
    public abstract class AnimationControl<TContext> : MonoBehaviour
        where TContext : class, IAnimationContext, new()
    {
        [SerializeField] private Animator targetAnimator;
        
        private void Awake()
        {
            Context = new TContext();
            
            Context.OnUpdated(DispatchContext);
        }

        /// <summary>
        /// Dispatches the current state to the animator
        /// and causes animator state to be updated
        /// </summary>
        protected abstract void DispatchContext();

        protected Animator Animator => targetAnimator;

        public TContext Context { get; private set; }
    }
}