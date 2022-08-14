using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRpg.Core.Constants
{
    public static class KeyCodeNames
    {
        private static Dictionary<KeyCode, string> _keyNames;
        
        public static string GetKeyName(KeyCode key)
        {
            if (_keyNames == null) InitializeKeyNames();
            return _keyNames?.FirstOrDefault(x => x.Key == key).Value;
        }

        private static void InitializeKeyNames()
        {
            _keyNames = new Dictionary<KeyCode, string>();
            
            foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
                _keyNames[k] = k.ToString();
            
            for (int i = 0; i < 10; i++){
                _keyNames[(KeyCode)((int)KeyCode.Alpha0+i)] = i.ToString();
                _keyNames[(KeyCode)((int)KeyCode.Keypad0+i)] = i.ToString();
            }
            
            _keyNames[KeyCode.Comma] = ",";
            _keyNames[KeyCode.Escape] = "Esc";
        }
    }
}