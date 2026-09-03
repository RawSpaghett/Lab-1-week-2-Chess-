using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] public int gridSize = 8;
    private Color light = Color.white;
    private Color dark = Color.black;

    void OnDrawGizmos()
    {
        for(int r = 0; r < gridSize; r++)
        {
            for(int c = 0; c < gridSize; c++)
            {
                Gizmos.color = ((r+c)%2 == 0) ? light: dark;
                Gizmos.DrawCube(new Vector3(r ,c,0),new Vector3(1,1,1));
            }
        }
    }
}
