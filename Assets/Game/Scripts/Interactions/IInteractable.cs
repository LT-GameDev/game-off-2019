namespace Game.Interactions
{
    public interface IInteractable
    {
        /// <summary>
        /// This method is used to notify interactable specific actions
        /// such as displaying interaction view with action text
        /// and other things such as glow effect etc
        /// </summary>
        /// <param name="notifiedState">Notifies interactable that source can interact or can no longer interact</param>
        void Notify(bool notifiedState);
        
        /// <summary>
        /// Triggers the interaction when called
        /// </summary>
        void Interact();
    }
}