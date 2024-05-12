using UnityEngine;

public class PuzzleBoardSlot : MonoBehaviour
{
	private PuzzleTile correctTile;
	private PuzzleTile insertedTile;
	private Vector2Int gridCoordinates;
	private bool treatAsEmpty;

	public bool TreatAsEmpty => treatAsEmpty;

	public bool HasCorrectTile => (insertedTile == correctTile);

	public Vector2Int GridCoordinates => gridCoordinates;

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
		insertedTile.transform.position = transform.position;
		insertedTile.transform.parent = transform;
		SetEmpty(false);
	}

	public void SetEmpty(bool isEmpty)
	{
		treatAsEmpty = isEmpty;
		insertedTile.gameObject.SetActive(!treatAsEmpty);
	}
}