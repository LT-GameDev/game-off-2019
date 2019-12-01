using System;
using Game.Managers;
using UnityEngine;

namespace Game.Components
{
    public class ZoneLoader : MonoBehaviour
    {
        [SerializeField] private int zoneId;
        [SerializeField] private string tag;

        private LevelManager levelManager;
        
        private void Awake()
        {
            levelManager = FindObjectOfType<LevelManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(tag) || levelManager.GetLevelData().levelData.zones.Contains(zoneId))
                return;
            
            levelManager.LoadZone(zoneId);
        }
    }
}