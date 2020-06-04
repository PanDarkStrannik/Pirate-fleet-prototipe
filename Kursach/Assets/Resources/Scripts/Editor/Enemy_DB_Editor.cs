using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy_Database))]
public class Enemy_DB_Editor : Editor
{
    private Enemy_Database database;

    private void Awake()
    {
        database = (Enemy_Database)target;
    }
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Enemy Index: " + database.Index.ToString());

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
