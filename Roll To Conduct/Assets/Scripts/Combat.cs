using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Combat : MonoBehaviour
{
	public List<DiceCore> queues;
	public Transform queueInterface;

    //Set this class to singleton
	public static Combat i {get{if(_i==null){_i = GameObject.FindObjectOfType<Combat>();}return _i;}} static Combat _i;

	public void QueueDice(DiceCore dice)
	{
		//Maximum queue allow are 40
		if(queues.Count >= 30) {print("Maxxed Dice Queue"); return;}
		//Get the UI of queue index
		Transform queueUI = queueInterface.GetChild(queues.Count);
		//Add given dice to queue
		queues.Add(dice);
		//Update queue UI icon to match given dice icon
		queueUI.GetChild(0).GetComponent<Image>().sprite = dice.icon;
		//Active the queue ui
		queueUI.gameObject.SetActive(true);
	}

	public void UnqueueDice(DiceCore dice)
	{
		//Go trough all the queue to check if already queue given dice
		for (int q = 0; q < queues.Count; q++) if(queues[q] == dice)
		{
			//Go trough all the queue left
			for (int u = q; u < queues.Count; u++)
			{
				//Get the UI of queue index
				Transform queueUI = queueInterface.GetChild(u);
				//Deactive the last queue UI
				if(u == queues.Count-1) {queueUI.gameObject.SetActive(false); break;}
				//Push the previous queue UI icon to match current dice icon
				queueUI.GetChild(0).GetComponent<Image>().sprite = queues[u+1].icon;
			}
			//Remove dice out of queue
			queues.Remove(dice);
		}
	}
}
