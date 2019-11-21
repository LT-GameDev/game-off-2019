using System;
using UnityEngine;

namespace Game.Managers
{
    public class PlayerResourceManager
    {
        public event Action StaminaConsumed;
        public event Action StaminaRecovered;
        
        private float staminaRecoveryRate;
        private float maxStamina;
        private float stamina;

        public void PrepareResources()
        {
            this.staminaRecoveryRate = 30;
            this.stamina             = maxStamina;
            this.maxStamina          = 100;
        }

        public bool ConsumeStamina(float amount)
        {
            var oldStamina   = stamina;
            
            stamina = Mathf.Clamp(stamina - Mathf.Abs(amount), 0, maxStamina);

            var deltaStamina   = oldStamina - stamina;
            var staminaChanged = Mathf.Abs(deltaStamina) > 0.1f;

            if (staminaChanged)
                StaminaConsumed?.Invoke();
                
            return staminaChanged;
        }

        public bool RecoverStamina(float deltaTime)
        {
            var oldStamina = stamina;
            
            stamina = Mathf.Clamp(stamina + Mathf.Abs(deltaTime) * staminaRecoveryRate, 0, maxStamina);

            var deltaStamina     = stamina - oldStamina;
            var staminaRecovered = Mathf.Abs(deltaStamina) > 0.1f;
            
            if (staminaRecovered)
                StaminaRecovered?.Invoke();

            return staminaRecovered;
        }
    }
}