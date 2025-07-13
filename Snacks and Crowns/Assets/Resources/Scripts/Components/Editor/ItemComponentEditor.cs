using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// Loads Item components and displays buttons in the Editor UI that allow for their initialization to a component list
/// </summary>

[CustomEditor(typeof(Item),true)]
public class ItemComponentEditor : Editor
{
    private static List<Type> dataComponetTypes = new List<Type>();
    private Item item;
    private bool showComponents;
    public override void OnInspectorGUI()
    {
        item = target as Item;

        base.OnInspectorGUI();
        showComponents = EditorGUILayout.Foldout(showComponents, "Add Item Components");
        if (showComponents)
        {
            foreach (var dataComponetType in dataComponetTypes)
            {
                if (GUILayout.Button(dataComponetType.Name))
                {
                    var comp = Activator.CreateInstance(dataComponetType) as ComponentDataGeneric;
                    if (comp == null) return;
                    item.AddDataUse(comp);
                }
            }
        }
    }
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnRecompile()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        var filteredTypes = types.Where(
        type => IsSubclassOfRawGeneric(typeof(ItemComponentData<>), type)
        && !type.ContainsGenericParameters
        && type.IsClass);
        dataComponetTypes = filteredTypes.ToList();
    }
    static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }
}
