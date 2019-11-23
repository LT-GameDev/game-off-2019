using System;
using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;

namespace Game.UI
{
    public class PauseMenuView : MonoBehaviour
    {
        private static readonly int pauseHash = Animator.StringToHash("pause");
        
        [SerializeField] private Animator pauseMenuAnimator;

        private GameManager gameManager;
        private PlayerInput input;
        private bool paused;
        
        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>(); 
            
            input = new PlayerInput();
            
            input.PauseMenu.TogglePause.performed += _ => Toggle();
        }

        private void OnEnable()
        {
            input.PauseMenu.Enable();
        }

        private void OnDisable()
        {
            input.PauseMenu.Disable();
        }

        public void ResumeGame()
        {
            Toggle();
        }

        public void MainMenu()
        {
            gameManager.MainMenu();
        }

        public void SaveAndQuit()
        {
            var persistenceManager = gameManager.GetService<PersistenceManager>();

            persistenceManager.SaveCompleted += OnSaved;
            
            gameManager.SaveGame();
            

            void OnSaved()
            {
                persistenceManager.SaveCompleted -= OnSaved;
                
                gameManager.QuitGame();
            }
        }

        private void Toggle()
        {
            pauseMenuAnimator.SetTrigger(pauseHash);
            
            if (paused)
                gameManager.ResumeGame();
            else
                gameManager.PauseGame();

            paused = !paused;
        }
    }
}