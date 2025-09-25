using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
 
public class FadeInOut : MonoBehaviour
{
    public CanvasGroup canvasGroup;
 
    void Awake()
    {
        canvasGroup.gameObject.SetActive(false);
    }
 
    public void FadeIn() //페이드 인 사용
    {
        StartCoroutine(Fade(true));
    }
 
    public void FadeOut() //페이드 아웃 사용
    {
        StartCoroutine(Fade(false));
    }
 
    private IEnumerator Fade(bool isFadeIn)
    {
        if (isFadeIn)
        {
            canvasGroup.alpha = 1;
            Tween tween = canvasGroup.DOFade(0f, 1f);
            yield return tween.WaitForCompletion();
            canvasGroup.gameObject.SetActive(false);
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(true);
            Tween tween = canvasGroup.DOFade(1f, 1f);
            yield return tween.WaitForCompletion();
            StartCoroutine(Fade(true));
        }
    }
    
    public IEnumerator FadeOutCoroutine()
    {
        yield return StartCoroutine(Fade(false)); // 기존 Fade(false) 코루틴 재사용
    }
}