using Game.Utility;
using UnityEngine;

namespace Game.Components.Movement.MovementControl
{
    public interface IMovementContext
    {
        bool Accelerate { get; }
        bool Sprinting { get; }
        bool Jumping { get; }
        bool Walking { get; }
        Rigidbody CharacterBody { get; }
        GroundCheck GroundCheckState { get; }
        Vector3 MovementDirection { get; }
    }
}