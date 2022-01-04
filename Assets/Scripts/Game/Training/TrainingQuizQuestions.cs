using SymptomsPlease.ScriptableObjects;
using SymptomsPlease.Utilities.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SymptomsPlease/Training/Quiz Questions Data")]
public class TrainingQuizQuestions : GameScriptableObject
{
    [Serializable]
    public class QuestionsStruct
    {
        public TrainingQuestionTypes Type;

        //[ConditionalField(nameof(Type), false, true, TrainingQuestionTypes.MULTIPLE_CHOICE)]
        public MultipleChoiceQuestionData MultipleChoiceData;
    }

    [Serializable]
    public class TopicsStruct
    {
        public Topic Topic;
        public List<QuestionsStruct> Questions;
    }

    [SerializeField] private List<TopicsStruct> m_questions;

    public QuestionsStruct GetRandomQuestion()
    {
        TopicsStruct topic = m_questions[Random.Range(0, m_questions.Count)];
        while (!ModificationsManager.IsTopicActive(topic.Topic))
        {
            topic = m_questions[Random.Range(0, m_questions.Count)];
        }

        return topic.Questions[Random.Range(0, topic.Questions.Count)];
    }
}