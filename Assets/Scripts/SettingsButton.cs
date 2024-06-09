using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
	[SerializeField] private Sprite mainSprite;
	[SerializeField] private Sprite returnSprite;
	[SerializeField] private Image image;

	public void ToggleSprite()
	{
		image.sprite = image.sprite == mainSprite ? returnSprite : mainSprite;
	}

	private void Start()
	{
		image.sprite = mainSprite;
	}
}
