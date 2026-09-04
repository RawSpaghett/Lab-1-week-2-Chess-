using UnityEngine;
using System.Collections.Generic;



public class PieceBase : MonoBehaviour
{
    [SerializeField] BoardManager boardManager;
    [SerializeField] private PieceType pieceType;
    [SerializeField] private Color tint = Color.white;
    
    [SerializeField] private Sprite[] pieceSprites = new Sprite[6];
    
    private enum PieceType
    {
        King,Queen,Rook,Knight,Bishop,Pawn
    }

     void OnDrawGizmos()
    {
        //Snap to halfpoints on grid automatically (0.5,1.5,etc)
        SnapToGrid();
        //Change texture based on enum
        //Create base movement gizmo that handles piece moves, and changes with texture
        ApplySprite();
    }
    private void ApplySprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            return;
        } 
        
        int index = (int)pieceType;
        
        // Array may still be empty while setting up so this runs constantly
        if (pieceSprites != null && index < pieceSprites.Length && pieceSprites[index] != null)
        {
            spriteRenderer.sprite = pieceSprites[index];
        }

        spriteRenderer.color = tint; 
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

    private List<Vector2Int> GetMoveCells(Vector2Int origin)
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
        }

        return cells;
    }

    // Walks one direction until it hits the edge
    private void AddLine(List<Vector2Int> cells, Vector2Int origin, Vector2Int direction)
    {
        Vector2Int current = origin + direction;
        
        while (IsOnBoard(current))
        {
            cells.Add(current);
            current += direction;
        }
    }
}
