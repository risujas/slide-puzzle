using TMPro;
using UnityEngine;

public class PuzzleTile : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI tileNumberText;

	private SpriteRenderer spriteRenderer;

	public void Initialize(Texture2D sourceTexture, Vector2Int puzzleBoardCoordinates, Vector2Int tileSize, int boardSize)
	{
		Vector2 cornerPixel = new Vector2Int(puzzleBoardCoordinates.x * tileSize.x, puzzleBoardCoordinates.y * tileSize.y);
		var rect = new Rect(cornerPixel.x, cornerPixel.y, tileSize.x, tileSize.y);
		var sprite = Sprite.Create(sourceTexture, rect, new Vector2(0.5f, 0.5f), tileSize.x);

		gameObject.name = "PuzzleTile_" + puzzleBoardCoordinates.x + "_" + puzzleBoardCoordinates.y; ;
		spriteRenderer.sprite = sprite;

		tileNumberText.text = ((puzzleBoardCoordinates.x + puzzleBoardCoordinates.y * boardSize) + 1).ToString();
	}

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
}
