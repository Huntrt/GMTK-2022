using UnityEngine;
using System;
using TMPro;

public class Heath : MonoBehaviour
{
    public int maxHeath, heath;
	public Action onHurt, onHeal, onDie;
	public TextMeshProUGUI heathcounter;
	public SpriteRenderer render;
	public float flashDuration;
	public Color hurtFlash, healflash; Color defaultColor;

	void OnEnable()
	{
		defaultColor = render.color;
		heath = maxHeath;
		heathcounter.text = heath + " / " + maxHeath; 
	}

	public void TakeDamage(int damage)
	{
		if(damage <= 0) return;
		heathcounter.text = heath + " / " + maxHeath; 
		heath -= damage;
		FlashColor(hurtFlash);
		onHurt?.Invoke();
		if(heath <= 0) {onDie?.Invoke(); Destroy(gameObject);}
	}

	public void GetHeal(int heal)
	{
		if(heal <= 0) return;
		heathcounter.text = heath + " / " + maxHeath; 
		heath += heal;
		FlashColor(healflash);
		if(heath >= maxHeath) {heath = maxHeath;}
		onHeal?.Invoke();
	}

	void FlashColor(Color color)
	{
		CancelInvoke("ClearFlash");
		render.color = color; Invoke("ClearFlash", flashDuration);
	}

	void ClearFlash() {render.color = defaultColor;}
}
