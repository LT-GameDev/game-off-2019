using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Models
{
    [Serializable]
    public class SaveData
    {
        public int levelId;
        public LevelData levelData;
        public CheckpointData checkpoints;
        public List<int> items;
    }
}