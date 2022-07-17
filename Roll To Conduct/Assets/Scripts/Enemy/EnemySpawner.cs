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
	[SerializeField] Transform spawnZone;
	EnemyManager em;

	void OnEnable() {em = EnemyManager.i;}

	public void SpawnDecide(int extraAmount = 0)
	{
		//Get total amount of extra and initial
		float totalAmount = initialAmount + extraAmount;
		//Get the amount of enemy will spawn using total amount scale with difficulty
		float amount = Random.Range(1f, totalAmount + (totalAmount * em.difficulty));
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
		//Set enemy order from nearest to furthest
		SetEnemyOrder();
	}

	void SetEnemyOrder()
	{
		List<Enemy> tempEnemy = new List<Enemy>(em.enemies);
		//Go through all the enemy
		for (int e = 0; e < tempEnemy.Count; e++)
		{
			//The nearest enemy index
			int nearestEnemy = -1;
			//Save the latest nearest position
			float nearest = 10000;
			//Go through all the enemy again but start at this enemy
			for (int n = 0; n < em.enemies.Count; n++)
			{
				//Get distance between this temp nemy and player
				float distance = Vector2.Distance(Player.i.transform.position,em.enemies[n].transform.position);
				//If the an new nearest distance
				if(distance < nearest)
				{
					//Overwrite current nearest distance
					nearest = distance;
					//Nearest enemy are now in this index
					nearestEnemy = n;
				}
			}
			//This temp enemy are now the use nearest enemy in it own list
			tempEnemy[e] = em.enemies[nearestEnemy];
			//Remove the nearst enemy from it list
			em.enemies.RemoveAt(nearestEnemy);
		}
		//Overwrite enemy with temp enemies
		em.enemies = tempEnemy;
	}
}
