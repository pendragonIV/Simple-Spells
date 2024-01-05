using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    public SceneChanger sceneChanger;
    public GameScene gameScene;
    public Transform pointContainer;

    #region Game status
    private Level currentLevelData;
    private bool isGameWin = false;
    private bool isGameLose = false;
    private bool isGamePause = false;
    private int moveLeft;
    #endregion

    private void Start()
    {
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);

        GameObject map = Instantiate(currentLevelData.map);
        WaysManager.instance.SetContainer(map.transform.GetChild(0), map.transform.GetChild(1), map.transform.GetChild(2));
        pointContainer = map.transform.GetChild(0);

        moveLeft = currentLevelData.moves;
        gameScene.SetMove(moveLeft);

        Time.timeScale = 1;
    }

    public void Win()
    {
        if (LevelManager.instance.levelData.GetLevels().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }
        isGameWin = true;
        StartCoroutine(WaitToWin());
        LevelManager.instance.levelData.SaveDataJSON();
    }

    private IEnumerator WaitToWin()
    {
        yield return new WaitForSecondsRealtime(.5f);
        gameScene.ShowWinPanel();
    }

    public void Lose()
    {
        isGameLose = true;
        gameScene.ShowLosePanel();
        Time.timeScale = 0;
    }

    public void DecreaseMoveLeft()
    {
        moveLeft--;
        gameScene.SetMove(moveLeft);
        if (moveLeft <= 0 && pointContainer.childCount > 3)
        {
            Lose();
        }
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool IsGameLose()
    {
        return isGameLose;
    }

    public void PauseGame()
    {
        isGamePause = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isGamePause = false;
        Time.timeScale = 1;
    }

    public bool IsGamePause()
    {
        return isGamePause;
    }
}

