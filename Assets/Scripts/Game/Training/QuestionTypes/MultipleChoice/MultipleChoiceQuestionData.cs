using System;
using System.Linq;

[Serializable]
public class MultipleChoiceQuestionData : QuestionTypeData
{
    public string[] CorrectAnswers;
    public string[] IncorrectAnswers;

    public override bool IsCorrect(string answer)
    {
        return CorrectAnswers.Contains(answer);
    }
}