using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFader : MonoBehaviour
{
    public CanvasGroup uiElement;
    public float Speed = 0.5f;

    public UnityEvent onFadeIn, onFadeOut;
    private bool Fading = false;

    private void Awake()
    {
        if (uiElement == null)
            uiElement = GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        if (uiElement.alpha == 0)
        {
            uiElement.blocksRaycasts = false;
        }
        else
        {
            uiElement.blocksRaycasts = true;
        }
    }
    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1, Speed));
        uiElement.blocksRaycasts = true;
        Fading = true;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, Speed));
        uiElement.blocksRaycasts = false;
        Fading = false;
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1)  break; 

            yield return new WaitForFixedUpdate();
        }

        if (Fading)
        {
            onFadeIn.Invoke();
        }
        else
        {
            onFadeOut.Invoke();
        }
    }
}
