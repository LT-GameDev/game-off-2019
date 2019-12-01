using System.Collections.Generic;
using Game.Containers;
using Game.Models;

namespace Game.Managers
{
    public class CheckpointManager
    {
        private CheckpointData checkpointData;
        private GameManager gameManager;

        public CheckpointManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void Initialize(CheckpointData data)
        {
            checkpointData = data ?? new CheckpointData();
        }

        public void SetCheckpoint(string id)
        {
            if (!checkpointData.checkpointIds.Contains(id))
            {
                checkpointData.checkpointIds.Add(id);
                gameManager.SaveGame();   
            }
        }

        public CheckpointData GetCheckpoints()
        {
            return checkpointData;
        }

        public bool IsActive(string id)
        {
            return !checkpointData.checkpointIds.Contains(id);
        }
    }
}