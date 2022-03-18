using UnityEngine;

namespace SymptomsPlease.UI.TabbedMenu
{
    public class TabMenu : MonoBehaviour
    {
        [SerializeField] private TabObject[] m_tabObjects = default;

        private TabObject m_selectedObject = default;

        private void Start()
        {
            m_selectedObject = m_tabObjects[0];

            foreach (TabObject tab in m_tabObjects)
            {
                tab.Initialise(this);
                tab.Deselect();
            }

            m_selectedObject.Select();
        }

        public void ChangeSelection(TabObject obj)
        {
            m_selectedObject.Deselect();
            m_selectedObject = obj;
            m_selectedObject.Select();
        }
    }
}