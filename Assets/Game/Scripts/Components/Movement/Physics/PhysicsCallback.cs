namespace Game.Components.Movement.Physics
{
    public abstract class PhysicsCallback
    {
        public abstract void Apply(MovementContext context, float physicsTimeStep);
    }
}