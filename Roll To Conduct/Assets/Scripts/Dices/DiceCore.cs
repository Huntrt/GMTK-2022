using UnityEngine;

public class DiceCore : MonoBehaviour
{
	[Range(1,6)] public int action;
	[Range(1,6)] public int punish;
	[Header("UI")]
	public Sprite icon;
	public string description;

	public int Roll()
	{
		return UnityEngine.Random.Range(0,6);
	}
}