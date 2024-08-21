using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelData
{
    public int gamePlay1;
    public int gamePlay2;
}

public class UniversalLevelManager : MonoBehaviour
{
    public List<LevelData> levels;
    public GameManager gameManager = null;

    public Player player;

    void Start()
    {
        int level = LevelSelector.selectedLevel - 1;
        gameManager = Instantiate(gameManager);

        int id1 = levels[level].gamePlay1;
        if (id1 == 1)
        {
            player.gameObject.SetActive(true);
        }
        else {
            player.gameObject.SetActive(false);
        }
        gameManager.Initialize(id1);
    }

    public void GoBackToLevelSelection()
    {
        SceneManager.LoadScene("level");
    }
}
