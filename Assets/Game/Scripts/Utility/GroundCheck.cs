#pragma warning disable 649

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
            if (Physics.SphereCast(position, radius, -Vector3.up, out var hit, maxDistance, layerMask.value))
            {
                if (hit.distance < halfHeight + 0.1f)
                {
                    Grounded = true;
                    Distance = 0;
                    Normal   = hit.normal;
                }
                else
                {
                    Grounded = false;
                    Distance = hit.distance;
                }
            }
        }

        public void Check(Vector3 position, float halfHeight, float radius, Vector3 up)
        {
            if (Physics.SphereCast(position, radius, -up, out var hit, maxDistance, layerMask.value))
            {
                if (hit.distance < halfHeight + 0.1f)
                {
                    Grounded = true;
                    Distance = 0;
                    Normal   = hit.normal;
                }
                else
                {
                    Grounded = false;
                    Distance = hit.distance;
                }
            }
        }

        public bool Grounded { get; private set; }

        public float Distance { get; private set; }

        public Vector3 Normal { get; private set; }
    }
}