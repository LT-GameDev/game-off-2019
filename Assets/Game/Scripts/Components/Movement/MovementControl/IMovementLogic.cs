using UnityEngine;

namespace Game.Components.Movement.MovementControl
{
    public interface IMovementLogic<TContext>
    {
        void Initialize();
        void Run(TContext context, float deltaTime);
    }
}