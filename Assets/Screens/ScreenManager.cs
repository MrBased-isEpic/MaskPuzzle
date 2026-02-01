using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public bool selfStart = true;
    public bool showFirstScreen = true;
    public bool createSingleton = true;
    
    private Dictionary<int, Screen> screens;
    private Screen current;

    private bool initialized;

    public static ScreenManager Instance;

    public void Start()
    {
        if (selfStart)
            Initialize();
    }
    public void Initialize()
    {
        if (initialized) return;
        
        if(createSingleton && Instance == null && Instance != this)
            Instance = this;
        
        screens = new Dictionary<int, Screen>(transform.childCount);

        Screen temp;
        for (int i = 0; i < transform.childCount; i++)
        {
            temp = transform.GetChild(i).GetComponent<Screen>();
            temp?.Setup();

            //DebugLogger.LogError(temp.GetType().Name);
            screens.Add(temp.GetType().Name.GetHashCode(), temp);
        }

        if(showFirstScreen)
            ShowScreen(screens.ElementAt(0).Value);

        initialized = true;
    }

    public void OnDestroy()
    {
        if(Instance != null)
            Instance = null;
    }
    
    public void GoToScreen<ScreenType>(float delay = 0)
    {
        Screen targetScreen = GetScreen<ScreenType>();

        if (targetScreen.content.activeSelf) return;
        
        List<Screen> closingScreens = new List<Screen>();

        foreach (Screen screen in screens.Values)
        {
            if (screen.content.activeSelf)
            {
                screen.Hide();
                closingScreens.Add(screen);
            }
        }
        
        StopAllCoroutines();
        StartCoroutine(WaitForHideAnimationAndShow(closingScreens, targetScreen, delay));
    }

    public void OpenScreen<ScreenType>(float delay = 0)
    {
        Screen targetScreen = GetScreen<ScreenType>();

        if (targetScreen.content.activeSelf) return;
        
        targetScreen.Show();
    }
    
    public Screen GetScreen<ScreenType>()
    {
        int hash = typeof(ScreenType).Name.GetHashCode();
        if (!screens.ContainsKey(hash))
        {
            Debug.LogError($"Can't find requested screen {typeof(ScreenType).Name}");
            return (screens.ElementAt(0).Value);
        }

        return screens[hash];
    }
    
    #region PRIVATE_METHODS
    
    private IEnumerator WaitForHideAnimationAndShow(List<Screen> hiding, Screen target, float delay)
    {
        yield return new WaitForSeconds(delay);

        bool isAnimationFinished = false;
        if (hiding != null && hiding.Count > 0)
        {
            do
            {
                yield return null;
                isAnimationFinished = true;
                foreach (Screen h in hiding)
                {
                    if (h.IsAnimationPlaying())
                    {
                        isAnimationFinished = false;
                        break;
                    }
                }
            }while (!isAnimationFinished);
            
        }

        ShowScreen(target);
    }

    private void ShowScreen(Screen screen)
    {
        screen.Show();
    }

    #endregion
}
