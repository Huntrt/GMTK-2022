﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
	public AudioManager audioM;
	public static GameManager i;

	void Awake()
	{
		//Only set to "don't destroy on load" if haven't then destroy any duplicate
		if(i == null) {i = this; DontDestroyOnLoad(this);} else {Destroy(gameObject);}
	}
}
