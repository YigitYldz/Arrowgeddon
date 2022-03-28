using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int Level { get; private set; }
    public int Gold { get; private set; }
    public int Arrow { get; private set; }

    private readonly string levelData = "level-data";
    private readonly string goldData = "gold-data";
    private readonly string arrowData = "arrow-data";

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
        Gold = PlayerPrefs.GetInt(goldData, 0);
        Arrow = PlayerPrefs.GetInt(arrowData, 1);
    }

    public void SetLevel(int level)
    {
        Level = level;
        PlayerPrefs.SetInt(levelData, level);
        PlayerPrefs.Save();
    }

    public void SetGold(int gold)
    {
        Gold = gold;
        PlayerPrefs.SetInt(goldData, gold);
        PlayerPrefs.Save();
    }

    public void SetArrow(int arrowCount)
    {
        Arrow = arrowCount;
        PlayerPrefs.SetInt(arrowData, arrowCount);
        PlayerPrefs.Save();
    }
}
