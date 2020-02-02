using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip GameMusic;
    public AudioClip WinMusic;

	private static AudioManager s_Instance;

	public static AudioManager Get()
	{
		if (!s_Instance)
		{
			s_Instance = GameObject.FindObjectOfType<AudioManager>();
			s_Instance.ReInit();
		}

		return s_Instance;
	}

	void ReInit()
	{
		PlayGameMusic();
	}

	private void Awake()
	{
		Get();
		ReInit();
	}

	void PlayGameMusic()
	{
		AudioSource source = GetComponent<AudioSource>();
		source.clip = GameMusic;
		source.loop = true;
		source.Play();
	}

	public void PlayGameOverMusic()
	{
		AudioSource source = GetComponent<AudioSource>();
		source.clip = WinMusic;
		source.loop = false;
		source.Play();
	}

	public void PlayRandomOneShot(GameObject obj, AudioClip[] audioClips, float volume, bool allowRandomPitch = true)
	{
		/*if (obj.GetComponent<AudioSource>())
		{
			Behaviour.Destroy(obj.GetComponent<AudioSource>());
		}
		AudioSource audioSource = obj.AddComponent<AudioSource>();
		*/

		AudioSource audioSource = obj.GetComponent<AudioSource>();

		if (!audioSource)
		{ 
			audioSource = obj.AddComponent<AudioSource>();
		}
		
		if (audioClips == null ||audioClips.Length == 0)
		{
			return;
		}

		AudioClip rndClip = audioClips[Random.Range(0, audioClips.Length)];

		audioSource.clip = rndClip;
		audioSource.time = 0;
		audioSource.pitch = allowRandomPitch ? Random.Range(0.9f, 1.1f) : 1.0f;
		audioSource.volume = volume;
		audioSource.spatialBlend = 1.0f;
		audioSource.spread = 360.0f;
		audioSource.rolloffMode = AudioRolloffMode.Custom;
		audioSource.minDistance = 500.0f;
		audioSource.maxDistance = 500.0f;
		audioSource.Play();
	}
}
