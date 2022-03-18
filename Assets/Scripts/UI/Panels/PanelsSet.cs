using SymptomsPlease.ScriptableObjects.Variables;
using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.UI.Panels
{
    [CreateAssetMenu(menuName = "SymptomsPlease/UI/Panels/Panels Set")]
    public class PanelsSet : RuntimeSet<Panel>
    {
        private Dictionary<string, Panel> m_panelsMap = new Dictionary<string, Panel>();

        public Panel GetPanelsObject(string panelID)
        {
            if (m_panelsMap.ContainsKey(panelID))
            {
                return m_panelsMap[panelID];
            }

            return null;
        }

        public override void Add(Panel thing)
        {
            if (!m_panelsMap.ContainsKey(thing.PanelID))
            {
                m_panelsMap.Add(thing.PanelID, thing);
                Items.Add(thing);
            }
        }

        public override void Remove(Panel thing)
        {
            if (m_panelsMap.ContainsKey(thing.PanelID))
            {
                m_panelsMap.Remove(thing.PanelID);
                Items.Remove(thing);
            }
        }

        public override void Reset()
        {
            m_panelsMap = new Dictionary<string, Panel>();
            Items = new List<Panel>();
        }
    }
}