using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
	[Serializable]
	public struct AudioMap
	{
		public EAudioClipType Type;
		public AudioClip[] Clips;
	}

	public static AudioManager Instance
	{
		get;
		private set;
	}

	[SerializeField] private AudioSource m_effectsAudioSource;
	[SerializeField] private AudioSource m_musicAudioSource;

	[SerializeField] private float m_lowPitchRange = .95f;
	[SerializeField] private float m_highPitchRange = 1.05f;

	[SerializeField] private AudioMap[] m_audioClips = new AudioMap[] { };

	private Dictionary<EAudioClipType, AudioClip[]> m_audioClipDictionary = new Dictionary<EAudioClipType, AudioClip[]>();

    private void Awake()
    {
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.LogWarning("More than one instance of AudioManager found.");
		}

		m_audioClipDictionary = new Dictionary<EAudioClipType, AudioClip[]>();
		foreach (AudioMap map in m_audioClips)
		{
			m_audioClipDictionary.Add(map.Type, map.Clips);
		}
    }

    public void Play(EAudioClipType clip)
	{
		int randomIndex = Random.Range(0, m_audioClipDictionary[clip].Length);
		float randomPitch = Random.Range(m_lowPitchRange, m_highPitchRange);

		m_effectsAudioSource.pitch = randomPitch;
		m_effectsAudioSource.clip = m_audioClipDictionary[clip][randomIndex];
		m_effectsAudioSource.Play();
	}

	public void PlayMusic(EAudioClipType clip)
	{
		int randomIndex = Random.Range(0, m_audioClipDictionary[clip].Length);

		m_musicAudioSource.clip = m_audioClipDictionary[clip][randomIndex];
		m_musicAudioSource.Play();
	}
}