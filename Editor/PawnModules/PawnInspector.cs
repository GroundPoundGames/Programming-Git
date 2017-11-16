using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Custom editor used for adding, removing and editing modules in a Pawn.
/// The editor automatically draws the list of all classes that derive from PawnModule.
/// </summary>
[CustomEditor(typeof(Pawn))]
public class PawnInspector : Editor
{
    SerializedObject Target;
    Pawn targetPawn;
    SerializedProperty ModuleList;

    const string PAWN_INSPECTOR_MODULES_HEADER = "Modules";

    int choice = 0;

    public override void OnInspectorGUI()
    {
        targetPawn = (Pawn)target;
        Target = new SerializedObject(target);
        ModuleList = Target.FindProperty("Modules");
        // For each module, we want to display a bar with its type, and under it its attributes.
        DrawModuleDetails();
    }

    void DrawModuleDetails()
    {
        // Header
        GUILayout.BeginHorizontal();
            GUILayout.Space(Screen.width / 2 - (PAWN_INSPECTOR_MODULES_HEADER.Length * 5));
            GUILayout.Label(PAWN_INSPECTOR_MODULES_HEADER, EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        //
         // Display modules and remove button

        for (int i = 0; i < ModuleList.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(ModuleList.GetArrayElementAtIndex(i).objectReferenceValue.GetType().ToString(), EditorStyles.boldLabel);
            if (GUILayout.Button("Remove"))
            {
                RemoveModuleFromPawn(i);
            }
            EditorGUILayout.EndHorizontal();

            DisplayModuleProperties(i);
        }


        // Add new button : adds a new module


        // Get all types deriving from PawnModule, cast to string and put them in a list.
        System.Type[] allModuleTypes = (from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                        from assemblyType in domainAssembly.GetTypes()
                        where typeof(PawnModule).IsAssignableFrom(assemblyType)
                        select assemblyType).ToArray();
        List<string> types = new List<string>();
        foreach(System.Type type in allModuleTypes)
        {
            if (type.ToString() != "PawnModule")
            types.Add(type.ToString());
        }

        // Display available types
        choice = EditorGUILayout.Popup(choice, types.ToArray());
        if (GUILayout.Button("Add new"))
        {
            System.Type type = allModuleTypes[choice];
            AddModuleToPawn(type);
        }
    }
    /// <summary>
    /// Adds a module to the current pawn and re-draws the inspector.
    /// </summary>
    void AddModuleToPawn(System.Type moduleType)
    {
        Debug.Log("Adding module type " + moduleType.ToString());

        for(int i = 0; i < ModuleList.arraySize; i++)
        {
            if (ModuleList.GetArrayElementAtIndex(i).objectReferenceValue.GetType() == moduleType)
            {
                Debug.LogError("ERROR : This Pawn already owns a module of type " + moduleType.ToString());
                return;
            }
        }

        PawnModule newModule = (PawnModule)ScriptableObject.CreateInstance(moduleType);
        if (!(newModule is PawnModule))
        {
            Debug.LogError("ERROR : This type is not derived from PawnModule !");
            return;
        }

        ModuleList.arraySize = ModuleList.arraySize + 1;
        Debug.Log("Number of modules is now " + ModuleList.arraySize);
        Target.ApplyModifiedProperties();
        ModuleList.GetArrayElementAtIndex(ModuleList.arraySize - 1).objectReferenceValue = newModule;
        Target.ApplyModifiedProperties();
    }

    void RemoveModuleFromPawn(int index)
    {
        ModuleList.DeleteArrayElementAtIndex(index);
        ModuleList.arraySize--;
        Target.ApplyModifiedProperties();
    }

    /// <summary>
    /// Displays all the module's properties at the given index in the ModuleList array.
    /// </summary>
    /// <param name="index"></param>
    void DisplayModuleProperties(int index)
    {
        if (ModuleList.arraySize >index)
        {
            PawnModule module = (PawnModule)ModuleList.GetArrayElementAtIndex(index).objectReferenceValue;
            SerializedObject obj = new SerializedObject(module);

            SerializedProperty prop = obj.GetIterator();
            while (prop.Next(true))
            {
                if (prop.depth == 0 && !CheckIgnore(prop.name))
                    EditorGUILayout.PropertyField(prop);
            }

            obj.ApplyModifiedProperties();
        }
    }

    bool CheckIgnore(string prop)
    {
        foreach(string str in PROPERTY_IGNORE)
        {
            if (str == prop) return true;
        }
        return false;
    }

    /// <summary>
    /// List of all ignored properties that will not show up in the editor.
    /// </summary>
    readonly string[] PROPERTY_IGNORE = new string[]
    {
        "Array",
        "m_ObjectHideFlags",
        "m_PrefabParentObject",
        "m_PrefabInternal",
        "m_GameObject",
        "m_Enabled",
        "m_EditorHideFlags",
        "m_Name",
        "m_EditorClassIdentifier"
    };
}

