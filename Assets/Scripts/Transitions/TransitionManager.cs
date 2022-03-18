using SymptomsPlease.Debugging.Logging;
using System.Collections;
using UnityEngine;

namespace SymptomsPlease.Transitions
{
    public class TransitionManager : MonoBehaviour
    {
        public static TransitionEvent OnTransitionBegin = new TransitionEvent();
        public static TransitionEvent OnPreloadBegin = new TransitionEvent();
        public static TransitionEvent OnPreloadComplete = new TransitionEvent();
        public static TransitionEvent OnLoadComplete = new TransitionEvent();
        public static TransitionEvent OnPostLoadComplete = new TransitionEvent();
        public static TransitionEvent OnTransitionComplete = new TransitionEvent();

        protected bool m_isTransitioning;

        private void OnEnable()
        {
            OnTransitionBegin.Subscribe(BeginTransition);
        }

        private void OnDisable()
        {
            OnTransitionBegin.UnSubscribe(BeginTransition);
        }

        private void BeginTransition(TransitionData data)
        {
            if (m_isTransitioning && !data.ForceTransition)
            {
                CustomLogger.Debug(LoggingChannels.Transitions, "Transition already in progress.");
                return;
            }

            m_isTransitioning = true;
            data.State = TransitionData.TransitionState.OUT;
            OnPreloadBegin.Invoke(data);
            StartCoroutine(PreloadTransition(data));
        }

        private IEnumerator PreloadTransition(TransitionData data)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => OnPreloadComplete.DependanciesComplete() == true);
            OnPreloadComplete.Invoke(data);
            StartCoroutine(LoadTransition(data));
        }

        private IEnumerator LoadTransition(TransitionData data)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => OnLoadComplete.DependanciesComplete() == true);
            data.State = TransitionData.TransitionState.IN;
            OnLoadComplete.Invoke(data);
            StartCoroutine(PostloadTransition(data));
        }

        private IEnumerator PostloadTransition(TransitionData data)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => OnPostLoadComplete.DependanciesComplete() == true);

            data.State = TransitionData.TransitionState.IN;
            OnPostLoadComplete.Invoke(data);
            StartCoroutine(TransitionComplete(data));
        }

        private IEnumerator TransitionComplete(TransitionData data)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => OnTransitionComplete.DependanciesComplete() == true);
            m_isTransitioning = false;
            OnTransitionComplete.Invoke(data);
        }
    }
}
