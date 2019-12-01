#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Game.Utility
{
    public class SelectSelectable : MonoBehaviour
    {
        private void OnEnable()
        {
            var selectables = GetComponentsInChildren<Selectable>();

            foreach (var selectable in selectables)
            {
                if (selectable.enabled)
                {
                    selectable.Select();
                    return;
                }
            }
        }
    }
}
