#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Game.Utility
{
    public class SelectSelectable : MonoBehaviour
    {
        [SerializeField] private Selectable selectable;
        
        private void OnEnable()
        {
            selectable.Select();
        }
    }
}
