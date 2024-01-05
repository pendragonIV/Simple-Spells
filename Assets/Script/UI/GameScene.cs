using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button homeButton;

    [SerializeField]
    private Transform winButtonGroup;
    [SerializeField]
    private Transform loseButtonGroup;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Text moveLeft;

    [SerializeField]
    private Transform nextBtn;

    public void SetMove(int move)
    {
        moveLeft.text = move.ToString();
    }

    private void HidePlayer()
    {
        player.GetComponent<CanvasGroup>().DOFade(0, .3f).SetUpdate(true);  
    }

    private void ShowWinButton()
    {
        foreach (Transform child in winButtonGroup)
        {
            Vector3 defaulPos = child.GetComponent<RectTransform>().anchoredPosition;

            child.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 200, 0);
            child.GetComponent<RectTransform>().DOAnchorPos(defaulPos, .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
            child.GetComponent<CanvasGroup>().alpha = 0f;
            child.GetComponent<CanvasGroup>().DOFade(1, .3f).SetUpdate(true);   
        }
    }

    private void ShowLoseButton()
    {
        foreach (Transform child in loseButtonGroup)
        {
            Vector3 defaulPos = child.GetComponent<RectTransform>().anchoredPosition;

            child.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 200, 0);
            child.GetComponent<RectTransform>().DOAnchorPos(defaulPos, .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
            child.GetComponent<CanvasGroup>().alpha = 0f;
            child.GetComponent<CanvasGroup>().DOFade(1, .3f).SetUpdate(true);
        }
    }

    public void ShowWinPanel()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
        nextBtn.transform.localScale = Vector3.zero;
        nextBtn.DOScale(1, 1f).SetEase(Ease.OutBack).SetUpdate(true);
        ShowWinButton();
        HidePlayer();
    }

    public void ShowLosePanel()
    {
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
        ShowLoseButton();
        HidePlayer();
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }
}
