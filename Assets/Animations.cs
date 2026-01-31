using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Animations
{
    //#region SingleTon

    //public static Animations Instance;

    //private void Awake()
    //{
    //    if(Instance == null) Instance = this;
    //}

    //private void OnDestroy()
    //{
    //    Instance = null;
    //}

    //#endregion

    #region Lerps
    public static IEnumerator LerpTransform(Transform target, Transform destination, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress = 0;

        Vector3 startPosition = target.position;
        Quaternion startRotation = target.rotation;
        Vector3 startScale = target.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            target.position = Vector3.Lerp(startPosition, destination.position, progress);
            target.rotation = Quaternion.Lerp(startRotation, destination.rotation, progress);
            target.localScale = Vector3.Lerp(startScale, destination.localScale, progress);

            yield return null;
        }
    }

    public static IEnumerator MoveTransform(Transform target, Transform destination, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress = 0;

        Vector3 startPosition = target.position;

        while (progress < 1)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            target.position = Vector3.Lerp(startPosition, destination.position, progress);

            yield return null;
        }

        target.transform.position = destination.position;
    }

    public static IEnumerator MoveTransform(Transform target, Vector3 destination, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress = 0;

        Vector3 startPosition = target.position;

        while (progress < 1)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            target.position = Vector3.Lerp(startPosition, destination, progress);

            yield return null;
        }

        target.transform.position = destination;
    }

    public static IEnumerator ScaleTransform(Transform target, Vector3 destination, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress = 0;

        Vector3 startPosition = target.localScale;

        while (progress < 1)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            target.localScale = Vector3.Lerp(startPosition, destination, progress);

            yield return null;
        }

        target.transform.localScale = destination;
    }
    
    public static IEnumerator MoveLocalTransform(Transform target, Vector3 destination, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress = 0;

        Vector3 startPosition = target.localPosition;

        while (progress < 1)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            target.localPosition = Vector3.Lerp(startPosition, destination, progress);

            yield return null;
        }

        target.transform.position = destination;
    }

    public static IEnumerator RotateTransform(Transform target, Quaternion targetRotation, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress = 0;

        Quaternion startRotation = target.rotation;


        while (progress < 1)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            target.rotation = Quaternion.Slerp(startRotation, targetRotation, progress);


            yield return null;
        }

        target.rotation = targetRotation;
    }

    public static IEnumerator MoveRectTransformAnchored(RectTransform target, Vector3 destination, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress = 0;

        Vector3 startPosition = target.anchoredPosition;

        while (progress < 1)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            target.anchoredPosition = Vector3.Lerp(startPosition, destination, progress);

            yield return null;
        }

        target.anchoredPosition = destination;
    }

    public static IEnumerator LerpColor(Image target, Color targetColor, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress = 0;

        Color startColor = target.color;

        while (progress < 1)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            target.color = Color.Lerp(startColor, targetColor, progress);


            yield return null;
        }

        target.color = targetColor;
    }

    public static IEnumerator LerpColor(RawImage target, Color targetColor, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress = 0;

        Color startColor = target.color;

        while (progress < 1)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            target.color = Color.Lerp(startColor, targetColor, progress);


            yield return null;
        }

        target.color = targetColor;
    }

    public static IEnumerator LerpAlpha(CanvasGroup target, float targetAlpha, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress = 0;

        float startColor = target.alpha;

        while (progress < 1)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            target.alpha = Mathf.Lerp(startColor, targetAlpha, progress);


            yield return null;
        }

        target.alpha = targetAlpha;
    }

    public static IEnumerator ScrollText(TextMeshProUGUI targetText, string content, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress;

        int length = content.Length;

        while (time <= duration)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            targetText.text = content.Substring(0, Mathf.Clamp(
                            ((int)Mathf.Lerp(0, length, progress)),
                            0, length));

            yield return null;
        }

        targetText.text = content;
    }

    public static IEnumerator LerpVolume(AudioSource audioSource, float volume, float duration = 1, Eases ease = Eases.None)
    {
        EaseFunction easeFunc = GetEaseFunction(ease);

        float time = 0;
        float progress;

        float startVolume = audioSource.volume;

        while (time <= duration)
        {
            time += Time.deltaTime;

            progress = Mathf.InverseLerp(0, duration, time);
            progress = easeFunc(progress);

            audioSource.volume = Mathf.Lerp(startVolume, volume, progress);

            yield return null;
        }

        audioSource.volume = volume;
    }

    #endregion

    #region EaseFunctions

    public delegate float EaseFunction(float t);

    public static EaseFunction GetEaseFunction(Eases easeType)
    {
        EaseFunction easeFunc = null;
        if (enumToFuncMap.ContainsKey(easeType))
        {
            easeFunc = enumToFuncMap[easeType];
            return easeFunc;
        }

        Debug.LogError($"Ease function for {easeType} not found, returning linear ease.");
        easeFunc = enumToFuncMap.Values.ElementAt(0);
        return easeFunc;
    }

    private static Dictionary<Eases, EaseFunction> enumToFuncMap = new Dictionary<Eases, EaseFunction>()
    {
        {Eases.None, NoEase},

        {Eases.EaseInSine, EaseInSine},
        {Eases.EaseOutSine, EaseOutSine},
        {Eases.EaseInOutSine, EaseInOutSine},

        {Eases.EaseInQuad, EaseInQuad},
        {Eases.EaseOutQuad, EaseOutQuad},
        {Eases.EaseInOutQuad, EaseInOutQuad},

        {Eases.EaseInCubic, EaseInCubic},
        {Eases.EaseOutCubic, EaseOutCubic},
        {Eases.EaseInOutCubic, EaseInOutCubic},

        {Eases.EaseInQuart, EaseInQuart},
        {Eases.EaseOutQuart, EaseOutQuart},
        {Eases.EaseInOutQuart, EaseInOutQuart},

        {Eases.EaseInQuint, EaseInQuint},
        {Eases.EaseOutQuint, EaseOutQuint},
        {Eases.EaseInOutQuint, EaseInOutQuint},

        {Eases.EaseInExpo, EaseInExpo},
        {Eases.EaseOutExpo, EaseOutExpo},
        {Eases.EaseInOutExpo, EaseInOutExpo},

        {Eases.EaseInCirc, EaseInCirc},
        {Eases.EaseOutCirc, EaseOutCirc},
        {Eases.EaseInOutCirc, EaseInOutCirc},

        {Eases.EaseInBack, EaseInBack},
        {Eases.EaseOutBack, EaseOutBack},
        {Eases.EaseInOutBack, EaseInOutBack},

        {Eases.EaseInElastic, EaseInElastic},
        {Eases.EaseOutElastic, EaseOutElastic},
        {Eases.EaseInOutElastic, EaseInOutElastic},

        {Eases.EaseInBounce, EaseInBounce},
        {Eases.EaseOutBounce, EaseOutBounce},
        {Eases.EaseInOutBounce, EaseInOutBounce},
    };

    static float NoEase(float x)
    {
        return x;
    }

    // Sine
    static float EaseInSine(float x)
    {
        return 1 - Mathf.Cos((x * Mathf.PI) / 2);
    }
    static float EaseOutSine(float x)
    {
        return Mathf.Sin((x * Mathf.PI) / 2);
    }
    static float EaseInOutSine(float x)
    {
        return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
    }

    // Quad
    static float EaseInQuad(float x)
    {
        return x * x;
    }
    static float EaseOutQuad(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
    static float EaseInOutQuad(float x)
    {
        return (x < 0.5) ? (2 * x * x) : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }

    // Cubic
    static float EaseInCubic(float x)
    {
        return x * x * x;
    }
    static float EaseOutCubic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 3);
    }
    static float EaseInOutCubic(float x)
    {
        return (x < 0.5) ? (4 * x * x * x) : (1 - Mathf.Pow(-2 * x + 2, 3) / 2);
    }

    // Quart
    static float EaseInQuart(float x)
    {
        return x * x * x * x;
    }
    static float EaseOutQuart(float x)
    {
        return 1 - Mathf.Pow(1 - x, 4);
    }
    static float EaseInOutQuart(float x)
    {
        return (x < 0.5) ? (8 * x * x * x * x) : (1 - Mathf.Pow(-2 * x + 2, 4) / 2);
    }

    // Quint
    static float EaseInQuint(float x)
    {
        return x * x * x * x * x;
    }
    static float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }
    static float EaseInOutQuint(float x)
    {
        return (x < 0.5) ? (16 * x * x * x * x * x) : (1 - Mathf.Pow(-2 * x + 2, 5) / 2);
    }

    // Expo
    static float EaseInExpo(float x)
    {
        return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
    }
    static float EaseOutExpo(float x)
    {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }
    static float EaseInOutExpo(float x)
    {
        return x == 0 ? 0 : 
               x == 1 ? 1 :
               x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2 :
               (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
    }

    // Circ
    static float EaseInCirc(float x)
    {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
    }
    static float EaseOutCirc(float x)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    }
    static float EaseInOutCirc(float x)
    {
        return x < 0.5 ?
               (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2 :
               (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
    }

    // Back
    static float EaseInBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return c3 * x * x * x - c1 * x * x;
    }
    static float EaseOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }
    static float EaseInOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return x < 0.5 ?
            (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2 :
            (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
    }

    // Elastic
    static float EaseInElastic(float x)
    {
        const float c4 = (2 * Mathf.PI) / 3;

        return x == 0 ? 0 :
          x == 1 ? 1 :
          -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((float)((x * 10 - 10.75) * c4));
    }
    static float EaseOutElastic(float x)
    {
        const float c4 = (2 * Mathf.PI) / 3;

        return x == 0 ? 0:
          x == 1 ? 1:
          Mathf.Pow(2, -10 * x) * Mathf.Sin((float)(x * 10 - 0.75) * c4) + 1;
    }
    static float EaseInOutElastic(float x)
    {
        const float c5 = (float)((2 * Mathf.PI) / 4.5);

        return x == 0 ? 0:
          x == 1 ? 1:
          x < 0.5 ?
          -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((float)(20 * x - 11.125) * c5)) / 2 :
          (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((float)(20 * x - 11.125) * c5)) / 2 + 1;
    }

    // Bounce
    static float EaseInBounce(float x)
    {
        return 1 - EaseOutBounce(1 - x);
    }
    static float EaseOutBounce(float x)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }
    static float EaseInOutBounce(float x)
    {
        return x < 0.5 ?
               (1 - EaseInBounce(1 - 2 * x)) / 2 :
               (1 + EaseOutBounce(2 * x - 1)) / 2;
    }

    #endregion
}

public enum Eases
{
    None,
    EaseInSine,
    EaseOutSine,
    EaseInOutSine,
    EaseInQuad,
    EaseOutQuad,
    EaseInOutQuad,
    EaseInCubic,
    EaseOutCubic,
    EaseInOutCubic,
    EaseInQuart,
    EaseOutQuart,
    EaseInOutQuart,
    EaseInExpo,
    EaseOutExpo,
    EaseInOutExpo,
    EaseInQuint,
    EaseOutQuint,
    EaseInOutQuint,
    EaseInCirc,
    EaseOutCirc,
    EaseInOutCirc,
    EaseInBack,
    EaseOutBack,
    EaseInOutBack,
    EaseInElastic,
    EaseOutElastic,
    EaseInOutElastic,
    EaseInBounce,
    EaseOutBounce,
    EaseInOutBounce,
}