using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Screen
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    
    [SerializeField] private AudioClip mainMenuBGM;

    protected override void Initialize()
    {
        playButton.onClick.AddListener(GoToScreen<PuzzleList>);
        
        quitButton.onClick.AddListener((() =>
        {
            Application.Quit();
        }));
        
        base.Initialize();
    }

    public override void Show()
    {
        base.Show();
        AudioManager.Instance.PlayMusic(mainMenuBGM, .8f);
    }
}
