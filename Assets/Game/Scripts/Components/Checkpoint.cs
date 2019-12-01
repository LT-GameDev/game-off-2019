using System;
using Game.Managers;
using UnityEngine;

namespace Game.Components
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private string playerTag;
        
        [HideInInspector]
        [SerializeField] private string id;
        
        private CheckpointManager checkpointManager;

        public Checkpoint()
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = System.Guid.NewGuid().ToString();
            }
        }
        
        private void Awake()
        {
            checkpointManager = FindObjectOfType<GameManager>().GetService<CheckpointManager>();
            
            gameObject.SetActive(checkpointManager.IsActive(id));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                checkpointManager.SetCheckpoint(id);
                gameObject.SetActive(false);
            }
        }
    }
}