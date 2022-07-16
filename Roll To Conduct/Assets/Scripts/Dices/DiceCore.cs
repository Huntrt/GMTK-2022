using UnityEngine;
using System;

public class DiceCore : MonoBehaviour
{
	[Range(1,6)] public int action;
	[Range(1,6)] public int punish;

	public int Roll()
	{
		return UnityEngine.Random.Range(0,6);
	}
}