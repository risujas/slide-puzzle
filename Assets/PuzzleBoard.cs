using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
	[SerializeField] private List<Texture2D> availableTextures = new List<Texture2D>();
	[SerializeField] private GameObject puzzleSlotPrefab;
	[SerializeField] private GameObject puzzleTilePrefab;
	[SerializeField] private Vector2Int boardSize;

	private PuzzleBoardSlot[,] puzzleBoardSlots;
	private PuzzleTile[] puzzleTiles;
	private Vector2Int tileSize;

	private void InitializeBoard()
	{
		puzzleBoardSlots = new PuzzleBoardSlot[boardSize.x, boardSize.y];

		for (int y = 0; y < boardSize.y; y++)
		{
			for (int x = 0; x < boardSize.x; x++)
			{
				puzzleBoardSlots[x, y] = Instantiate(puzzleSlotPrefab).GetComponent<PuzzleBoardSlot>();
				puzzleBoardSlots[x, y].Prepare(this, puzzleTiles[x + y * boardSize.x], new Vector2Int(x, y));
			}
		}
	}

	private void CreateTilesFromTexture(Texture2D source)
	{
		tileSize.x = source.width / boardSize.x;
		tileSize.y = source.height / boardSize.y;

		puzzleTiles = new PuzzleTile[boardSize.x * boardSize.y];

		for (int y = 0; y < boardSize.y; y++)
		{
			for (int x = 0; x < boardSize.x; x++)
			{
				Vector2 cornerPixel = new Vector2Int(x * tileSize.x, y * tileSize.y);
				var rect = new Rect(cornerPixel.x, cornerPixel.y, tileSize.x, tileSize.y);
				var sprite = Sprite.Create(source, rect, new Vector2(0.5f, 0.5f), tileSize.x);

				var tile = Instantiate(puzzleTilePrefab, Vector3.zero, Quaternion.identity).GetComponent<PuzzleTile>();
				tile.name = "PuzzleTile_" + x + "_" + y;
				tile.GetComponent<SpriteRenderer>().sprite = sprite;
				tile.gameObject.SetActive(false);
				puzzleTiles[x + y * boardSize.x] = tile;
			}
		}
	}

	private void ShuffleTiles()
	{
		System.Random random = new System.Random();
		puzzleTiles = puzzleTiles.OrderBy(a => random.Next()).ToArray();
	}

	private void InsertTiles()
	{
		for (int y = 0; y < boardSize.y; y++)
		{
			for (int x = 0; x < boardSize.x; x++)
			{
				puzzleBoardSlots[x, y].InsertTile(puzzleTiles[x + y * boardSize.x]);
			}
		}
	}

	private void Start()
	{
		CreateTilesFromTexture(availableTextures[0]);
		InitializeBoard();
		ShuffleTiles();
		InsertTiles();
	}
}
