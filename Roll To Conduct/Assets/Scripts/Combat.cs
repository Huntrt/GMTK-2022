using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Combat : MonoBehaviour
{
	//? 0 = player | 1+ = enemy
	public int turn;
	public List<DiceCore> queues;
	public Transform queueInterface;
	public bool rolled;
	public Action onEndTurn;
	public Sprite[] diceIcon;

    //Set this class to singleton
	public static Combat i {get{if(_i==null){_i = GameObject.FindObjectOfType<Combat>();}return _i;}} static Combat _i;

	void Update()
	{
		//! Custom input
		if(Input.GetKeyDown(KeyCode.R))
		{
			RollDice();
		}
	}

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

	public void RollDice()
	{
		if(queues.Count <= 0) return;
		rolled = true;
		//For eavery dice in queue
		for (int d = 0; d < queues.Count; d++)
		{
			int rolled = queues[d].Roll();
			GameObject diceDisplay = queueInterface.GetChild(d).GetChild(2).gameObject;
			//Update the dice icon of this queue's interfact to be the number has roll
			diceDisplay.GetComponent<Image>().sprite = diceIcon[rolled-1];
			diceDisplay.SetActive(true);
		}
	}

	public void ClearCombatQueue()
	{
		queues.Clear();
		for (int q = 0; q < queueInterface.childCount; q++)
		{
			queueInterface.GetChild(q).gameObject.SetActive(false);
		}
	}

	public void PlayerEndTurn()
	{
		onEndTurn?.Invoke();
		rolled = false;
		//Deactive all the dice
		for (int q = 0; q < queueInterface.childCount; q++)
		{
			queueInterface.GetChild(q).GetChild(2).gameObject.SetActive(false);
		}
	}
}
