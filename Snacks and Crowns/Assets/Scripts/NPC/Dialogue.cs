using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
	public List<Item> required_items = new List<Item>();

	[TextArea(3, 10)]
	public string text;
	public List<Dialogue> next_dialogues;
	public List<string> button_names;
}