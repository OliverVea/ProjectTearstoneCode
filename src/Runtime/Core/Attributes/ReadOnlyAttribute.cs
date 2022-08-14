using System;
using UnityEngine;

namespace MyRpg.Core.Attributes
{
    
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
     
    }
}