using Newtonsoft.Json;
using UnityEngine;

namespace SymptomsPlease.ScriptableObjects.Variables
{
    [JsonObject(MemberSerialization.OptIn)]
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "SymptomsPlease/Variables/FloatVariable")]
    public class FloatVariable : VariableType<float>
    {
    }
}
