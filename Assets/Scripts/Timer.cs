using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer;
    [SerializeField] private TextMeshProUGUI timerText;

    public void StartTimer(Action OnEnd, float time = 30f)
    {
        if(timerCoroutine != null)
            StopCoroutine(timerCoroutine);
        
        timerCoroutine = StartCoroutine(TimerRoutine(OnEnd, time));
    }
    
    Coroutine timerCoroutine;
    IEnumerator TimerRoutine(Action OnEnd, float time)
    {
        timer = time;

        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
        }

        timerCoroutine = null;
        OnEnd?.Invoke();
    }
    
}
