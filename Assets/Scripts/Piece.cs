using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    public int id;
    
    public int upId
    {
        get; private set;
    }
    public int downId
    {
        get; private set;
    }
    public int leftId
    {
        get; private set;
    }
    public int rightId
    {
        get; private set;
    }

    [SerializeField] public RectTransform rTransform;
    [SerializeField] private Image image;

    // public bool isComplete
    // {
    //     get
    //     {
    //         
    //     }
    // }
    

    public void SetIds(List<PieceData> pieces, int index)
    {
        id = index;
        image.sprite = pieces[index].sprite;
        rTransform.sizeDelta = new Vector2(image.sprite.rect.width, image.sprite.rect.height);
        
        upId = -1;
        downId = -1;
        leftId = -1;
        rightId = -1;
        
        Debug.Log($"Searching ids for {index}");
        Debug.Log($"Position is {pieces[index].position}");
        
        Vector2 upPos = new Vector2(pieces[index].position.x, 
            pieces[index].position.y + image.sprite.rect.height);
        
        Debug.Log($"upPos is {upPos}");
        
        Vector2 leftPos = new Vector2(pieces[index].position.x - image.sprite.rect.width, 
            pieces[index].position.y);
        
        Debug.Log($"leftPos is {leftPos}");
        
        Vector2 downPos = new Vector2(pieces[index].position.x, 
            pieces[index].position.y - image.sprite.rect.height);
        
        Debug.Log($"downPos is {downPos}");
        
        Vector2 rightPos = new Vector2(pieces[index].position.x + image.sprite.rect.width, 
            pieces[index].position.y);
        
        Debug.Log($"rightPos is {rightPos}");
        
        
        for (int i = index; i > 0; i--)
        {
            if (leftId == -1 && pieces[i].position == leftPos)
                leftId = i;
            
            if (upId == -1 && pieces[i].position == upPos)
                upId = i;

            if (upId != -1 && leftId != -1) break;
        }
        
        for (int i = index; i < pieces.Count; i++)
        {
            if (rightId == -1 && pieces[i].position == rightPos)
                rightId = i;
            
            if (downId == -1 && pieces[i].position == downPos)
                downId = i;
            
            if (rightId != -1 && downId != -1) break;
        }
    }
}
