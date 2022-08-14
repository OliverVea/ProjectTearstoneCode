#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace MyRpg.UI.Menu
{
    public class ExitButtonComponent : MonoBehaviour
    {
        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}