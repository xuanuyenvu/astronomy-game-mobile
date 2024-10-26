using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;


public class PositionSelectionSpawner_2 : IGamePlay
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


    // protected
    // thành phần game
    protected AstronomicalObject planet1 = null;
    protected AstronomicalObject planet2 = null;
    protected AstronomicalObject planetAnswer = null;
    protected RocketController rocket = null;
    protected GameObject target1 = null;
    protected GameObject target2 = null;
    protected ParticleSystem boomInstance = null;
    protected ParticleSystem winEffectInstance = null;

    // giá trị màn hình
    protected int screenWidth;
    protected int screenHeight;
    protected Vector2 screenCenter;
    protected bool isLeft;

    // biến bool
    protected bool animationStart = false;
    protected bool playing;

    // lưu các phần trong màn hình
    protected List<float> partOfScreen;
    // --------------------------------------------
    // lưu giá trị bound cho từng target
    protected float xMargin = 0.6f;
    protected float target1XMin;
    protected float target1XMax;
    protected float target2XMin;
    protected float target2XMax;
    protected bool overlap12;
    protected Vector3 target1Position;
    protected Vector3 target2Position;
    protected int targetCloser = 0;
    // --------------------------------------------

    void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);

        // Chia màn hình thành 12 phần bằng nhau theo trục x
        partOfScreen = new List<float>();
        float partWidth = screenWidth / 14;
        for (int i = 0; i < 14; i++)
        {
            partOfScreen.Add(partWidth * i);
        }
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
    }

    protected void DestroyEffect()
    {
        // Hủy boom instance
        if (boomInstance != null)
        {
            Destroy(boomInstance.gameObject);
            boomInstance = null;
        }

        // Đặt lại giá trị
        cameraShake.IsShake = -1;

        // Thiết lập lại game
        ReSetUpGame();
    }

    protected void ReSetUpGame()
    {
        if (healthManager.health > 0)
        {
            // Set up lại các object trong màn chơi
            RePlayGame();
            // Bật tính năng chọn thẻ bài
            cardController.turnOnPointerHandler();
            cardController.ShowACard();
        }
        else
        {
            GameOver();
        }
    }

    protected virtual void RePlayGame()
    {
        planet1.gameObject.SetActive(true);
        target1.SetActive(true);
        target2.SetActive(true);

        Destroy(planetAnswer.gameObject);

        FindMeanAndSetRocket();
        StartCoroutine(SetPositionBeforePlaying(0.5f));
    }

    public void RandomizePosition()
    {
        // var id1 = UnityEngine.Random.Range(0, allPlanets.Count);
        var id1 = planets[0];
        planet1 = allPlanets[id1];

        var id2 = planets[1];
        // if (id1 < 4)
        // {
        //     id2 = UnityEngine.Random.Range(0, 4);
        // }
        // else
        // {
        //     id2 = UnityEngine.Random.Range(4, allPlanets.Count);
        // }
        planet2 = allPlanets[id2];

        isLeft = Random.Range(0, 2) == 0 ? true : false;
        planet1 = Clone(planet1, isLeft);
        planet1.transform.SetParent(planetsGroupTransform);
        planet1.gameObject.SetActive(true);

        planet2 = Clone(planet2, !isLeft);
        planet2.transform.SetParent(planetsGroupTransform);
        planet2.gameObject.SetActive(false);

        target1 = Instantiate(targetPrefab, planet2.transform.position, Quaternion.Euler(63, 0, 0));
        target1.transform.SetParent(planetsGroupTransform);
        // SetLocalScaleOfTarget(target1);
        target1.name = target1.name.Replace("(Clone)", "1");
        target1.SetActive(true);
    }

    public int GetScreenSegmentIndex(float objectScreenX)
    {
        for (int i = 0; i < partOfScreen.Count - 1; i++)
        {
            if (objectScreenX >= partOfScreen[i] && objectScreenX < partOfScreen[i + 1])
            {
                return i;
            }
        }
        if (objectScreenX >= partOfScreen[partOfScreen.Count - 1])
        {
            return partOfScreen.Count - 1;
        }
        return -1;
    }

    protected virtual void FakeTargetSpawner()
    {
        int indexOfTarget1 = GetScreenSegmentIndex(Camera.main.WorldToScreenPoint(target1.transform.position).x);
        int indexOfRocket = GetScreenSegmentIndex(Camera.main.WorldToScreenPoint(rocket.transform.position).x);
        int indexOfTarget2 = -1;

        // Màn hình được chia làm 12 phần theo trục x
        // Quy tắc là chọn vị trí cho target 2 sao cho 
        // khoảng cách từ target2 đến rocket > 2/12 phần màn hình (do đuôi rocket dài) 
        // và target2 đến target1 > 1/12 phần màn hình
        if (isLeft) // target1 nằm bên phải màn hình
        {
            do
            {
                indexOfTarget2 = Random.Range(indexOfRocket + 1, 13);
            } while (Mathf.Abs(indexOfTarget2 - indexOfRocket) < 2
                    || Mathf.Abs(indexOfTarget2 - indexOfTarget1) < 1);
        }
        else // target1 nằm bên trái màn hình
        {
            // chạy từ indexOfRocket - 2 đến phần con thứ 1 trên màn hình 
            // (không xét phần con số 0 vì nó là padding)
            do
            {
                indexOfTarget2 = Random.Range(1, indexOfRocket - 1);
            } while (Mathf.Abs(indexOfTarget2 - indexOfRocket) < 2
                    || Mathf.Abs(indexOfTarget2 - indexOfTarget1) < 1);
        }
        // Debug.Log("planet: " + GetScreenSegmentIndex(Camera.main.WorldToScreenPoint(planet1.transform.position).x));
        // Debug.Log("rocket: " + indexOfRocket);
        Debug.Log("target: " + indexOfTarget1);
        Debug.Log("target2: " + indexOfTarget2);

        if (indexOfTarget2 > 0)
        {
            target2 = CloneFakeTarget(indexOfTarget2);
            target2.transform.SetParent(planetsGroupTransform);
            target2.name = target2.name.Replace("(Clone)", "2");
        }

        CalculateTargetDistance();
    }

    protected GameObject CloneFakeTarget(int index)
    {
        float spawnX = (partOfScreen[index] + partOfScreen[index + 1]) / 2;

        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(spawnX, screenHeight / 2, Camera.main.transform.position.z));
        position.y = target1.transform.position.y;
        position.z = target1.transform.position.z;
        GameObject clonedTarget = Instantiate(targetPrefab, position, Quaternion.Euler(63, 0, 0));

        // SetLocalScaleOfTarget(clonedTarget);
        clonedTarget.SetActive(true);
        return clonedTarget;
    }

    protected AstronomicalObject Clone(AstronomicalObject origin, bool isLeftPart = true)
    {
        float spawnY = screenHeight / 2;
        float spawnX = GetSpawnX(isLeftPart);

        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(spawnX, spawnY, Camera.main.transform.position.z));
        spawnPosition.z = -3;

        AstronomicalObject clonedPlanet = SpawnObject(origin, spawnPosition);
        return clonedPlanet;
    }

    protected AstronomicalObject SpawnObject(AstronomicalObject origin, Vector3 spawnPosition)
    {
        AstronomicalObject clonedPlanet = Instantiate(origin, spawnPosition, Quaternion.identity);
        // SetLocalScaleOfAstronimicalObject(clonedPlanet.gameObject);
        clonedPlanet.name = clonedPlanet.name.Replace("(Clone)", "");

        if (clonedPlanet.name == "07_saturn")
        {
            Vector3 newRotation = clonedPlanet.transform.rotation.eulerAngles;
            newRotation.x = -10f;
            clonedPlanet.transform.rotation = Quaternion.Euler(newRotation);
        }

        return clonedPlanet;
    }

    protected float GetSpawnX(bool isLeftPart = true)
    {
        float spawnX = 0;
        float padding = screenWidth / 6;

        if (isLeftPart)
        {
            spawnX = Random.Range(0 + padding, screenWidth / 2 - padding * 1.5f);
        }
        else
        {
            spawnX = Random.Range(screenWidth / 2 + padding * 1.5f, screenWidth - padding);
        }

        return spawnX;
    }

    public void FindMeanAndSetRocket()
    {
        Vector3 answer;
        do
        {
            var d = Vector3.Distance(planet1.transform.position, planet2.transform.position);
            var d2 = d / (1 + Math.Sqrt(planet1.Mass / planet2.Mass));
            var direction = (planet1.transform.position - planet2.transform.position).normalized;
            answer = planet2.transform.position + direction * ((float)d2);

            int idAnswer = GetScreenSegmentIndex(Camera.main.WorldToScreenPoint(answer).x);

            // Nếu vị trí rocket nằm trong đoạn 4 < rocket.position < 9
            // thì thoát vòng while
            if ((isLeft && idAnswer < 9)
                || (!isLeft && idAnswer > 4))
            {
                break;
            }

            // Ngược lại rocket vi phạm bound đã đặt
            // thì phải destroy và tạo lại từ đầu
            Destroy(planet1.gameObject);
            Destroy(planet2.gameObject);
            Destroy(target1.gameObject);
            RandomizePosition();

        } while (true);


        rocket = Instantiate(rocketPrefab, answer, Quaternion.identity);
        // SetLocalScaleOfAstronimicalObject(rocket.gameObject);
        rocket.name = rocket.name.Replace("(Clone)", "");
        rocket.transform.SetParent(planetsGroupTransform);
        rocket.RotateRocket(planet1.gameObject.transform.position);
        rocket.gameObject.SetActive(true);
    }

    protected IEnumerator SetPositionBeforePlaying(float time)
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

        cardController.DisplayACard(planet2.name);

        FakeTargetSpawner();
        StartCoroutine(SetPositionBeforePlaying(0.5f));
    }


    protected void SetLocalScaleOfTarget(GameObject targetObject)
    {
        targetObject.transform.localScale *= 0.8f;
        Transform particleSystemTransform = targetObject.transform.Find("Particle System");
        if (particleSystemTransform != null)
        {
            particleSystemTransform.localScale *= .8f;
        }
    }

    protected void SetLocalScaleOfAstronimicalObject(GameObject _object)
    {
        _object.transform.localScale *= .85f;
    }

    public override void CheckDragPosition(Vector3 dragPos, string planetName)
    {
        float radiusPS = CheckPlanetName(planetName);
        float validDragRange = screenHeight / 8;

        Vector3 screenDragPos = Camera.main.WorldToScreenPoint(dragPos);

        float screenHeightMiddle = screenHeight / 2;
        float lowerBound = screenHeightMiddle - validDragRange;
        float upperBound = screenHeightMiddle + validDragRange;

        if (screenDragPos.y >= lowerBound && screenDragPos.y <= upperBound)
        {
            targetCloser = CheckIfDragIsInTargetArea(dragPos);
            switch (targetCloser)
            {
                case 0:
                    ResetAllTarget();
                    break;
                case 1:
                    ChangePSColorAlpha(target1, 1f);
                    ChangePSColorAlpha(target2, 0.05f);
                    IncreasePSShapeRadius(target1, radiusPS, 0f);
                    IncreasePSShapeRadius(target2, 1f, 63f);
                    break;
                case 2:
                    ChangePSColorAlpha(target1, 0.05f);
                    ChangePSColorAlpha(target2, 1f);
                    IncreasePSShapeRadius(target1, 1f, 63f);
                    IncreasePSShapeRadius(target2, radiusPS, 0f);
                    break;
                case 3:
                    ChangePSColorAlpha(target1, 0.05f);
                    ChangePSColorAlpha(target2, 0.05f);
                    IncreasePSShapeRadius(target1, 1f, 63f);
                    IncreasePSShapeRadius(target2, 1f, 63f);
                    break;
            }
        }
        else
        {
            targetCloser = 0;
            ResetAllTarget();
        }
    }

    protected virtual void ResetAllTarget()
    {
        if (target1 == null)
        {
            target1 = GameObject.Find("target1");
            target1.name = "target11";
        }
        if (target2 == null)
        {
            target2 = GameObject.Find("target2");
            target2.name = "target21";

        }
        ChangePSColorAlpha(target1, 1f);
        ChangePSColorAlpha(target2, 1f);
        IncreasePSShapeRadius(target1, 1f, 63f);
        IncreasePSShapeRadius(target2, 1f, 63f);
    }

    protected void ChangePSColorAlpha(GameObject psObject, float _alpha)
    {
        ParticleSystem particleSystem = psObject.transform.Find("Particle System").GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            var mainModule = particleSystem.main;
            Color currentColor = mainModule.startColor.color;
            currentColor.a = _alpha;
            mainModule.startColor = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a);
        }
    }

    public void IncreasePSShapeRadius(GameObject psObject, float increaseAmount, float _rotation)
    {
        Quaternion rotation = psObject.transform.rotation;
        rotation.eulerAngles = new Vector3(_rotation, rotation.eulerAngles.y, rotation.eulerAngles.z);
        psObject.transform.rotation = rotation;

        ParticleSystem particleSystem = psObject.transform.Find("Particle System").GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            var shapeModule = particleSystem.shape;
            shapeModule.radius = increaseAmount;
        }
    }

    protected bool IsOverlapping(float min1, float max1, float min2, float max2)
    {
        return (min1 < max2 && max1 > min2);
    }

    protected virtual void CalculateTargetDistance()
    {
        target1Position = target1.transform.position;
        target1XMin = target1Position.x - xMargin;
        target1XMax = target1Position.x + xMargin;

        target2Position = target2.transform.position;
        target2XMin = target2Position.x - xMargin;
        target2XMax = target2Position.x + xMargin;

        bool overlap12 = IsOverlapping(target1XMin, target1XMax, target2XMin, target2XMax);
    }

    protected virtual int CheckIfDragIsInTargetArea(Vector3 dragPos)
    {
        if (overlap12)
        {
            float distanceToTarget1 = Mathf.Abs(dragPos.x - target1Position.x);
            float distanceToTarget2 = Mathf.Abs(dragPos.x - target2Position.x);

            if (distanceToTarget1 < distanceToTarget2)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        // Kiểm tra dragPos nằm trong vùng target nào
        else if (dragPos.x >= target1XMin && dragPos.x <= target1XMax)
        {
            return 1;
        }
        else if (dragPos.x >= target2XMin && dragPos.x <= target2XMax)
        {
            return 2;
        }
        return 0;
    }

    public override void HandleConfirmButton(string planetName, Vector3 planetPosition)
    {
        Debug.Log("HandleConfirmButton");
        if (targetCloser == 0)
        {
            cardController.ShowACard();
            return;
        }

        // Tắt tính năng lựa chọn thẻ bài
        cardController.turnOffPointerHandler();

        Debug.Log("Planet name: " + planetName);
        // Hiển thị câu trả lời (hành tinh) vào màn chơi và gán vào biến planetAnswer
        planetAnswer = DisplaySelectedPlanet(planetName, planetPosition);
        Debug.Log("Planet answer-----: " + planetAnswer.name);
        // Hàm thực hiện bay hành tinh và bay rocket
        StartCoroutine(CoroutineExcutesequentially());
    }

    protected float CheckPlanetName(string planetName)
    {
        if (planetName == "01_mercury" || planetName == "04_mars")
        {
            return 2.06f;
        }
        else if (planetName == "03_earth" || planetName == "02_venus")
        {
            return 2.7f;
        }
        else if (planetName == "06_jupiter")
        {
            return 4.1f;
        }
        else
        {
            return 3.5f;
        }
    }

    protected AstronomicalObject DisplaySelectedPlanet(string planetName, Vector3 planetPosition)
    {
        // Tìm hành tinh đã chọn trong danh sách
        AstronomicalObject selectedPlanet = allPlanets.Find(planet => planet.name == planetName);
        // Khởi tạo hành tinh này trên màn hình
        AstronomicalObject clonedPlanet = SpawnObject(selectedPlanet, planetPosition);
        clonedPlanet.transform.SetParent(effectsGroupTransform);

        return clonedPlanet;
    }

    protected IEnumerator CoroutineExcutesequentially()
    {
        // Cho hành tinh bay đến vị trí
        yield return StartCoroutine(FlySelectedPlanetToTarget());

        // Tính toán và thực thi thiên thạch
        RocketFlyAnimation();
    }

    protected virtual IEnumerator FlySelectedPlanetToTarget()
    {
        Vector3 startingPos = planetAnswer.transform.position;
        Debug.Log("Starting position: " + startingPos + " - " + planetAnswer.name);
        Vector3 finalPos;

        if (targetCloser == 1)
        {
            finalPos = target1.transform.position;
        }
        else
        {
            finalPos = target2.transform.position;
        }


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
        ResetAllTarget();
        if (target1 != null)
        {
            target1.SetActive(false);
        }
        if (target2 != null)
        {
            target2.SetActive(false);
        }
    }

    protected void RocketFlyAnimation()
    {
        // Nếu kết quả đúng thì xoay rocket
        if (targetCloser == 1)
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
            // Bắt đầu lắc và bay
            StartCoroutine(rocket.ShakeAndFlyTo(planet1.gameObject.transform.position));
        }
        else
        {
            // Bắt đầu lắc và bay
            StartCoroutine(rocket.ShakeAndFlyTo(planet2.gameObject.transform.position));
        }
        targetCloser = 0;
    }

    public override void ExecuteAfterCollision(AstronomicalObject planet)
    {
        FindBoomMatchPlanet(planet);
    }

    protected void FindBoomMatchPlanet(AstronomicalObject planet)
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

    protected IEnumerator PlayBoomAndShake()
    {
        boomInstance.gameObject.SetActive(true);
        boomInstance.Play();
        cameraShake.ShakeCamera();

        // Mất 1 mạng
        healthManager.health--;
        yield return new WaitForSeconds(2f);
        DestroyEffect();
    }

    protected IEnumerator CoroutineCorrectAnwser()
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

    protected IEnumerator PlayWinEffect(ParticleSystem winEffect)
    {
        if (winEffect != null)
        {
            winEffect.gameObject.SetActive(true);
            winEffect.Play();
        }
        yield return new WaitForSeconds(2f);
        timerManager.StopTimer();
        StartCoroutine(IncreaseEnergyAndDestroyPlanetsCoroutine());
    }

    private IEnumerator IncreaseEnergyAndDestroyPlanetsCoroutine()
    {
        energyManager.ChangeEnergy(30);
        yield return new WaitForSeconds(2f);
        DestroyAllPlanetsInGroup();
        universalLevelManager.EndStage();
    }

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
        DestroyAllPlanetsInGroup();
        universalLevelManager.GameOver();
    }
}
