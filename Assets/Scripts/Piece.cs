using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Piece : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
{
    public int id;

    public bool isParented = false;

    public PieceManager pieceManager;

    [SerializeField] public RectTransform rTransform;
    [SerializeField] private Image image;
    [SerializeField] private Image shadow;
    
    private int attachedId = -1;
    
    public bool isComplete
    {
        get
        {
            return attachedId != -1 && attachedId == id;
        }
    }

    public int Init(List<PieceData> pieces, int index)
    {
        id = index;
        SetSprite(pieces[index].sprite);
        
        //Debug.Log($"Searching ids for {index}");
        //Debug.Log($"Position is {pieces[index].position}");
        
        Vector2 upPos = new Vector2(pieces[index].position.x, 
            pieces[index].position.y + image.sprite.rect.height);
        
        //Debug.Log($"upPos is {upPos}");
        
        Vector2 leftPos = new Vector2(pieces[index].position.x - image.sprite.rect.width, 
            pieces[index].position.y);
        
        //Debug.Log($"leftPos is {leftPos}");
        
        Vector2 downPos = new Vector2(pieces[index].position.x, 
            pieces[index].position.y - image.sprite.rect.height);
        
        //Debug.Log($"downPos is {downPos}");
        
        Vector2 rightPos = new Vector2(pieces[index].position.x + image.sprite.rect.width, 
            pieces[index].position.y);
        
        //Debug.Log($"rightPos is {rightPos}");
        int sides = 0;

        if (index > 0 && pieces[index - 1].position.y == pieces[index].position.y) sides += 10;
        if (index < pieces.Count-1 && pieces[index + 1].position.y == pieces[index].position.y) sides += 1;
        
        for (int i = index - 1; i >= 0; i--)
        {
            if (pieces[i].position == upPos)
            {
                sides += 1000;
                break;
            }
        }
        
        for (int i = index + 1; i < pieces.Count; i++)
        {
            if (pieces[i].position == downPos)
            {
                sides += 100;
                break;
            }
        }

        //Debug.Log($"Sides : {sides}");
        return sides;
    }


    public void SetSprite(Sprite sprite)
    {
        shadow.sprite = sprite;
        image.sprite = sprite;
        
        rTransform.sizeDelta = new Vector2(image.sprite.rect.width, image.sprite.rect.height);
        shadow.GetComponent<RectTransform>().sizeDelta = rTransform.sizeDelta;
        image.GetComponent<RectTransform>().sizeDelta = rTransform.sizeDelta;
    }

    private Vector2 dragOffset;
    
    public void OnDrag(PointerEventData eventData)
    {
        rTransform.anchoredPosition = eventData.position - dragOffset;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (isParented) return;
        
        int cell = pieceManager.GetAttachablePosition(eventData.position, rTransform.anchoredPosition);
        if (cell == -1) return;
        
        attachedId = cell;
        pieceManager.AttachPieceToCell(this, cell);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (attachedId != -1)
        {
            pieceManager.DetachPieceFromCell(attachedId);
            attachedId = -1;
        }

        dragOffset = eventData.position - rTransform.anchoredPosition;
    }
}
