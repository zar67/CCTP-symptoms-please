using System.Linq;
using UnityEngine;

public class MultipleChoiceQuestionBehaviour : QuestionTypeBehaviour
{
    [SerializeField] private MultipleChoiceAnswer[] m_questionAnswers = default;

    public override void Initialise(QuestionTypeData data)
    {
        if (data is MultipleChoiceQuestionData multipleChoiceData)
        {
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

    public void OnAnswerSelected(MultipleChoiceAnswer selectedAnswer)
    {
        foreach (MultipleChoiceAnswer answer in m_questionAnswers)
        {
            if (answer == selectedAnswer)
            {
                answer.Select();
                InvokeAnswerChanged(answer.AnswerText);
            }
            else
            {
                answer.Deselect();
            }
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