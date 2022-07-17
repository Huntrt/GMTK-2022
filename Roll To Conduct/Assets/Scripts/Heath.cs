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
	public ParticleSystem hurtEffect, healEffect, failEffect;

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
		hurtEffect.Play();
		heathCounter.text = heath + " / " + maxHeath; 
		onHurt?.Invoke();
		if(heath <= 0) 
		{
			onDie?.Invoke();
			//Only for player
			if(gameObject.name == "Player")
			{
				gameObject.SetActive(false);
				Player.i.gameOverPanel.SetActive(true);
				return;
			}
			//Play the kill effect
			Combat.i.killEffect.transform.position = transform.position;
			Combat.i.killEffect.Play();
			Destroy(gameObject);
		}
	}

	public void GetHeal(int heal)
	{
		if(heal <= 0) return;
		heath += heal;
		if(heath >= maxHeath) {heath = maxHeath;}
		FlashColor(healflash);
		healEffect.Play();
		heathCounter.text = heath + " / " + maxHeath; 
		onHeal?.Invoke();
	}

	void FlashColor(Color color)
	{
		CancelInvoke("ClearFlash");
		render.color = color; Invoke("ClearFlash", flashDuration);
	}

	public void PlayFail()
	{
		failEffect.Play();
	}

	void ClearFlash() {render.color = defaultColor;}
}
