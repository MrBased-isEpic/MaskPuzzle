using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleList : Screen
{

    [SerializeField] private Transform gridLayout;
    [SerializeField] private List<PuzzleSO> puzzleList;
    [SerializeField] private GameObject listButtonPrefab;

    protected override void Initialize()
    {
        for (var index = 0; index < puzzleList.Count; index++)
        {
            Button listButton = Instantiate(listButtonPrefab, gridLayout).GetComponent<Button>();
            
            int i = index;
            listButton.onClick.AddListener(() =>
            {
                Debug.Log(i);
                (screenManager.GetScreen<PuzzleScreen>() as PuzzleScreen).currentPuzzle = puzzleList[i];
                GoToScreen<PuzzleScreen>();
            });
        }

        base.Initialize();
    }
}
