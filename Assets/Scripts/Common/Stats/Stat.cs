using SymptomsPlease.Events;
using SymptomsPlease.Utilities.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.Common.Stats
{
    [Serializable]
    public class Stat
    {
        public virtual float Value
        {
            get
            {
                if (m_hasChanged || BaseValue != m_lastBaseValue)
                {
                    m_lastBaseValue = BaseValue;
                    m_mostRecentValue = CalculateFinalValue();
                    m_hasChanged = false;
                }

                return m_mostRecentValue;
            }
        }

        public float BaseValue;

        public List<StatModifier> StatModifiers => m_modifiers;
        public GameEvent OnValueChanged;

        protected bool m_hasChanged = true;
        protected float m_mostRecentValue;
        [SerializeField, ReadOnly]
        protected float m_lastBaseValue = float.MinValue;

        protected List<StatModifier> m_modifiers;

        public Stat()
        {
            m_modifiers = new List<StatModifier>();
        }

        public Stat(float baseValue)
        {
            BaseValue = baseValue;
            m_modifiers = new List<StatModifier>();
        }

        /// <summary>
        /// Removes all modifiers from the stat
        /// </summary>
        public void ResetModifiers()
        {
            m_modifiers = new List<StatModifier>();
            OnValueChanged.Invoke();
            m_hasChanged = true;
        }

        /// <summary>
        /// Adds a given modifier to the list
        /// </summary>
        /// <param name="modifier">The modifer to add</param>
        public virtual void AddModifier(StatModifier modifier)
        {
            m_hasChanged = true;
            m_modifiers.Add(modifier);
            m_modifiers.Sort(CompareModifierPriority);
            OnValueChanged.Invoke();
        }

        /// <summary>
        /// Removes a given modifier from the modifiers list
        /// </summary>
        /// <param name="modifier">The modifier to remove</param>
        /// <returns>True if the modifier was successfully removed</returns>
        public virtual bool RemoveModifier(StatModifier modifier)
        {
            if (m_modifiers.Remove(modifier))
            {
                m_hasChanged = true;
                OnValueChanged.Invoke();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes modifiers originating from a specified source, used instead of storing a reference to StatModifiers
        /// Will usually pass 'this' as the parameter
        /// </summary>
        /// <param name="source"></param>
        /// <returns>True if at least one modifier was removed</returns>
        public virtual bool RemoveModifiersFromSource(object source)
        {
            bool modifierRemoved = false;
            for (int i = m_modifiers.Count - 1; i >= 0; i--)
            {
                if (m_modifiers[i].Source == source)
                {
                    m_hasChanged = true;
                    modifierRemoved = true;
                    m_modifiers.RemoveAt(i);
                    OnValueChanged.Invoke();
                }
            }

            return modifierRemoved;
        }

        protected virtual int CompareModifierPriority(StatModifier first, StatModifier second)
        {
            if (first.Priority < second.Priority)
            {
                return -1;
            }
            else if (first.Priority > second.Priority)
            {
                return 1;
            }

            return 0;
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float totalPercentageAdditive = 0;

            for (int i = 0; i < m_modifiers.Count; i++)
            {
                StatModifier modifier = m_modifiers[i];
                switch (modifier.Type)
                {
                    case StatModificationType.FLAT:
                    {
                        finalValue += modifier.Value;
                        break;
                    }
                    case StatModificationType.PERCENTAGE_MULTIPLY:
                    {
                        finalValue *= 1 + modifier.Value;
                        break;
                    }
                    case StatModificationType.PERCENTAGE_ADDITIVE:
                    {
                        totalPercentageAdditive += modifier.Value;

                        if (i + 1 >= m_modifiers.Count || m_modifiers[i + 1].Type != StatModificationType.PERCENTAGE_ADDITIVE)
                        {
                            finalValue *= 1 + totalPercentageAdditive;
                            totalPercentageAdditive = 0;
                        }

                        break;
                    }
                }
            }

            return (float)Math.Round(finalValue, 4);
        }
    }
}
