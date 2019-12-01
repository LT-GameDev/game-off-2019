using UnityEngine;

namespace Game.Utility
{
    public static class Extensions
    {
        public static bool IsNull(this object obj) => obj == null || obj.Equals(null);

        public static Color Alpha(this Color color, float alpha)
        {
            color.a = alpha;
            
            return color;
        }
    }
}