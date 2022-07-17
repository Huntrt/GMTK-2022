using System.Collections.Generic;
using System.Collections;
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
	public Action playerAttack;
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
		if(Input.GetKeyDown(KeyManager.i.continueRoll)) RollDice();
		if(Input.GetKeyDown(KeyManager.i.conductAction)) PlayerEndTurn(true);
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
		//If queue are out of count or it not player turn
		if(queues.Count <= 0 || turn != 0) return;
		rolled = true;
		//For every dice in queue
		for (int d = 0; d < queues.Count; d++)
		{
			int rolled = queues[d].Roll();
			GameObject diceDisplay = queueInterface.GetChild(d).GetChild(2).gameObject;
			//Stop if queue rolled into punish
			if(rolled <= queues[d].punish) 
			{
				//? Punish effect
			}
			//Update the dice icon of this queue's interfact to be the number has roll
			diceDisplay.GetComponent<Image>().sprite = diceIcon[rolled-1];
			diceDisplay.SetActive(true);
		}
	}

	public void PlayerEndTurn(bool successful)
	{
		//Stop if not player turn
		if(turn != 0) return;
		//Nobody turn
		turn = -1;
		//Only allow player to attack if successful end turn
		if(successful) {playerAttack?.Invoke();}
		//Begin switch turn to the frist enemy with slight delay
		StartCoroutine(SwitchTurn(1,1));
	}

	public IEnumerator SwitchTurn(int order, float delay = 0)
	{
		yield return new WaitForSeconds(delay);
		turn = order;
		List<Enemy> enemies = EnemyManager.i.enemies;
		//? Player turn
		if(order == 0)
		{
			rolled = false;
			//Active all enemy
			for (int e = 0; e < enemies.Count; e++) {enemies[e].gameObject.SetActive(true);}
			//Deactive all the dice
			for (int q = 0; q < queueInterface.childCount; q++)
			{
				queueInterface.GetChild(q).GetChild(2).gameObject.SetActive(false);
			}
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
				//Back to the player turn
				StartCoroutine(SwitchTurn(0)); yield break;
			}
			//Go through all enemy
			for (int e = 0; e < enemies.Count; e++) 
			{
				//Deatice all the hurt display of enemy
				enemies[e].totalHurtDisplay.transform.parent.gameObject.SetActive(false);
			}
			//The enemy in this turn
			Enemy turn = enemies[order-1];
			//Move indicator to the enemy got turn
			turnIndicator.position = (Vector2)turn.transform.position + turnIndicatorOffset;
			//Begin turn of the enemy at given order
			turn.TakeTurn(order);
		}
	}
}
