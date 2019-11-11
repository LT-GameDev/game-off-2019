using System;
using UnityEngine;

namespace Game.Models
{
    [Serializable]
    public class SaveData
    {
        public int levelId;
        public LevelData levelData;
    }
}