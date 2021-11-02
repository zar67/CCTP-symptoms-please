using SymptomsPlease.ScriptableObjects.Variables;
using UnityEngine;

namespace SymptomsPlease.Managers
{
    public class GamePausedHandler : MonoBehaviour
    {
        [SerializeField] private BoolVariable m_gamePaused = default;

        private void OnEnable()
        {
            m_gamePaused.OnValueChanged.Subscribe(HandleGamePausedChange);
        }

        private void OnDisable()
        {
            m_gamePaused.OnValueChanged.UnSubscribe(HandleGamePausedChange);
        }

        private void HandleGamePausedChange()
        {
            Time.timeScale = m_gamePaused.Value ? 0 : 1;
        }
    }
}
