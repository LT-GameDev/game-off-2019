using UnityEngine;

namespace Game.Components.Movement.MovementControl
{
    public interface IMovementLogic
    {
        void Initialize();
        void Run(float deltaTime);
    }
}