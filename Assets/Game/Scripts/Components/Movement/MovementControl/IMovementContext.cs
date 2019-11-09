using Game.Utility;
using UnityEngine;

namespace Game.Components.Movement.MovementControl
{
    public interface IMovementContext
    {
        Rigidbody CharacterBody { get; }
        GroundCheck GroundCheckState { get; }
        Vector3 MovementDirection { get; }
    }
}