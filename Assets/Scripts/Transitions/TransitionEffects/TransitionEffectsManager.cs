using SymptomsPlease.Events;
using SymptomsPlease.Transitions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.SceneManagement
{
    public class TransitionEffectsManager : MonoBehaviour, IEventDependancy
    {
        [Serializable]
        public struct TransitionEffectData
        {
            public string Type;
            public Animator Animator;
            public string InTriggerName;
            public string OutTriggerName;
        }

        [SerializeField] private TransitionEffectData[] m_transitions = default;

        private const string DEPENDANCY_COMPLETE_TRANSITION_OUT = "TRANSITION_OUT_COMPLETED";
        private const string DEPENDANCY_COMPLETE_TRANSITION_IN = "TRANSITION_IN_COMPLETED";

        private float m_startTime = 0;
        private float m_totalDuration = 0;

        private Dictionary<string, TransitionEffectData> m_transitionEffectsDictionary;

        private void Awake()
        {
            m_transitionEffectsDictionary = new Dictionary<string, TransitionEffectData>();
            foreach (TransitionEffectData data in m_transitions)
            {
                m_transitionEffectsDictionary.Add(data.Type, data);
            }
        }

        private void OnEnable()
        {
            TransitionManager.OnPreloadBegin.Subscribe(Transition);
            TransitionManager.OnLoadComplete.Subscribe(Transition);
        }

        private void OnDisable()
        {
            TransitionManager.OnPreloadBegin.UnSubscribe(Transition);
            TransitionManager.OnPreloadComplete.CompleteDependancy(DEPENDANCY_COMPLETE_TRANSITION_OUT);

            TransitionManager.OnLoadComplete.UnSubscribe(Transition);
            TransitionManager.OnPostLoadComplete.CompleteDependancy(DEPENDANCY_COMPLETE_TRANSITION_IN);
        }

        public void Transition(TransitionData data)
        {
            if (data.State == TransitionData.TransitionState.OUT)
            {
                TransitionManager.OnPreloadComplete.AddDependancy(DEPENDANCY_COMPLETE_TRANSITION_OUT, this);
            }
            else
            {
                TransitionManager.OnPostLoadComplete.AddDependancy(DEPENDANCY_COMPLETE_TRANSITION_IN, this);
            }

            if (data.TransitionType != string.Empty)
            {
                StartCoroutine(RunTransition(data.TransitionType, data.State));
                return;
            }

            if (data.State == TransitionData.TransitionState.OUT)
            {
                TransitionManager.OnPreloadComplete.CompleteDependancy(DEPENDANCY_COMPLETE_TRANSITION_OUT);
            }
            else
            {
                TransitionManager.OnPostLoadComplete.CompleteDependancy(DEPENDANCY_COMPLETE_TRANSITION_IN);
            }
        }

        private IEnumerator RunTransition(string transition, TransitionData.TransitionState state)
        {
            m_startTime = Time.time;

            TransitionEffectData data = m_transitionEffectsDictionary[transition];

            if (state == TransitionData.TransitionState.OUT)
            {
                data.Animator.SetTrigger(data.OutTriggerName);
                m_totalDuration = data.Animator.GetCurrentAnimatorStateInfo(0).length * data.Animator.speed;
                yield return new WaitForSeconds(data.Animator.GetCurrentAnimatorStateInfo(0).length * data.Animator.speed);
                TransitionManager.OnPreloadComplete.CompleteDependancy(DEPENDANCY_COMPLETE_TRANSITION_OUT);
            }
            else // state == TransitionData.TransitionState.IN
            {
                data.Animator.SetTrigger(data.InTriggerName);
                m_totalDuration = data.Animator.GetCurrentAnimatorStateInfo(0).length * data.Animator.speed;
                yield return new WaitForSeconds(data.Animator.GetCurrentAnimatorStateInfo(0).length * data.Animator.speed);

                TransitionManager.OnPostLoadComplete.CompleteDependancy(DEPENDANCY_COMPLETE_TRANSITION_IN);
            }

            m_startTime = 0;
            m_totalDuration = 0;
        }

        public float PercentageComplete(string identifier)
        {
            if (identifier == DEPENDANCY_COMPLETE_TRANSITION_OUT || identifier == DEPENDANCY_COMPLETE_TRANSITION_IN)
            {
                if (m_startTime == 0 || m_totalDuration == 0)
                {
                    return 0;
                }

                return Time.time / (m_startTime + m_totalDuration) * 100;
            }

            return 0;
        }
    }
}