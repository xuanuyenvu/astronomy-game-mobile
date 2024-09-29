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
            SetUpLevel(0);
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

    private void SetUpLevel(int st)
    {
        StartStage(level.stages[st].stage, level.stages[st].elements, level.cards_displayed);
    }

    private void StartStage(int id, int[] planets, int cardsDisplayed)
    {
        if (id == 2 || id == 3)
        {
            player.RegisterInputEvents();
        }
        else
        {
            player.UnregisterInputEvents();
        }

        // Gọi phương thức Initialize trên thể hiện gameManager
        gameManager.Initialize(id, planets, cardsDisplayed);
    }

    public void EndStage()
    {
        gameManager.DestroyCurrentGamePlay();
        if (level.total_stages == 1)
        {
            GoBackToMap();
        }
        else
        {
            level.total_stages--;
            SetUpLevel(1);
        }
    }

    public void GoBackToMap()
    {
        SceneManager.LoadScene("level");
    }
}