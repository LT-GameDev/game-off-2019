#pragma warning disable 649

using Game.Managers;
using UnityEngine;

namespace Game.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject continueButton;

        private GameManager gameManager;
        private PersistenceManager persistenceManager;
        
        private void Awake()
        {
            gameManager        = FindObjectOfType<GameManager>();
            persistenceManager = gameManager.GetService<PersistenceManager>();
        }
        
        private void OnEnable()
        {
            continueButton.SetActive(persistenceManager.HasSave());
        }

        public void StartGame()
        {
            gameManager.StartGame();
        }

        public void ContinueGame()
        {
            gameManager.ContinueGame();
        }

        public void QuitGame()
        {
            gameManager.QuitGame();
        }
    }
}