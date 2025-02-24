using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlanetSelectionSpawner : IGamePlay
{
    // public
    [Header("List of Planets")]
    public List<AstronomicalObject> allPlanets;

    [Header("Rocket and Target")]
    public RocketController rocketPrefab;
    public GameObject targetPrefab;

    [Header("List Particle System Boom")]
    public List<ParticleSystem> boomPSPrefab;
    public GameObject winEffectPSPrefab;


    // private
    // thành phần game
    private AstronomicalObject planet1 = null;
    private AstronomicalObject planet2 = null;
    private AstronomicalObject planetAnswer = null;
    private RocketController rocket = null;
    private GameObject target = null;
    private ParticleSystem boomInstance = null;
    private ParticleSystem winEffectInstance = null;

    // giá trị màn hình
    private int screenWidth;
    private int screenHeight;
    private Vector2 screenCenter;
    private bool isLeft;

    // biến bool
    private bool isAnimationPlaying = false;

    void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);
    }

    void Update()
    {
        if (!isAnimationPlaying)
        {
            rocket.TurnOnCollider = true;
        }
        else
        {
            rocket.TurnOnCollider = false;
        }
    }

    private void DestroyEffect()
    {
        // Hủy boom instance
        if (boomInstance != null)
        {
            Destroy(boomInstance.gameObject);
            boomInstance = null;
        }

        // Đặt lại giá trị
        cameraShake.IsShake = -1;
        
        if (healthManager.health == 0 || cardController.GetNumOfCards() == 0)
        {
            GameOver();
        }
        else
        {
            ReSetUpGame();
        }
    }

    private void ReSetUpGame()
    {
        // Set up lại các object trong màn chơi
        RePlayGame();
        // Bật tính năng chọn thẻ bài
        cardController.turnOnPointerHandler();
    }

    private void RePlayGame()
    {
        planet1.gameObject.SetActive(true);
        target.SetActive(true);

        Destroy(planetAnswer.gameObject);

        FindMeanAndSetRocket();
        StartCoroutine(SetPositionBeforePlaying(0.6f));
    }

    public void RandomizePosition()
    {
        // var id1 = UnityEngine.Random.Range(0, allPlanets.Count);
        var id1 = planets[0];
        planet1 = allPlanets[id1];

        var id2 = planets[1];
        planet2 = allPlanets[id2];

        isLeft = Random.Range(0, 2) == 0 ? true : false;
        planet1 = Clone(planet1, isLeft);
        planet1.transform.SetParent(planetsGroupTransform);
        planet1.gameObject.SetActive(true);

        planet2 = Clone(planet2, !isLeft);
        planet2.transform.SetParent(planetsGroupTransform);
        planet2.gameObject.SetActive(false);

        target = Instantiate(targetPrefab, planet2.transform.position, Quaternion.Euler(63, 0, 0));
        target.name = target.name.Replace("(Clone)", "");
        target.transform.SetParent(planetsGroupTransform);
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
        rocket.transform.SetParent(planetsGroupTransform);
        rocket.RotateRocket(planet1.gameObject.transform.position);
        rocket.gameObject.SetActive(true);
    }

    IEnumerator SetPositionBeforePlaying(float time)
    {
        isAnimationPlaying = true;
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

        isAnimationPlaying = false;
        
        // Kết thúc quá trình kết quả (sau khi đã chọn sai 1 lần), được phép bấm pause
        isResultPlaying  = false;
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
        StartCoroutine(SetPositionBeforePlaying(0.6f));
    }

    public override void CheckDragPosition(Vector3 dragPos, string planetName)
    {
        // do nothing
    }

    public override void HandleConfirmButton(string planetName, Vector3 planetPosition)
    {
        // Tắt tutorial nếu có
        universalLevelManager.StopTutorial();
        
        // Tắt tính năng lựa chọn thẻ bài
        cardController.turnOffPointerHandler();

        // Hiển thị câu trả lời (hành tinh) vào màn chơi và gán vào biến planetAnswer
        planetAnswer = DisplaySelectedPlanet(planetName, planetPosition);
        planetAnswer.transform.SetParent(effectsGroupTransform);

        // Hàm thực hiện bay hành tinh và bay rocket
        isResultPlaying = true;
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
        isAnimationPlaying = true;
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
            isAnimationPlaying = false;
            target.SetActive(false);
        }
    }

    private void RocketFlyAnimation()
    {
        // Nếu kết quả đúng thì xoay rocket
        if (planetAnswer.name == planet2.name)
        {
            // Lắc rocket và hiển thị hiệu ứng correct
            StartCoroutine(CoroutineCorrectAnwser());
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
            // FindBoomMatchPlanet(planet1);
            // Bắt đầu lắc và bay
            StartCoroutine(rocket.ShakeAndFlyTo(planet1.gameObject.transform.position));
        }
        else
        {
            // Tìm loại boom phùm hợp với hành tinh trả lời
            // FindBoomMatchPlanet(planetAnswer);
            // Bắt đầu lắc và bay
            StartCoroutine(rocket.ShakeAndFlyTo(planet2.gameObject.transform.position));
        }
    }

    public override void ExecuteAfterCollision(AstronomicalObject planet)
    {
        FindBoomMatchPlanet(planet);
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
                boomInstance.transform.SetParent(effectsGroupTransform);
                boomInstance.gameObject.SetActive(false); // Đặt gameObject về không hoạt động để không hiển thị
                break;
            }
        }
        if (boomInstance != null)
        {
            StartCoroutine(PlayBoomAndShake());
        }
    }

    private IEnumerator PlayBoomAndShake()
    {
        boomInstance.gameObject.SetActive(true);
        AudioManager.Instance.PlaySFX("Explosion");
        boomInstance.Play();
        cameraShake.ShakeCamera();

        // Mất 1 mạng
        healthManager.health--;
        yield return new WaitForSeconds(2f);
        DestroyEffect();
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
        winEffectInstance.transform.SetParent(effectsGroupTransform);
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
            AudioManager.Instance.PlaySFX("Correct");
            winEffect.Play();
        }
        yield return new WaitForSeconds(2f);
        if (timerManager != null)
        {
            timerManager.StopTimer();
        }
        DestroyAllPlanetsInGroup();
        universalLevelManager.EndStage();
    }

    // private IEnumerator IncreaseEnergyAndDestroyPlanetsCoroutine()
    // {
    //     energyManager.ChangeEnergy(30);
    //     yield return new WaitForSeconds(2f);
        
    // }


    protected void DestroyAllPlanetsInGroup()
    {
        if (planetsGroupTransform != null)
        {
            foreach (Transform child in planetsGroupTransform)
            {
                Destroy(child.gameObject);
            }
        }
        if (effectsGroupTransform != null)
        {
            foreach (Transform child in effectsGroupTransform)
            {
                Destroy(child.gameObject);
            }
        }
        cardController.ResetCards();
    }

    private void GameOver()
    {
        if (timerManager != null)
        {
            timerManager.StopTimer();
        }
        DestroyAllPlanetsInGroup();
        cardController.DestroyPlanetSelection();
        universalLevelManager.GameOver();
    }

    public override void OnTimeOver()
    {
        this.GameOver();
    }

    public override void OnFullEnergy()
    {

    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
        DestroyAllPlanetsInGroup();
    }

}
