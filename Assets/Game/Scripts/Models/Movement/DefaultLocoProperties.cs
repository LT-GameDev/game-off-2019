#pragma warning disable 649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Models.Movement
{
    [CreateAssetMenu(menuName = "Movement/Humanoid Loco Properties")]
    public class DefaultLocoProperties : ScriptableObject
    {
        [SerializeField] private float sprintMultiplier;
        [SerializeField] private float walkMultiplier;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private float airControl;

        public float SprintMultiplier => sprintMultiplier;
        public float WalkMultiplier => walkMultiplier;
        public float MovementSpeed => movementSpeed;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public float AirControl => airControl;
    }
}
