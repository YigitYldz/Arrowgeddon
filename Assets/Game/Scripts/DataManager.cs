using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int Level { get; private set; }

    private readonly string levelData = "level-data";

    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        Level = PlayerPrefs.GetInt(levelData, 1);
    }

    public void SetLevel(int level)
    {
        Level = level;
        PlayerPrefs.SetInt(levelData, level);
        PlayerPrefs.Save();
    }
}
