using UnityEngine;

public class PuzzleBoardBackground : MonoBehaviour
{
	private const float backgroundBorderThickness = 0.2f;

	public void SetBackgroundTransform(PuzzleBoard board)
	{
		Vector3 pos = board.transform.position;
		pos.x -= 0.5f + (backgroundBorderThickness * 0.5f);
		pos.y -= 0.5f + (backgroundBorderThickness * 0.5f);
		transform.position = pos;

		transform.localScale = Vector3.one * (board.boardSize + backgroundBorderThickness);
	}
}
