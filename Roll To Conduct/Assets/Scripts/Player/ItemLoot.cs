using System.Collections.Generic;
using UnityEngine;

public class ItemLoot : MonoBehaviour
{
	public List<WeaponDrop> drops;
	[System.Serializable] public class WeaponDrop
	{
		public float rate;
		public DiceType weapon;
	}
	//Set this class to singleton
	public static ItemLoot i {get{if(_i==null){_i = GameObject.FindObjectOfType<ItemLoot>();}return _i;}} static ItemLoot _i;

	public void DropItem()
	{
		//Get the sum of all drop rate
		float sum = 0; for (int w = 0; w < drops.Count; w++) sum += drops[w].rate;
		//Get the chance to drop
		float chance = Random.Range(0, sum);
		//For each of the drop
		for (int d = 0; d < drops.Count; d++)
		{
			//If this rate can use all chance
			if((chance - drops[d].rate) <= 0)
			{
				//Player got the dropped weapon
				Player.i.inventory.AddDice(drops[d].weapon); return;
			}
			else
			{
				//Chance will lose this rate
				chance -= drops[d].rate;
			}
		}
	}
}
