using UnityEngine;

public class PuzzleTile : MonoBehaviour
{
	private Vector2 correctCoordinates;

	public Vector2 CorrectCoordinates => correctCoordinates;

	public void Initialize(Sprite sprite, Vector2 correctCoordinates)
	{
		GetComponent<SpriteRenderer>().sprite = sprite;
		this.correctCoordinates = correctCoordinates;
	}
}
