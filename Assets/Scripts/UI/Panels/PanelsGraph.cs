using System;
using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.UI.Panels
{
    [Serializable]
    public class PanelsGraph
    {
        [Serializable]
        public struct Node
        {
            public string PanelID;
            public List<string> Transitions;
        }

        [SerializeField] private List<Node> m_nodes = new List<Node>();

        private Dictionary<string, Node> m_nodesMap = new Dictionary<string, Node>();

        public void Initialise()
        {
            m_nodesMap = new Dictionary<string, Node>();
            foreach (Node node in m_nodes)
            {
                m_nodesMap.Add(node.PanelID, node);
            }
        }

        public List<string> FindPath(string startPanel, string endPanel)
        {
            var priorityQueue = new List<string>();
            var distances = new Dictionary<string, int>();
            var visited = new Dictionary<string, bool>();
            var previous = new Dictionary<string, string>();

            foreach (Node node in m_nodes)
            {
                distances.Add(node.PanelID, int.MaxValue);
                visited.Add(node.PanelID, false);
            }

            distances[startPanel] = 0;
            priorityQueue.Add(startPanel);
            previous.Add(startPanel, string.Empty);

            while (priorityQueue.Count != 0)
            {
                foreach (string connection in GetNode(priorityQueue[0]).Transitions)
                {
                    int newDistance = distances[priorityQueue[0]] + 1;
                    int currentDistance = distances[connection];

                    if (newDistance < currentDistance)
                    {
                        distances[connection] = newDistance;
                        previous[connection] = priorityQueue[0];
                    }

                    if (!visited[connection] && !priorityQueue.Contains(connection))
                    {
                        priorityQueue.Add(connection);
                    }
                }

                visited[priorityQueue[0]] = true;
                priorityQueue.RemoveAt(0);
            }

            if (previous.ContainsKey(endPanel))
            {
                var path = new List<string>
                {
                    endPanel
                };

                string currentPanel = endPanel;

                while (currentPanel != startPanel)
                {
                    path.Add(previous[currentPanel]);
                    currentPanel = previous[currentPanel];
                }

                path.Reverse();

                return path;
            }

            return new List<string>();
        }

        public Node GetNode(string panel)
        {
            foreach (Node node in m_nodes)
            {
                if (node.PanelID == panel)
                {
                    return node;
                }
            }

            return default;
        }

        public bool HasTransition(string startPanel, string endPanel)
        {
            if (m_nodesMap.ContainsKey(startPanel))
            {
                return m_nodesMap[startPanel].Transitions.Contains(endPanel);
            }

            return false;
        }
    }
}