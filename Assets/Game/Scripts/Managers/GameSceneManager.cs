using System;
using Game.Models;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class GameSceneManager
    {
        private bool levelsInitialized;

        public void LoadSync(int levelId)
        {
            SceneManager.LoadScene(levelId, LoadSceneMode.Single);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelId));

            levelsInitialized = true;
        }
        
        public void LoadLevel(int levelId, Action onComplete)
        {
            UnloadActiveLevel(InternalLoad);

            
            void InternalLoad()
            {
                var asyncLoadOp = SceneManager.LoadSceneAsync(levelId, LoadSceneMode.Additive);

                asyncLoadOp.allowSceneActivation = true;
                asyncLoadOp.completed += InitializeLevel;
            }

            void InitializeLevel(AsyncOperation operation)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelId));
                
                GameObject.FindObjectOfType<LevelManager>().Initialize(OnZonesLoaded);

                levelsInitialized = true;
            }

            void OnZonesLoaded()
            {
                onComplete?.Invoke();
            }
        }

        public void LoadLevel(int levelId, LevelData levelData, Action onComplete)
        {
            UnloadActiveLevel(InternalLoad);


            void InternalLoad()
            {
                var asyncLoadOp = SceneManager.LoadSceneAsync(levelId, LoadSceneMode.Additive);

                asyncLoadOp.allowSceneActivation = true;
                asyncLoadOp.completed += InitializeLevel;
            }
            
            void InitializeLevel(AsyncOperation operation)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelId));
                
                GameObject.FindObjectOfType<LevelManager>().Initialize(levelData, OnZonesLoaded);

                levelsInitialized = true;
            }

            void OnZonesLoaded()
            {
                onComplete?.Invoke();
            }
        }

        private void UnloadActiveLevel(Action onComplete)
        {
            if (!levelsInitialized)
            {
                onComplete?.Invoke();
                return;
            }
            
            var asyncOp = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene(), UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            
            asyncOp.allowSceneActivation = true;
            asyncOp.completed += OnCompleted;


            void OnCompleted(AsyncOperation op)
            {
                onComplete?.Invoke();
            }
        }
    }
}