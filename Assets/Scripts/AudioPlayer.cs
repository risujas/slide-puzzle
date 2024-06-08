using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	[SerializeField] private RandomSoundPlayer tileSoundPlayer;
	[SerializeField] private RandomSoundPlayer popSoundPlayer;
	[SerializeField] private RandomSoundPlayer bellSoundPlayer;

	public RandomSoundPlayer TileSoundPlayer => tileSoundPlayer;
	public RandomSoundPlayer PopSoundPlayer => popSoundPlayer;
	public RandomSoundPlayer BellSoundPlayer => bellSoundPlayer;
}
