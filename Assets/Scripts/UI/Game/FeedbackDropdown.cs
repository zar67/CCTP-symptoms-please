using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class FeedbackDropdown : MonoBehaviour
{
    [SerializeField] private RectTransform m_backgroundTransform = default;
    [SerializeField] private TextMeshProUGUI m_effectivenessText = default;
    [SerializeField] private TextMeshProUGUI m_afflictionText = default;
    [SerializeField] private TextMeshProUGUI m_actionText = default;
    [SerializeField] private TextMeshProUGUI m_scoreText = default;

    [Header("Background Tween Postion")]
    [SerializeField] private Vector3 m_hiddenPosition = new Vector3(0.0f, -5.0f, 0.0f);
    [SerializeField] private Vector3 m_shownPosition = new Vector3(0.0f, -5.0f, 0.0f);

    [Header("Timing Values")]
    [SerializeField] private float m_tweenInDuration = 0.4f;
    [SerializeField] private float m_displayDuration = 0.7f;
    [SerializeField] private float m_tweenOutDuration = 0.4f;

    [Header("Data References")]
    [SerializeField] private ActionEffectivenessData m_actionEffectivnessData = default;
    [SerializeField] private ActionsData m_actionsData = default;

    private void OnEnable()
    {
        PatientManager.OnPatientSeen += OnPatientSeen;
        m_backgroundTransform.anchoredPosition = m_hiddenPosition;
    }

    private void OnDisable()
    {
        PatientManager.OnPatientSeen -= OnPatientSeen;
    }

    private void OnPatientSeen(PatientManager.PatientSeenData data)
    {
        if (data.ActionEffectiveness > ActionEffectiveness.GOOD)
        {
            m_afflictionText.text = data.PatientData.AfflictionData.DisplayName;
            AudioManager.Instance.Play(EAudioClipType.CURED);
        }
        if (data.ActionEffectiveness < ActionEffectiveness.NEUTRAL)
        {
            AudioManager.Instance.Play(EAudioClipType.INCORRECT);
        }
        else
        {
            m_afflictionText.text = "???";
        }

        m_actionText.text = m_actionsData.GetInfoForAction(data.ActionTaken).DisplayName;

        m_effectivenessText.text = m_actionEffectivnessData.GetInfoForEffectiveness(data.ActionEffectiveness).DisplayText;
        m_scoreText.text = data.ScoreGained < 0 ? data.ScoreGained.ToString() : "+" + data.ScoreGained.ToString();

        m_backgroundTransform.DOAnchorPos(m_shownPosition, m_tweenInDuration).OnComplete(OnTweenInComplete);
    }

    private void OnTweenInComplete()
    {
        StartCoroutine(DelayTweenOut());
    }

    private IEnumerator DelayTweenOut()
    {
        yield return new WaitForSeconds(m_displayDuration);
        TweenOut();
    }

    private void TweenOut()
    {
        m_backgroundTransform.DOAnchorPos(m_hiddenPosition, m_tweenOutDuration);
    }
}