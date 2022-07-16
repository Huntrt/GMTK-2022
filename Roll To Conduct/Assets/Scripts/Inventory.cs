using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{

	//%
	public DiceType diceWanted;

	public int slotAmount;
	public List<DiceCore> dices;
	public List<Slot> slots;
	DiceManager dm;
    [System.Serializable] public class Slot
	{
		public DiceCore dice;
		public Image icon;
		public Button button;
		public TextMeshProUGUI text;
	}

	void OnEnable()
	{
		//Get the dice manager
		dm = DiceManager.i;
	}

	void Update()
	{
		//% 
		if(Input.GetKeyDown(KeyCode.Space))
		{
			AddDice(diceWanted);
		}
	}

	public void AddDice(DiceType type)
	{
		//Stop if there no slot left in inventory
		if(slots.Count >= slotAmount) {print("Full inventory"); return;}
		//Add an new empty slot
		slots.Add(new Slot());
		//Create an new dice object with given type
		DiceCore createdDice = Instantiate(dm.GetDice(type));
		//Set created dice as child of the manager
		createdDice.transform.SetParent(dm.transform);
		//Add the created to dice to it list
		dices.Add(createdDice);
		//Assign the newly created dice to slot
		AssignSlot(slots.Count-1, createdDice);
	}

#region Interface
	[SerializeField] Transform inventoryInterface;

	public void AssignSlot(int slotIndex, DiceCore dice)
	{
		//Get the slot will be assign
		Slot slot = slots[slotIndex];
		//Get the Ui of slot index
		Transform slotUI = inventoryInterface.GetChild(slotIndex);
		//Active the slot ui
		slotUI.gameObject.SetActive(true);
		slot.dice = dice;
		//@ Get the UI component
		slot.icon = slotUI.GetChild(0).GetComponent<Image>();
		slot.button = slotUI.GetChild(1).GetComponent<Button>();
		slot.text = slotUI.GetChild(2).GetComponent<TextMeshProUGUI>();
		//@ Assign UI component
		slot.icon.sprite = dices[slotIndex].icon;
	}

	public InfoPanel infoPanel; [System.Serializable] public class InfoPanel
	{
		public GameObject panel;
		public TextMeshProUGUI name, description;
	}

	public void OnSlotEnter(Transform slotTF)
	{
		//Get the index of slot given
		int index = slotTF.GetSiblingIndex();
		infoPanel.name.text = dices[index].type.ToString();
		infoPanel.description.text = dices[index].description;
		infoPanel.panel.SetActive(true);
	}

	public void OnSlotExit(Transform slotTF)
	{
		//Get the index of slot given
		int index = slotTF.GetSiblingIndex();
		infoPanel.panel.SetActive(false);
	}
#endregion
}
