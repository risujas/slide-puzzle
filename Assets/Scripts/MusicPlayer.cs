using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
	private List<AudioClip> musicClips = new List<AudioClip>();
	private int currentClipIndex = 0;

	private AudioSource audioSource;

	private const string musicFolder = "Music";

	private void LoadTracks()
	{
		musicClips = Resources.LoadAll<AudioClip>(musicFolder).ToList();
	}

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		LoadTracks();
	}

	private void Update()
	{
		if (!audioSource.isPlaying)
		{
			currentClipIndex = (currentClipIndex + 1) % musicClips.Count;
			audioSource.clip = musicClips[currentClipIndex];
			audioSource.Play();
		}
	}
}
