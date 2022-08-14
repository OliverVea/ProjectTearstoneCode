using System.Globalization;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.UI.Tooltips
{
    public class EffectTooltipTriggerComponent : AbstractTooltipTriggerComponent
    {
        [SerializeField] private Effect _effect;
        
        public void Initialize(Effect effect)
        {
            _effect = effect;
        }

        protected override string GetTitle()
        {
            return _effect.EffectName;
        }

        protected override (string, string)[] GetAttributes()
        {
            return new []
            {
                ("Duration", _effect.Duration.ToString(CultureInfo.InvariantCulture))
            };
        }

        protected override string GetText()
        {
            return _effect.Description;
        }
    }
}