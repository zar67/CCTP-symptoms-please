using SymptomsPlease.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct QuizQuestionData
{
    public Topic Topic;
    public TrainingQuestionTypes QuestionType;
    public QuestionTypeData QuestionData;
}

[CreateAssetMenu(menuName = "SymptomsPlease/Training/Quiz Questions Data")]
public class TrainingQuizQuestions : GameScriptableObject
{
    [Serializable]
    public struct QuestionsStruct
    {
        public TrainingQuestionTypes Type;

        //[ConditionalField(nameof(Type), false, true, TrainingQuestionTypes.MULTIPLE_CHOICE)]
        public MultipleChoiceQuestionData MultipleChoiceData;
    }

    [Serializable]
    public struct TopicsStruct
    {
        public Topic Topic;
        public List<QuestionsStruct> Questions;
    }

    [SerializeField] private List<TopicsStruct> m_questions = new List<TopicsStruct>();

    public QuizQuestionData GetRandomQuestion()
    {
        TopicsStruct topic = m_questions[Random.Range(0, m_questions.Count)];
        while (!ModificationsManager.IsTopicActive(topic.Topic))
        {
            topic = m_questions[Random.Range(0, m_questions.Count)];
        }

        QuestionsStruct question = topic.Questions[Random.Range(0, topic.Questions.Count)];

        return new QuizQuestionData()
        {
            Topic = topic.Topic,
            QuestionType = question.Type,
            QuestionData = question.MultipleChoiceData
        };
    }
}