#pragma warning disable 649

using System;
using Game.Components;
using Game.Models;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private Transform playerStartPosition;
        
        [SerializeField] private int thisLevelId;
        [SerializeField] private int defaultZoneId;
        
        private LevelData levelData;

        private void OnDestroy()
        {
            for (var index = levelData.zones.Count - 1; index >= 0; index--)
            {
                var zone = levelData.zones[index];
                UnloadZone(zone);
            }
        }

        public void Initialize(Action initializationComplete)
        {
            levelData = LevelData.Create();
            
            LoadZone(defaultZoneId, OnZoneLoaded);

            
            void OnZoneLoaded()
            {
                PlacePlayer();
                
                initializationComplete?.Invoke();
            }
        }

        public void Initialize(LevelData levelData, Action initializationComplete)
        {
            this.levelData = levelData;

            int loadOps = 0;
            foreach (var id in this.levelData.zones)
            {
                loadOps++;
                LoadZone(id, InternalZoneLoaded);


                void InternalZoneLoaded()
                {
                    loadOps--;

                    if (loadOps < 1)
                    {
                        PlacePlayer();
                        
                        initializationComplete?.Invoke();
                    }
                }
            }
        }

        public (int levelId, LevelData levelData) GetLevelData()
        {
            var playerTransform = player.transform;

            levelData.playerPosition = playerTransform.position;
            levelData.playerRotation = playerTransform.rotation.eulerAngles;
            
            return (thisLevelId, levelData);
        }

        private void LoadZone(int zoneId, Action onComplete = null)
        {
            var asyncLoadOp = SceneManager.LoadSceneAsync(zoneId, LoadSceneMode.Additive);

            asyncLoadOp.allowSceneActivation = true;
            asyncLoadOp.completed += InternalLoadComplete;


            void InternalLoadComplete(AsyncOperation op)
            {
                levelData.zones.Add(zoneId);
                onComplete?.Invoke();
            }
        }

        private void UnloadZone(int zoneId)
        {
            var asyncUnloadOp = SceneManager.UnloadSceneAsync(zoneId, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

            if (asyncUnloadOp == null)
            {
                OnZoneUnloaded(null);
                return;
            }
            
            asyncUnloadOp.allowSceneActivation = true;
            asyncUnloadOp.completed += OnZoneUnloaded;


            void OnZoneUnloaded(AsyncOperation op)
            {
                levelData.zones.Remove(zoneId);
            }
        }

        private void PlacePlayer()
        {
            var playerTransform = player.transform;
                
            playerTransform.position = playerStartPosition.position;
            playerTransform.rotation = playerStartPosition.rotation;
        }
    }
}