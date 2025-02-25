using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalLevelManager : MonoBehaviour
{
    public Level level = null;
    public Player player;
    public GameManager gameManagerPrefab;

    public HealthManager healthManager;
    public TimerManager timerManager;
    public EnergyManager energyManager;
    public StageUIManager stageManager;
    public TutorialManager tutorialManager;
    
    public WarpSpeedController wrapSpeedController;
    public GameOverUIController gameOverUiController;
    public WinGameUIController winGameUiController;
    public UIController uiController;
    
    private int currentStageID = 0;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab);
        }
        
        AudioManager.Instance.PlayMusic("In Game");
        LoadAndSetUpLevel(DataSaver.Instance.selectedLevel);
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
        TextAsset jsonFile = Resources.Load<TextAsset>("level_data_part1");

        if (jsonFile != null)
        {
            // phân tích file JSON thành LevelsData object
            LevelsData levelsData = JsonUtility.FromJson<LevelsData>(jsonFile.text);

            // gán các thông tin vào biến level
            Debug.Log("selectedLevel = " + _selectedLevel);
            level = levelsData.levels[_selectedLevel - 1];
            Debug.Log("level Data = " + levelsData.levels[_selectedLevel - 1].level);
            
            energyManager.SetUp();
            healthManager.SetUp(level.lives);
            stageManager.Initialize(level.total_stages);
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
        GameManager.Instance.Initialize(id, planets, cardsDisplayed);
        
        tutorialManager.gameObject.SetActive(true);
        switch (level.level)
        {
            case 1:
                tutorialManager.StartTutorialLevel1();
                break;
            case 2:
                tutorialManager.StartTutorialLevel2();
                break;
            case 4:
                tutorialManager.StartTutorialLevel4();
                break;
            case 7:
                tutorialManager.StartTutorialLevel7();
                break;
        }
    }

    public void EndStage()
    {
        int energyToAdd = GetEnergyForLevel(level.level);
        currentStageID++;
        
        if (level.total_stages - currentStageID == 0)
        {
            stageManager.UpdateStageUI();
            winGameUiController.StartUI((float)energyToAdd);
            // gameManager.UpdateFinalEnergy();
            // gameManager.DestroyCurrentGamePlay();
            // GoBackToMap();
        }
        else
        {
            energyManager.ChangeEnergy(energyToAdd);
            GameManager.Instance.DestroyCurrentGamePlay();

            stageManager.UpdateStageUI();
            StartCoroutine(HandleStageCompletion(currentStageID));
        }
    }

    private int GetEnergyForLevel(int level)
    {
        if (level <= 2)
        {
            return 100;
        }
        else if (level <= 6)
        {
            return 45;
        }
        else if (level <= 12)
        {
            return 35;
        }
        else
        {
            return 25;
        }
    }

    private IEnumerator HandleStageCompletion(int stageIndex)
    {
        AudioManager.Instance.PlaySFX("Warp");
        yield return StartCoroutine(wrapSpeedController.ActivateForThreeSeconds());
        SetUpLevel(stageIndex);
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
        uiController.Reset();
        gameOverUiController.StopUI(level.level > 6);
        timerManager.gameObject.SetActive(true);
        
        stageManager.gameObject.SetActive(true);
        stageManager.SetUp();
        
        StartCoroutine(DelayedLoadLevel());
    }
    
    private IEnumerator DelayedLoadLevel()
    {
        GameManager.Instance.DestroyCurrentGamePlay();
        yield return new WaitForEndOfFrame(); // Chờ đến cuối frame hiện tại
        LoadAndSetUpLevel(level.level);
    }

    public void Retry()
    {
        Debug.Log("Retry");
        gameOverUiController.StopUI(level.level > 6);
        StartCoroutine(DelayedRetry());
    }

    private IEnumerator DelayedRetry()
    {
        GameManager.Instance.DestroyCurrentGamePlay();
        yield return new WaitForEndOfFrame(); // Chờ đến cuối frame hiện tại

        if (level.level > 6)
        {
            if (healthManager.health == 0 && (timerManager.IsLessThan15Seconds || timerManager.IsTimeOver))
            {
                healthManager.SetUp(1);
                timerManager.AddTime();
            }
            else if (healthManager.health == 0)
            {
                healthManager.SetUp(2);
            }
            else
            {
                timerManager.AddTime();
            }

            // timerManager.IsTimeOver = false;
            timerManager.gameObject.SetActive(true);
        }
        else if (level.level > 2)
        {
            healthManager.SetUp(2);
        }
        else
        {
            healthManager.SetUp(1);
        }
        
        stageManager.gameObject.SetActive(true);
        SetUpLevel(currentStageID);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        gameOverUiController.StartUI();
    }

    public void StopTutorial()
    {
        tutorialManager.StopTutorialLevel();
    }
}