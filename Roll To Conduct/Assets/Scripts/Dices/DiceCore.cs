using UnityEngine;

public class DiceCore : MonoBehaviour
{
	public DiceType type;
	[Range(1,6)] public int action;
	[Range(1,6)] public int punish;
	[Header("UI")]
	public Sprite icon;
	[TextArea(5,20)] public string description;

	public int Roll()
	{
		return UnityEngine.Random.Range(0,6);
	}
}