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
        int selectedLevel = 2;
        LoadAndSetUpLevel(selectedLevel);
    }

    private void LoadAndSetUpLevel(int level)
    {
        if (IsJsonLoaded(level))
        {
            SetUpLevel(0);
        }
    }


    private bool IsJsonLoaded(int _selectedLevel)
    {
        // load JSON từ Resources
        TextAsset jsonFile = Resources.Load<TextAsset>("levels");

        if (jsonFile != null)
        {
            // phân tích file JSON thành LevelsData object
            LevelsData levelsData = JsonUtility.FromJson<LevelsData>(jsonFile.text);

            // gán các thông tin vào biến level
            level = levelsData.levels[_selectedLevel - 1];
            Debug.Log("Level: " + level);
            healthManager.SetUp(level.lives);
            if (level.level > 6)
            {
                timerManager.SetUp(ConvertTimeToSeconds(level.total_time));
            }
            else
            {
                timerManager.gameObject.SetActive(false);
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
        SceneManager.LoadScene("chapter1");
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

    public void LoadGame()
    {
        Debug.Log("Load Game");
        LoadAndSetUpLevel(level.level);
    }

    public void Retry()
    {
        Debug.Log("Retry");
        gameOverUiController.StopUI(level.level > 6);
        gameManager.DestroyCurrentGamePlay();

        if (level.level > 6)
        {
            if (healthManager.health == 0 && timerManager.IsTimeOver)
            {
                healthManager.SetUp(1);

                timerManager.SetUp(15);
                timerManager.IsTimeOver = false;
            }
            else if (healthManager.health == 0)
            {
                healthManager.SetUp(2);
            }
            else //(timerManager.IsTimeOver == true)
            {
                timerManager.SetUp(25);
                timerManager.IsTimeOver = false;
            }
        }
        else if (level.level > 2)
        {
            healthManager.SetUp(2);
        }
        else
        {
            healthManager.SetUp(1);
        }

        SetUpLevel(level.total_stages == 1 ? 0 : 1);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        gameOverUiController.StartUI();
    }
}