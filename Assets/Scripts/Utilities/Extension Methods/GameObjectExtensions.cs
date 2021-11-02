using UnityEngine;

namespace SymptomsPlease.Utilities.ExtensionMethods
{
    public static class GameObjectExtensions
    {
        public static void DestroyChildren(this GameObject obj)
        {
            obj.transform.DestroyChildren();
        }
    }
}