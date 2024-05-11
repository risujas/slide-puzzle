using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
	[SerializeField] private List<Texture2D> availableTextures = new List<Texture2D>();
	[SerializeField] private GameObject puzzleTilePrefab;

	[SerializeField] private int numTilesX = 7;
	[SerializeField] private int numTilesY = 7;

	private int sourceWidth;
	private int sourceHeight;

	private PuzzleTile[] puzzleTiles;
	private PuzzleTile missingPuzzleTile;

	private int tileSizeX;
	private int tileSizeY;

	private void CreateTilesFromTexture(Texture2D source)
	{
		sourceWidth = source.width;
		sourceHeight = source.height;

		tileSizeX = sourceWidth / numTilesX;
		tileSizeY = sourceHeight / numTilesY;

		puzzleTiles = new PuzzleTile[numTilesX * numTilesY];

		for (var y = numTilesY - 1; y >= 0; y--)
		{
			for (var x = 0; x < numTilesX; x++)
			{
				var bottomLeftPixelX = x * tileSizeX;
				var bottomLeftPixelY = y * tileSizeY;

				var rect = new Rect(bottomLeftPixelX, bottomLeftPixelY, tileSizeX, tileSizeY);
				var sprite = Sprite.Create(source, rect, new Vector2(0.5f, 0.5f), tileSizeX);

				var tile = Instantiate(puzzleTilePrefab).GetComponent<PuzzleTile>();
				tile.Initialize(sprite, new Vector2(x, y));
				tile.transform.position = new Vector2(x, y);
				puzzleTiles[x + y * numTilesX] = tile;
			}
		}
	}

	private void SetMissingTile()
	{
		int index = Random.Range(0, puzzleTiles.Length - 1);
		missingPuzzleTile = puzzleTiles[index];
		missingPuzzleTile.gameObject.SetActive(false);
		puzzleTiles[index] = null;
	}

	private void Start()
	{
		CreateTilesFromTexture(availableTextures[0]);
		SetMissingTile();
	}
}
