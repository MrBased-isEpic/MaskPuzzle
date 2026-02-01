using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PieceManager : MonoBehaviour
{
    [SerializeField] private PuzzleSO puzzle;
    [SerializeField] private GameObject piecePrefab;

    public List<Piece> pieces;

    [Space] [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject gridPrefab;

    [SerializeField] private int[] sides;
    [SerializeField] private Sprite[] gridSprites;
    
    [Space]
    [SerializeField] private float lockOnDistance;
    
    private Dictionary<int, Sprite> gridSpritesDictionary;

    private Vector2[] positions;
    private int[] piecesAttached;

    public float halfWidth;
    public float halfHeight;

    public Vector2 cellOffset;
    
    #region StateMachine


    public State currentState;
    private void SwitchState(State nextState)
    {
        currentState = nextState;

        switch (currentState)
        {
            case State.Intro:
                
                
                break;
            case State.Puzzle:
                break;
            case State.Fail:
                break;
            case State.Success:
                break;
        }
    }
    
    #endregion
    
    
    public void StartPuzzle(PuzzleSO puzzleData)
    {
        puzzle =  puzzleData;
        gridSpritesDictionary = new Dictionary<int, Sprite>();
        for (int i = 0; i < sides.Length; i++)
        {
            gridSpritesDictionary.Add(sides[i], gridSprites[i]);
        }
        
        SpawnAllPieces();
    }

    private void SpawnAllPieces()
    {
        List<PieceData> pieceData = puzzle.GetPieceData();

        positions = new Vector2[pieceData.Count];
        piecesAttached = new int[pieceData.Count];
        
        pieces = new List<Piece>();

        Vector2 offset = new Vector2((UnityEngine.Screen.width / 2) - (puzzle.fullSprite.rect.width / 2),
            (UnityEngine.Screen.height / 2) - (puzzle.fullSprite.rect.height / 2));
        
        halfWidth =  puzzle.pieces[0].rect.width / 2;
        halfHeight = puzzle.pieces[0].rect.height / 2;

        cellOffset = new Vector2(-halfWidth, -halfHeight);

        for (int i = 0; i < pieceData.Count; i++)
        {
            Piece piece = Instantiate(piecePrefab, this.transform).GetComponent<Piece>();

            piece.pieceManager = this;
            
            positions[i] = (pieceData[i].position + offset);
            piecesAttached[i] = -1;
            
            piece.rTransform.anchoredPosition = pieceData[i].position + offset;
            int sides = piece.Init(pieceData, i);
            
            RectTransform rect = Instantiate(gridPrefab, gridParent).GetComponent<RectTransform>();
            rect.GetComponent<Image>().sprite = gridSpritesDictionary[sides];

            int heightAdd = 0;
            int widthAdd = 0;
            rect.anchoredPosition = pieceData[i].position + offset;

            //Debug.Log($"Id {i}, Sides {sides}");
            
            
            if (sides - 1000 < 0)
            {
                //Debug.Log($"Up is free");
                heightAdd += 3;
            }
            else
                sides -= 1000;

            if (sides - 100 < 0)
            {
                //Debug.Log($"Down is free");
                heightAdd += 3;
                rect.anchoredPosition -= new Vector2(0, 3);
            }
            else
                sides -= 100;

            if (sides - 10 < 0)
            {
                //Debug.Log($"Left is free");
                widthAdd += 3;
                rect.anchoredPosition -= new Vector2(3, 0);
            }
            else
                sides -= 10;
            
            if (sides - 1 < 0)
            {
                //Debug.Log($"Right is free");
                widthAdd += 3;
            }
            else
                sides -= 1;
            
            rect.sizeDelta = new Vector2(pieceData[i].sprite.rect.width + widthAdd, pieceData[i].sprite.rect.height + heightAdd);
            
            pieces.Add(piece);
        }
    }

    public int GetAttachablePosition(Vector2 mousePosition, Vector2 piecePosition)
    {
        //Debug.Log("Requested GetAttachablePosition");
        for (int i = 0; i < positions.Length; i++)
        {
            if(piecesAttached[i] != -1) continue;
            
            //Debug.Log($"Distance between {i} and {position} is {(positions[i] - position).magnitude}");
            if ((positions[i] - (mousePosition + cellOffset)).magnitude <= lockOnDistance
                || (positions[i] - piecePosition).magnitude <= lockOnDistance)
                return i;
        }

        return -1;
    }


    private Coroutine pieceMoveRoutine;
    public void AttachPieceToCell(Piece piece, int cell)
    {
        piecesAttached[cell] = piece.id;
        pieceMoveRoutine = StartCoroutine(
            Animations.MoveRectTransformAnchored(piece.rTransform, positions[cell], .1f, Eases.EaseInCubic));
        
        if(IsComplete())
            Debug.Log($"Puzzle is complete");
    }

    public void DetachPieceFromCell(int cell)
    {
        piecesAttached[cell] = -1;
    }

    public bool IsComplete()
    {
        foreach (Piece piece in pieces)
        {
            if (!piece.isComplete) return false;
        }
        return true;
    }
}

public enum State
{
    Intro,
    Puzzle,
    Fail,
    Success
}
