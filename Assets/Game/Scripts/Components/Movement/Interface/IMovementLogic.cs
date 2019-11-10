using UnityEngine;

namespace Game.Components.Movement.Interface
{
    public interface IMovementLogic
    {
        void Run(float deltaTime);
    }

    public interface IMovementLogic<TContext> : IMovementLogic
        where TContext : class
    {
        void Initialize(TContext context);
    }
}