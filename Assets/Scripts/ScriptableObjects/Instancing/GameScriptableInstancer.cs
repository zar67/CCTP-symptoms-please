using SymptomsPlease.Utilities.Attributes;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SymptomsPlease.ScriptableObjects.Instancing
{
    public abstract class GameScriptableInstancer<T> : MonoBehaviour where T : GameScriptableObject
    {
        [Serializable]
        public class InstantiateEvent : UnityEvent<T>
        {
        }

        [SerializeField] public InstantiateEvent OnInstantiateEvent = null;
        [SerializeField] protected T m_scriptableToInstantiate = default;
        [SerializeField, ReadOnly] protected T m_instancedObject = default;

        private void Awake()
        {
            Instantiate();
        }

        public virtual void Instantiate()
        {
            m_instancedObject = Instantiate(m_scriptableToInstantiate);
            OnInstantiateEvent?.Invoke(m_instancedObject);
        }
    }
}