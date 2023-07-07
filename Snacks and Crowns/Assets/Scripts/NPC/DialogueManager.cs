using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    public GameObject uiTemplate;
	GameObject uiInstance;
	public Dialogue dialogue;

	TextMeshProUGUI nameText;
	TextMeshProUGUI dialogueText;

	private Queue<string> sentences;

	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
	}

	public void StartDialogue(GameObject player)
	{
		player.GetComponent<Player_State_Manager>().Change_State(Player_State.in_ui_menu);
		uiInstance = Instantiate(uiTemplate,player.GetComponent<Player_Inventory>().canvas.transform);
		
		
		Transform tr = uiInstance.transform.Find("Dialogue Box");
		dialogueText = tr.Find("Dialogue Text").GetComponent<TextMeshProUGUI>();
		nameText = uiInstance.transform.Find("Dialogue Name").GetComponent<TextMeshProUGUI>();
		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		nameText.text = gameObject.name;
		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
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

	public void EndDialogue()
	{
		Destroy(uiInstance);
		Debug.Log("DEstrooyed");
	}
}
