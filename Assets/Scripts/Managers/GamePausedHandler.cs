using UnityEngine;

namespace SymptomsPlease.Managers
{
    public class GamePausedHandler : MonoBehaviour
    {
        public static bool IsGamePaused { get; private set; }

        public static void PauseGame(bool paused)
        {
            IsGamePaused = paused;
            Time.timeScale = paused ? 0 : 1;
        }
    }
}