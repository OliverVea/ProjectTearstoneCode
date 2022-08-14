using System.Collections.Generic;
using Mirror;
using MyRpg.Core.Components;
using UnityEngine;

namespace MyRpg.Components.Abilities
{
    public class InteractableKiller : NetworkBehaviour, IInteractableComponent
    {
        [SerializeField] private List<GameObject> targets;

        [Server]
        public void ServerInteract()
        {
            foreach (var target in targets)
                KillTarget(target);
        }

        private static void KillTarget(GameObject target)
        {
            var healthComponent = target.GetComponent<IHealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ServerTakeDamage(null, float.MaxValue);
                return;
            }

            // Probably not necessary...
            var deathComponent = target.GetComponent<IDeathComponent>();
            if (deathComponent != null)
            {
                deathComponent.ServerDeath();
                return;
            }
        }
    }
}