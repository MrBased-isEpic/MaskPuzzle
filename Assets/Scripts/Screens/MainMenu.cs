using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Screen
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    protected override void Initialize()
    {
        playButton.onClick.AddListener(() =>
        {
            RequestScreen<PuzzleScreen>();
        });
        
        quitButton.onClick.AddListener((() =>
        {
            Application.Quit();
        }));
        
        base.Initialize();
    }
}
