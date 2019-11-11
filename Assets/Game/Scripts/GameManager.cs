﻿#pragma warning disable 649

using System;
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
        [Header("Levels")]
        [SerializeField] private int level1;
        
        private ServiceContainer serviceContainer;

        private void Awake()
        {
            serviceContainer = new ServiceContainer();
            
            serviceContainer.AddService(new GameSceneManager());
        }

        public void StartGame()
        {
            GetService<GameSceneManager>().LoadLevel(level1, OnLoaded);
            

            void OnLoaded()
            {   
                // TODO: Close loading view
            }
        }

        public void ContinueGame()
        {
            // TODO: Open loading view

            var persistedLevelId = level1;        // TODO: Dummy, load from actual data here...
            var loadedLevelData  = new LevelData();
            
            GetService<GameSceneManager>().LoadLevel(persistedLevelId, loadedLevelData, OnLoaded);
            

            void OnLoaded()
            {   
                // TODO: Close loading view
            }
        }

        public void SaveGame()
        {
            // TODO: display save game indicator
            
            var data = FindObjectOfType<LevelManager>().GetLevelData();

            var levelId   = data.levelId;
            var levelData = data.levelData;

            // Asyncronously save data...
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