using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



/// <summary>
/// Loads Ability components and displays buttons in the Editor UI that allow for their initialization to a component list
/// </summary>

[CustomEditor(typeof(Ability),true)]
public class AbilityComponentEditor : Editor
{
    private static List<Type> dataComponetTypes = new List<Type>();
    private Ability ability;
    private bool showComponents;
    public override void OnInspectorGUI()
    {
        ability = target as Ability;

        base.OnInspectorGUI();
        showComponents = EditorGUILayout.Foldout(showComponents, "Add Ability Components");
        if (showComponents)
        {
            foreach (var dataComponetType in dataComponetTypes)
            {
                if (GUILayout.Button(dataComponetType.Name))
                {
                    var comp = Activator.CreateInstance(dataComponetType) as ComponentDataGeneric;
                    if (comp == null) return;
                    ability.AddAbilityComponent(comp);
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
            type => typeof(IGenericComponent).IsAssignableFrom(type)//IsSubclassOfRawGeneric(typeof(IGenericComponent), type)
            && !type.ContainsGenericParameters
            && type.IsClass);
        dataComponetTypes = filteredTypes.ToList();
    }
    /// <summary>
    /// 
    /// </summary>
    static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        // we climb along our fathers until we reach a generic type or equal our "generic" father
        while (toCheck != null && toCheck != typeof(object))
        { 
            if(toCheck.Name == "StunSelf")
                Debug.Log("Checking " + toCheck.Name);
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                if (toCheck.Name == "StunSelf")
                    Debug.Log("TRUE ");
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }
}
