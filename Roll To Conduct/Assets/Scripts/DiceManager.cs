using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
	///!!! FOLLOW DICE TYPE ENUM ORDER
	public GameObject[] dicePrefab;

    //Set this class to singleton
	public static DiceManager i {get{if(_i==null){_i = GameObject.FindObjectOfType<DiceManager>();}return _i;}} static DiceManager _i;

	public DiceCore GetDice(DiceType type)
	{
		//Return the dice prefab that has the same index as given
		return dicePrefab[(int)type].GetComponent<DiceCore>();
	}
}

public enum DiceType
{
	knife, bow, pellet,
	sword, crossbow, cannon,
	shield, flask
}
