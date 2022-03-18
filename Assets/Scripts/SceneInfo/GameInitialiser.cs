using SymptomsPlease.Events;
using UnityEngine;

namespace SymptomsPlease
{
    public class GameInitialiser : MonoBehaviour
    {
        public static GameEvent OnGameLoaded = new GameEvent();
    }
}