using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PositionSelectionSpawner_3 : PositionSelectionSpawner_2
{
    private GameObject target3 = null;
    private float target3XMin;
    private float target3XMax;
    private bool overlap13;
    private bool overlap23;
    private Vector3 target3Position;
    // --------------------------------------------


    override protected void RePlayGame()
    {
        planet1.gameObject.SetActive(true);
        target1.SetActive(true);
        target2.SetActive(true);
        target3.SetActive(true);

        Destroy(planetAnswer.gameObject);

        FindMeanAndSetRocket();
        StartCoroutine(SetPositionBeforePlaying(0.6f));
    }

    override protected void FakeTargetSpawner()
    {
        int indexOfTarget1 = GetScreenSegmentIndex(Camera.main.WorldToScreenPoint(target1.transform.position).x);
        int indexOfRocket = GetScreenSegmentIndex(Camera.main.WorldToScreenPoint(rocket.transform.position).x);
        int indexOfTarget2 = -1;
        int indexOfTarget3 = -1;

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
            do
            {
                indexOfTarget3 = Random.Range(indexOfRocket + 1, 13);
            } while (Mathf.Abs(indexOfTarget3 - indexOfRocket) < 2
                    || Mathf.Abs(indexOfTarget3 - indexOfTarget1) < 1
                    || Mathf.Abs(indexOfTarget3 - indexOfTarget2) < 1);
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
            do
            {
                indexOfTarget3 = Random.Range(1, indexOfRocket - 1);
            } while (Mathf.Abs(indexOfTarget3 - indexOfRocket) < 2
                    || Mathf.Abs(indexOfTarget3 - indexOfTarget1) < 1
                    || Mathf.Abs(indexOfTarget3 - indexOfTarget2) < 1);

        }
        // Debug.Log("planet: " + GetScreenSegmentIndex(Camera.main.WorldToScreenPoint(planet1.transform.position).x));
        // Debug.Log("rocket: " + indexOfRocket);
        // Debug.Log("target: " + indexOfTarget1);
        // Debug.Log("target2: " + indexOfTarget2);
        // Debug.Log("target3: " + indexOfTarget3);

        if (indexOfTarget2 > 0)
        {
            target2 = CloneFakeTarget(indexOfTarget2);
            target2.transform.SetParent(planetsGroupTransform);
            target2.name = target2.name.Replace("(Clone)", "2");
        }
        if (indexOfTarget3 > 0)
        {
            target3 = CloneFakeTarget(indexOfTarget3);
            target3.transform.SetParent(planetsGroupTransform);
            target3.name = target3.name.Replace("(Clone)", "3");
        }

        CalculateTargetDistance();
    }

    override public void CheckDragPosition(Vector3 dragPos, string planetName)
    {
        float radiusPS = CheckPlanetName(planetName);
        Debug.Log("_ " + planetName + " " + radiusPS);
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
                    ChangePSColorAlpha(target3, 0.05f);
                    IncreasePSShapeRadius(target1, radiusPS, 0f);
                    IncreasePSShapeRadius(target2, 1f, 63f);
                    IncreasePSShapeRadius(target3, 1f, 63f);
                    break;
                case 2:
                    ChangePSColorAlpha(target1, 0.05f);
                    ChangePSColorAlpha(target2, 1f);
                    ChangePSColorAlpha(target3, 0.05f);
                    IncreasePSShapeRadius(target1, 1f, 63f);
                    IncreasePSShapeRadius(target2, radiusPS, 0f);
                    IncreasePSShapeRadius(target3, 1f, 63f);
                    break;
                case 3:
                    ChangePSColorAlpha(target1, 0.05f);
                    ChangePSColorAlpha(target2, 0.05f);
                    ChangePSColorAlpha(target3, 1f);
                    IncreasePSShapeRadius(target1, 1f, 63f);
                    IncreasePSShapeRadius(target2, 1f, 63f);
                    IncreasePSShapeRadius(target3, radiusPS, 0f);
                    break;
            }
        }
        else
        {
            targetCloser = 0;
            ResetAllTarget();
        }
    }

    override protected void ResetAllTarget()
    {
        ChangePSColorAlpha(target1, 1f);
        ChangePSColorAlpha(target2, 1f);
        ChangePSColorAlpha(target3, 1f);
        IncreasePSShapeRadius(target1, 1f, 63f);
        IncreasePSShapeRadius(target2, 1f, 63f);
        IncreasePSShapeRadius(target3, 1f, 63f);
    }

    override protected void CalculateTargetDistance()
    {
        target1Position = target1.transform.position;
        target1XMin = target1Position.x - xMargin;
        target1XMax = target1Position.x + xMargin;

        target2Position = target2.transform.position;
        target2XMin = target2Position.x - xMargin;
        target2XMax = target2Position.x + xMargin;

        target3Position = target3.transform.position;
        target3XMin = target3Position.x - xMargin;
        target3XMax = target3Position.x + xMargin;

        bool overlap12 = IsOverlapping(target1XMin, target1XMax, target2XMin, target2XMax);
        bool overlap13 = IsOverlapping(target1XMin, target1XMax, target3XMin, target3XMax);
        bool overlap23 = IsOverlapping(target2XMin, target2XMax, target3XMin, target3XMax);
    }

    override protected int CheckIfDragIsInTargetArea(Vector3 dragPos)
    {
        if (overlap12 || overlap13 || overlap23)
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

            if (overlap13)
            {
                float distanceToTarget1 = Mathf.Abs(dragPos.x - target1Position.x);
                float distanceToTarget3 = Mathf.Abs(dragPos.x - target3Position.x);

                if (distanceToTarget1 < distanceToTarget3)
                {
                    return 1;
                }
                else
                {
                    return 3;
                }
            }

            if (overlap23)
            {
                float distanceToTarget2 = Mathf.Abs(dragPos.x - target2Position.x);
                float distanceToTarget3 = Mathf.Abs(dragPos.x - target3Position.x);

                if (distanceToTarget2 < distanceToTarget3)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
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
        else if (dragPos.x >= target3XMin && dragPos.x <= target3XMax)
        {
            return 3;
        }
        return 0;
    }

    override protected IEnumerator FlySelectedPlanetToTarget()
    {
        Vector3 startingPos = planetAnswer.transform.position;
        Vector3 finalPos;
        if (targetCloser == 1)
        {
            finalPos = target1.transform.position;
        }
        else if (targetCloser == 2)
        {
            finalPos = target2.transform.position;
        }
        else
        {
            finalPos = target3.transform.position;
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
        if (target3 != null)
        {
            target3.SetActive(false);
        }
    }
}
