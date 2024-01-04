using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScene : MonoBehaviour
{
    public static LevelScene instance;

    [SerializeField]
    private Transform levelHolderPrefab;
    [SerializeField]
    private Transform levelsContainer;

    public Transform sceneTransition;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        PrepareLevels();
    }
    public void PlayChangeScene()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransition");
    }

    private void PrepareLevels()
    {
        bool isBot = false;
        for (int i = 0; i < LevelManager.instance.levelData.GetLevels().Count; i++)
        {
            Transform holder = Instantiate(levelHolderPrefab, levelsContainer);
            holder.name = i.ToString();
            if (isBot)
            {
                Transform placeHolder = holder.GetChild(0);
                Transform levelBall = holder.GetChild(1);
                placeHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -67);
                levelBall.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
                levelBall.GetComponent<RectTransform>().DOAnchorPosY(-50, 0.5f).SetEase(Ease.InOutBack);
            }

            Level level = LevelManager.instance.levelData.GetLevelAt(i);
            if (LevelManager.instance.levelData.GetLevelAt(i).isPlayable)
            {
                holder.GetComponent<LevelHolder>().EnableHolder();
            }
            else
            {
                holder.GetComponent<LevelHolder>().DisableHolder();
            }
            isBot = !isBot;
        }
    }

}
