using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyManager : MonoBehaviour
{
	public float difficulty;
	public List<Enemy> enemies;
    //Set this class to singleton
    public static EnemeyManager i {get{if(_i==null){_i = GameObject.FindObjectOfType<EnemeyManager>();}return _i;}} static EnemeyManager _i;

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