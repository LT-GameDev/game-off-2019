using System;
using UnityEngine;

namespace Game.Components
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private string playerTag;

        private GameManager gameManager;
        
        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
                gameManager.SaveGame();
        }
    }
}