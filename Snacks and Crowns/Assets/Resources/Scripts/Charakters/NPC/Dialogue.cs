using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
	public List<Item> requiredItems = new List<Item>();
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