using UnityEngine;

public class PuzzleBoardSlot : MonoBehaviour
{
	private PuzzleTile correctTile;
	private Vector2Int gridCoordinates;
	private PuzzleTile insertedTile;

	public void Prepare(PuzzleBoard board, PuzzleTile correctTile, Vector2Int gridCoordinates)
	{
		name = "PuzzleBoardSlot_" + gridCoordinates.x + "_" + gridCoordinates.y;

		transform.parent = board.transform;
		transform.position = (Vector3.right * gridCoordinates.x) + (Vector3.up * gridCoordinates.y);

		this.correctTile = correctTile;
		this.gridCoordinates = gridCoordinates;
	}

	public void InsertTile(PuzzleTile tile)
	{
		insertedTile = tile;
		insertedTile.gameObject.SetActive(true);
		insertedTile.transform.position = transform.position;
		insertedTile.transform.parent = transform;
	}
}