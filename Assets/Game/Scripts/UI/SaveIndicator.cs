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
        [SerializeField] private Image mask;
        [SerializeField] private Image icon;
        
        private PersistenceManager persistenceManager;
        private Tweener _maskTween;
        private Tweener _iconTween;
        private int operationCount;
        
        private void Start()
        {
            persistenceManager = FindObjectOfType<GameManager>().GetService<PersistenceManager>();
            
            persistenceManager.SaveBegin     += OnSaveBegin;
            persistenceManager.SaveCompleted += OnSaveCompleted;
        }

        private void OnDestroy()
        {
            persistenceManager.SaveCompleted -= OnSaveCompleted;
            persistenceManager.SaveBegin     -= OnSaveBegin;
        }

        private void OnSaveBegin()
        {
            operationCount++;

            if ((_maskTween != null && DOTween.IsTweening(_maskTween)) || DOTween.IsTweening(_iconTween))
                return;

            icon.color = icon.color.Alpha(0);
            _iconTween = icon.DOFade(1, 1f).SetLoops(-1, LoopType.Yoyo);

            mask.color = mask.color.Alpha(0);
            _maskTween = mask.DOFade(0.5f, 1f).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnSaveCompleted()
        {
            operationCount--;

            _maskTween.OnStepComplete(OnMaskStepComplete);
            _iconTween.OnStepComplete(OnIconStepComplete);


            void OnMaskStepComplete()
            {
                if (mask.color.a < 0.1f)
                {
                    _maskTween.Kill();
                    
                    mask.color = mask.color.Alpha(0);
                }
            }

            void OnIconStepComplete()
            {
                if (icon.color.a < 0.1f)
                {
                    _iconTween.Kill();
                    
                    icon.color = icon.color.Alpha(0);
                }
            }
        }
    }
}