using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
	public Heath heath;
    public float value;
	[SerializeField] float rollDelay;
	int turnOrder;
	[Header("Stats")]
	public int stackedDamage;
	public float damageScaling;
	public SpriteRenderer[] dices;
	float greed; [SerializeField] float intitialGreed, greedGrowth;
	[Header("Attack")]
	[SerializeField] GameObject attackPrefab; GameObject attackInstance;
	[SerializeField] Sprite attackSprite;
	[SerializeField] float attackVelocity;
	[Header("Take Damage")]
	public List<int> hurtQueue = new List<int>();
	public int totalHurt;
	public TextMeshProUGUI totalHurtDisplay;
	Transform playerTF; EnemyManager em;

	void Start()
	{
		UpdateHurtTake(false, "");
	}

	void OnEnable()
	{
		playerTF = Player.i.transform;
		em = EnemyManager.i;
		Combat.i.onEndTurn += GetAllHurt;
		heath.onDie += Die;
	}

	void OnDisable()
	{
		if(Combat.i != null) Combat.i.onEndTurn -= GetAllHurt;
		if(heath != null) heath.onDie -= Die;
	}

	void Update()
	{
		if(attackInstance != null && attackInstance.activeInHierarchy)
		{
			//Get the attack transform
			Transform atk = attackInstance.transform;
			//Make attack move toward player
			atk.position = Vector2.MoveTowards(atk.position, playerTF.position, attackVelocity*Time.deltaTime);
			if(atk.position == playerTF.position)
			{
				//Make player take the stack damage
				Player.i.heath.TakeDamage(stackedDamage);
				//Deactive the instance
				attackInstance.SetActive(false);
				//Go to the next turn
				NextTurn();
			}
		}
	}

	public void TakeTurn(int order)
	{
		turnOrder = order;
		greed = intitialGreed;
		//Reset the stacked damage of player
		Player.i.hurtCounter.text = "0"; 
		StartCoroutine(StackingDamage());
	}

	IEnumerator StackingDamage()
	{
		//Wat for an delay before roll
		yield return new WaitForSeconds(rollDelay);
		//Roll all the dice
		int rolled = RollDice();
		//Increase stacked damage by rolled amount multiply with combine scaling and difficulty 
		stackedDamage += (int)(rolled * (damageScaling * em.difficulty));
		//Display the stacked damage of player
		Player.i.hurtCounter.text = stackedDamage.ToString(); 
		//End this enemy turn if it roll are failed
		if(rolled == -1) 
		{
			Player.i.hurtCounter.text = "-1";
			//Delay an bit to see failed roll
			yield return new WaitForSeconds(rollDelay);
			NextTurn();
			yield break;
		}
		//If has stop greddy
		if(Random.Range(0f,100f) < greed)
		{
			//Delay an bit to see attack roll
			yield return new WaitForSeconds(rollDelay); 
			CreateAttack();
			yield break;
		}
		//Became less greddy on each roll
		greed += greed * greedGrowth;
		//Repat stack if still greddy
		StartCoroutine(StackingDamage());
	}

	public int RollDice()
	{
		int totalResult = 0;
		//For all the dice could rolled
		for (int d = 0; d < dices.Length; d++)
		{
			int result = Random.Range(1,7);
			//Update the sprite of dice base on result
			dices[d].sprite = Combat.i.diceIcon[result-1];
			dices[d].gameObject.SetActive(true);
			//Return as failed if result are 1
			if(result == 1) return -1;
			//Increase total result will return later
			totalResult += result;
		}
		return totalResult;
	}

	void CreateAttack()
	{
		//Create an new attack instance also set it spritr if have;t
		if(attackInstance == null)
		{
			GameObject attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
			attackInstance = attack;
			attack.GetComponent<SpriteRenderer>().sprite = attackSprite;
			attack.SetActive(false);
		}
		//Reset instance position
		attackInstance.transform.position = transform.position;
		//make the instance look toward player
		attackInstance.transform.up = -Vector3.Normalize(transform.position - playerTF.position);
		//Active the attack instance
		attackInstance.SetActive(true);
	}

	void NextTurn() 
	{
		//Deactive all the dice
		for (int d = 0; d < dices.Length; d++) {dices[d].gameObject.SetActive(false);}
		//Reset greed and inital greed
		greed = intitialGreed; stackedDamage = 0;
		Combat.i.SwitchTurn(turnOrder+1);
	}

#region Take Damage
	public void AddHurt(int damage)
	{
		UpdateHurtTake(true, totalHurt.ToString());
		hurtQueue.Add(damage);
		totalHurt += damage;
	}

	public void ClearHurt()
	{
		hurtQueue.Clear();
		totalHurt = 0;
		UpdateHurtTake(false, "");
	}

	public void UpdateHurtTake(bool show, string text)
	{
		if(totalHurtDisplay == null) return;
		totalHurtDisplay.text = text;
		totalHurtDisplay.transform.parent.gameObject.SetActive(show);
	}

	public void GetAllHurt()
	{
		if(totalHurt <= 0) return;
		heath.TakeDamage(totalHurt);
		ClearHurt();
	}

	void Die()
	{
		Destroy(attackInstance);
		EnemyManager.i.enemies.Remove(this);
	}
#endregion
}
