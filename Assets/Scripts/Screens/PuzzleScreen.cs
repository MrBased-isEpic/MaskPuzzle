using System.Collections;
using UnityEngine;

public class PuzzleScreen : Screen
{
    [SerializeField] private PieceManager pieceManager;
    
    public PuzzleSO currentPuzzle;

    protected override void Initialize()
    {
        base.Initialize();

        pieceManager.Init();
    }
    
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
        SwitchState(State.Intro);
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
        Vector2[] startPositions = pieceManager.StartPuzzle(currentPuzzle);
        
        yield return new WaitForSeconds(2);
        
        yield return StartCoroutine(
            Animations.ScaleTransform(pieceManager.transform, Vector3.one, .6f, Eases.EaseInCubic));
        
        yield return StartCoroutine(pieceManager.AnimatePiecesRoutine(startPositions));

        pieceManager.EnablePieceDragging();

        SwitchState(State.Puzzle);
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
