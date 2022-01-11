using SymptomsPlease.Events;
using System.Collections;
using UnityEngine;

namespace SymptomsPlease.Common.Timer
{
    public class Timer
    {
        public GameEvent OnComplete = new GameEvent();

        private float m_timerDuration;
        private float m_timePassed;
        private MonoBehaviour m_behaviour;

        public Timer(MonoBehaviour behaviour, float duration)
        {
            m_behaviour = behaviour;
            m_timerDuration = duration;
        }

        public void SetDuration(float duration)
        {
            m_timerDuration = duration;
            m_timePassed = 0;
        }

        public void Start()
        {
            m_behaviour.StartCoroutine(Tick());
        }

        public void Pause()
        {
            m_behaviour.StopCoroutine(Tick());
        }

        public void Stop()
        {
            m_behaviour.StopCoroutine(Tick());
            m_timePassed = 0;
        }

        private IEnumerator Tick()
        {
            while (m_timePassed < m_timerDuration)
            {
                m_timePassed += Time.deltaTime;
                yield return null;
            }

            OnComplete.Invoke();
            yield return null;
        }
    }
}