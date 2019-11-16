using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Utility
{
    public class SelectSelectable : MonoBehaviour
    {
        [SerializeField] private Selectable selectable;
        
        private void OnEnable()
        {
            selectable.Select();
        }
    }
}
