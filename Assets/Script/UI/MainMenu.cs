using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform gameLogo;
    [SerializeField]
    private Transform tutorPanel;
    [SerializeField]
    private Transform guideLine;
    [SerializeField]
    private Transform components;
    [SerializeField]
    private Transform startButton;


    private void Start()
    {
        tutorPanel.gameObject.SetActive(false);

        startButton.localScale = Vector3.zero;
        startButton.DOScale(1, 2f).SetEase(Ease.OutElastic).SetUpdate(true);

        gameLogo.GetComponent<Image>().fillAmount = 0;

        gameLogo.GetComponent<CanvasGroup>().alpha = 0f;
        gameLogo.GetComponent<CanvasGroup>().DOFade(1, 2.2f).SetUpdate(true);

        StartCoroutine(DisplayLogo());
    }

    private IEnumerator DisplayLogo()
    {
        while (gameLogo.GetComponent<Image>().fillAmount < 1)
        {
            gameLogo.GetComponent<Image>().fillAmount += Time.deltaTime / 1.2f;
            yield return new WaitForEndOfFrame();
        }
        gameLogo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-10, 130), 1.5f).SetUpdate(true).OnComplete(() =>
        {
            gameLogo.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10, 130);
            gameLogo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(10, 130), 3f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        });
    }

    public void ShowTutorPanel()
    {
        tutorPanel.gameObject.SetActive(true);
        guideLine.gameObject.SetActive(true);
        FadeIn(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>());
        components.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

    public void HideTutorPanel()
    {
        StartCoroutine(FadeOut(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>()));
        components.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
    }   

    private void FadeIn(CanvasGroup canvasGroup ,RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        rectTransform.GetComponent<CanvasGroup>().alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true).OnComplete(() =>
        {
            rectTransform.GetComponent<CanvasGroup>().DOFade(1, .3f).SetUpdate(true);
        });
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(.3f);
        guideLine.gameObject.SetActive(true);
        tutorPanel.gameObject.SetActive(false);

    }

}
