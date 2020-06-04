using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Bullet_Database))]
public class Bullet_DB_Editor : Editor
{
    private Bullet_Database database;

    private void Awake()
    {
        database = (Bullet_Database)target;
    }
    public override void OnInspectorGUI()
    {
       

        GUILayout.BeginHorizontal();

        GUILayout.Label("Bullet Index: " + database.Index.ToString());

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
