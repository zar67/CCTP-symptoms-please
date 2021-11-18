using System;
using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.Debugger
{
    public class OptionsPanel : MonoBehaviour
    {
        public struct OptionsCategoryData
        {
            public OptionsCategory OptionsCategory;
            public List<Option> Options;

            public OptionsCategoryData(OptionsCategory category)
            {
                OptionsCategory = category; 
                Options = new List<Option>();
            }
        }

        private const string DEFAULT_OPTION_NAME = "Default";

        [SerializeField] private OptionsCategory m_optionsCategoryPrefab = default;
        [SerializeField] private ActionOption m_actionOptionPrefab = default;
        [SerializeField] private NumericalOption m_numericalOptionPrefab = default;
        [SerializeField] private ToggleOption m_toggleOptionPrefab = default;

        [SerializeField] private Transform m_categoryScrollParent = default;

        private Dictionary<string, OptionsCategoryData> m_optionsCategoryDictionary = new Dictionary<string, OptionsCategoryData>();

        public ActionOption AddActionOption(Action action, string optionsCategory = "")
        {
            var newOption = (ActionOption)AddOption(m_actionOptionPrefab, optionsCategory);

            newOption.OnAction += action;
            newOption.SetOptionText(action.Method.Name);

            return newOption;
        }

        public ToggleOption AddToggleOption(Action<bool> action, string optionsCategory = "")
        {
            var newOption = (ToggleOption)AddOption(m_toggleOptionPrefab, optionsCategory);

            newOption.OnToggled += action;
            newOption.SetOptionText(action.Method.Name);

            return newOption;
        }

        public NumericalOption AddNumericalOption(Action<int> action, int defaultValue, string optionsCategory = "")
        {
            var newOption = (NumericalOption)AddOption(m_numericalOptionPrefab, optionsCategory);

            newOption.OnValueChanged += action;
            newOption.SetDefaultValue(defaultValue);
            newOption.SetOptionText(action.Method.Name);

            return newOption;
        }

        public void RemoveOption(Option option, string optionsCategory = "")
        {
            if (optionsCategory == "")
            {
                foreach (KeyValuePair<string, OptionsCategoryData> value in m_optionsCategoryDictionary)
                {
                    if (value.Value.Options.Contains(option))
                    {
                        value.Value.Options.Remove(option);
                        Destroy(option.gameObject);

                        if (value.Value.Options.Count == 0)
                        {
                            Destroy(value.Value.OptionsCategory.gameObject);
                            m_optionsCategoryDictionary.Remove(value.Key);
                        }

                        break;
                    }
                }
            }
            else
            {
                m_optionsCategoryDictionary[optionsCategory].Options.Remove(option);
                Destroy(option);

                if (m_optionsCategoryDictionary[optionsCategory].Options.Count == 0)
                {
                    Destroy(m_optionsCategoryDictionary[optionsCategory].OptionsCategory.gameObject);
                    m_optionsCategoryDictionary.Remove(optionsCategory);
                }
            }
        }

        private Option AddOption(Option option, string category)
        {
            Option newOption;
            if (category == "")
            {
                if (!m_optionsCategoryDictionary.ContainsKey(DEFAULT_OPTION_NAME))
                {
                    AddCategory(DEFAULT_OPTION_NAME);
                }

                newOption = Instantiate(option, m_optionsCategoryDictionary[DEFAULT_OPTION_NAME].OptionsCategory.CategoryParent);
                m_optionsCategoryDictionary[DEFAULT_OPTION_NAME].Options.Add(newOption);
            }
            else
            {
                if (!m_optionsCategoryDictionary.ContainsKey(category))
                {
                    AddCategory(category);
                }

                newOption = Instantiate(option, m_optionsCategoryDictionary[category].OptionsCategory.CategoryParent);
                m_optionsCategoryDictionary[category].Options.Add(newOption);
            }

            return newOption;
        }

        private void AddCategory(string categoryName)
        {
            OptionsCategory category = Instantiate(m_optionsCategoryPrefab, m_categoryScrollParent);
            category.SetTitleText(categoryName);
            m_optionsCategoryDictionary.Add(categoryName, new OptionsCategoryData(category));
        }
    }
}