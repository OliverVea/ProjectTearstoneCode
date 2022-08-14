using MyRpg.Core.Components;
using MyRpg.Core.Events;
using UnityEngine;

namespace MyRpg.UI.UnitFrame.Bars
{
    public abstract class AbstractHealthBar : Bar
    {
        private IHealthEventHandler _healthEventHandler;
        private IHealthComponent _healthComponent;

        protected override void SetTarget(GameObject target)
        {
            ClearTarget();

            if (target == null) return;
            
            _healthEventHandler = target.GetComponent<IHealthEventHandler>();
            if (_healthEventHandler != null) _healthEventHandler.RegisterOnHealthChanged(OnHealthChanged);
            
            _healthComponent = target.GetComponent<IHealthComponent>();
            if (_healthComponent != null) UpdateValue(_healthComponent.GetCurrentHealth(), _healthComponent.GetMaxHealth());
        }

        protected override void ClearTarget()
        {
            if (_healthEventHandler != null) _healthEventHandler.UnregisterOnHealthChanged(OnHealthChanged);
            _healthEventHandler = null;
            _healthComponent = null;
        }

        private void OnDestroy()
        {
            ClearTarget();
        }

        private void OnHealthChanged(HealthChangedData data)
        {
            UpdateValue(data.NewHealth, data.MaxHealth);
        }
    }
}