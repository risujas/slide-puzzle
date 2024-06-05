using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
	[SerializeField] private PuzzleBoardSlot puzzleSlotPrefab;
	[SerializeField] private PuzzleBoardTile puzzleTilePrefab;
	[SerializeField] private PuzzleBoardBackground puzzleBoardBackgroundPrefab;

	[SerializeField] private RandomSoundPlayer tileMotionSoundPlayer;
	[SerializeField] private RandomSoundPlayer popSoundPlayer;
	[SerializeField] private RandomSoundPlayer bellSoundPlayer;

	[SerializeField] private LayerMask tileLayerMask;
	[SerializeField] private LayerMask slotLayerMask;

	private const float tileMovementSpeed = 0.2f;

	private PuzzleBoardSlot[] puzzleBoardSlots;
	private PuzzleBoardSlot finalTileSlot;
	private PuzzleBoardBackground background;
	private Vector2Int tileSize;

	private bool enableInteraction;
	private bool isMovingTiles;
	private bool puzzleIsCompleted;
	private bool puzzleWasCompletedThisFrame;

	private int finalTileAnimationStage = 0;
	private float finalTileDistanceThreshold = 0.035f;

	public int boardSize { get; private set; }

	public Texture2D currentGraphic { get; private set; }

	private Vector3 finalTileStage1TargetPos
	{
		get
		{
			return transform.position + (Vector3.right * (boardSize + 0.5f));
		}
	}

	public void InitializeBoard(Texture2D texture, int size, int numMoves)
	{
		boardSize = size;

		var tiles = CreateTilesFromTexture(texture);
		CreateSlotsForTiles(tiles);

		CenterBoardOnWorldOrigin();

		background = Instantiate(puzzleBoardBackgroundPrefab, transform);
		background.SetBackgroundTransform(this);

		InsertTilesToSlots(tiles);
		SetEmptyCornerTile();
		StartCoroutine(ShuffleBoard(numMoves, 0.00f));
	}

	private void CenterBoardOnWorldOrigin()
	{
		Vector3 pos = Vector3.zero;
		pos.x = -(boardSize / 2.0f) + 0.5f;
		pos.y = -(boardSize / 2.0f) + 0.5f;
		transform.position = pos;
	}

	private IEnumerator MoveTileBetweenSlots(PuzzleBoardSlot originSlot, PuzzleBoardSlot destinationSlot, float duration, bool playSound)
	{
		if (!isMovingTiles)
		{
			var tile = originSlot.InsertedTile;

			if (tile != null)
			{
				isMovingTiles = true;

				if (playSound)
				{
					tileMotionSoundPlayer.Play();
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
			}

			isMovingTiles = false;
		}

		yield return null;
	}

	private PuzzleBoardSlot GetSlotByCoordinates(int x, int y)
	{
		return puzzleBoardSlots[x + y * boardSize];
	}

	private List<PuzzleBoardSlot> GetAdjacentSlots(PuzzleBoardSlot firstSlot)
	{
		return GetAdjacentSlots(firstSlot, null);
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

	private void CreateSlotsForTiles(PuzzleBoardTile[] unshuffledTiles)
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

	private PuzzleBoardTile[] CreateTilesFromTexture(Texture2D source)
	{
		tileSize.x = source.width / boardSize;
		tileSize.y = source.height / boardSize;

		var puzzleTiles = new PuzzleBoardTile[boardSize * boardSize];

		for (int y = 0; y < boardSize; y++)
		{
			for (int x = 0; x < boardSize; x++)
			{
				var tile = Instantiate(puzzleTilePrefab).GetComponent<PuzzleBoardTile>();
				tile.Initialize(source, new Vector2Int(x, y), tileSize, boardSize);

				puzzleTiles[x + y * boardSize] = tile;
			}
		}

		return puzzleTiles;
	}

	private IEnumerator ShuffleBoard(int numMoves, float moveInterval)
	{
		enableInteraction = false;
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

		enableInteraction = true;
	}


	private void InsertTilesToSlots(PuzzleBoardTile[] tiles)
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
		finalTileSlot = puzzleBoardSlots[boardSize - 1];
		finalTileSlot.SetEmpty();

		finalTileSlot.CorrectTile.gameObject.SetActive(true);
		finalTileSlot.CorrectTile.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		finalTileSlot.CorrectTile.GetComponentInChildren<Canvas>().sortingLayerName = "Default";
	}

	private void HandleRegularTileInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 100.0f, slotLayerMask);
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

	private void HandleFinalTileInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 100.0f, tileLayerMask);
			if (hit.collider != null)
			{
				var tile = hit.collider.GetComponent<PuzzleBoardTile>();
				if (tile == finalTileSlot.CorrectTile)
				{
					finalTileAnimationStage = 1;
					tileMotionSoundPlayer.Play();
				}
			}
		}
	}

	private void CheckForCompletion()
	{
		bool wasAlreadyCompleted = puzzleIsCompleted;

		foreach (var s in puzzleBoardSlots)
		{
			if (!s.HasCorrectTile && !s.IsEmpty)
			{
				puzzleIsCompleted = false;
				return;
			}
		}

		puzzleIsCompleted = true;
		puzzleWasCompletedThisFrame = !wasAlreadyCompleted && puzzleIsCompleted;
	}

	private void LerpFinalTile()
	{
		var finalTile = finalTileSlot.CorrectTile;
		Vector3 finalTileTargetPos;
		Quaternion finalTileTargetRot = Quaternion.identity;
		float lerpSpeedModifier = 1.0f;

		if (finalTileAnimationStage == 1)
		{
			finalTileTargetPos = finalTileStage1TargetPos;
			lerpSpeedModifier = 2.0f;
		}
		else if (finalTileAnimationStage == 2)
		{
			finalTileTargetPos = finalTileSlot.transform.position;
			lerpSpeedModifier = 3.0f;
		}
		else
		{
			if (puzzleIsCompleted)
			{
				finalTileTargetPos = transform.position + (Vector3.right * boardSize) - (Vector3.right * 0.5f);
				finalTileTargetRot = Quaternion.Euler(0f, 0f, -45.0f);
			}
			else
			{
				finalTileTargetPos = finalTileSlot.transform.position;
			}
		}

		finalTile.transform.position = Vector3.Lerp(finalTile.transform.position, finalTileTargetPos, Time.deltaTime * 4.0f * lerpSpeedModifier);
		finalTile.transform.rotation = Quaternion.Lerp(finalTile.transform.rotation, finalTileTargetRot, Time.deltaTime * 5.0f * lerpSpeedModifier);
	}

	private void HandleFinalTileStages()
	{
		var finalTile = finalTileSlot.CorrectTile;

		if (finalTileAnimationStage == 1)
		{
			float distance = Vector3.Distance(finalTile.transform.position, finalTileStage1TargetPos);
			if (distance < finalTileDistanceThreshold)
			{
				finalTileAnimationStage = 2;
				finalTile.GetComponent<SpriteRenderer>().sortingLayerName = "PuzzleBoardTiles";
				finalTileSlot.CorrectTile.GetComponentInChildren<Canvas>().sortingLayerName = "PuzzleBoardTiles";
				tileMotionSoundPlayer.Play();
			}
		}

		if (finalTileAnimationStage == 2)
		{
			float distance = Vector3.Distance(finalTile.transform.position, finalTileSlot.transform.position);
			if (distance < finalTileDistanceThreshold)
			{
				bellSoundPlayer.Play();
				enableInteraction = false;
				StartCoroutine(LerpToPosition.Lerp(finalTile.gameObject, finalTileSlot.transform.position, 0.001f, 10.0f));
			}
		}
	}

	private void Update()
	{
		if (enableInteraction)
		{
			if (finalTileAnimationStage == 0)
			{
				HandleRegularTileInput();
				CheckForCompletion();

				if (puzzleIsCompleted)
				{
					HandleFinalTileInput();

					if (puzzleWasCompletedThisFrame)
					{
						popSoundPlayer.Play();
					}
				}
			}
			else
			{
				HandleFinalTileStages();
			}

			LerpFinalTile();
		}
	}
}
