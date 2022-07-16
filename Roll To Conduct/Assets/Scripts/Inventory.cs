using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
	public int slotAmount;
	public List<DiceCore> dices;
	public List<Slot> slots;
	DiceManager dm;
    [System.Serializable] public class Slot
	{
		public DiceCore dice;
		public Image icon;
		public Button button;
	}

	void OnEnable()
	{
		//Get the dice manager
		dm = DiceManager.i;
	}

	void Update()
	{
		//% Testing recive weapon
		if(Input.GetKeyDown(KeyCode.Space)) AddDice((DiceType)Random.Range(0,8));
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
	public InfoPanel infoPanel; [System.Serializable] public class InfoPanel
	{
		public GameObject panel;
		public TextMeshProUGUI name, description;
	}

	public SlotColor slotColor; [System.Serializable] public class SlotColor
	{
		public Color none, use, queued, cancel;
	}

	public void AssignSlot(int slotIndex, DiceCore dice)
	{
		//Get the slot will be assign
		Slot slot = slots[slotIndex];
		//Get the Ui of slot index
		Transform slotUI = inventoryInterface.GetChild(slotIndex);
		slot.dice = dice;
		//@ Get the UI component
		slot.icon = slotUI.GetChild(0).GetComponent<Image>();
		slot.button = slotUI.GetChild(1).GetComponent<Button>();
		//@ Assign UI component
		slot.icon.sprite = dices[slotIndex].icon;
		//Active the slot ui
		slotUI.gameObject.SetActive(true);
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

	public void QueueingDice(Transform slotTF)
	{
		//Get the index of slot given
		int index = slotTF.GetSiblingIndex();
		//Save the combat manager
		Combat c = Combat.i;
		//Get the dice core of given index
		DiceCore dice = dices[index];
		//Get the color state of button
		ColorBlock colors = slots[index].button.colors;
		//If get dice are already queue
		if(c.queues.Contains(dice)) 
		{
			//Remove dice from queue
			c.UnqueueDice(dice);
			colors.highlightedColor = slotColor.use; 
			colors.normalColor = slotColor.none; 
			slots[index].button.colors = colors;
			return;
		}
		//Queue the dice has get
		c.QueueDice(dice);
		colors.normalColor = slotColor.queued; 
		colors.highlightedColor = slotColor.cancel; 
		slots[index].button.colors = colors;
	}
#endregion
}
