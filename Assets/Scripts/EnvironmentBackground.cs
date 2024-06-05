using UnityEngine;

public class EnvironmentBackground : MonoBehaviour
{
	[SerializeField] private GameObject background1;
	[SerializeField] private GameObject background2;
	[SerializeField] private PuzzleBoard puzzleBoard;

	private Texture2D previousGraphic;
	private bool useFirstBackground = true;

	private void SetBackground()
	{
		Sprite newSprite = Sprite.Create(puzzleBoard.currentGraphic, new Rect(0.0f, 0.0f, puzzleBoard.currentGraphic.width, puzzleBoard.currentGraphic.height), new Vector2(0.5f, 0.5f), 100.0f);

		if (useFirstBackground)
		{
			background1.GetComponent<SpriteRenderer>().sprite = newSprite;
			background1.SetActive(true);
			background2.SetActive(false);
		}
		else
		{
			background2.GetComponent<SpriteRenderer>().sprite = newSprite;
			background1.SetActive(false);
			background2.SetActive(true);
		}

		useFirstBackground = !useFirstBackground;
		previousGraphic = puzzleBoard.currentGraphic;
	}

	private void Update()
	{
		if (puzzleBoard.currentGraphic != previousGraphic)
		{
			SetBackground();
		}
	}
}
