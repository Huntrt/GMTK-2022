using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public float difficulty;
	public List<Enemy> enemies;
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
}