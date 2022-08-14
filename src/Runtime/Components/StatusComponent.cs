using System.Linq;
using Mirror;
using MyRpg.Core.Attributes;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using MyRpg.Core.Modifiers;
using UnityEngine;

namespace MyRpg.Components
{
    public class StatusComponent : NetworkBehaviour, IStatusComponent
    {
        [SerializeField] [SyncVar] [ReadOnly] private Character character;
        [SerializeField] [SyncVar] [ReadOnly] private string characterName;
        [SerializeField] [SyncVar] [ReadOnly] private Class characterClass;
        [SerializeField] private Faction faction;
        [SerializeField] private float movementSpeed = 2f;
        [SerializeField] private Sprite portraitImage;
        [SerializeField] private float damageReduction;
        [SerializeField] private float killExperience;
        
        private IDeathProvider[] _deathProviders;
        private ICharacterStatusProvider[] _statusProviders;
        private IEffectsComponent _effectsComponent;

        private void Awake()
        {
            _effectsComponent = GetComponent<IEffectsComponent>();
            _deathProviders = GetComponents<IDeathProvider>();
            _statusProviders = GetComponents<ICharacterStatusProvider>();
        }

        public bool IsDead()
        {
            return _deathProviders.Any(x => x.Dead);
        }

        public Faction GetFaction()
        {
            return faction;
        }

        public bool CanMove()
        {
            return _statusProviders.All(x => x.GetCharacterStatus().CanMove);
        }

        public bool CanAttack()
        {
            return _statusProviders.All(x => x.GetCharacterStatus().CanAttack);
        }

        public float GetAttackRange()
        {
            return ConstantValues.MeleeAttackRange;
        }

        public float GetMovementSpeed()
        {
            return movementSpeed * _effectsComponent.GetMovementSpeedModifier();
        }

        public Sprite GetPortrait()
        {
            return portraitImage;
        }

        public string GetName()
        {
            return character.IsValid ? character.characterName : characterName;
        }

        public float GetDamageReduction()
        {
            return damageReduction;
        }

        public Class GetClass()
        {
            return characterClass;
        }

        public string GetCharacterId()
        {
            return character.characterId;
        }

        public float GetHealthForClass(Class characterCharacterClass)
        {
            return characterCharacterClass switch
            {
                Class.Mage => 80f,
                Class.Priest => 100f,
                Class.Warrior => 150f,
                _ => 1f
            };
        }

        public float GetManaForClass(Class characterCharacterClass)
        {
            return characterCharacterClass switch
            {
                Class.Mage => 100f,
                Class.Priest => 100f,
                Class.Warrior => 0f,
                _ => 0f
            };
        }

        public float GetKillExperience()
        {
            return killExperience;
        }

        public void ServerInitialize(Character character)
        {
            this.character = character;
        }
    }
}