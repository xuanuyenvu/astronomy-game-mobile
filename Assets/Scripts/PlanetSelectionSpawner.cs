using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlanetSelectionSpawner : IGamePlay
{
    [Header("List of Planets")]
    public List<AstronomicalObject> allPlanets;

    [Header("Rocket and Target")]
    public RocketController rocketPrefab;
    public GameObject targetPrefab;

    private AstronomicalObject planet1 = null;
    private AstronomicalObject planet2 = null;
    private AstronomicalObject planetAnswer = null;
    private RocketController rocket = null;
    private GameObject target = null;

    [Header("List Particle System Boom")]
    public List<ParticleSystem> boomPSPrefab;
    private int screenWidth;
    private int screenHeight;
    private Vector2 screenCenter;
    private bool isLeft;

    private bool animationStart = false;
    private bool animationResult = false;
    private bool playing;

    private ParticleSystem boomInstance = null;

    [Header("Scripts")]
    public CameraShake cameraShake;
    public CardController cardController;
    public HealthManager healthManager;
    public ScoreManager scoreManager;

    void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);

        if (cameraShake == null)
        {
            cameraShake = FindObjectOfType<CameraShake>();
        }
        if (cardController == null)
        {
            cardController = FindObjectOfType<CardController>();
        }
    }

    void Start()
    {
        //Play();
    }

    void Update()
    {
        if (!animationStart && playing)
        {
            rocket.TurnOnCollider = true;
        }
        else
        {
            rocket.TurnOnCollider = false;
        }

        if (cardController.GetNumOfCards() == 0)
        {
            GameOver();
        }

        DestroyEffectAfterFin();
    }

    private void DestroyEffectAfterFin()
    {
        // Hủy hiệu ứng sau khi hành tinh phát nổ
        if (cameraShake.IsShake == 0 && animationResult)
        {
            // Hủy boom instance
            Destroy(boomInstance.gameObject);

            // Đặt lại giá trị
            animationResult = false;
            cameraShake.IsShake = -1;

            // Thiết lập lại game
            ReSetUpGame();
        }
    }

    private void ReSetUpGame()
    {
        if (healthManager.health > 0)
        {
            // Set up lại các object trong màn chơi
            RePlayGame();
            // Bật tính năng chọn thẻ bài
            cardController.turnOnPointerHandler();
        }
        else
        {
            GameOver();
        }
    }

    private void RePlayGame()
    {
        planet1.gameObject.SetActive(true);
        target.SetActive(true);

        Destroy(planetAnswer.gameObject);

        FindMeanAndSetRocket();
        StartCoroutine(SetPositionBeforePlaying(0.5f));
    }

    public void RandomizePosition()
    {
        var id1 = UnityEngine.Random.Range(0, allPlanets.Count);
        planet1 = allPlanets[id1];

        var id2 = 0;
        if (id1 < 4)
        {
            id2 = UnityEngine.Random.Range(0, 4);
        }
        else
        {
            id2 = UnityEngine.Random.Range(4, allPlanets.Count);
        }
        planet2 = allPlanets[id2];

        isLeft = Random.Range(0, 2) == 0 ? true : false;
        planet1 = Clone(planet1, isLeft);
        planet1.gameObject.SetActive(true);

        planet2 = Clone(planet2, !isLeft);
        planet2.gameObject.SetActive(false);

        target = Instantiate(targetPrefab, planet2.transform.position, Quaternion.Euler(49, 0, 0));
        target.name = target.name.Replace("(Clone)", "");
        target.SetActive(true);
    }

    private AstronomicalObject Clone(AstronomicalObject origin, bool isLeftPart = true)
    {
        float spawnY = screenHeight / 2;
        float spawnX = GetSpawnX(isLeftPart);

        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(spawnX, spawnY, Camera.main.transform.position.z));
        spawnPosition.z = -3;

        AstronomicalObject clonedPlanet = SpawnObject(origin, spawnPosition);
        return clonedPlanet;
    }

    private AstronomicalObject SpawnObject(AstronomicalObject origin, Vector3 spawnPosition)
    {
        AstronomicalObject clonedPlanet = Instantiate(origin, spawnPosition, Quaternion.identity);
        clonedPlanet.name = clonedPlanet.name.Replace("(Clone)", "");

        if (clonedPlanet.name == "07_saturn")
        {
            Vector3 newRotation = clonedPlanet.transform.rotation.eulerAngles;
            newRotation.x = -10f;
            clonedPlanet.transform.rotation = Quaternion.Euler(newRotation);
        }

        return clonedPlanet;
    }

    private float GetSpawnX(bool isLeftPart = true)
    {
        float spawnX = 0;
        float padding = screenWidth / 6;

        if (isLeftPart)
        {
            spawnX = Random.Range(0 + padding * 1.2f, screenWidth / 2 - padding * 1.5f);
        }
        else
        {
            spawnX = Random.Range(screenWidth / 2 + padding * 2, screenWidth - padding * 1.2f);
        }

        return spawnX;
    }

    public void FindMeanAndSetRocket()
    {
        var d = Vector3.Distance(planet1.transform.position, planet2.transform.position);
        var d2 = d / (1 + Math.Sqrt(planet1.Mass / planet2.Mass));
        var direction = (planet1.transform.position - planet2.transform.position).normalized;

        var answer = planet2.transform.position + direction * ((float)d2);

        rocket = Instantiate(rocketPrefab, answer, Quaternion.identity);
        rocket.name = rocket.name.Replace("(Clone)", "");
        rocket.RotateRocket(planet1.gameObject);
        rocket.gameObject.SetActive(true);
    }

    IEnumerator SetPositionBeforePlaying(float time)
    {
        animationStart = true;
        var center = GetCenterPoint();
        var planet1Pos = planet1.transform.position;
        var rocketPos = rocket.transform.position;

        planet1.transform.position = center;
        rocket.transform.position = center;

        for (float t = 0f; t <= 1; t += Time.deltaTime / time)
        {
            planet1.transform.position = Vector3.Lerp(center, planet1Pos, t); ;
            rocket.transform.position = Vector3.Lerp(center, rocketPos, t); ;

            yield return null;
        }
        planet1.transform.position = planet1Pos;
        rocket.transform.position = rocketPos;

        animationStart = false;
        playing = true;
    }

    virtual protected Vector3 GetCenterPoint()
    {
        var spawnScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(spawnScreen);

        if (Physics.Raycast(ray, out hit, 100))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public override void Play()
    {
        RandomizePosition();
        FindMeanAndSetRocket();
        StartCoroutine(SetPositionBeforePlaying(0.5f));

        // Bắt đầu tính thời gian
        scoreManager.StartGame();
    }

    public void HandleConfirmButton(string planetName)
    {
        animationResult = true;

        // Tắt tính năng lựa chọn thẻ bài
        cardController.turnOffPointerHandler();

        // Hiển thị câu trả lời (hành tinh) vào màn chơi và gán vào biến planetAnswer
        planetAnswer = DisplaySelectedPlanet(planetName);

        // Tính toán và thực thi thiên thạch
        RocketFlyAnimation(planetAnswer);
    }

    private AstronomicalObject DisplaySelectedPlanet(string planetName)
    {
        // Tìm hành tinh đã chọn trong danh sách
        AstronomicalObject selectedPlanet = allPlanets.Find(planet => planet.name == planetName);
        // Khởi tạo hành tinh này trên màn hình
        AstronomicalObject clonedPlanet = SpawnObject(selectedPlanet, planet2.transform.position);
        // Ẩn target 
        if (target != null)
        {
            target.SetActive(false);
        }
        return clonedPlanet;
    }

    private void RocketFlyAnimation(AstronomicalObject planetAnswer)
    {
        // Nếu kết quả đúng thì xoay rocket
        if (planetAnswer.name == planet2.name)
        {
            rocket.transform.rotation = Quaternion.identity;
            scoreManager.FinalScore(healthManager.health);
            return;
        }

        // Tính lực hấp dẫn giữa hành tinh 1 và thiên thạch
        var attractiveForce1 = planet1.GetAttractiveForce(rocket);

        // Tính lực hấp dẫn giữa hành tinh trả lời và thiên thạch
        var attractiveForceAnswer = planetAnswer.GetAttractiveForce(rocket);

        // Thực hiện so sánh
        if (attractiveForce1 > attractiveForceAnswer)
        {
            // Tìm loại boom phù hợp với hành tinh 1
            FindBoomMatchPlanet(planet1);
            // Bắt đầu bay
            StartCoroutine(rocket.FlyTo(planet1.gameObject));
        }
        else
        {
            // Tìm loại boom phùm hợp với hành tinh trả lời
            FindBoomMatchPlanet(planetAnswer);
            // Bắt đầu bay
            StartCoroutine(rocket.FlyTo(planetAnswer.gameObject));
        }
    }

    private void FindBoomMatchPlanet(AstronomicalObject planet)
    {
        foreach (ParticleSystem boomPS in boomPSPrefab)
        {
            if (boomPS.name.Replace("_hit", "") == planet.name)
            {
                boomInstance = Instantiate(boomPS, planet.transform.position, Quaternion.identity);
                var mainModule = boomInstance.main;
                mainModule.playOnAwake = false;  // Tắt playOnAwake để ParticleSystem không tự động phát
                boomInstance.gameObject.SetActive(false); // Đặt gameObject về không hoạt động để không hiển thị
                return;
            }
        }
        Debug.LogWarning("Planet with name " + planet.name + " not found in boomPSPrefab list.");
        return;
    }

    public IEnumerator PlayBoomAndShake()
    {
        if (boomInstance != null)
        {
            boomInstance.gameObject.SetActive(true);
            boomInstance.Play();
        }
        cameraShake.ShakeCamera();

        // Mất 1 mạng
        healthManager.health--;
        yield return null;
    }

    private void GameOver()
    {
        Debug.Log("gameOver");
    }
}
