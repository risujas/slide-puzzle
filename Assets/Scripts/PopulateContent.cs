using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateContent : MonoBehaviour
{
	[SerializeField] private GameObject contentPrefab;
	[SerializeField] private PuzzleBoardManager puzzleBoardManager;

	private List<GameObject> content = new List<GameObject>();

	private void PopulateFromPuzzleManager()
	{
		var puzzleGraphics = puzzleBoardManager.PuzzleGraphics;
		foreach (var g in puzzleGraphics)
		{
			var newContent = Instantiate(contentPrefab, transform);
			newContent.name = g.name;

			Sprite newSprite = Sprite.Create(g, new Rect(0.0f, 0.0f, g.width, g.height), new Vector2(0.5f, 0.5f), 100.0f);
			newContent.GetComponent<Image>().sprite = newSprite;
		}
	}

	private void Start()
	{
		PopulateFromPuzzleManager();
	}
}
