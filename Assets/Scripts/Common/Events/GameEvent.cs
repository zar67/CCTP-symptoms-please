using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SymptomsPlease.Events
{
    /// <summary>
    /// An object to handle an event.
    /// </summary>
    public class GameEvent
    {
        /// <summary>
        /// Event dependancy that needs to be completed before the event can be invoked.
        /// </summary>
        public class Dependancy
        {
            /// <summary>
            /// Unique identified for the dependancy.
            /// </summary>
            public string Identifier;

            /// <summary>
            /// Object that created the dependancy.
            /// </summary>
            public IEventDependancy Sender;

            /// <summary>
            /// Bool whether the dependancy has been completed or not.
            /// </summary>
            public bool Complete;

            /// <summary>
            /// Initializes a new instance of the <see cref="Dependancy"/> class.
            /// </summary>
            /// <param name="identifier">Identifier of the dependancy.</param>
            /// <param name="sender">Object that has created the dependancy.</param>
            public Dependancy(string identifier, IEventDependancy sender)
            {
                Identifier = identifier;
                Sender = sender;
                Complete = false;
            }
        }

        /// <summary>
        /// Current dependancies of the event.
        /// </summary>
        protected Dictionary<string, Dependancy> m_dependancies = new Dictionary<string, Dependancy>();
        private event Action Event = null;

        /// <summary>
        /// Invokes the event.
        /// </summary>
        /// <param name="sender">Object calling the event.</param>
        public void Invoke()
        {
            if (DependanciesComplete())
            {
                Event?.Invoke();
                ResetDependancies();
            }
        }

        /// <summary>
        /// Adds a new action to call when the event is invoked.
        /// </summary>
        /// <param name="action">Action to add.</param>
        public void Subscribe(Action action)
        {
            Event += action;
        }

        /// <summary>
        /// Removes an action from the invoke list.
        /// </summary>
        /// <param name="action">Action to remove.</param>
        public void UnSubscribe(Action action)
        {
            Event -= action;
        }

        /// <summary>
        /// Gets the current dependancies of the event.
        /// </summary>
        /// <returns>IEnumerable of the dependancies.</returns>
        public IEnumerable<Dependancy> Dependancies()
        {
            foreach (KeyValuePair<string, Dependancy> dependancy in m_dependancies)
            {
                yield return dependancy.Value;
            }
        }

        /// <summary>
        /// Checks if the dependancies are complete.
        /// </summary>
        /// <param name="exceptions">Exceptions to not count in the complete check.</param>
        /// <returns>
        /// <para>true: all dependancies (excluding exceptions) are complete.</para>
        /// <para>false: at least one dependancy isn't complete.</para>
        /// </returns>
        public bool DependanciesComplete(params string[] exceptions)
        {
            foreach (KeyValuePair<string, Dependancy> dependancy in m_dependancies)
            {
                if (!dependancy.Value.Complete && !exceptions.Contains(dependancy.Key))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the event has a dependancy with the identifier.
        /// </summary>
        /// <param name="dependancy">Identifier to check for.</param>
        /// <returns>
        /// <para>true: event has the dependancy.</para>
        /// <para>false: event does not have the dependancy.</para>
        /// </returns>
        public bool HasDependancy(string dependancy)
        {
            return m_dependancies.ContainsKey(dependancy);
        }

        /// <summary>
        /// Adds a dependancy to the event.
        /// </summary>
        /// <param name="dependancy">String identifier of the dependancy.</param>
        /// <param name="sender">Object that's adding the dependancy.</param>
        /// <returns>
        /// <para>true: dependancy was added correctly.</para>
        /// <para>false: dependancy was not added correctly.</para>
        /// </returns>
        public bool AddDependancy(string dependancy, IEventDependancy sender)
        {
            if (!m_dependancies.ContainsKey(dependancy))
            {
                m_dependancies.Add(dependancy, new Dependancy(dependancy, sender));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Completes a dependancy.
        /// </summary>
        /// <param name="dependancy">Identifier of the dependancy to complete.</param>
        public void CompleteDependancy(string dependancy)
        {
            if (m_dependancies.ContainsKey(dependancy))
            {
                m_dependancies[dependancy].Complete = true;
            }
        }

        /// <summary>
        /// Removes a dependancy from the list.
        /// </summary>
        /// <remarks>
        /// To avoid bugs, dependancies should be completed instead of removed unless necessary.
        /// </remarks>
        /// <param name="dependancy">Identifier of the dependancy to remove.</param>
        /// <returns>
        /// <para>true: the dependancy was removed successfully.</para>
        /// <para>false: the dependancy was not removed successfully.</para>
        /// </returns>
        public bool RemoveDependancy(string dependancy)
        {
            return m_dependancies.Remove(dependancy);
        }

        /// <summary>
        /// Returns the percentage of dependancies that are complete.
        /// </summary>
        /// <returns>Percentage value from 0 to 100.</returns>
        public float GetDependancyPercentageComplete()
        {
            float totalPercent = 0;

            foreach (KeyValuePair<string, Dependancy> dependancy in m_dependancies)
            {
                if (dependancy.Value.Complete == true)
                {
                    totalPercent += 100;
                    continue;
                }

                totalPercent += dependancy.Value.Sender.PercentageComplete(dependancy.Key);
            }

            return totalPercent / m_dependancies.Count;
        }

        /// <summary>
        /// Removes all dependancies.
        /// </summary>
        protected void ResetDependancies()
        {
            m_dependancies = new Dictionary<string, Dependancy>();
        }
    }

    /// <summary>
    /// Generic game event.
    /// </summary>
    /// <typeparam name="T">Type of the data to be sent when the event is invoked.</typeparam>
    public abstract class GameEvent<T> : GameEvent
    {
        private event Action<T> Event = null;

        /// <summary>
        /// Invokes the event.
        /// </summary>
        /// <param name="sender">Object invoking the event.</param>
        /// <param name="item">Data to send to the subscribers.</param>
        public void Invoke(T item)
        {
            if (DependanciesComplete())
            {
                Event?.Invoke(item);
                ResetDependancies();
            }
        }

        /// <summary>
        /// Subscribe a new even to the event.
        /// </summary>
        /// <param name="action">Action to subscribe.</param>
        public void Subscribe(Action<T> action)
        {
            Event += action;
        }

        /// <summary>
        /// Unsubscribe an action from the event.
        /// </summary>
        /// <param name="action">Action to unsubscribe.</param>
        public void UnSubscribe(Action<T> action)
        {
            Event -= action;
        }
    }
}