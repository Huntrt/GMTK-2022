using System.Collections.Generic;
using UnityEngine;

public class AttackDice : DiceCore
{
	public int stackedDamage;
	public TargetMode targetMode; public enum TargetMode {first, last, random, all}
	public StackMode stackMode; public enum StackMode {increase, exponentially}
	EnemyManager em;
	Combat cm;

	void Start()
	{
		em = EnemyManager.i;
	}

    void OnEnable()
	{
		cm = Combat.i;
		onAction += Attack;
		onPunish += Failed;
		cm.playerAttack += ResetStats;
	}
	void OnDisable()
	{
		onAction -= Attack;
		onPunish -= Failed;
		cm.playerAttack -= ResetStats;
	}

	void Attack(int roll)
	{
		if(em.enemies.Count <= 0) return;

		//? Damage
		if(stackMode == StackMode.increase)
		{
			stackedDamage += roll;
		}
		if(stackMode == StackMode.exponentially)
		{
			if(stackedDamage <= 0 ) stackedDamage = 1;
			stackedDamage *= roll;
		}
		//? Target
		if(targetMode == TargetMode.first)
		{
			em.enemies[0].AddHurt(stackedDamage);
		}
		if(targetMode == TargetMode.last)
		{
			em.enemies[em.enemies.Count-1].AddHurt(stackedDamage);
		}
		if(targetMode == TargetMode.random)
		{
			em.enemies[Random.Range(0,em.enemies.Count)].AddHurt(stackedDamage);
		}
	}

	void Failed(int roll)
	{
		//End player turn
		Combat.i.PlayerEndTurn(false);
	}

	void ResetStats()
	{
		stackedDamage = 0;
	}
}
