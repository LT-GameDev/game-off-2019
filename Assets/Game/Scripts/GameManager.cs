#pragma warning disable 649

using System;
using System.Collections;
using System.Threading.Tasks;
using Game.Components;
using Game.Containers;
using Game.Managers;
using Game.Models;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour, IServiceContainer
    {
        [SerializeField] private int mainMenu;
    
        [Header("Levels")]
        [SerializeField] private int level1;

        [Header("General Purpose UI")] 
        [SerializeField] private GameObject loadingView;
        
        private ServiceContainer serviceContainer;

        private void Awake()
        {
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            
            ConfigureServices();
            
            GetService<GameSceneManager>().LoadSync(mainMenu);
        }

        private void ConfigureServices()
        {
            serviceContainer = new ServiceContainer();

            serviceContainer.AddService(new GameSceneManager());
            serviceContainer.AddService(new PersistenceManager());
        }

        public void StartGame()
        {
            loadingView.SetActive(true);

            StartCoroutine(LoadLevelDelayed());

            void OnLoaded()
            {   
                loadingView.SetActive(false);
            }

            IEnumerator LoadLevelDelayed()
            {
                yield return new WaitForSeconds(0.3f);
                GetService<GameSceneManager>().LoadLevel(level1, OnLoaded);
            }
        }

        public void ContinueGame()
        {
            loadingView.SetActive(true);

            StartCoroutine(LoadLevelDelayed());


            void OnPersistentData(SaveData saveData)
            {
                GetService<GameSceneManager>().LoadLevel(saveData.levelId, saveData.levelData, OnLoaded);
            }

            void OnLoaded()
            {
                loadingView.SetActive(false);
            }

            IEnumerator LoadLevelDelayed()
            {
                yield return new WaitForSeconds(0.3f);
                GetService<PersistenceManager>().Load(OnPersistentData, StartGame);
            }
        }

        public void SaveGame()
        {
            // TODO: display save game indicator
            
            var data = FindObjectOfType<LevelManager>().GetLevelData();

            var saveData = new SaveData();

            saveData.levelId   = data.levelId;
            saveData.levelData = data.levelData;

            GetService<PersistenceManager>().Save(saveData);
        }

        public void QuitGame()
        {
            // Play quit sequence or something, or nothing, idk
            
            // Then... quit
            Application.Quit();
        }

        public TService GetService<TService>()
        {
            return serviceContainer.GetService<TService>();
        }
    }
}