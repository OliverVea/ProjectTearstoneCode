﻿#if UNITY_EDITOR
using MyRpg.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace MyRpg.Inspector
{
    
     [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
     public class ReadOnlyDrawer : PropertyDrawer
     {
         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
         {
             return EditorGUI.GetPropertyHeight(property, label, true);
         }
     
         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
         {
             GUI.enabled = false;
             EditorGUI.PropertyField(position, property, label, true);
             GUI.enabled = true;
         }
     }
}
#endif