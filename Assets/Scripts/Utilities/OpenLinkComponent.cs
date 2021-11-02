using UnityEngine;

namespace SymptomsPlease.Utilities
{
    public class OpenLinkComponent : MonoBehaviour
    {
        [SerializeField] private string m_link = "";

        public void OpenLink()
        {
            Application.OpenURL(m_link);
        }
    }
}