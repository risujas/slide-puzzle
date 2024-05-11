using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleBoardSlot
{
	public PuzzleTile insertedTile;
	public Vector2Int gridCoordinates;
}

public class PuzzleBoard : MonoBehaviour
{
	[SerializeField] private List<Texture2D> availableTextures = new List<Texture2D>();
	[SerializeField] private GameObject puzzleTilePrefab;
	[SerializeField] private Vector2Int boardSize;

	private PuzzleBoardSlot[,] puzzleBoardSlots;
	private PuzzleTile[,] puzzleTiles;
	private Vector2Int tileSize;

	private void InitializeBoard()
	{
		puzzleBoardSlots = new PuzzleBoardSlot[boardSize.x, boardSize.y];

		for (int x = 0; x < boardSize.x; x++)
		{
			for (int y = 0; y < boardSize.y; y++)
			{
				puzzleBoardSlots[x, y] = new PuzzleBoardSlot();
				puzzleBoardSlots[x, y].gridCoordinates = new Vector2Int(x, y);
			}
		}
	}

	private void CreateTilesFromTexture(Texture2D source)
	{
		tileSize.x = source.width / boardSize.x;
		tileSize.y = source.height / boardSize.y;

		puzzleTiles = new PuzzleTile[boardSize.x, boardSize.y];

		for (int y = 0; y < boardSize.y; y++)
		{
			for (int x = 0; x < boardSize.x; x++)
			{
				Vector2 cornerPixel = new Vector2Int(x, y);

				var rect = new Rect(cornerPixel.x, cornerPixel.y, tileSize.x, tileSize.y);
				var sprite = Sprite.Create(source, rect, new Vector2(0.5f, 0.5f), tileSize.x);

				var tile = Instantiate(puzzleTilePrefab, Vector3.zero, Quaternion.identity).GetComponent<PuzzleTile>();
				tile.Initialize(sprite, new Vector2(x, y));
				puzzleTiles[x, y] = tile;
			}
		}
	}

	private void InsertTilesToBoard()
	{

	}

	private void Start()
	{
		InitializeBoard();
		CreateTilesFromTexture(availableTextures[0]);
		InsertTilesToBoard();
	}
}
