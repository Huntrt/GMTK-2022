using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{

	//% TESTING

	public DiceType diceWanted;
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			AddDice(diceWanted);
		}
	}

	//% TESTING

	public int slotAmount;
	[SerializeField] bool GetUI;
	[SerializeField] Transform inventoryInterface;
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
}
