namespace Game.Components.Movement.MovementControl
{
    public abstract class MovementLogic<TContext>
    {
        public abstract void ApplyLogic(TContext context);
    }
}