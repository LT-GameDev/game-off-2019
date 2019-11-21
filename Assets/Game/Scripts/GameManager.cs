#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Containers;
using Game.Managers;
using Game.Models;
using Game.Models.Inventory;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour, IServiceContainer
    {
        [SerializeField] private int mainMenu;
    
        [Header("Levels")]
        [SerializeField] private int level1;

        [Header("Data")] 
        [SerializeField] private ItemDatabase items;

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
            serviceContainer.AddService(new InventoryManager());
            serviceContainer.AddService(new PlayerResourceManager());
        }

        public void StartGame()
        {
            loadingView.SetActive(true);

            GetService<PlayerResourceManager>().PrepareResources();
            GetService<InventoryManager>().InitializeInventory();
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
                GetService<PlayerResourceManager>().PrepareResources();
                GetService<GameSceneManager>().LoadLevel(saveData.levelId, saveData.levelData, OnLoaded);
                GetService<InventoryManager>().InitializeInventory(saveData.items.Select(id => items.GetById(id)).ToList());
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
            
            var levelData = FindObjectOfType<LevelManager>().GetLevelData();
            var itemsData = GetService<InventoryManager>().GetItems();

            var saveData = new SaveData();
                
            saveData.levelId   = levelData.levelId;
            saveData.levelData = levelData.levelData;
            saveData.items     = itemsData.Select(item => item.ItemId).ToList();

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