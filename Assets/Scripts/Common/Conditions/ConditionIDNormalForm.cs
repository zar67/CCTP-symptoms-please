using System;
using System.Collections.Generic;

namespace SymptomsPlease.Common.Conditions
{
    [Serializable]
    public struct ConditionIDConjunction
    {
        public List<string> OR;
    }

    [Serializable]
    public class ConditionIDNormalForm
    {
        public List<ConditionIDConjunction> AND;
    }
}