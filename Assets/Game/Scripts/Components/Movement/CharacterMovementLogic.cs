using System;
using Game.Components.Movement.MovementControl;

namespace Game.Components.Movement
{
    [Serializable]
    public abstract class CharacterMovementLogic<TContext> : IMovementLogic
        where TContext : class, new()
    {
        public CharacterMovementLogic()
        {
            Context = new TContext();
        }
        
        public void Run(float deltaTime)
        {
            DeltaTime = deltaTime;    // Delta time is only valid while running logic
            
            Preprocess();
            
            LocoPhysics?.Apply(Context, deltaTime);
            JumpAndFallPhysics?.Apply(Context, deltaTime);

            Postprocess();

            DeltaTime = 0;            // Reset delta time after logic update is finished 
        }

        public abstract void Initialize();
        public abstract void Preprocess();
        public abstract void Postprocess();

        public TContext Context { get; protected set; }
        protected abstract PhysicsCallback<TContext> LocoPhysics { get; }
        protected abstract PhysicsCallback<TContext> JumpAndFallPhysics { get; }
        protected float DeltaTime { get; private set; }
    }
}