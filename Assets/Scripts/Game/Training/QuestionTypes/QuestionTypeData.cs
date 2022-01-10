using System;

[Serializable]
public abstract class QuestionTypeData
{
    public string Question;

    public abstract bool IsCorrect(string answer);
}