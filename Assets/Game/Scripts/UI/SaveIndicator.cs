using System;
using DG.Tweening;
using Game.Managers;
using Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SaveIndicator : MonoBehaviour
    {
        [SerializeField] private Image filledImage;
        
        private PersistenceManager persistenceManager;
        private Tweener _tweener;
        private int operationCount;
        
        private void Start()
        {
            persistenceManager = FindObjectOfType<GameManager>().GetService<PersistenceManager>();
            
            persistenceManager.SaveBegin     += OnSaveBegin;
            persistenceManager.SaveCompleted += OnSaveCompleted;
            
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            persistenceManager.SaveCompleted -= OnSaveCompleted;
            persistenceManager.SaveBegin     -= OnSaveBegin;
        }

        private void OnSaveBegin()
        {
            operationCount++;

            if (_tweener != null && DOTween.IsTweening(_tweener))
                return;
            
            gameObject.SetActive(true);

            filledImage.fillClockwise = true;
            
            _tweener = DOTween.To(() => filledImage.fillAmount, val => filledImage.fillAmount = val, 0f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .OnStepComplete(OnLoopStepComplete);


            void OnLoopStepComplete()
            {
                filledImage.fillClockwise = !filledImage.fillClockwise;
                
                if (operationCount <= 0)
                {
                    _tweener.Kill();
                    
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnSaveCompleted()
        {
            operationCount--;
        }
    }
}