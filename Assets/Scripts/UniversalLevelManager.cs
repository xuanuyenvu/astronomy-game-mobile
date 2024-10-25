using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalLevelManager : MonoBehaviour
{
    public Level level = null;
    public Player player;
    public GameManager gameManager;

    public HealthManager healthManager;
    public TimerManager timerManager;
    public WarpSpeedController wrapSpeedController;
    public GameOverUIController gameOverUiController;

    void Start()
    {
        // nhận giá trị level từ nút bấm
        // int selectedLevel = LevelSelector.selectedLevel;
        int selectedLevel = 18;

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
            healthManager.SetUp(level.lives);
            if (level.level > 6)
            {
                timerManager.SetUp(ConvertTimeToSeconds(level.total_time));
            }
            else
            {
                timerManager.timerSlider.gameObject.SetActive(false);
            }

            if (level != null)
            {
                return true;
            }
            else
            {
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
        // gameOverUiController.StartUI();
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
        if (level.total_stages == 1)
        {
            gameManager.UpdateFinalEnergy();
            // gameManager.DestroyCurrentGamePlay();
            // GoBackToMap();
        }
        else
        {
            gameManager.DestroyCurrentGamePlay();
            level.total_stages--;
            StartCoroutine(HandleStageCompletion());
        }
    }

    private IEnumerator HandleStageCompletion()
    {
        yield return StartCoroutine(wrapSpeedController.ActivateForThreeSeconds());
        SetUpLevel(1);
    }



    public void GoBackToMap()
    {
        SceneManager.LoadScene("level");
    }

    private int ConvertTimeToSeconds(string total_time)
    {
        // Split the string by colon
        string[] timeParts = total_time.Split(':');

        // Parse minutes and seconds
        int minutes = int.Parse(timeParts[0]);
        int seconds = int.Parse(timeParts[1]);

        // Calculate total seconds
        return (minutes * 60) + seconds;
    }
}