using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [SerializeField] private PuzzleSO puzzle;
    [SerializeField] private GameObject piecePrefab;

    private void Start()
    {
        SpawnAllPieces();
    }

    private void SpawnAllPieces()
    {
        List<PieceData> pieceData = puzzle.GetPieceData();

        Vector2 offset = new Vector2((UnityEngine.Screen.width / 2) - (puzzle.fullSprite.rect.width / 2),
            (UnityEngine.Screen.height / 2) - (puzzle.fullSprite.rect.height / 2));

        for (int i = 0; i < pieceData.Count; i++)
        {
            Piece piece = Instantiate(piecePrefab, this.transform).GetComponent<Piece>();
            piece.rTransform.anchoredPosition = pieceData[i].position + offset;
            piece.SetIds(pieceData, i);
        }
    }
}
