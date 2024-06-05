using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleBoardManager : MonoBehaviour
{
	[SerializeField] private PuzzleBoard puzzleBoardPrefab;
	[SerializeField] private EnvironmentBackground environmentBackground;
	[SerializeField] private int boardSize = 3;

	private List<Texture2D> puzzleGraphics = new List<Texture2D>();
	private PuzzleBoard puzzleBoard;

	public void CreatePuzzleBoard()
	{
		Texture2D nextGraphic = GetRandomTexture();

		if (puzzleBoard != null)
		{
			Destroy(puzzleBoard.gameObject);
		}

		puzzleBoard = Instantiate(puzzleBoardPrefab, transform).GetComponent<PuzzleBoard>();
		puzzleBoard.InitializeBoard(nextGraphic, boardSize);

		environmentBackground.SetBackground(nextGraphic);

		SetCameraSize(1.0f);
	}

	private void LoadTextures()
	{
		string folderPath = "Textures/PuzzleArt";
		puzzleGraphics = Resources.LoadAll<Texture2D>(folderPath).ToList();
	}

	private Texture2D GetRandomTexture()
	{
		List<Texture2D> availableGraphics = new List<Texture2D>();
		foreach (var graphic in puzzleGraphics)
		{
			if (puzzleBoard == null || graphic != puzzleBoard.currentGraphic)
			{
				availableGraphics.Add(graphic);
			}
		}

		return availableGraphics[Random.Range(0, availableGraphics.Count)];
	}

	private void SetCameraSize(float boardMargin)
	{
		Camera.main.orthographicSize = (boardSize / 2.0f) + boardMargin;
	}

	private void Start()
	{
		LoadTextures();
		CreatePuzzleBoard();
	}
}
