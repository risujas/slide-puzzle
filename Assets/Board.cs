using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
	[SerializeField] private List<Texture2D> availableTextures = new List<Texture2D>();
	[SerializeField] private GameObject tilePrefab;

	[SerializeField] private int numTilesX = 10;
	[SerializeField] private int numTilesY = 10;

	private int sourceWidth;
	private int sourceHeight;

	private Tile[] tiles;

	private int tileSizeX;
	private int tileSizeY;

	private void SplitTextureToSprites(Texture2D source)
	{
		sourceWidth = source.width;
		sourceHeight = source.height;

		tileSizeX = sourceWidth / numTilesX;
		tileSizeY = sourceHeight / numTilesY;

		tiles = new Tile[numTilesX * numTilesY];

		for (var y = numTilesY - 1; y >= 0; y--)
		{
			for (var x = 0; x < numTilesX; x++)
			{
				var bottomLeftPixelX = x * tileSizeX;
				var bottomLeftPixelY = y * tileSizeY;

				var rect = new Rect(bottomLeftPixelX, bottomLeftPixelY, tileSizeX, tileSizeY);
				var sprite = Sprite.Create(source, rect, new Vector2(0.5f, 0.5f), 100.0f);

				var tile = Instantiate(tilePrefab).GetComponent<Tile>();
				tile.Initialize(sprite, new Vector2(x, y));
				tile.transform.position = new Vector2(x, y);
				tiles[x + y * numTilesX] = tile;
			}
		}
	}

	private void Start()
	{
		SplitTextureToSprites(availableTextures[0]);
	}
}
