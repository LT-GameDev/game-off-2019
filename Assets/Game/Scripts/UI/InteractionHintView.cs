using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class InteractionHintView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactionTextView;
        
        public void DisplayInteraction(string text)
        {
            interactionTextView.text = text;
            
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}