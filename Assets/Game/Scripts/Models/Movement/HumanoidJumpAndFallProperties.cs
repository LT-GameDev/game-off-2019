using UnityEngine;

namespace Game.Models.Movement
{
    [CreateAssetMenu(menuName = "Movement/Humanoid Jump and Fall Properties")]
    public class HumanoidJumpAndFallProperties : ScriptableObject
    {
        [SerializeField] private float additionalFallGravity;
        [SerializeField] private float jumpPower;

        public float JumpPower => jumpPower;

        public float AdditionalFallGravity => additionalFallGravity;
    }
}