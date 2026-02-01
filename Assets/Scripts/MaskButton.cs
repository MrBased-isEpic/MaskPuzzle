using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaskButton : MonoBehaviour, IPointerEnterHandler
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsIdleAnimationPlaying()) return;

        if (Mathf.Abs(eventData.delta.x) < 2) return;
        
        string triggerName = eventData.delta.x > 0? "Right" : "Left";
        
        animator.SetTrigger(triggerName);
    }

    private bool IsIdleAnimationPlaying()
    {
        AnimatorStateInfo clipInfo = animator.GetCurrentAnimatorStateInfo(0);
        return clipInfo.normalizedTime >= 1 || clipInfo.IsName("MB_Start");
    }
}
