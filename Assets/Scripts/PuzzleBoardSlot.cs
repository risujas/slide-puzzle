using UnityEngine;

public class PuzzleBoardSlot : MonoBehaviour
{
	public PuzzleBoardTile CorrectTile { get; private set; }
	public PuzzleBoardTile InsertedTile { get; private set; }
	public Vector2Int GridCoordinates { get; private set; }
	public bool IsEmpty => InsertedTile == null;
	public bool HasCorrectTile => InsertedTile == CorrectTile;

	public void Initialize(PuzzleBoard board, PuzzleBoardTile correctTile, Vector2Int gridCoordinates)
	{
		name = "PuzzleBoardSlot_" + gridCoordinates.x + "_" + gridCoordinates.y;

		transform.parent = board.transform;
		transform.localPosition = (Vector3.right * gridCoordinates.x) + (Vector3.up * gridCoordinates.y);

		CorrectTile = correctTile;
		GridCoordinates = gridCoordinates;
	}

	public void InsertTile(PuzzleBoardTile tile)
	{
		InsertedTile = tile;
		InsertedTile.gameObject.SetActive(true);

		InsertedTile.transform.position = transform.position;
		InsertedTile.transform.parent = transform;
	}
	public void InsertCorrectTile()
	{
		InsertTile(CorrectTile);
	}

	public void SetEmpty()
	{
		InsertedTile.gameObject.SetActive(false);
		InsertedTile = null;
	}
}