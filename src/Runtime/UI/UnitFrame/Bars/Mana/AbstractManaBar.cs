using MyRpg.Core.Components;
using MyRpg.Core.Events;
using UnityEngine;

namespace MyRpg.UI.UnitFrame.Bars
{
    public abstract class AbstractManaBar : Bar
    {
        private IManaEventHandler _manaEventHandler;
        private IManaComponent _manaComponent;

        protected override void SetTarget(GameObject target)
        {
            ClearTarget();

            if (target == null) return;
            
            _manaEventHandler = target.GetComponent<IManaEventHandler>();
            if (_manaEventHandler != null) _manaEventHandler.RegisterOnManaChanged(OnManaChanged);
            
            _manaComponent = target.GetComponent<IManaComponent>();
            if (_manaComponent != null) UpdateValue(_manaComponent.GetCurrentMana(), _manaComponent.GetMaxMana());
            else Show(false);
        }

        protected override void ClearTarget()
        {
            if (_manaEventHandler != null) _manaEventHandler.UnregisterOnManaChanged(OnManaChanged);
            _manaEventHandler = null;
            _manaComponent = null;
        }

        private void OnDestroy()
        {
            ClearTarget();
        }

        private void OnManaChanged(ManaChangedData data)
        {
            UpdateValue(data.NewMana, data.MaxMana);
        }
    }
}