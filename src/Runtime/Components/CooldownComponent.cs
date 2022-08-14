using System.Linq;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Components
{
    public class CooldownComponent : NetworkBehaviour, ICooldownComponent
    {
        private readonly SyncDictionary<string, Cooldown> _spellsOnCooldown = new SyncDictionary<string, Cooldown>();

        [ServerCallback]
        private void Update()
        {
            ServerUpdateCooldowns();
            
            var expiredSpellIds = _spellsOnCooldown
                .Where(x => x.Value.RemainingCooldown <= 0)
                .Select(x => x.Key)
                .ToArray();

            foreach (var spellId in expiredSpellIds) _spellsOnCooldown.Remove(spellId);
        }

        [Server]
        private void ServerUpdateCooldowns()
        {
            var spellIds = _spellsOnCooldown.Keys.ToArray();

            foreach (var spellId in spellIds)
            {
                var timeSinceCasted = _spellsOnCooldown[spellId].timeSinceCasted + Time.deltaTime;
                
                _spellsOnCooldown[spellId] = new Cooldown
                {
                    spellId = spellId,
                    timeSinceCasted = timeSinceCasted
                };
            }
        }

        [Server]
        public void ServerAddToCooldown(string spellId)
        {
            _spellsOnCooldown[spellId] = new Cooldown
            {
                spellId = spellId,
                timeSinceCasted = 0
            };
        }

        public Cooldown GetCooldown(string spellId)
            => _spellsOnCooldown.FirstOrDefault(x => x.Key == spellId).Value;

        public bool IsOnCooldown(string spellId)
            => GetCooldown(spellId) != null;

        public void ServerInitialize(Character character)
        {
            
        }
    }
}