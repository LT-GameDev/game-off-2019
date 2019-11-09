using System;
using Game.Components.Movement.MovementControl;

namespace Game.Components.Movement
{
    [Serializable]
    public abstract class CharacterMovementLogic<TContext> : IMovementLogic<TContext>
    {
        public void Run(TContext context, float deltaTime)
        {
            DeltaTime = deltaTime;    // Delta time is only valid while running logic
            
            context = Preprocess(context);
            
            LocoPhysics?.Apply(context, deltaTime);
            JumpAndFallPhysics?.Apply(context,deltaTime);

            Postprocess(context);

            DeltaTime = 0;            // Reset delta time after logic update is finished 
        }

        public abstract void Initialize();
        public abstract TContext Preprocess(TContext context);
        public abstract void Postprocess(TContext context);

        public TContext Context { get; protected set; }
        protected abstract PhysicsCallback<TContext> LocoPhysics { get; }
        protected abstract PhysicsCallback<TContext> JumpAndFallPhysics { get; }
        protected float DeltaTime { get; private set; }
    }
}