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

	public void PlayRandomOneShot(GameObject obj, AudioClip[] audioClips, float volume)
	{
		if (!obj.GetComponent<AudioSource>())
		{
			obj.AddComponent<AudioSource>();
		}

		if (audioClips == null ||audioClips.Length == 0)
		{
			return;
		}

		AudioClip rndClip = audioClips[Random.Range(0, audioClips.Length)];

		AudioSource audioSource = obj.GetComponent<AudioSource>();
		audioSource.PlayOneShot(rndClip);
		audioSource.pitch = Random.Range(-0.1f, 1.0f);
		audioSource.volume = volume;
	}
}
