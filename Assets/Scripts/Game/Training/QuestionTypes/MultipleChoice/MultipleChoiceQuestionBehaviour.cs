using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultipleChoiceQuestionBehaviour : QuestionTypeBehaviour
{
    [SerializeField] private MultipleChoiceAnswer[] m_questionAnswers = default;

    private List<MultipleChoiceAnswer> m_selectedAnswers = new List<MultipleChoiceAnswer>();

    private MultipleChoiceQuestionData m_questionData = default;

    public override void Initialise(QuestionTypeData data)
    {
        m_selectedAnswers = new List<MultipleChoiceAnswer>();

        if (data is MultipleChoiceQuestionData multipleChoiceData)
        {
            m_questionData = multipleChoiceData;

            var answers = multipleChoiceData.CorrectAnswers.ToList();

            if (multipleChoiceData.CorrectAnswers.Length < m_questionAnswers.Length)
            {
                int numAdditionalAnswers = m_questionAnswers.Length - multipleChoiceData.CorrectAnswers.Length;
                var additionalAnswers = multipleChoiceData.IncorrectAnswers.ToList();
                for (int i = 0; i < numAdditionalAnswers; i++)
                {
                    if (additionalAnswers.Count == 0)
                    {
                        break;
                    }

                    int index = Random.Range(0, additionalAnswers.Count);
                    answers.Add(additionalAnswers[index]);
                    additionalAnswers.RemoveAt(index);
                }
            }
            else if (multipleChoiceData.CorrectAnswers.Length > m_questionAnswers.Length)
            {
                answers = answers.GetRange(0, m_questionAnswers.Length);
            }

            int count = answers.Count;
            while (count > 1)
            {
                count--;
                int randomIndex = Random.Range(0, count + 1);
                string value = answers[randomIndex];
                answers[randomIndex] = answers[count];
                answers[count] = value;
            }

            for (int i = 0; i < m_questionAnswers.Length; i++)
            {
                m_questionAnswers[i].gameObject.SetActive(i < answers.Count);

                if (i < answers.Count)
                {
                    m_questionAnswers[i].Initialise(answers[i]);
                }
            }
        }
    }

    public override void AnswerSubmitted(string answer)
    {
        foreach (MultipleChoiceAnswer multipleChoiceAnswer in m_questionAnswers)
        {
            multipleChoiceAnswer.AnswerSubmitted(
                m_selectedAnswers.Contains(multipleChoiceAnswer), 
                m_questionData.CorrectAnswers.Contains(multipleChoiceAnswer.AnswerText)
            );
        }
    }

    public override int GetAnswerScore(string answer)
    {
        int score = 0;

        foreach (MultipleChoiceAnswer selectedAnswer in m_selectedAnswers)
        {
            if (m_questionData.CorrectAnswers.Contains(selectedAnswer.AnswerText))
            {
                score += 1;
            }
            else
            {
                score -= 1;
            }
        }

        return score;
    }

    public void OnAnswerSelected(MultipleChoiceAnswer selectedAnswer)
    {
        if (m_selectedAnswers.Contains(selectedAnswer))
        {
            selectedAnswer.Deselect();
            m_selectedAnswers.Remove(selectedAnswer);
        }
        else
        {
            selectedAnswer.Select();
            InvokeAnswerChanged(selectedAnswer.AnswerText);
            m_selectedAnswers.Add(selectedAnswer);
        }
    }

    private void OnEnable()
    {
        MultipleChoiceAnswer.OnSelected += OnAnswerSelected;
    }

    private void OnDisable()
    {
        MultipleChoiceAnswer.OnSelected -= OnAnswerSelected;
    }
}