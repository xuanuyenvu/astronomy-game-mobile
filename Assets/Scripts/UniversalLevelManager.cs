using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalLevelManager : MonoBehaviour
{
    public Level level = null;
    public Player player;
    public GameManager gameManager;

    void Start()
    {
        // nhận giá trị level từ nút bấm
        int selectedLevel = LevelSelector.selectedLevel;

        if (loadJson(selectedLevel))
        {
            SetUpLevel();
        }
    }

    private bool loadJson(int _selectedLevel)
    {
        // load JSON từ Resources
        TextAsset jsonFile = Resources.Load<TextAsset>("levels");

        if (jsonFile != null)
        {
            // phân tích file JSON thành LevelsData object
            LevelsData levelsData = JsonUtility.FromJson<LevelsData>(jsonFile.text);

            // gán các thông tin vào biến level
            level = levelsData.levels[_selectedLevel - 1];

            if(level != null)
            {
                return true;
            }
            else {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void SetUpLevel()
    {
        StartStage(level.stages[0].stage, level.stages[0].elements);
    }

    private void StartStage(int id, int[] planets)
    {
        if (id == 2 || id == 3)
        {
            player.gameObject.SetActive(true);
        }
        else
        {
            player.gameObject.SetActive(false);
        }

        // Gọi phương thức Initialize trên thể hiện gameManager
        gameManager.Initialize(id, planets);
    }

    public void GoBackToMap()
    {
        SceneManager.LoadScene("level");
    }
}
