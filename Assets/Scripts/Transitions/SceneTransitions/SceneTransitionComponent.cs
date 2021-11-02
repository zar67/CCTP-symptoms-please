using SymptomsPlease.SceneManagement;
using UnityEngine;

namespace SymptomsPlease.Transitions.Scenes
{
    public class SceneTransitionComponent : MonoBehaviour
    {
        [SerializeField] private SceneData m_sceneData = default;
        [SerializeField] private SceneTransitionData m_transitionData = default;

        public void Transition()
        {
            m_transitionData.State = TransitionData.TransitionState.OUT;
            m_sceneData.TransitionToScene(m_transitionData);
        }
    }
}