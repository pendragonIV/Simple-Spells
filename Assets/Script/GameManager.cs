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
    //public GameScene gameScene;

    #region Game status
    private Level currentLevelData;
    private bool isGameWin = false;
    private bool isGameLose = false;
    private bool isGamePause = false;

    [SerializeField]
    public int achivement = 0;
    private const int MAX_ACHIVE = 3;
    #endregion

    private void Start()
    {
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);

        Time.timeScale = 1;
    }

    public void Win()
    {
        if (LevelManager.instance.levelData.GetLevels().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false, 0);
            }
        }
        SetAchivement();
        if (achivement > LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex).achivement)
        {
            LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex, true, true, achivement);
        }

        isGameWin = true;

        //gameScene.ShowWinPanel();
        Time.timeScale = 0;
        LevelManager.instance.levelData.SaveDataJSON();
    }

    private void SetAchivement()
    {
        
    }

    public void Lose()
    {
        isGameLose = true;
        //gameScene.ShowLosePanel();
        Time.timeScale = 0;
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

