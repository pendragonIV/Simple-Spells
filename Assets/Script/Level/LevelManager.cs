using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public LevelData levelData;

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        levelData.LoadDataJSON();
    }

    public int currentLevelIndex;

    private void OnApplicationQuit()
    {
        levelData.SaveDataJSON();
    }
}
