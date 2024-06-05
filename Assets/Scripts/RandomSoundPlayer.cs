using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
	[SerializeField] private List<AudioClip> availableSounds;

	private AudioSource audioSource;
	private AudioClip previousClip;

	public void Play()
	{
		var clip = GetNextClip();

		audioSource.Stop();
		audioSource.clip = clip;
		audioSource.Play();

		previousClip = clip;
	}

	private AudioClip GetNextClip()
	{
		List<AudioClip> clips = new List<AudioClip>();
		foreach (var c in availableSounds)
		{
			if (availableSounds.Count > 1 && previousClip == c)
			{
				continue;
			}
			clips.Add(c);
		}
		return clips[Random.Range(0, clips.Count)];
	}

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}
}
