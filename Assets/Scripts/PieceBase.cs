using UnityEngine;

public class PieceBase : MonoBehaviour
{
    [SerializeField] BoardManager boardManager;
    
    private enum PieceType
    {
        King,Queen,Rook,Knight,Bishop,Pawn
    }

     void OnDrawGizmos()
    {
        //Snap to halfpoints on grid automatically (0.5,1.5,etc)
        //Change texture based on enum
        //Create base movement gizmo that handles piece moves, and changes with texture
    }
}
