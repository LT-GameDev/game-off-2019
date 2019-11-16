using System;
using Game.Components.Animations;

namespace Game.Models.Animations
{
    public class PlayerAnimationContext : IAnimationContext
    {
        private bool walking;
        private bool sprinting;
        private bool loco;
        private bool jump;
        
        private Action updated;

        public void OnUpdated(Action callback)
        {
            updated = callback;
        }

        public bool Walking
        {
            get => walking;
            set
            {
                if (walking == value)
                    return;

                walking = value;
                
                updated?.Invoke();
            }
        }

        public bool Sprinting
        {
            get => sprinting;
            set
            {
                if (sprinting == value)
                    return;

                sprinting = value;
                
                updated?.Invoke();
            }
        }

        public bool Loco
        {
            get => loco;
            set
            {
                if (loco == value)
                    return;

                loco = value;
                
                updated?.Invoke();
            }
        }

        public bool Jump
        {
            get
            {
                var state = jump;
                jump = false;
                return state;
            }
            set
            {
                if (jump == value)
                    return;

                jump = value;
                
                updated?.Invoke();
            }
        }
    }
}