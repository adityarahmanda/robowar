using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
	public static AudioManager i;

	[Header("BGM Settings")]
	[SerializeField] private AudioClip m_BGMClip;
	[SerializeField] private AudioSource m_BGMAudioSource;

	[Header("SFX Settings")]
	[SerializeField] private AudioSource m_SFXAudioSource;
	[SerializeField] private AudioClip[] m_SFXClips;
	private Dictionary<string, AudioClip> m_SFXClipDictionary;

	private void Awake () 
	{
		if (i != null) {
			Destroy (gameObject);
		} else {
			i = this;
		}
	}

	private void Start()
	{
		m_BGMAudioSource.loop = true;
		m_BGMAudioSource.clip = m_BGMClip;
		m_BGMAudioSource.Play();

		m_SFXClipDictionary = new Dictionary<string, AudioClip>();
		for(int i = 0; i < m_SFXClips.Length; i++)
		{
			m_SFXClipDictionary.Add(m_SFXClips[i].name, m_SFXClips[i]);
		}
	}

	public void PlaySFX(string _clipName)
	{
		if(!m_SFXClipDictionary.ContainsKey(_clipName)) return;

		m_SFXAudioSource.PlayOneShot(m_SFXClipDictionary[_clipName]);
	}
}
