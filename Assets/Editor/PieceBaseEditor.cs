using UnityEngine;
using UnityEditor;

// Draws the outline of the piece in the scene view
[CustomEditor(typeof(PieceBase))]
public class PieceBaseEditor : Editor
{
    private const float Borderhalf = 0.5f;

    public void OnSceneGUI()
    {
        var t = target as PieceBase;
        var pos = t.transform.position;

        Handles.color = Color.cyan;

        // square so that it lines up with the grid cells
        Handles.DrawPolyLine(
            pos + new Vector3(-Borderhalf, -Borderhalf, 0),
            pos + new Vector3(Borderhalf, -Borderhalf, 0),
            pos + new Vector3(Borderhalf, Borderhalf, 0),
            pos + new Vector3(-Borderhalf, Borderhalf, 0),
            pos + new Vector3(-Borderhalf, -Borderhalf, 0));

            GUI.color = Color.cyan;
            Handles.Label(pos + new Vector3(-Borderhalf, Borderhalf + 0.2f, 0), t.name); 
    }
}
