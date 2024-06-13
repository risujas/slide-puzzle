using System.Collections;
using TMPro;
using UnityEngine;

public class PuzzleBoardTile : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI tileNumberText;
	[SerializeField] private CanvasGroup canvasGroup;

	private SpriteRenderer spriteRenderer;

	public IEnumerator FadeAlphaToValue(float time, float value)
	{
		float elapsedTime = 0f;
		float startAlpha = canvasGroup.alpha;
		float targetAlpha = value;

		while (elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / time);
			canvasGroup.alpha = alpha;
			yield return null;
		}

		canvasGroup.alpha = targetAlpha;
	}

	public void ForceSetAlphaToValue(float value)
	{
		canvasGroup.alpha = value;
	}

	public void Initialize(Texture2D sourceTexture, Vector2Int puzzleBoardCoordinates, Vector2Int tileSize, int boardSize)
	{
		Vector2 cornerPixel = new Vector2Int(puzzleBoardCoordinates.x * tileSize.x, puzzleBoardCoordinates.y * tileSize.y);
		var rect = new Rect(cornerPixel.x, cornerPixel.y, tileSize.x, tileSize.y);
		var sprite = Sprite.Create(sourceTexture, rect, new Vector2(0.5f, 0.5f), tileSize.x);

		gameObject.name = "PuzzleTile_" + puzzleBoardCoordinates.x + "_" + puzzleBoardCoordinates.y; ;
		spriteRenderer.sprite = sprite;

		if (PlayerPrefs.GetInt("InvertNumbers", 0) == 1)
		{
			tileNumberText.text = ((puzzleBoardCoordinates.x + puzzleBoardCoordinates.y * boardSize) + 1).ToString();
		}
		else
		{
			tileNumberText.text = ((puzzleBoardCoordinates.x + (boardSize - 1 - puzzleBoardCoordinates.y) * boardSize) + 1).ToString();
		}
	}

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
}
