using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public bool selfStart = true;
    public bool showFirstScreen = true;
    
    private Dictionary<string, Screen> screens;
    private Screen current;

    private bool initialized;


    public void Start()
    {
        if (selfStart)
            Initialize();
    }
    public void Initialize()
    {
        if (initialized) return;

        screens = new Dictionary<string, Screen>(transform.childCount);

        Screen temp;
        for (int i = 0; i < transform.childCount; i++)
        {
            temp = transform.GetChild(i).GetComponent<Screen>();
            temp?.Setup();

            //DebugLogger.LogError(temp.GetType().Name);
            screens.Add(temp.GetType().Name, temp);
        }

        if(showFirstScreen)
            ShowScreen(screens.ElementAt(0).Value);

        initialized = true;
    }

    public void GoToScreen(Type type, float delay = 0)
    {
        if (!screens.ContainsKey(type.Name)) return;
        
        Screen targetScreen = screens[type.Name];
        //Debug.LogError($"targetScreen exists");
        if (targetScreen == current && targetScreen.content.activeSelf) return;
        
        current?.Hide();
        
        StopAllCoroutines();
        StartCoroutine(WaitForHideAnimationAndShow(current, targetScreen, delay));
    }

    public Screen GetScreen<ScreenType>()
    {
        if (!screens.ContainsKey(typeof(ScreenType).Name))
            return (screens.ElementAt(0).Value);
        
        return screens[typeof(ScreenType).Name];
    }
    
    #region PRIVATE_METHODS

    private IEnumerator WaitForHideAnimationAndShow(Screen hiding, Screen target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (hiding != null)
        {
            while (hiding.IsInAnimation())
            {
                yield return null;
            }
        }

        ShowScreen(target);
    }

    private void ShowScreen(Screen screen)
    {
        current = screen;

        if (current != null)
            current.Show();
    }

    #endregion
}
