using System.Collections;
using UnityEngine;

public class PuzzleScreen : Screen
{
    [SerializeField] private PieceManager pieceManager;
    [SerializeField] private Timer timer;
    [SerializeField] private AudioClip playBGM;
    
    
    public PuzzleSO currentPuzzle;

    protected override void Initialize()
    {
        pieceManager.Init();
        base.Initialize();
    }
    
    public override void Show()
    {
        base.Show();

        StartCoroutine(StartAfterAnimation());
        AudioManager.Instance.StopMusic();
    }

    public override void Hide()
    {
        base.Hide();
        pieceManager.Cleanup();
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
                StartCoroutine(Intro1());
                break;
            case State.Puzzle:
                StartCoroutine(Puzzle());
                break;
            case State.Fail:
                (screenManager.GetScreen<GameOver>() as GameOver).SetWon(false);
                GoToScreen<GameOver>();
                break;
            case State.Success:
                (screenManager.GetScreen<GameOver>() as GameOver).SetWon(true);
                GoToScreen<GameOver>();
                break;
        }
    }

    #region Intro

    private IEnumerator Intro1()
    {
        yield return null;

        pieceManager.transform.localScale = Vector3.one * 1.2f;
        Vector2[] startPositions = pieceManager.StartPuzzle(currentPuzzle);
        
        timer.StartTimer(StartIntro2, 3);
    }

    private void StartIntro2()
    {
        StartCoroutine(Intro2());
    }

    private IEnumerator Intro2()
    {
        Vector2[] startPositions = pieceManager.GetRandomPositions();
        
        AudioManager.Instance.PlayMusic(playBGM, .2f);
        
        yield return StartCoroutine(
            Animations.ScaleTransform(pieceManager.transform, Vector3.one, .6f, Eases.EaseInCubic));
        
        yield return StartCoroutine(pieceManager.AnimatePiecesRoutine(startPositions));

        pieceManager.EnablePieceDragging();

        SwitchState(State.Puzzle);
    }

    #endregion

    #region Puzzle

    private IEnumerator Puzzle()
    {
        yield return null;
        timer.StartTimer(OnTimerEnd);
    }

    private void OnTimerEnd()
    {
        StartCoroutine(TimerEndRoutine());
    }

    private IEnumerator TimerEndRoutine()
    {
        yield return new WaitForSeconds(1f);
        SwitchState(State.Fail);
    }

    public void OnPuzzleSolved()
    {
        SwitchState(State.Success);
    }

    #endregion
    
    #endregion

}

public enum State
{
    Intro,
    Puzzle,
    Fail,
    Success
}
