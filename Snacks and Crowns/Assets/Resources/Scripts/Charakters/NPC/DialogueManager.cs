using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.UI;


/// <summary>
/// Manages dialogue for an NPC, loads dialogues, checks conditions and activates effects
/// </summary>
public class DialogueManager : InteractibleInMenu
{

	GameObject uiTemplate;
	GameObject buttonTemplate;
	GameObject uiInstance;
	public Dialogue startDialogue;
	Dialogue dialogue;
	TextMeshProUGUI nameText;
	TextMeshProUGUI dialogueText;

	GameObject player = null;
	List<GameObject> buttons = new List<GameObject>();
	public override bool LockMove { get { return lockMove; } }
	void Start()
	{
		lockMove = true;
		uiTemplate = (GameObject)Resources.Load("Prefabs/UI/Dialogue UI");
		buttonTemplate = (GameObject)Resources.Load("Prefabs/UI/Dialogue Button");
	}

	public override void Interact(GameObject newPlayer)
	{
		if (player != null || startDialogue == null)
		{
			lockMove = false;
			return;
		}
		else
			lockMove = true;

		
		dialogue = startDialogue;
		player = newPlayer;
		uiInstance = Instantiate(uiTemplate,player.GetComponent<MenuManager>().canvas.transform);

		Transform tr = uiInstance.transform.Find("Dialogue Box");
		dialogueText = tr.Find("Dialogue Text").GetComponent<TextMeshProUGUI>();
		
		nameText = uiInstance.transform.Find("Dialogue Name").GetComponent<TextMeshProUGUI>();
		nameText.text = gameObject.name;

		DisplayDialogue(dialogue);
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
		foreach(string _ in dialogue.buttonNames)
        {
			GameObject button = Instantiate(buttonTemplate, uiInstance.transform.Find("Dialogue Options"));
			buttons.Add(button);
			button.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.buttonNames[buttonIndex];
			int delegateIndex = buttonIndex;
			if (CheckDialogueRequirements(buttonIndex))
				buttons[buttonIndex].GetComponent<Button>().onClick.AddListener(delegate { DisplayDialogue(dialogue.nextDialogues[delegateIndex]); });
			else button.GetComponent<Image>().color = Color.red;
			buttonIndex++;
		}
		
		player.GetComponent<MenuManager>().SelectObject(buttons[0]);
	}
	public bool CheckDialogueRequirements(int dialogueIndex)
    {
		if (dialogue.nextDialogues[dialogueIndex] == null) return true;
		List<Item> playerItems = new List<Item>();
		foreach (Item item in player.GetComponent<Inventory>())
		{
			if (item != null) playerItems.Add(item);
		}
		foreach (Item item in dialogue.nextDialogues[dialogueIndex].requiredItems)
        {
			int item_index = 0;
			bool item_found = false;

			foreach (Item p_item in playerItems)
			{
				if (item.itemName == p_item.itemName)
				{
					item_found = true;
					break;
				}
				item_index++;
			}
			if (item_found && playerItems.Count > 0)
			{
				playerItems.RemoveAt(item_index);
			}
			else
			{
				return false;
			};

		}
		if (dialogue.nextDialogues[dialogueIndex].requiredGold > player.GetComponent<GoldTracker>().Gold)
			return false;

		if (dialogue.nextDialogues[dialogueIndex].mustBeAlly)
		{
			if (!FactionState.Allies(player, gameObject))
			{
				return false;
			}
		}

		return true;
	}
	public void DisplayDialogue(Dialogue newDialogue)
	{
		dialogue = newDialogue;
		if (dialogue == null)
		{
			UnInteract(player);
			return;
		}
		RemoveRequired();
		foreach(ComponentDataGeneric component in dialogue.componentData)
        {
			component.InitializeComponent(player, this.gameObject);
        }

        if (dialogue.autoEndConversation)
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
	void RemoveRequired()
    {
		 
		Inventory playerInventory = player.GetComponent<Inventory>();
		foreach (Item item in dialogue.requiredItems)
        {
			playerInventory.RemoveItem(item);
        }
		player.GetComponent<GoldTracker>().AddGold(-dialogue.requiredGold);

    }
	public override void UnInteract(GameObject player)
	{
		player.GetComponent<PlayerInteractManager>().UnInteract();
		this.player = null;
		Destroy(uiInstance);
		lockMove = true;
	}
}
