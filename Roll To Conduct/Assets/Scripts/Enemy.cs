using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
	public Heath heath;
    public float value;
	public List<int> damageQueue = new List<int>();
	public int totalDamageWillTake;
	public TextMeshProUGUI totalDamageWillTake_Display;
	Transform playerTF;
	int turnOrder;
	[Header("Attack")]
	[SerializeField] GameObject attackPrefab; GameObject attackInstance;
	[SerializeField] Sprite attackSprite;
	[SerializeField] Vector2 attackHitbox;
	[SerializeField] float attackVelocity;

	void OnEnable()
	{
		playerTF = Player.i.transform;
		Combat.i.onEndTurn += TakeAllDamage;
		heath.onDie += Die;
		UpdateDamageTake(false, "");
	}

	void OnDisable()
	{
		if(Combat.i != null) Combat.i.onEndTurn -= TakeAllDamage;
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
				print("Dealt Damage To Player");
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

	void NextTurn() {Combat.i.SwitchTurn(turnOrder+1);}

#region Take Damage

	public void CallDamage()
	{
		//Take the earliest damage then remove it
		heath.TakeDamage(damageQueue[0]);
		damageQueue.RemoveAt(0);
	}

	public void AddDamage(int damage)
	{
		UpdateDamageTake(true, totalDamageWillTake.ToString());
		damageQueue.Add(damage);
		totalDamageWillTake += damage;
	}

	public void ClearTakeDamage()
	{
		damageQueue.Clear();
		totalDamageWillTake = 0;
		UpdateDamageTake(false, "");
	}

	public void UpdateDamageTake(bool show, string text)
	{
		if(totalDamageWillTake_Display == null) return;
		totalDamageWillTake_Display.text = text;
		totalDamageWillTake_Display.transform.parent.gameObject.SetActive(show);
	}

	public void TakeAllDamage()
	{
		heath.TakeDamage(totalDamageWillTake);
		ClearTakeDamage();
	}

	void Die()
	{
		Destroy(attackInstance);
		EnemeyManager.i.enemies.Remove(this);
	}
#endregion
}
