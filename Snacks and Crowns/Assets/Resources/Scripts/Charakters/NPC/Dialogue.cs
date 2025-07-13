using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds dialogue data, the text, responces, requirements and effects
/// </summary>
[CreateAssetMenu(fileName = "New_Dialogue", menuName = "Dialogue")]

public class Dialogue : ScriptableObject
{
	public bool autoEndConversation;
	public List<Item> requiredItems = new List<Item>();
	public int requiredGold = 0;
	[SerializeField]
	public bool mustBeAlly;
	[SerializeReference] public List<ComponentDataGeneric> componentData;


	[TextArea(3, 10)]
	public string text;
	public List<Dialogue> nextDialogues;
	public List<string> buttonNames;
	public void AddComponentData(ComponentDataGeneric data)
	{
		componentData.Add(data);
	}

}