using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Characters
{
    public static class CharacterLoader
    {
        private static readonly string CharacterDirectory = Application.persistentDataPath + "/characters";
        private static string GetFileName(Character character) => character.characterId + ".json";
        
        


        public static IEnumerable<Character> LoadCharacters()
        {
            EnsurePathExists();
            
            var directoryInfo = new DirectoryInfo(CharacterDirectory);
            return directoryInfo.EnumerateFiles()
                .Select(x => x.ToString())
                .Where(FileIsValid)
                .Select(File.ReadAllText)
                .Select(JsonUtility.FromJson<CharacterEntity>)
                .Select(Map);
        }

        private static void EnsurePathExists()
        {
            var directoryInfo = new DirectoryInfo(CharacterDirectory);
            if (directoryInfo.Exists) return;
            
            directoryInfo.Create();
        }

        private static bool FileIsValid(string filePath)
            => File.Exists(filePath) && 
               filePath.EndsWith(".json");

        private static Character Map(CharacterEntity entity)
        {
            if (!Enum.TryParse<Class>(entity.characterClass, out var characterClass)) 
                return new Character();

            var spellBindings = Map(entity.spellKeys, entity.spellIds);
            
            return new Character
            {
                characterClass = characterClass,
                characterId = entity.characterId,
                characterName = entity.characterName,
                spellBindings = spellBindings,
            };
        }

        private static KeyBinding[] Map(IEnumerable<string> spellKeys, IEnumerable<string> spellIds)
            => spellKeys.Zip(spellIds, Map).ToArray();

        private static KeyBinding Map(string key, string spellId)
        {
            var success = Enum.TryParse<KeyCode>(key, out var keyCode);
            if (!success)
                throw new InvalidEnumArgumentException($"Key {key} is an invalid KeyCode.");

            return new KeyBinding
            {   
                key = keyCode,
                spellId = spellId
            };
        }

        
        
        
        public static void SaveCharacter(Character character)
        {
            EnsurePathExists();
            
            var filePath = CharacterDirectory + "/" + GetFileName(character);
            var entity = Map(character);
            var jsonEncodedCharacter = JsonUtility.ToJson(entity, true);
            File.WriteAllText(filePath, jsonEncodedCharacter);
        }

        private static CharacterEntity Map(Character character)
        {
            return new CharacterEntity
            {
                characterClass = character.characterClass.ToString(),
                characterId = character.characterId,
                characterName = character.characterName,
                spellKeys = character.spellBindings.Select(x => x.key.ToString()).ToArray(),
                spellIds = character.spellBindings.Select(x => x.spellId).ToArray()
            };
        }
    }
}