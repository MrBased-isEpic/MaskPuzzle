using System.Collections;
using UnityEngine;

public class PuzzleScreen : Screen
{
    [SerializeField] private PieceManager pieceManager;
    
    public PuzzleSO currentPuzzle;

    // protected override void Initialize()
    // {
    //     base.Initialize();
    // }
    
    public override void Show()
    {
        base.Show();

        StartCoroutine(StartAfterAnimation());
    }

    IEnumerator StartAfterAnimation()
    {
        yield return new WaitUntil(() => !IsInAnimation());
        
        pieceManager.Init(currentPuzzle);
    }
}
