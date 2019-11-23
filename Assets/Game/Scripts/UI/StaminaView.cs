using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Managers;
using Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class StaminaView : MonoBehaviour
    {
        [SerializeField] private Image filledImage;
        [SerializeField] private Image[] outlines;

        private PlayerResourceManager resourceManager;

        private void Awake()
        {
            var gameManager = FindObjectOfType<GameManager>();

            resourceManager = gameManager.GetService<PlayerResourceManager>();
            
            resourceManager.StaminaConsumed  += OnStaminaConsumed;
            resourceManager.StaminaRecovered += OnStaminaRecovered;
            
            gameObject.SetActive(false);
        }

        private void OnStaminaConsumed()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                
                filledImage.color = filledImage.color.Alpha(0);
                filledImage.DOFade(1, 0.3f);

                foreach (var image in outlines)
                {
                    image.color = image.color.Alpha(0);
                    image.DOFade(1, 0.3f);
                }
            }

            filledImage.fillAmount = resourceManager.GetStamina() / resourceManager.GetMaxStamina();
        }

        private void OnStaminaRecovered()
        {
            filledImage.fillAmount = resourceManager.GetStamina() / resourceManager.GetMaxStamina();

            if (filledImage.fillAmount > 0.99f)
            {
                filledImage.DOFade(0, 0.3f);
                gameObject.SetActive(false);

                foreach (var image in outlines)
                    image.DOFade(0, 0.3f);
            }
        }

        private void OnDestroy()
        {
            if (resourceManager != null)
            {
                resourceManager.StaminaRecovered -= OnStaminaRecovered;
                resourceManager.StaminaConsumed  -= OnStaminaConsumed;
            }
        }
    }
}
