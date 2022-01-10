using System;
using UnityEngine;

public abstract class QuestionTypeBehaviour : MonoBehaviour
{
    public static event Action<string> OnAnswerChanged;

    public abstract void Initialise(QuestionTypeData data);

    public static void InvokeAnswerChanged(string answer)
    {
        OnAnswerChanged?.Invoke(answer);
    }
}