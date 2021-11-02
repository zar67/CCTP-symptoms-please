using SymptomsPlease.Events;
using UnityEngine;

namespace SymptomsPlease.ScriptableObjects.Variables
{
    public class VariableType<T> : GameScriptableObject
    {
        [SerializeField] protected T m_value;

        public GameEvent OnValueChanged = new GameEvent();

        public T Value
        {
            get => m_value;
            set
            {
                m_value = value;
                OnValueChanged.Invoke();
            }
        }
    }
}
