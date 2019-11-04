using UnityEngine;

namespace Game.Components.Movement.MovementControl
{
    public interface IMovementLogic<TContext>
    {
        TContext CreateContext();
        
        void Run(TContext context, float deltaTime);
    }
}