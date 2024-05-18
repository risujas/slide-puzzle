using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
	[SerializeField] private List<Texture2D> availableTextures = new List<Texture2D>();
	[SerializeField] private GameObject puzzleSlotPrefab;
	[SerializeField] private GameObject puzzleTilePrefab;
	[SerializeField] private GameObject background;
	[SerializeField] private float backgroundBorderThickness = 0.2f;
	[SerializeField] private int boardSize = 3;
	[SerializeField] private float tileMovementSpeed = 0.2f;

	private PuzzleBoardSlot[] puzzleBoardSlots;
	private Vector2Int tileSize;

	private bool isShuffling = false;
	private bool isMovingTiles = false;

	private void CenterBoardOnWorldOrigin()
	{
		Vector3 pos = Vector3.zero;
		pos.x = -(boardSize / 2.0f) + 0.5f;
		pos.y = -(boardSize / 2.0f) + 0.5f;
		transform.position = pos;
	}

	private void SetCameraSize(float boardMargin)
	{
		Camera.main.orthographicSize = (boardSize / 2.0f) + boardMargin;
	}

	private void SetBackgroundTransform()
	{
		Vector3 pos = transform.position;
		pos.x -= 0.5f + (backgroundBorderThickness * 0.5f);
		pos.y -= 0.5f + (backgroundBorderThickness * 0.5f);
		background.transform.position = pos;

		background.transform.localScale = Vector3.one * (boardSize + backgroundBorderThickness);
	}

	private IEnumerator MoveTileBetweenSlots(PuzzleBoardSlot originSlot, PuzzleBoardSlot destinationSlot, float duration, bool playSound)
	{
		if (!isMovingTiles)
		{
			isMovingTiles = true;
			var tile = originSlot.InsertedTile;

			if (playSound)
			{
				tile.PlayMotionSound();
			}

			float t = 0.0f;
			while (t < duration)
			{
				t += Time.deltaTime;
				float ratio = t / duration;

				tile.transform.position = Vector3.Lerp(originSlot.transform.position, destinationSlot.transform.position, ratio);
				yield return null;
			}

			originSlot.SetEmpty();
			destinationSlot.InsertTile(tile);

			isMovingTiles = false;
		}

		yield return null;
	}

	private PuzzleBoardSlot GetEmptySlot()
	{
		foreach (var s in puzzleBoardSlots)
		{
			if (s.IsEmpty)
			{
				return s;
			}
		}
		return null;
	}

	private PuzzleBoardSlot GetSlotByCoordinates(int x, int y)
	{
		return puzzleBoardSlots[x + y * boardSize];
	}

	private List<PuzzleBoardSlot> GetAdjacentSlots(PuzzleBoardSlot firstSlot, PuzzleBoardSlot ignoredSlot)
	{
		List<PuzzleBoardSlot> adjacentSlots = new List<PuzzleBoardSlot>();

		int[] offsetX = { -1, 1, 0, 0 };
		int[] offsetY = { 0, 0, -1, 1 };

		for (int i = 0; i < offsetX.Length; i++)
		{
			int secondSlotX = firstSlot.GridCoordinates.x + offsetX[i];
			int secondSlotY = firstSlot.GridCoordinates.y + offsetY[i];

			if (secondSlotX >= 0 && secondSlotX < boardSize && secondSlotY >= 0 && secondSlotY < boardSize)
			{
				PuzzleBoardSlot secondSlot = GetSlotByCoordinates(secondSlotX, secondSlotY);
				if (secondSlot == ignoredSlot)
				{
					continue;
				}

				adjacentSlots.Add(secondSlot);
			}
		}

		if (adjacentSlots.Count == 0)
		{
			adjacentSlots = null;
		}

		return adjacentSlots;
	}

	private List<PuzzleBoardSlot> GetAdjacentSlots(PuzzleBoardSlot firstSlot)
	{
		return GetAdjacentSlots(firstSlot, null);
	}

	private PuzzleBoardSlot GetAdjacentEmptySlot(PuzzleBoardSlot firstSlot)
	{
		var adjacentSlots = GetAdjacentSlots(firstSlot);
		foreach (var slot in adjacentSlots)
		{
			if (slot.IsEmpty)
			{
				return slot;
			}
		}

		return null;
	}

	private void CreateSlotsForTiles(PuzzleTile[] unshuffledTiles)
	{
		puzzleBoardSlots = new PuzzleBoardSlot[boardSize * boardSize];

		for (int y = 0; y < boardSize; y++)
		{
			for (int x = 0; x < boardSize; x++)
			{
				int index = x + y * boardSize;
				puzzleBoardSlots[index] = Instantiate(puzzleSlotPrefab).GetComponent<PuzzleBoardSlot>();
				puzzleBoardSlots[index].Initialize(this, unshuffledTiles[index], new Vector2Int(x, y));
			}
		}
	}

	private PuzzleTile[] CreateTilesFromTexture(Texture2D source)
	{
		tileSize.x = source.width / boardSize;
		tileSize.y = source.height / boardSize;

		var puzzleTiles = new PuzzleTile[boardSize * boardSize];

		for (int y = 0; y < boardSize; y++)
		{
			for (int x = 0; x < boardSize; x++)
			{
				var tile = Instantiate(puzzleTilePrefab).GetComponent<PuzzleTile>();
				tile.Initialize(source, new Vector2Int(x, y), tileSize);

				puzzleTiles[x + y * boardSize] = tile;
			}
		}

		return puzzleTiles;
	}

	private IEnumerator ShuffleBoard(int seed, int numMoves, float moveInterval)
	{
		isShuffling = true;
		Random.InitState(seed);
		PuzzleBoardSlot previousEmptySlot = null;

		for (int i = 0; i < numMoves; i++)
		{
			while (isMovingTiles)
			{
				yield return null;
			}

			var emptySlot = GetEmptySlot();
			var adjacentSlots = GetAdjacentSlots(emptySlot, previousEmptySlot);
			var randomAdjacent = adjacentSlots[Random.Range(0, adjacentSlots.Count)];
			previousEmptySlot = emptySlot;

			StartCoroutine(MoveTileBetweenSlots(randomAdjacent, emptySlot, moveInterval, false));
		}

		bool emptySlotAtCorrectPosition = false;
		while (!emptySlotAtCorrectPosition)
		{
			while (isMovingTiles)
			{
				yield return null;
			}

			var emptySlot = GetEmptySlot();

			if (emptySlot.GridCoordinates.x < boardSize - 1)
			{
				var nextSlot = GetSlotByCoordinates(emptySlot.GridCoordinates.x + 1, emptySlot.GridCoordinates.y);
				StartCoroutine(MoveTileBetweenSlots(nextSlot, emptySlot, moveInterval, false));
			}
			else if (emptySlot.GridCoordinates.y > 0)
			{
				var nextSlot = GetSlotByCoordinates(emptySlot.GridCoordinates.x, emptySlot.GridCoordinates.y - 1);
				StartCoroutine(MoveTileBetweenSlots(nextSlot, emptySlot, moveInterval, false));
			}
			else
			{
				emptySlotAtCorrectPosition = true;
			}
		}

		isShuffling = false;
	}


	private void InsertTilesToSlots(PuzzleTile[] tiles)
	{
		for (int y = 0; y < boardSize; y++)
		{
			for (int x = 0; x < boardSize; x++)
			{
				puzzleBoardSlots[x + y * boardSize].InsertTile(tiles[x + y * boardSize]);
			}
		}
	}

	private void SetEmptyCornerTile()
	{
		puzzleBoardSlots[boardSize - 1].SetEmpty();
	}

	private void InitializeBoard(Texture2D texture)
	{
		var tiles = CreateTilesFromTexture(texture);
		CreateSlotsForTiles(tiles);

		CenterBoardOnWorldOrigin();
		SetCameraSize(1.0f);
		SetBackgroundTransform();

		InsertTilesToSlots(tiles);
		SetEmptyCornerTile();
		StartCoroutine(ShuffleBoard(0, 1000, 0.00f));
	}

	private void HandleInput()
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
					var adjacentEmptySlot = GetAdjacentEmptySlot(slot);
					if (adjacentEmptySlot && !isMovingTiles)
					{
						StartCoroutine(MoveTileBetweenSlots(slot, adjacentEmptySlot, tileMovementSpeed, true));
					}
				}
			}
		}
	}

	private bool CheckForCompletion()
	{
		foreach (var s in puzzleBoardSlots)
		{
			if (!s.HasCorrectTile && !s.IsEmpty)
			{
				return false;
			}
		}

		return true;
	}

	private void Start()
	{
		InitializeBoard(availableTextures[0]);
	}

	private void Update()
	{
		if (!isShuffling)
		{
			HandleInput();
			if (CheckForCompletion())
			{
				Debug.Log("Completed");
			}
		}
	}
}
