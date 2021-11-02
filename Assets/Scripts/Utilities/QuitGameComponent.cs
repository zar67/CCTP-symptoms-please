using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.Utilities
{
    public class QuitGameComponent : MonoBehaviour
    {
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}