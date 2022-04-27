using System;
using UnityEngine;

public abstract class QuestionTypeBehaviour : MonoBehaviour
{
    public static event Action<string> OnAnswerChanged;

    public abstract void Initialise(QuestionTypeData data);

    public abstract void AnswerSubmitted(string answer);

    public abstract int GetAnswerScore(string answer);

    public void InvokeAnswerChanged(string answer)
    {
        OnAnswerChanged?.Invoke(answer);
    }
}