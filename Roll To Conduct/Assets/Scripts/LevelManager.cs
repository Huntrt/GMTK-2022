using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public enum LevelType {enemy, horde}
	public GameObject pathPanel;
	[SerializeField] LevelInfo[] levels;
	[SerializeField] float shopRate;
	[SerializeField] PathOption[] pathOption;
	public TMPro.TextMeshProUGUI pathName, pathDescription;
	[System.Serializable] public class LevelInfo
	{
		public LevelType type;
		public Sprite icon;
		[TextArea(3,30)]public string description;
	}
	[System.Serializable] public class PathOption
	{
		public LevelType type;
		public Image icon;
		public Button enterPath;
		public string name, description;
	}
    public int levelCount;
	//Set this class to singleton
	public static LevelManager i {get{if(_i==null){_i = GameObject.FindObjectOfType<LevelManager>();}return _i;}} static LevelManager _i;

	void OnEnable()
	{
		EnemyManager.i.onCleared += SpawnPath;
	}

	void OnDisable()
	{
		EnemyManager.i.onCleared -= SpawnPath;
	}

	void Start()
	{
		//Spawn basic first
		EnemyManager.i.spawner.SpawnDecide();
	}

	void SpawnPath()
	{
		//Go through all the path option
		for (int p = 0; p < pathOption.Length; p++)
		{
			//Randomly choose level will for path to be horde or enemy
			LevelInfo level = levels[Random.Range(0,2)];
			pathOption[p].type = level.type;
			pathOption[p].icon.sprite = level.icon;
			pathOption[p].name = level.type.ToString();
			pathOption[p].description = level.description;
		}
		pathPanel.SetActive(true);
	}

	public void SetPathInfo(int index)
	{
		//Set the path  info as name and description at index given
		pathName.text = pathOption[index].name;
		pathDescription.text = pathOption[index].description;
	}
	public void ClearPathInfo() {pathName.text = ""; pathDescription.text = "";}

	public void EnterPath(int index)
	{
		levelCount++;
		EnemyManager.i.IncreaseDifficulty();
		EnemySpawner spawner = EnemyManager.i.spawner;
		if(pathOption[index].type == LevelType.enemy)
		{
			//Begin spawn enemy normaly
			spawner.SpawnDecide();
		}
		if(pathOption[index].type == LevelType.horde)
		{
			//Begin spawn double enemy
			spawner.SpawnDecide(spawner.initialAmount);
		}
		pathPanel.SetActive(false);
	}
}
