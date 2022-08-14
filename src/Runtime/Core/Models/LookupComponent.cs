using System.Collections.Generic;
using MyRpg.Core.Components;
using UnityEngine;

namespace MyRpg.Core.Models
{
    public class LookupComponent : MonoBehaviour
    {
        [SerializeField] private Effect[] effects;
        [SerializeField] private Spell[] spells;
        [SerializeField] private GameObject[] characters;

        private static readonly Dictionary<string, Effect> EffectLookup = new Dictionary<string, Effect>();
        private static readonly Dictionary<string, Spell> SpellLookup = new Dictionary<string, Spell>();
        private static readonly Dictionary<Class, GameObject> CharacterPrefabLookup = new Dictionary<Class, GameObject>();

        private void OnEnable()
        {
            foreach (var effect in effects) EffectLookup[effect.Id] = effect;
            
            foreach (var spell in spells) SpellLookup[spell.Id] = spell;
            
            foreach (var character in characters)
            {
                var characterClass = character.GetComponent<IStatusComponent>().GetClass();
                CharacterPrefabLookup[characterClass] = character;
            }
        }

        public static Effect GetEffect(string effectId)
        {
            EffectLookup.TryGetValue(effectId, out var effect);
            return effect;
        }

        public static Spell GetSpell(string spellId)
        {
            SpellLookup.TryGetValue(spellId, out var spell);
            return spell;
        }
        
        public static GameObject GetCharacterPrefab(Class characterClass) 
        {
            CharacterPrefabLookup.TryGetValue(characterClass, out var characterGameObject);
            return characterGameObject;
        }
    }
}