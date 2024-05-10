using UnityEngine;

public class Tile : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	private Vector2 correctGridPosition;

	public Vector2 CorrectGridPosition => correctGridPosition;

	public void Initialize(Sprite sprite, Vector2 correctGridPos)
	{
		spriteRenderer.sprite = sprite;
		correctGridPosition = correctGridPos;
	}

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
}
