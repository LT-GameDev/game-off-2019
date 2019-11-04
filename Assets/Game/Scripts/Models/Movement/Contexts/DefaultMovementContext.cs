using Game.Components.Movement.MovementControl;
using Game.Utility;
using UnityEngine;

namespace Game.Models.Movement.Contexts
{
    public class DefaultMovementContext : IMovementContext
    {
        public bool Accelerate { get; set; }
        public bool Sprinting { get; set; }
        public bool Jumping { get; set; }
        public bool Walking { get; set; }
        public Rigidbody CharacterBody { get; set; }
        public GroundCheck GroundCheckState { get; set; }
        public Vector3 MovementDirection { get; set; }
    }
}