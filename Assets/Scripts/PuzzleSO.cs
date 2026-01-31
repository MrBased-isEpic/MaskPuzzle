using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleSO")]
public class PuzzleSO : ScriptableObject
{
    public string Name;
    
    [Space]
    public Sprite fullSprite;
    public Sprite[] pieces;
    
    public List<PieceData> GetPieceData()
    {
        List<PieceData> pieceDatas = new List<PieceData>();

        foreach (Sprite sprite in pieces)
        {
            PieceData pieceData = new PieceData();
            pieceData.position = sprite.rect.position;
            pieceData.sprite = sprite;
            pieceDatas.Add(pieceData);
        }
        
        return pieceDatas;
    }
}

public struct PieceData
{
    public Vector2 position;
    public Sprite sprite;
}
