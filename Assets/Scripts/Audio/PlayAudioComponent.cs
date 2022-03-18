using SymptomsPlease.Common;
using UnityEngine;

public class PlayAudioComponent : Triggerable
{
    [SerializeField] private EAudioClipType m_audioClip;

    public override void Trigger()
    {
        AudioManager.Instance.Play(m_audioClip);
    }
}