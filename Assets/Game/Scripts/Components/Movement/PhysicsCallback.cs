namespace Game.Components.Movement
{
    public abstract class PhysicsCallback<TContext>
    {
        public abstract void Apply(TContext context, float physicsTimeStep);
    }
}