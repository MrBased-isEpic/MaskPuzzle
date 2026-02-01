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

    public override void Hide()
    {
        pieceManager.Cleanup();
        
        base.Hide();
    }

    IEnumerator StartAfterAnimation()
    {
        yield return new WaitUntil(() => !IsAnimationPlaying());
    }
    
    #region StateMachine


    public State currentState;
    private void SwitchState(State nextState)
    {
        currentState = nextState;

        switch (currentState)
        {
            case State.Intro:
                StartCoroutine(Intro());
                break;
            case State.Puzzle:
                break;
            case State.Fail:
                break;
            case State.Success:
                break;
        }
    }

    private IEnumerator Intro()
    {
        yield return null;

        pieceManager.transform.localScale = Vector3.one * 1.3f;
        pieceManager.StartPuzzle(currentPuzzle);
        
        
    }
    
    #endregion
}

public enum State
{
    Intro,
    Puzzle,
    Fail,
    Success
}
