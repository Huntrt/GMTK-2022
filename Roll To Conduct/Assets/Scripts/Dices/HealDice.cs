using UnityEngine;

public class HealDice : DiceCore
{
	public int stackedHeal;
	public StackMode stackMode; public enum StackMode {once, increase, exponentially}
	EnemyManager em; Combat cm; Player player;

    void OnEnable()
	{
		em = EnemyManager.i; cm = Combat.i; player = Player.i;
		onAction += Attack;
		onPunish += Failed;
		cm.playerAttack += Healing;
	}

	void OnDisable()
	{
		onAction -= Attack;
		onPunish -= Failed;
		cm.playerAttack -= Healing;
	}

	void Attack(int roll)
	{
		//? Damage
		if(stackMode == StackMode.once)
		{
			stackedHeal++;
		}
		if(stackMode == StackMode.increase)
		{
			stackedHeal += roll;
		}
		if(stackMode == StackMode.exponentially)
		{
			if(stackedHeal <= 0 ) stackedHeal = 1;
			stackedHeal *= roll;
		}
		player.healCounter.transform.parent.gameObject.SetActive(true);
		player.healCounter.text = float.Parse(player.healCounter.text) + stackedHeal + "";
	}

	void Healing()
	{
		player.heath.GetHeal(stackedHeal);
		ResetStats();
	}

	void Failed(int roll)
	{
		player.heath.TakeDamage(stackedHeal/2);
		ResetStats();
		//End player turn
		Combat.i.PlayerEndTurn(false);
	}

	void ResetStats()
	{
		stackedHeal = 0;
	}
}
