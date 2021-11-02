using UnityEngine;
using UnityEngine.UI;

namespace SymptomsPlease.UI.TabbedMenu
{
    public class TabObject : MonoBehaviour
    {
        [SerializeField] private Button m_tabButton = default;
        [SerializeField] private Image m_backgroundImage = default;
        [SerializeField] private GameObject m_objectContent = default;

        [SerializeField] private Color m_selectedColour = default;
        [SerializeField] private Color m_deselectedColour = default;

        private TabMenu m_tabMenu = default;

        public void Initialise(TabMenu menu)
        {
            m_tabMenu = menu;
        }

        public void Select()
        {
            m_objectContent.SetActive(true);
            m_backgroundImage.color = m_selectedColour;
        }

        public void Deselect()
        {
            m_objectContent.SetActive(false);
            m_backgroundImage.color = m_deselectedColour;
        }

        private void OnEnable()
        {
            m_tabButton.onClick.AddListener(HandleSelection);
        }

        private void OnDisable()
        {
            m_tabButton.onClick.RemoveListener(HandleSelection);
        }

        private void HandleSelection()
        {
            m_tabMenu.ChangeSelection(this);
        }
    }
}