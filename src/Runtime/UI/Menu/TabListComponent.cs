using System.Collections.Generic;
using UnityEngine;

namespace MyRpg.UI.Menu
{
    public class TabListComponent : MonoBehaviour
    {
        [SerializeField] private int selectedTab;
        [SerializeField] private List<GameObject> tabs;

        private void Start()
        {
            UpdateTabs();
        }

        // Called via. events.
        public void SetSelectedTab(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= tabs.Count) return;
            
            selectedTab = tabIndex;
            UpdateTabs();
        }

        private void UpdateTabs()
        {
            for (int i = 0; i < tabs.Count; i++) tabs[i].SetActive(i == selectedTab);
        }
    }
}