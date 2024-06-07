using UnityEngine;
using UnityEngine.UI;

public class PuzzleGraphicSetter : MonoBehaviour
{
	private PuzzleBoardManager puzzleBoardManager;
	private ClickableImage clickableImage;

	private void Start()
	{
		puzzleBoardManager = FindFirstObjectByType<PuzzleBoardManager>();
		clickableImage = GetComponent<ClickableImage>();
	}

	private void Update()
	{
		if (clickableImage.WasClicked)
		{
			puzzleBoardManager.CreatePuzzleWithGraphic(GetComponent<Image>().sprite.texture);
		}
	}
}
