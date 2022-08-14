using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyRpg.UI.Tooltips
{
    public class FixedTooltipTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string title = string.Empty;
        [SerializeField] private string[] attributeKeys = Array.Empty<string>();
        [SerializeField] private string[] attributeValues = Array.Empty<string>();
        [SerializeField] private string text = string.Empty;

        private (string, string)[] GetAttributes()
        {
            var attributes = new List<(string, string)>();
            for (var i = 0; i < Mathf.Min(attributeKeys.Length, attributeValues.Length); i++)
                attributes.Add((attributeKeys[i], attributeValues[i]));
            return attributes.ToArray();
        }
        
        private void OnMouseEnter()
        {
            var attributes = GetAttributes();
            TooltipManager.Show(title, attributes, text);
        }

        public void OnMouseExit()
        {
            TooltipManager.Hide();
        }
    }
}