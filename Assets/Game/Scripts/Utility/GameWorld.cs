using UnityEngine;
using static UnityEngine.Mathf;

namespace Game.Utility
{
    public static class GameWorld
    {
        public static Vector3 GetGroundPlane() => Vector3.one - GetGravityAxis();

        public static Vector3 GetGravityAxis()
        {
            var gravity = Physics.gravity.normalized;
            
            return new Vector3(Abs(gravity.x), Abs(gravity.y), Abs(gravity.z));
        }
    }
}