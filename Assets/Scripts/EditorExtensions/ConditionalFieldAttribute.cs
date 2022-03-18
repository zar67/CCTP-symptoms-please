using UnityEngine;

namespace SymptomsPlease.Utilities.Attributes
{
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public readonly string FieldToCheck;
        public readonly object[] CompareValues;
        public readonly bool Inverse;
        public readonly bool ANDOperator;

        /// <param name="fieldToCheck">String name of field to check value</param>
        /// <param name="inverse">Inverse check result</param>
        /// <param name="andOperator">Whether the values will use AND operator (OR if false)</param>
        /// <param name="compareValues">On which values field will be shown in inspector</param>
        public ConditionalFieldAttribute(string fieldToCheck, bool inverse, bool andOperator, params object[] compareValues)
        {
            FieldToCheck = fieldToCheck;
            Inverse = inverse;
            ANDOperator = andOperator;
            CompareValues = compareValues;
        }
    }
}