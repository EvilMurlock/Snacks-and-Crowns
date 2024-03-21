using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.UI;

public class DialogueManager : InteractibleInMenu
{
	public GameObject uiTemplate;
	public GameObject buttonTemplate;
	GameObject uiInstance;
	public Dialogue start_dialogue;
	Dialogue dialogue;
	TextMeshProUGUI nameText;
	TextMeshProUGUI dialogueText;

	GameObject player;
	List<GameObject> buttons = new List<GameObject>();
	// Use this for initialization
	void Start()
	{
	}

	public override void Interact(GameObject new_player)
	{
		dialogue = start_dialogue;
		player = new_player;
		//uiInstance = Instantiate(uiTemplate,player.GetComponent<Inventory>().canvas.transform);
		
		
		Transform tr = uiInstance.transform.Find("Dialogue Box");
		dialogueText = tr.Find("Dialogue Text").GetComponent<TextMeshProUGUI>();
		nameText = uiInstance.transform.Find("Dialogue Name").GetComponent<TextMeshProUGUI>();

		nameText.text = gameObject.name;
		DisplayNextDialogue(dialogue);
	}
	void DisplayButtons()
    {
		if (buttons.Count > 0)
		{
			foreach (GameObject b in buttons)
			{
				Destroy(b);
			}
		}
		buttons = new List<GameObject>();
		int buttonIndex = 0;
		foreach(string button_name in dialogue.button_names)
        {
			GameObject button = Instantiate(buttonTemplate, uiInstance.transform.Find("Dialogue Options"));
			buttons.Add(button);
			button.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.button_names[buttonIndex];
			int delegateIndex = buttonIndex;
			if (CheckDialogueRequirements(buttonIndex))
				buttons[buttonIndex].GetComponent<Button>().onClick.AddListener(delegate { DisplayNextDialogue(dialogue.next_dialogues[delegateIndex]); });
			else button.GetComponent<Image>().color = Color.red;
			buttonIndex++;
		}
		/*
		Inventory player_inventory = player.GetComponent<Inventory>();
		player_inventory.event_system.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(buttons[0]);*/
	}
	public bool CheckDialogueRequirements(int dialogueIndex)
    {
		if (dialogue.next_dialogues[dialogueIndex] == null) return true;
		List<Item> player_items = new List<Item>();/*
		foreach (ItemSlot item_slot in player.GetComponent<Inventory>().items)
		{
			if (item_slot.item != null) player_items.Add(item_slot.item);
		}
		foreach (Item item in dialogue.next_dialogues[dialogueIndex].required_items)
        {
			int item_index = 0;
			bool item_found = false;

			foreach (Item p_item in player_items)
			{
				if (item.itemName == p_item.itemName)
				{
					item_found = true;
					break;
				}
				item_index++;
			}
			if (item_found && player_items.Count > 0)
			{
				player_items.RemoveAt(item_index);
			}
			else
			{
				return false;
			};

		}*/
		return true;
	}
	public void DisplayNextDialogue(Dialogue newDialogue)
	{
		dialogue = newDialogue;
		if (dialogue == null)
		{
			UnInteract(player);
			return;
		}

		string sentence = dialogue.text;
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
		DisplayButtons();
	}

	IEnumerator TypeSentence(string sentence) 
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	public override void UnInteract(GameObject player)
	{
		Destroy(uiInstance);
	}
}
