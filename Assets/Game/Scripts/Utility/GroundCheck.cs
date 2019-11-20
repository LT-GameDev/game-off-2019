﻿#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    [CreateAssetMenu(menuName = "Utility/Ground Checker")]
    public class GroundCheck : ScriptableObject
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float maxDistance;

        public void Check(Vector3 position, float halfHeight, float radius)
        {
            if (Physics.SphereCast(position, radius, Physics.gravity.normalized, out var hit, maxDistance, layerMask.value))
            {
                if (hit.distance < halfHeight + 0.1f)
                {
                    Grounded = true;
                    Distance = 0;
                    Normal   = hit.normal;
                    
                    return;
                }
                
                Grounded = false;
                Distance = hit.distance;

                return;
            }

            Distance = maxDistance;
            Grounded = false;
        }
        
        public float MaxDistance => maxDistance;

        public bool Grounded { get; private set; }

        public float Distance { get; private set; }

        public Vector3 Normal { get; private set; }

    }
}