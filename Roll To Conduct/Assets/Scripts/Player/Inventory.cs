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
		//% Testing recive weapon
		for (int i = 0; i < 10; i++) AddDice((DiceType)Random.Range(0,8));
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

	public void DeleteDice(int index)
	{
		//Deactive the last slot in inventory
		inventoryInterface.GetChild(slots.Count-1).gameObject.SetActive(false);
		//Unqueue then destroy the dice at given index
		Combat.i.UnqueueDice(dices[index]); Destroy(dices[index].gameObject);
		//Remove the last slots
		slots.RemoveAt(slots.Count-1);
		dices.RemoveAt(index);
		for (int s = 0; s < slots.Count; s++)
		{
			slots[s].dice = dices[s];
			slots[s].icon.sprite = dices[s].icon;
		}
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
		public Color none, use, queued, cancel, warning, delete;
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
		//Stop if has begin rolled
		if(c.rolled) return;
		//Delete dice at given index
		if(deleteMode) {DeleteDice(index); return;}
		//Get the dice core of given index
		DiceCore dice = dices[index];
		//If get dice are already queue
		if(c.queues.Contains(dice)) 
		{
			//Remove dice from queue
			c.UnqueueDice(dice);
			//Set back to default slot colot
			SetSlotColor(index, slotColor.none, slotColor.use);
			return;
		}
		//Queue the dice has get
		c.QueueDice(dice);
		//Set to queued slot color
		SetSlotColor(index, slotColor.queued, slotColor.cancel);
	}

	void SetSlotColor(int index, Color normal, Color highlight)
	{
		//Get the color state of button
		ColorBlock colors = slots[index].button.colors;
		colors.normalColor = normal; colors.highlightedColor = highlight;
		slots[index].button.colors = colors;
	}

	bool deleteMode; public void ToggleDeleteMode(Button button) 
	{
		deleteMode = !deleteMode;
		//get the button' color block
		ColorBlock color = button.colors;
		ColorBlock swaps = button.colors;
		if(deleteMode)
		{
			//Swap normal and highlight color of button
			color.normalColor = swaps.highlightedColor;
			color.highlightedColor = swaps.normalColor;
			button.colors = color;
			//Set color for all slot to be delete color
			for (int s = 0; s < slots.Count; s++) SetSlotColor(s, slotColor.warning, slotColor.delete);
		}
		else
		{
			color.highlightedColor = swaps.normalColor;
			color.normalColor = swaps.highlightedColor;
			button.colors = color;
			if(Combat.i.rolled)
			{
				//Set color for all slot back to be queued color
				for (int s = 0; s < slots.Count; s++) SetSlotColor(s, slotColor.queued, slotColor.cancel);
			}
			else
			{
				//Set color for all slot back to be default color
				for (int s = 0; s < slots.Count; s++) SetSlotColor(s, slotColor.none, slotColor.use);
			}
		}
	}
#endregion
}
