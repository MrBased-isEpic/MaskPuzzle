using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour
{
    private bool initialized;

    protected ScreenManager screenManager;
    protected Animator animator;
    
    [Tooltip("Leave as Empty, Sets Automatically")]
    public GameObject content;
    
    private AnimatorStateInfo clipInfo;
   
    public void Setup()
    {
        //DebugLogger.Log($"Setup Screen Called {transform.name}");
        screenManager = GetComponentInParent<ScreenManager>();
        content = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        content.SetActive(false);
        initialized = false;
    }

    #region INTERFACE

    public virtual void Show()
    {
        Show(true);
    }

    public virtual void Show(bool animate)
    {
        //DebugLogger.Log($"Screen Check For {initialized} {transform.name}");
        if (!initialized)
        {
            Initialize();
            //DebugLogger.Log($"Init Screen {transform.name}");
        }

        if (content.activeSelf)
        {
            //DebugLogger.Log($"Content is Active {content.activeSelf} {transform.name}");
            return;
        }
        
        content.SetActive(true);

        if (animator.enabled && animate)
        {
            //DebugLogger.Log($"Animator is  {animator.enabled} and Called Entry {transform.name}");
            //animator.ResetTrigger("Entry");
            animator.SetTrigger("Entry");
        }
    }

    public virtual void Hide()
    {
        if (animator.enabled)
        {
            //DebugLogger.Log($"Animator is  {animator.enabled} and Called Exit {transform.name}");
            //animator.ResetTrigger("Exit");
            animator.SetTrigger("Exit");
        }
        StartCoroutine(WaitForHideAnimationAndTurnOff());
    }

    #endregion

    #region FOR_CHILDREN

    protected virtual void Initialize()
    {
        //DebugLogger.Log($"BAse Screen init Called  {transform.name}");
    }

    protected void RequestScreen<Screen>()
    {
        if(screenManager != null)
            screenManager.GoToScreen(typeof(Screen));
        else
            Debug.LogError("No manager controls this screen");
    }

    #endregion


    private IEnumerator WaitForHideAnimationAndTurnOff()
    {
        Debug.Log($"Waiting For Hide Animation complete {transform.name}");
        yield return null;
        while (IsInAnimation())
        {
            yield return null;
        }
        Debug.Log($"Turn off Content {transform.name}");
        content.SetActive(false);
    }

    public bool IsInAnimation()
    {
        clipInfo = animator.GetCurrentAnimatorStateInfo(0);
        //DebugLogger.Log($"ISinAnimation {!(clipInfo.normalizedTime > 1)} {transform.name}");
        return !(clipInfo.normalizedTime > 1);
    }
}
