using UnityEngine;
using UnityEngine.UI;

public class SfxControls : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource; // TODO
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
			audioSource.volume = 0.0f;
		}
		else
		{
			audioSource.volume = scrollbar.value;
		}
	}

	public void Unmute()
	{
		isMuted = false;
		audioSource.volume = scrollbar.value;
	}

	private void HandleImageSprite()
	{
		if (image.sprite == activeSprite)
		{
			if (isMuted || audioSource.volume == 0.0f)
			{
				image.sprite = mutedSprite;
			}
		}
		else if (image.sprite == mutedSprite)
		{
			if (!isMuted && audioSource.volume > 0.0f)
			{
				image.sprite = activeSprite;
			}
		}
	}

	private void Start()
	{
		scrollbar.value = audioSource.volume;
	}

	private void Update()
	{
		HandleImageSprite();
	}
}
