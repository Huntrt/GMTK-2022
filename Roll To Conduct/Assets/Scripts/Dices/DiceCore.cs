using UnityEngine;
using System;

public class DiceCore : MonoBehaviour
{
	public DiceType type;
	[Range(1,6)] public int action;
	[Range(1,6)] public int punish;
	[Header("UI")]
	public Sprite icon;
	[TextArea(5,20)] public string description;
	public delegate void Rolled(int roll);
	public Rolled onAction, onPunish;

	public int Roll()
	{
		int result = UnityEngine.Random.Range(1,7);	
		if(result >= action) 
		{
			onAction?.Invoke(result);
		}
		else
		{
			onPunish?.Invoke(result);
		}
		return result;
	}
}