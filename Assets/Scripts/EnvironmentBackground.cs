using System.Collections;
using UnityEngine;

public class EnvironmentBackground : MonoBehaviour
{
	[SerializeField] private GameObject background1;
	[SerializeField] private GameObject background2;
	[SerializeField] private PuzzleBoard puzzleBoard;

	private Texture2D previousGraphic;
	private bool useFirstBackground = true;

	private IEnumerator LerpAlpha(float target, float time, SpriteRenderer renderer)
	{
		float startAlpha = renderer.color.a;
		float elapsedTime = 0f;

		while (elapsedTime < time)
		{
			float alpha = Mathf.Lerp(startAlpha, target, elapsedTime / time);
			Color newColor = renderer.color;
			newColor.a = alpha;
			renderer.color = newColor;

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Color finalColor = renderer.color;
		finalColor.a = target;
		renderer.color = finalColor;
	}

	private void SetBackground()
	{
		Sprite newSprite = Sprite.Create(puzzleBoard.currentGraphic, new Rect(0.0f, 0.0f, puzzleBoard.currentGraphic.width, puzzleBoard.currentGraphic.height), new Vector2(0.5f, 0.5f), 100.0f);

		if (useFirstBackground)
		{
			background1.GetComponent<SpriteRenderer>().sprite = newSprite;
			StartCoroutine(LerpAlpha(1f, 1f, background1.GetComponent<SpriteRenderer>()));
			StartCoroutine(LerpAlpha(0f, 1f, background2.GetComponent<SpriteRenderer>()));
		}
		else
		{
			background2.GetComponent<SpriteRenderer>().sprite = newSprite;
			StartCoroutine(LerpAlpha(0f, 1f, background1.GetComponent<SpriteRenderer>()));
			StartCoroutine(LerpAlpha(1f, 1f, background2.GetComponent<SpriteRenderer>()));
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