#pragma warning disable 649

using UnityEngine;

namespace Game.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        private static readonly int openHash = Animator.StringToHash("open");
        private static readonly int closeHash = Animator.StringToHash("close");
        
        [SerializeField] private Animator animator;
        
        private void OnEnable()
        {
            animator.SetTrigger(openHash);
        }

        private void OnDisable()
        {
            animator.SetTrigger(closeHash);
        }
    }
}