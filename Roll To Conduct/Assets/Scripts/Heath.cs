using UnityEngine;
using System;
using TMPro;

public class Heath : MonoBehaviour
{
    public int maxHeath, heath;
	public Action onHurt, onHeal, onDie;
	public TextMeshProUGUI heathCounter;
	public SpriteRenderer render;
	public float flashDuration;
	public Color hurtFlash, healflash; Color defaultColor;

	void Start()
	{
		defaultColor = render.color;
		heath = maxHeath;
		heathCounter.text = heath + " / " + maxHeath; 
	}

	public void TakeDamage(int damage)
	{
		heath -= damage;
		FlashColor(hurtFlash);
		heathCounter.text = heath + " / " + maxHeath; 
		onHurt?.Invoke();
		if(heath <= 0) 
		{
			onDie?.Invoke();
			//Onky for player
			if(gameObject.name == "Player")
			{
				gameObject.SetActive(false);
				return;
			}
			Destroy(gameObject);
		}
	}

	public void GetHeal(int heal)
	{
		heath += heal;
		FlashColor(healflash);
		heathCounter.text = heath + " / " + maxHeath; 
		onHeal?.Invoke();
		if(heath >= maxHeath) {heath = maxHeath;}
	}

	void FlashColor(Color color)
	{
		CancelInvoke("ClearFlash");
		render.color = color; Invoke("ClearFlash", flashDuration);
	}

	void ClearFlash() {render.color = defaultColor;}
}
