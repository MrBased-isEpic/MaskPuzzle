using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleSO")]
public class PuzzleSO : ScriptableObject
{
    public string Name;
    
    [Space]
    public Sprite fullSprite;
    public Sprite[] pieces;
    
    public List<Vector2> GetPositions()
    {
        List<Vector2> positions = new List<Vector2>();

        foreach (Sprite sprite in pieces)
        {
            positions.Add(sprite.rect.position);
        }
        
        return positions;
    }
}
