using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int playLevel = -1;

    private List<GameObject> levels;
    private GameObject levelObject;

    private void Awake()
    {
        GameManager.Instance.endGameEvent.AddListener(HandleEndGameEvent);

        Construct();
    }

    private void Construct()
    {
        if (playLevel == -1)
        {
            playLevel = DataManager.Instance.Level;
        }

        levels = new List<GameObject>(Resources.LoadAll<GameObject>("Levels"));

        levelObject = Instantiate(levels[playLevel - 1], Vector3.zero, Quaternion.identity, null);
    }

    private void HandleEndGameEvent(bool success)
    {
        if (success)
        {
            LevelUp();
        }
    }
        
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LevelUp()
    {
        int nextLevel = DataManager.Instance.Level + 1;

        if (nextLevel > levels.Count)
        {
            nextLevel = 1;
        }

        DataManager.Instance.SetLevel(nextLevel);
    }
}
