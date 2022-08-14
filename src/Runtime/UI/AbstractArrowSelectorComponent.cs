using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MyRpg.UI
{
    public abstract class AbstractArrowSelectorComponent : MonoBehaviour
    {
        [Serializable]
        private class SelectorOption
        {
            public string Key;
            public string DisplayValue;
        }
        
        [SerializeField] private List<SelectorOption> options;
        [SerializeField] private int currentIndex;
        private bool InvalidIndex => currentIndex < 0 || currentIndex >= options.Count;
        protected string CurrentSelection => InvalidIndex ? "" : options[currentIndex].Key;

        [SerializeField] private TMP_Text selectionText;

        private void Start()
        {
            UpdateText();
        }

        public virtual void Increment()
        {
            currentIndex = (currentIndex + 1) % options.Count;
            UpdateText();
        }

        public virtual void Decrement()
        {
            currentIndex = (currentIndex - 1 + options.Count) % options.Count;
            UpdateText();
        }
        
        private void UpdateText()
        {
            selectionText.text = InvalidIndex ? "?" : options[currentIndex].DisplayValue;
        }
    }
}