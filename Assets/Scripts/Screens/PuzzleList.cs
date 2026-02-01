using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleList : Screen
{
    
    [SerializeField] private List<PuzzleSO> puzzleList;
    [SerializeField] private GameObject listButtonPrefab;
    [SerializeField] private Transform gridLayout;

    protected override void Initialize()
    {
        for (var index = 0; index < puzzleList.Count; index++)
        {
            Button listButton = Instantiate(listButtonPrefab, gridLayout).GetComponent<Button>();
            
            listButton.GetComponent<Image>().sprite = puzzleList[index].fullSprite;
            
            int i = index;
            listButton.onClick.AddListener(() =>
            {
                (screenManager.GetScreen<PuzzleScreen>() as PuzzleScreen).currentPuzzle = puzzleList[i];
                GoToScreen<PuzzleScreen>();
            });
        }

        base.Initialize();
    }
}
