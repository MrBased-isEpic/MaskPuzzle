using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : Screen
{
    [SerializeField] private Image textImage;
    
    [SerializeField] private Sprite gameOverSprite;
    [SerializeField] private Sprite gameWinSprite;
    
    [SerializeField] private Button restartButton;
    
    [SerializeField] private AudioClip GameWinSound;
    [SerializeField] private AudioClip GameOverSound;

    protected override void Initialize()
    {
        restartButton.onClick.AddListener(GoToScreen<MainMenu>);
        base.Initialize();
    }

    public void SetWon(bool won)
    {
        if (won)
        {
            textImage.sprite = gameWinSprite;
            AudioManager.Instance.PlayMusic(GameWinSound, 1);
        }
        else
        {
            textImage.sprite = gameOverSprite;
            AudioManager.Instance.PlayMusic(GameOverSound, 1);
        }
    }
}
