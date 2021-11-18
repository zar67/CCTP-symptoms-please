using UnityEngine;

namespace SymptomsPlease.Common.Conditions
{
    public abstract class BaseCondition : MonoBehaviour
    {
        public string ID;
        public abstract bool IsSatisfied();
    }
}