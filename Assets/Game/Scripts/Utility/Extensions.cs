namespace Game.Utility
{
    public static class Extensions
    {
        public static bool IsNull(this object obj) => obj == null || obj.Equals(null);
    }
}