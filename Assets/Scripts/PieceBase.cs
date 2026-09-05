using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;



public class PieceBase : MonoBehaviour
{
    [SerializeField] BoardManager boardManager;
    [SerializeField] private PieceType pieceType;
    [SerializeField] private Color tint = Color.white;
    
    
    public enum PieceType
    {
        King,Queen,Rook,Knight,Bishop,Pawn
    }

    private Dictionary<PieceType,string> textureDict = new Dictionary<PieceType, string>() //for gizmos.drawicon
    {
        {PieceType.King,"King"},
        {PieceType.Queen,"Queen"},
        {PieceType.Bishop,"Bishop"},
        {PieceType.Knight,"Knight"},
        {PieceType.Rook, "Rook"},
        {PieceType.Pawn, "Pawn"}
    };
    private void OnDrawGizmos()
    {
        SnapToGrid(); 
        //Create base movement gizmo that handles piece moves, and changes with enum
        ApplySprite();
    }
    private void ApplySprite()
    {
        if(textureDict.TryGetValue(pieceType,out string path))
            Gizmos.DrawIcon(this.transform.position,path,true,tint);
    }



    private void SnapToGrid()
    {
        Vector3 pos = transform.position;
        Vector3 snapped = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), pos.z);
        // Skips the write when nothing happens to keep the scene from being dirty
        if (pos != snapped)
        {
            transform.position = snapped;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (boardManager == null)
        {
            return;
        }

        Vector2Int origin = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);

        foreach (Vector2Int cell in GetMoveCells(origin))
        {
            if (!IsOnBoard(cell))
            {
                continue;
            }

            Gizmos.DrawCube(new Vector3(cell.x, cell.y, transform.position.z), new Vector3(0.8f, 0.8f, 0.1f));
        }
    }

    private bool IsOnBoard(Vector2Int cell)
    { 
        return cell.x >= 0 && cell.x < boardManager.gridSize && cell.y >= 0 && cell.y < boardManager.gridSize; 
    }

    private List<Vector2Int> GetMoveCells(Vector2Int origin) //if this wasnt a lab, would fix DRY
    {
        List<Vector2Int> cells = new List<Vector2Int>();
        switch (pieceType)
        {
            case PieceType.Rook:
                AddLine(cells, origin, Vector2Int.up);
                AddLine(cells, origin, Vector2Int.down);
                AddLine(cells, origin, Vector2Int.left);
                AddLine(cells, origin, Vector2Int.right);
                break;
            case PieceType.Bishop: //diagonals
                AddLine(cells,origin, Vector2Int.up + Vector2Int.right);
                AddLine(cells,origin, Vector2Int.up + Vector2Int.left);
                AddLine(cells,origin, Vector2Int.down + Vector2Int.right);
                AddLine(cells,origin, Vector2Int.down + Vector2Int.left);
                break;
            case PieceType.King: //one way any direction
                AddLine(cells, origin, Vector2Int.up,1);
                AddLine(cells, origin, Vector2Int.down,1);
                AddLine(cells, origin, Vector2Int.left,1);
                AddLine(cells, origin, Vector2Int.right,1);
                //corners
                AddLine(cells, origin, Vector2Int.up + Vector2Int.right,1); //top right
                AddLine(cells, origin, Vector2Int.up + Vector2Int.left,1); //top left
                AddLine(cells,origin, Vector2Int.down + Vector2Int.right,1); //bottom right
                AddLine(cells,origin, Vector2Int.down + Vector2Int.left,1); //bottom left
                break;
            case PieceType.Knight: //L
            //up
                AddLine(cells, origin, (Vector2Int.up * 2) + Vector2Int.right,1);
                AddLine(cells, origin, Vector2Int.up  + (Vector2Int.right * 2),1);
                AddLine(cells, origin, (Vector2Int.up * 2) + Vector2Int.left,1);
                AddLine(cells, origin, Vector2Int.up  + (Vector2Int.left * 2),1);
            //down
                AddLine(cells, origin, (Vector2Int.down * 2) + Vector2Int.right,1);
                AddLine(cells, origin, Vector2Int.down  + (Vector2Int.right * 2),1);
                AddLine(cells, origin, (Vector2Int.down * 2) + Vector2Int.left,1);
                AddLine(cells, origin, Vector2Int.down  + (Vector2Int.left * 2),1);
                break;
            case PieceType.Pawn://one way, forward
                AddLine(cells, origin, Vector2Int.down,1);
                //AddLine(cells, origin, Vector2Int.up,1);
                break;
            case PieceType.Queen:// anywhere
                //columns
                AddLine(cells, origin, Vector2Int.up);
                AddLine(cells, origin, Vector2Int.down);
                AddLine(cells, origin, Vector2Int.left);
                AddLine(cells, origin, Vector2Int.right);
                //diagonals
                AddLine(cells,origin, Vector2Int.up + Vector2Int.right);
                AddLine(cells,origin, Vector2Int.up + Vector2Int.left);
                AddLine(cells,origin, Vector2Int.down + Vector2Int.right);
                AddLine(cells,origin, Vector2Int.down + Vector2Int.left);
                break;
        }
        return cells;
    }

    // Walks one direction until it hits the edge
    private void AddLine(List<Vector2Int> cells, Vector2Int origin, Vector2Int direction , int steps = 15)
    {
        Vector2Int current = origin + direction;
        
        while (steps > 0 && IsOnBoard(current))
        {
                cells.Add(current);
                current += direction;
                steps--;
        }
    }
}
