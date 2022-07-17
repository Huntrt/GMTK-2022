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
	EnemyManager em;
	[SerializeField] Vector2 turnIndicatorOffset;
	[SerializeField] Transform turnIndicator;

    //Set this class to singleton
	public static Combat i {get{if(_i==null){_i = GameObject.FindObjectOfType<Combat>();}return _i;}} static Combat _i;

	void Start()
	{
		em = EnemyManager.i;
	}

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
		//Begin switch turn to the frist enemy
		SwitchTurn(1);
	}

	public void SwitchTurn(int order)
	{
		List<Enemy> enemies = EnemyManager.i.enemies;
		//? Player turn
		if(order == 0)
		{
			//Active all enemy
			for (int e = 0; e < enemies.Count; e++) {enemies[e].gameObject.SetActive(true);}
			//Hide player hurt couter
			Player.i.hurtCounter.transform.parent.gameObject.SetActive(false);
			turnIndicator.position = (Vector2)Player.i.transform.position + turnIndicatorOffset;
		}
		//? Enemy turn
		else
		{
			//Display player hurt couter
			Player.i.hurtCounter.transform.parent.gameObject.SetActive(true);
			//If the order are past the last enemy
			if(order > enemies.Count)
			{
				//back to the player turn
				SwitchTurn(0); return;
			}
			//Go through all enemy
			for (int e = 0; e < enemies.Count; e++) 
			{
				//Deatice all the hurt display of enemy
				enemies[e].totalHurtDisplay.transform.parent.gameObject.SetActive(false);
				//Deactive all of them
				enemies[e].gameObject.SetActive(false);
			}
			//The enemy in this turn
			Enemy turn = enemies[order-1];
			//Only actice the enemy got turn
			turn.gameObject.SetActive(true);
			//Move indicator to the enemy got turn
			turnIndicator.position = (Vector2)turn.transform.position + turnIndicatorOffset;
			//Begin turn of the enemy at given order
			turn.TakeTurn(order);
		}
	}
}
