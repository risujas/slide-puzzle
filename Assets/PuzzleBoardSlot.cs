using UnityEngine;

public class PuzzleBoardSlot : MonoBehaviour
{
	public PuzzleTile correctTile;
	public Vector2Int gridCoordinates;

	private PuzzleTile insertedTile;

	public void InsertTile(PuzzleTile tile)
	{
		insertedTile = tile;
		insertedTile.gameObject.SetActive(true);
		insertedTile.transform.position = transform.position;
	}
}