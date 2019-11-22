using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Models
{
    [Serializable]
    public class CheckpointData
    {
        public List<string> checkpointIds;

        public CheckpointData()
        {
            checkpointIds = new List<string>();
        }
    }
}