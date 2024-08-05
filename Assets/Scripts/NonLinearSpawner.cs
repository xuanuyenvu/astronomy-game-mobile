using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NonLinearSpawner : IGamePlay
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
    public GameObject winEffectPSPrefab;


    private int screenWidth;
    private int screenHeight;
    private Vector2 screenCenter;
    private bool isLeft;
    private bool isTop;

    private bool animationStart = false;
    private bool animationResult = false;
    private bool playing;

    private ParticleSystem boomInstance = null;
    private ParticleSystem winEffectInstance = null;

    RocketController monster = null;

    void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);
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

        DestroyEffect();
    }

    private void DestroyEffect()
    {
        // Hủy hiệu ứng sau khi hành tinh phát nổ
        if (cameraShake.IsShake == 0 && animationResult)
        {
            // Hủy boom instance
            Destroy(boomInstance.gameObject);
            boomInstance = null;

            // Đặt lại giá trị
            animationResult = false;
            cameraShake.IsShake = -1;

            // Thiết lập lại game
            ReSetUpGame();
        }
        if (winEffectInstance != null)
        {
            if (!winEffectInstance.isPlaying && animationResult)
            {
                // Hủy win effect
                Destroy(winEffectInstance.gameObject);
                winEffectInstance = null;

                // Đặt lại giá trị
                animationResult = false;

                // Thắng hoặc qua màn sau
            }
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

        SetRocket();
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
        isTop = Random.Range(0, 2) == 0 ? true : false;
        planet1 = Clone(planet1, isLeft, isTop);
        planet1.gameObject.SetActive(true);

        planet2 = Clone(planet2, !isLeft, !isTop);
        planet2.gameObject.SetActive(false);

        target = Instantiate(targetPrefab, planet2.transform.position, Quaternion.Euler(63, 0, 0));
        target.name = target.name.Replace("(Clone)", "");
        target.SetActive(true);
    }

    private AstronomicalObject Clone(AstronomicalObject origin, bool isLeftPart = true, bool isTopPart = true)
    {
        // float spawnY = screenHeight / 2;
        float spawnY = GetSpawnY(isTopPart);
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

    private float GetSpawnY(bool isTopPart = true)
    {
        float spawnY = 0;
        float padding = screenHeight / 8;

        if (isTopPart)
        {
            spawnY = Random.Range(0 + padding * 2f, screenHeight / 2 - padding * 1.5f);
        }
        else
        {
            spawnY = Random.Range(screenHeight / 2 + padding * 1.2f, screenHeight - padding * 2f);
        }

        return spawnY;
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

    private void SetDestinationPosition()
    {
        float spawnY;
        float spawnX;
        if (Random.Range(0, 2) == 0 ? true : false)
        {
            spawnY = GetSpawnY(!isTop);
            spawnX = GetSpawnX(isLeft);
        }
        else
        {
            spawnY = GetSpawnY(isTop);
            spawnX = GetSpawnX(!isLeft);
        }

        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(spawnX, spawnY, Camera.main.transform.position.z));
        spawnPosition.z = -3;

        monster = Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);
        monster.gameObject.SetActive(false);
    }

    public void SetRocket()
    {
        Vector3 rocketPostion = CalculateResultantPosition(planet1.gameObject.transform.position, 
                                                    planet2.gameObject.transform.position,
                                                    monster.gameObject.transform.position);

        rocket = Instantiate(rocketPrefab, rocketPostion, Quaternion.identity);
        rocket.name = rocket.name.Replace("(Clone)", "");
        rocket.RotateRocket(planet1.gameObject.transform.position);
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
        SetDestinationPosition();
        SetRocket();
        StartCoroutine(SetPositionBeforePlaying(0.5f));

        // Bắt đầu tính thời gian
        scoreManager.StartGame();
    }

    public override void CheckDragPosition(Vector3 dragPos, string planetName)
    {
        // do nothing
    }

    public override void HandleConfirmButton(string planetName, Vector3 planetPosition)
    {
        animationResult = true;

        // Tắt tính năng lựa chọn thẻ bài
        cardController.turnOffPointerHandler();

        // Hiển thị câu trả lời (hành tinh) vào màn chơi và gán vào biến planetAnswer
        planetAnswer = DisplaySelectedPlanet(planetName, planetPosition);

        // Hàm thực hiện bay hành tinh và bay rocket
        StartCoroutine(CoroutineExcutesequentially());
    }

    private AstronomicalObject DisplaySelectedPlanet(string planetName, Vector3 planetPosition)
    {
        // Tìm hành tinh đã chọn trong danh sách
        AstronomicalObject selectedPlanet = allPlanets.Find(planet => planet.name == planetName);
        // Khởi tạo hành tinh này trên màn hình
        AstronomicalObject clonedPlanet = SpawnObject(selectedPlanet, planetPosition);

        return clonedPlanet;
    }

    private IEnumerator CoroutineExcutesequentially()
    {
        // Cho hành tinh bay đến vị trí
        yield return StartCoroutine(FlySelectedPlanetToTarget());

        // Tính toán và thực thi thiên thạch
        RocketFlyAnimation();
    }

    private IEnumerator FlySelectedPlanetToTarget()
    {
        Vector3 startingPos = planetAnswer.transform.position;
        Vector3 finalPos = target.transform.position;

        // Tính chiều dài quãng đường
        float distance = Vector3.Distance(startingPos, finalPos);
        // Đường bay càng xa thì thời gian càng chậm
        // Ví dụ: bay 1km --> 2s, thì 2km phải bay lâu hơn
        float time = distance / 9f;

        for (float t = 0f; t <= 1f; t += Time.deltaTime / time)
        {
            planetAnswer.transform.position = Vector3.Lerp(startingPos, finalPos, t);
            yield return null;
        }

        yield return null;

        // Ẩn target 
        if (target != null)
        {
            target.SetActive(false);
        }
    }

    private Vector3 CalculateResultantPosition(Vector3 positionP1, Vector3 positionP2, Vector3 positionR)
    {
        // Tính vector lực hấp dẫn giữa hành tinh 1 và thiên thạch
        Vector3 directionToP1 = positionP1 - positionR;

        // Tính vector lực hấp dẫn giữa hành tinh 2 và thiên thạch
        Vector3 directionToP2 = positionP2 - positionR;

        // Tính vector hợp lực
        Vector3 resultant = directionToP1 + directionToP2;
        Vector3 resultantPosition = resultant + positionR;
        return resultantPosition;
    }

    private void RocketFlyAnimation()
    {
        // Nếu kết quả đúng thì xoay rocket
        // if (planetAnswer.name == planet2.name)
        // {
        //     // Lắc rocket và hiển thị hiệu ứng correct
        //     StartCoroutine(CoroutineCorrectAnwser());
        //     scoreManager.FinalScore(healthManager.health);
        //     return;
        // }

        Vector3 resultant = CalculateResultantPosition(planet1.gameObject.transform.position, 
                                                planetAnswer.gameObject.transform.position,
                                                rocket.transform.position);
        // resultant.Normalize();
        StartCoroutine(rocket.ShakeAndFlyTo(resultant));
    }

    public override IEnumerator PlayBoomAndShake()
    {
        // if (boomInstance != null)
        // {
        //     boomInstance.gameObject.SetActive(true);
        //     boomInstance.Play();
        // }
        // cameraShake.ShakeCamera();

        // // Mất 1 mạng
        // healthManager.health--;
        yield return null;
    }

    private IEnumerator CoroutineCorrectAnwser()
    {
        yield return StartCoroutine(rocket.ShakeRocket());

        // Rocket cân bằng giữa 2 hành tinh
        rocket.transform.rotation = Quaternion.identity;

        // Khởi tạo hiệu ứng tại vị trí planetAnswer
        winEffectInstance = Instantiate(winEffectPSPrefab, planetAnswer.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<ParticleSystem>();
        var mainModule = winEffectInstance.main;
        mainModule.playOnAwake = false;  // Tắt playOnAwake để ParticleSystem không tự động phát
        winEffectInstance.gameObject.SetActive(false);

        // Chỉnh lại giá trị scale
        int idPlanetInList = planetAnswer.name[1] - '0';

        if (idPlanetInList <= 4)
        {
            winEffectInstance.gameObject.transform.localScale = new Vector3(0.66f, 0.66f, 0.66f);
        }
        else
        {
            winEffectInstance.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        StartCoroutine(PlayWinEffect(winEffectInstance));
    }

    private IEnumerator PlayWinEffect(ParticleSystem winEffect)
    {
        if (winEffect != null)
        {
            winEffect.gameObject.SetActive(true);
            winEffect.Play();
        }
        yield return null;
    }

    private void GameOver()
    {
        Debug.Log("gameOver");
    }
}