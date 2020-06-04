using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player_Database))]
public class Player_DB_Editor : Editor
{
    private Player_Database database;

    private void Awake()
    {
        database = (Player_Database)target;
    }
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Player Index: " + database.Index.ToString());

        if (GUILayout.Button("RemoveAll"))
        {
            database.ClearDatabase();
        }
        if (GUILayout.Button("Remove"))
        {
            database.RemoveElement();
        }
        if (GUILayout.Button("Add"))
        {
            database.AddElement();
        }
        if (GUILayout.Button("<="))
        {
            database.GetPrev();
        }
        if (GUILayout.Button("=>"))
        {
            database.GetNext();
        }

        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}