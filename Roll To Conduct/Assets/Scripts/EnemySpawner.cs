using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public int initialAmount;
	[System.Serializable] public class SpawnInfo
	{
		public GameObject enemyPrefab;
		public int difficultyScaling;
	}
	public List<SpawnInfo> spawns;
	//% Testing
	[SerializeField] Transform spawnZone;
	EnemeyManager em;

	void OnEnable() {em = EnemeyManager.i;}

	//% Testing
	void Start()
	{
		SpawnDecide();
	}

	public void SpawnDecide()
	{
		//Get the amount of enemy will spawn using inital amount scale with difficulty
		float amount = Random.Range(1f, initialAmount + (initialAmount * em.difficulty));
		//For every time need to spawn enemy
		for (int e = 0; e < amount; e++)
		{
			//Get the total amount of difficulty scaling of all enemny
			float sum = 0; for(int s = 0; s < spawns.Count; s++) sum += spawns[s].difficultyScaling + (spawns[s].difficultyScaling * em.difficulty);
			//The total chance will be spawn enemy
			float chance = Random.Range(0, sum);
			//Go through all the enemy could spawn
			for (int s = 0; s < spawns.Count; s++)
			{	
				//Scaling the current difficulty of each spawn
				float scaling = spawns[s].difficultyScaling + (spawns[s].difficultyScaling * em.difficulty);
				//If this enemy scaling got all of the chance
				if((chance - scaling) <= 0)
				{
					//Randomly get spawn position inside the spawn zone
					Vector2 pos = new Vector2
					(
						spawnZone.position.x + Random.Range(-spawnZone.localScale.x, spawnZone.localScale.x)/2,
						spawnZone.position.y + Random.Range(-spawnZone.localScale.y, spawnZone.localScale.y)/2
					);
					//Spawn an enemy at position has get
					em.SpawnEnemy(spawns[s].enemyPrefab, pos);
					//No need to spawn more enemy
					break;
				}
				//Dexcrease chance if scaling still has chance
				else {chance -= scaling;}	
			}
		}
	}
}
