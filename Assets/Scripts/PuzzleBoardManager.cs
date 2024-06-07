using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleBoardManager : MonoBehaviour
{
	[SerializeField] private PuzzleBoard puzzleBoardPrefab;
	[SerializeField] private EnvironmentBackground environmentBackground;
	[SerializeField] private int boardSize = 3;

	private const string puzzleArtFolder = "Textures/PuzzleArt";
	private List<Texture2D> puzzleGraphics = new List<Texture2D>();
	private PuzzleBoard puzzleBoard;

	public IList<Texture2D> PuzzleGraphics => puzzleGraphics.AsReadOnlyList();

	public void CreatePuzzleWithRandomGraphic()
	{
		Texture2D randomGraphic = GetRandomTexture();
		CreatePuzzleWithGraphic(randomGraphic);
	}

	public void CreatePuzzleWithGraphic(Texture2D graphic)
	{
		if (puzzleBoard != null)
		{
			Destroy(puzzleBoard.gameObject);
		}

		puzzleBoard = Instantiate(puzzleBoardPrefab, transform).GetComponent<PuzzleBoard>();

		int numMoves = (int)Mathf.Pow(boardSize, 4.0f);
		puzzleBoard.InitializeBoard(graphic, boardSize, numMoves);

		environmentBackground.SetBackground(graphic);

		SetCameraSize(1.0f);
	}

	private void LoadTextures()
	{
		puzzleGraphics = Resources.LoadAll<Texture2D>(puzzleArtFolder).ToList();
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
		CreatePuzzleWithRandomGraphic();
	}
}
