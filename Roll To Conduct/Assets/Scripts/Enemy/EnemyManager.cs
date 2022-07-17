using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
	public float difficulty;
	//The diffculty will be increase each level
	public float difficultyIncrease;
	//How many level for difficulty increase will itself
	public float difficultyFrequent;
	public List<Enemy> enemies;
	public Action onCleared;
	public EnemySpawner spawner;
    //Set this class to singleton
    public static EnemyManager i {get{if(_i==null){_i = GameObject.FindObjectOfType<EnemyManager>();}return _i;}} static EnemyManager _i;

	public Enemy SpawnEnemy(GameObject enemy, Vector2 position)
	{
		//Create given enemy at given position
		Enemy spawned = Instantiate(enemy, position, Quaternion.identity).GetComponent<Enemy>();
		//Add the spawned enemy into list
		enemies.Add(spawned);
		//Return the spawned enemy
		return spawned;
	}

	public void IncreaseDifficulty()
	{
		difficulty += difficultyIncrease;
		//Each time level count reach frequent
		if((LevelManager.i.levelCount / difficultyFrequent) % 1 == 0)
		{
			//Diffcult now will be increase even more
			difficultyIncrease += difficultyIncrease;
		}
	}
}