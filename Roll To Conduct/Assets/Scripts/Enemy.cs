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

	void OnEnable()
	{
		Combat.i.onEndTurn += TakeAllDamage;
		heath.onDie += Die;
		UpdateDamageTake(false, "");
	}

	void OnDisable()
	{
		if(Combat.i != null) Combat.i.onEndTurn -= TakeAllDamage;
		if(heath != null) heath.onDie -= Die;
	}

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
		EnemeyManager.i.enemies.Remove(this);
	}
}
