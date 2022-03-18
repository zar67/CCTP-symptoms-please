using System;
using System.Collections.Generic;

namespace SymptomsPlease.Common.Conditions
{
    [Serializable]
    public struct ConditionConjunction
    {
        public List<BaseCondition> OR;
    }

    [Serializable]
    public class ConditionNormalForm
    {
        public List<ConditionConjunction> AND;
    }
}