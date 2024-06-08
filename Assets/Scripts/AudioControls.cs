using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControls : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private string audioMixerParameter;

	[SerializeField] private Sprite activeSprite;
	[SerializeField] private Sprite mutedSprite;

	[SerializeField] private Image image;
	[SerializeField] private Button button;
	[SerializeField] private Scrollbar scrollbar;

	private bool isMuted = false;

	public void ToggleMute()
	{
		isMuted = !isMuted;

		if (isMuted)
		{
			audioMixer.SetFloat(audioMixerParameter, LinearToLogarithmic(0.0f));
		}
		else
		{
			audioMixer.SetFloat(audioMixerParameter, LinearToLogarithmic(scrollbar.value));
		}
	}

	public void Unmute()
	{
		isMuted = false;
		audioMixer.SetFloat(audioMixerParameter, LinearToLogarithmic(scrollbar.value));
	}

	private float LinearToLogarithmic(float value)
	{
		if (value == 0.0f)
		{
			return -80f;
		}

		float adjustedLinear = value * 45f - 45f;
		return adjustedLinear;
	}

	private float LogarithmicToLinear(float value)
	{
		if (value == -80f)
		{
			return 0.0f;
		}

		float adjustedLog = (value + 45f) / 45f;
		return adjustedLog;
	}

	private void HandleImageSprite()
	{
		float logVolume = 0.0f;
		audioMixer.GetFloat(audioMixerParameter, out logVolume);

		if (image.sprite == activeSprite)
		{
			if (isMuted || LogarithmicToLinear(logVolume) == 0.0f)
			{
				image.sprite = mutedSprite;
			}
		}
		else if (image.sprite == mutedSprite)
		{
			if (!isMuted && LogarithmicToLinear(logVolume) > 0.0f)
			{
				image.sprite = activeSprite;
			}
		}
	}

	private void Start()
	{
		float logVolume = 0.0f;
		audioMixer.GetFloat(audioMixerParameter, out logVolume);

		scrollbar.value = LogarithmicToLinear(logVolume);
	}

	private void Update()
	{
		HandleImageSprite();
	}
}
