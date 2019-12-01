using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Models
{
    [Serializable]
    public struct LevelData
    {
        public List<int> zones;
        public Vector3 playerPosition;
        public Vector3 playerRotation;

        public static LevelData Create()
        {
            var levelData = new LevelData();
            
            levelData.zones = new List<int>();

            return levelData;
        }
    }
}