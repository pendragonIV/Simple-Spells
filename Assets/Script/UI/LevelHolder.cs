using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelHolder : MonoBehaviour, IPointerClickHandler
{
    private const string GAME = "GameScene";
    [SerializeField]
    private Text levelIndexText;
    [SerializeField]
    private Transform holderFilter;
    [SerializeField]
    private CanvasGroup holderCG;

    private void Start()
    {
        levelIndexText.text = (int.Parse(this.gameObject.name) + 1).ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LevelManager.instance.currentLevelIndex = int.Parse(this.gameObject.name);
        ChangeToGameScene();
    }

    public void DisableHolder()
    {
        holderFilter.gameObject.SetActive(true);
        holderCG.interactable = false;
        holderCG.blocksRaycasts = false;
    }

    public void EnableHolder()
    {
        holderFilter.gameObject.SetActive(false);
        holderCG.interactable = true;
        holderCG.blocksRaycasts = true;
    }

    public void ChangeToGameScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(GAME));
    }

    private IEnumerator ChangeScene(string sceneName)
    {

        //Optional: Add animation here
        LevelScene.instance.PlayChangeScene();
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadSceneAsync(sceneName);

    }
}
