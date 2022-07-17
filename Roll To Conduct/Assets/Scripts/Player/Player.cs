using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Heath heath;
	public Inventory inventory;
	public TMPro.TextMeshProUGUI hurtCounter, healCounter, shieldCounter;
	public GameObject gameOverPanel;
	//Set this class to singleton
	public static Player i {get{if(_i==null){_i = GameObject.FindObjectOfType<Player>();}return _i;}} static Player _i;
}
