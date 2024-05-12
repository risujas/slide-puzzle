using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
	[SerializeField] private List<Texture2D> availableTextures = new List<Texture2D>();
	[SerializeField] private GameObject puzzleSlotPrefab;
	[SerializeField] private GameObject puzzleTilePrefab;
	[SerializeField] private Vector2Int boardSize;

	private PuzzleBoardSlot[] puzzleBoardSlots;
	private Vector2Int tileSize;

	private PuzzleBoardSlot GetSlotByCoordinates(int x, int y)
	{
		return puzzleBoardSlots[x + y * boardSize.x];
	}

	private PuzzleBoardSlot GetAdjacentEmptySlot(PuzzleBoardSlot firstSlot)
	{
		int[] offsetX = { -1, 1, 0, 0 };
		int[] offsetY = { 0, 0, -1, 1 };

		for (int i = 0; i < offsetX.Length; i++)
		{
			int secondSlotX = firstSlot.GridCoordinates.x + offsetX[i];
			int secondSlotY = firstSlot.GridCoordinates.y + offsetY[i];

			if (secondSlotX >= 0 && secondSlotX < boardSize.x && secondSlotY >= 0 && secondSlotY < boardSize.y)
			{
				PuzzleBoardSlot secondSlot = GetSlotByCoordinates(secondSlotX, secondSlotY);
				if (secondSlot.TreatAsEmpty)
				{
					return secondSlot;
				}
			}
		}

		return null;
	}

	private void CreateSlotsForTiles(PuzzleTile[] unshuffledTiles)
	{
		puzzleBoardSlots = new PuzzleBoardSlot[boardSize.x * boardSize.y];

		for (int y = 0; y < boardSize.y; y++)
		{
			for (int x = 0; x < boardSize.x; x++)
			{
				int index = x + y * boardSize.x;
				puzzleBoardSlots[index] = Instantiate(puzzleSlotPrefab).GetComponent<PuzzleBoardSlot>();
				puzzleBoardSlots[index].Prepare(this, unshuffledTiles[index], new Vector2Int(x, y));
			}
		}
	}

	private PuzzleTile[] CreateTilesFromTexture(Texture2D source)
	{
		tileSize.x = source.width / boardSize.x;
		tileSize.y = source.height / boardSize.y;

		var puzzleTiles = new PuzzleTile[boardSize.x * boardSize.y];

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

		return puzzleTiles;
	}

	private void ShuffleTiles(ref PuzzleTile[] tiles)
	{
		System.Random random = new System.Random();
		tiles = tiles.OrderBy(a => random.Next()).ToArray();
	}

	private void InsertTilesToSlots(PuzzleTile[] tiles)
	{
		for (int y = 0; y < boardSize.y; y++)
		{
			for (int x = 0; x < boardSize.x; x++)
			{
				puzzleBoardSlots[x + y * boardSize.x].InsertTile(tiles[x + y * boardSize.x]);
			}
		}
	}

	private void CreateEmptySlot()
	{
		int index = Random.Range(0, boardSize.x * boardSize.y);
		puzzleBoardSlots[index].SetEmpty(true);
	}

	private void Start()
	{
		var tiles = CreateTilesFromTexture(availableTextures[0]);
		CreateSlotsForTiles(tiles);
		ShuffleTiles(ref tiles);
		InsertTilesToSlots(tiles);
		CreateEmptySlot();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
			if (hit.collider != null)
			{
				var slot = hit.collider.GetComponentInParent<PuzzleBoardSlot>();
				if (slot)
				{
					Debug.Log(GetAdjacentEmptySlot(slot));
				}
			}
		}
	}
}
