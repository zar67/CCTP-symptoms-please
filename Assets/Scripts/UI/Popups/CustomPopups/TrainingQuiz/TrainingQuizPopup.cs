using SymptomsPlease.SaveSystem;
using SymptomsPlease.UI.Popups;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingQuizPopup : Popup
{
    [Serializable]
    public struct AnswerTypeReference
    {
        public TrainingQuestionTypes Type;
        public QuestionTypeBehaviour Holder;
    }

    [Header("Question References")]
    [SerializeField] private TextMeshProUGUI m_questionsCorrectText = default;
    [SerializeField] private TextMeshProUGUI m_totalQuestionsText = default;
    [SerializeField] private TextMeshProUGUI m_questionText = default;

    [Header("Answer References")]
    [SerializeField] private AnswerTypeReference[] m_answerHolders = default;
    [SerializeField] private Button m_submitAnswerButton = default;

    [Header("Result References")]
    [SerializeField] private TextMeshProUGUI m_scoreText = default;
    [SerializeField] private Button m_nextQuestionButton = default;

    [Header("Questions Data")]
    [SerializeField] private TrainingQuizQuestions m_trainingQuizQuestions = default;

    private Dictionary<TrainingQuestionTypes, QuestionTypeBehaviour> m_answerHolderDictionary = new Dictionary<TrainingQuestionTypes, QuestionTypeBehaviour>();

    private QuizQuestionData m_currentQuestionData;

    private int m_questionsCorrect = 0;
    private int m_totalQuestions = 0;

    private string m_currentAnswer = "";
    private int m_currentScore = 0;

    public override void Initialise()
    {
        base.Initialise();

        m_answerHolderDictionary = new Dictionary<TrainingQuestionTypes, QuestionTypeBehaviour>();
        foreach (AnswerTypeReference reference in m_answerHolders)
        {
            m_answerHolderDictionary.Add(reference.Type, reference.Holder);
        }
    }

    public override void OnOpenBegin()
    {
        base.OnOpenBegin();

        m_questionsCorrect = 0;
        m_totalQuestions = 0;
        m_currentScore = 0;

        m_questionsCorrectText.text = m_questionsCorrect.ToString();
        m_totalQuestionsText.text = m_totalQuestions.ToString();
        m_scoreText.text = "0";

        OnNextQuestion();
    }

    public override void OnCloseBegin()
    {
        base.OnCloseBegin();

        SaveSystem.Save();
    }

    private void OnEnable()
    {
        m_submitAnswerButton.onClick.AddListener(OnQuestionAnswered);
        m_nextQuestionButton.onClick.AddListener(OnNextQuestion);

        QuestionTypeBehaviour.OnAnswerChanged += OnCurrentAnswerChanged;
    }

    private void OnDisable()
    {
        m_submitAnswerButton.onClick.RemoveListener(OnQuestionAnswered);
        m_nextQuestionButton.onClick.RemoveListener(OnNextQuestion);

        QuestionTypeBehaviour.OnAnswerChanged -= OnCurrentAnswerChanged;
    }

    private void OnCurrentAnswerChanged(string answer)
    {
        m_currentAnswer = answer;
    }

    private void OnQuestionAnswered()
    {
        AudioManager.Instance.Play(EAudioClipType.CLICK);
        int score = m_answerHolderDictionary[m_currentQuestionData.QuestionType].GetAnswerScore(m_currentAnswer);
        bool answerCorrect = score > 0;

        GameData.TotalScore += score;
        m_currentScore += score;
        m_scoreText.text = "Score: " + m_currentScore.ToString();

        TrainingManager.RegisterQuestion(m_currentQuestionData.Topic, answerCorrect);

        if (answerCorrect)
        {
            m_questionsCorrect++;
        }

        m_totalQuestions++;

        m_questionsCorrectText.text = m_questionsCorrect.ToString();
        m_totalQuestionsText.text = m_totalQuestions.ToString();

        m_nextQuestionButton.gameObject.SetActive(true);
        m_submitAnswerButton.gameObject.SetActive(false);

        m_answerHolderDictionary[m_currentQuestionData.QuestionType].AnswerSubmitted(m_currentAnswer);
    }

    private void OnNextQuestion()
    {
        AudioManager.Instance.Play(EAudioClipType.CLICK);
        m_currentQuestionData = m_trainingQuizQuestions.GetRandomQuestion();

        m_questionText.text = m_currentQuestionData.QuestionData.Question;

        foreach (KeyValuePair<TrainingQuestionTypes, QuestionTypeBehaviour> questionBehaviour in m_answerHolderDictionary)
        {
            questionBehaviour.Value.gameObject.SetActive(questionBehaviour.Key == m_currentQuestionData.QuestionType);

            if (questionBehaviour.Key == m_currentQuestionData.QuestionType)
            {
                questionBehaviour.Value.Initialise(m_currentQuestionData.QuestionData);
            }
        }

        m_nextQuestionButton.gameObject.SetActive(false);
        m_submitAnswerButton.gameObject.SetActive(true);
    }
}