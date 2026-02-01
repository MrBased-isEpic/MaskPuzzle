using System.Collections;
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
    private CanvasGroup gridAlpha;
    
    [Space]
    [SerializeField] private float lockOnDistance;
    private Dictionary<int, Sprite> gridSpritesDictionary;

    [Space]
    public AudioClip pickUpSFX;
    public AudioClip putDownSFX;

    private Vector2[] positions;
    private int[] piecesAttached;

    public float halfWidth;
    public float halfHeight;

    public Vector2 cellOffset;

    public void Init()
    {
        gridSpritesDictionary = new Dictionary<int, Sprite>();
        for (int i = 0; i < sides.Length; i++)
        {
            gridSpritesDictionary.Add(sides[i], gridSprites[i]);
        }
        
        gridAlpha = gridParent.GetComponent<CanvasGroup>();
    }
    
    public Vector2[] StartPuzzle(PuzzleSO puzzleData)
    {
        puzzle =  puzzleData;
        
        return SpawnAllPieces();
    }

    private Vector2[] SpawnAllPieces()
    {
        List<PieceData> pieceData = puzzle.GetPieceData();

        positions = new Vector2[pieceData.Count];
        piecesAttached = new int[pieceData.Count];

        Vector2[] startPositions =new Vector2[pieceData.Count];
        
        pieces = new List<Piece>();

        gridAlpha.alpha = 0;

        Vector2 worldOffset = new Vector2((UnityEngine.Screen.width / 2) - (puzzle.fullSprite.rect.width / 2),
            (UnityEngine.Screen.height / 2) - (puzzle.fullSprite.rect.height / 2));
        
        halfWidth =  puzzle.pieces[0].rect.width / 2;
        halfHeight = puzzle.pieces[0].rect.height / 2;

        cellOffset = new Vector2(-halfWidth, -halfHeight);

        for (int i = 0; i < pieceData.Count; i++)
        {
            Piece piece = Instantiate(piecePrefab, this.transform).GetComponent<Piece>();

            piece.pieceManager = this;
            
            positions[i] = (pieceData[i].position + worldOffset - cellOffset);
            piecesAttached[i] = -1;

            piece.rTransform.anchoredPosition = positions[i];
            
            int sides = piece.Init(pieceData, i);
            
            RectTransform rect = Instantiate(gridPrefab, gridParent).GetComponent<RectTransform>();
            rect.GetComponent<Image>().sprite = gridSpritesDictionary[sides];

            int heightAdd = 0;
            int widthAdd = 0;
            rect.anchoredPosition = pieceData[i].position + worldOffset;

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
        
        return startPositions;
    }

    public void Cleanup()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Piece piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        
        positions = new Vector2[0];
        pieces.Clear();
        
    }

    public Vector2[] GetRandomPositions()
    {
        Vector2[] startPositions =new Vector2[pieces.Count];

        for (int i = 0; i < pieces.Count; i++)
        {
            startPositions[i] = new Vector2(
                Random.Range(halfWidth, UnityEngine.Screen.width - halfWidth),
                Random.Range(halfHeight, UnityEngine.Screen.height - halfHeight)
            );
        }

        return startPositions;
    }
    
    #region Animations

    public IEnumerator AnimatePiecesRoutine(Vector2[] startPositions)
    {
        Coroutine[] routine = new Coroutine[startPositions.Length];
        
        for (int i = 0; i < startPositions.Length; i++)
        {
            routine[i] = StartCoroutine(
                Animations.MoveRectTransformAnchored(
                    pieces[i].rTransform, startPositions[i], 1f, Eases.EaseOutCubic)
                );
        }

        yield return new WaitForSeconds(1);
        
        yield return StartCoroutine(
            Animations.LerpAlpha(gridAlpha, 1, 1, Eases.None));

        foreach (Coroutine ro in routine)
        {
            yield return ro;
        }
    }

    public void EnablePieceDragging()
    {
        foreach (Piece piece in pieces)
        {
            piece.GetComponent<Image>().raycastTarget = true;
        }
    }
    
    #endregion

    #region PickupDropInterface
    
    public int GetAttachablePosition(Vector2 mousePosition, Vector2 piecePosition)
    {
        //Debug.Log("Requested GetAttachablePosition");
        for (int i = 0; i < positions.Length; i++)
        {
            if(piecesAttached[i] != -1) continue;
            
            //Debug.Log($"Distance between {i} and {position} is {(positions[i] - position).magnitude}");
            if ((positions[i] - mousePosition).magnitude <= lockOnDistance
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
            AttachRoutine(piece, cell));
    }

    private IEnumerator AttachRoutine(Piece piece, int cell)
    {
        yield return StartCoroutine(
            Animations.MoveRectTransformAnchored(piece.rTransform, positions[cell], .1f, Eases.EaseInCubic));
        
        AudioManager.Instance.PlaySfx(putDownSFX);
        
        if(IsComplete())
            (ScreenManager.Instance.GetScreen<PuzzleScreen>() as PuzzleScreen).OnPuzzleSolved();
    }

    public void DetachPieceFromCell(int cell)
    {
        piecesAttached[cell] = -1;
    }
    
    #endregion

    public bool IsComplete()
    {
        foreach (Piece piece in pieces)
        {
            if (!piece.isComplete) return false;
        }
        return true;
    }
}
