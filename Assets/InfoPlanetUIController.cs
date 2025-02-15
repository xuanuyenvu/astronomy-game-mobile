using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlanetInfo
{
    public int planetID;
    public string planetName;
    public string description;
}

[System.Serializable]
public class PlanetInfoList
{
    public PlanetInfo[] planets;
}

public class InfoPlanetUIController : MonoBehaviour
{
    public GameObject[] planets;
    public TextMeshProUGUI planetName;
    public TextMeshProUGUI planetDescription;
    
    private PlanetInfoList planetData;
    private float screenHeight;

    void Start()
    {
        screenHeight = Screen.height;
        LoadJsonData();
        DisplayPlanetInfo(0);
    }
    
    public void DisplayPlanetInfo(int planetID = 0)
    {
        SelectedPlanet(planetID);
        AddText(planetID);
    }

    private void LoadJsonData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("planet_info");
        if (jsonFile != null)
        {
            planetData = JsonUtility.FromJson<PlanetInfoList>(jsonFile.text);
        }
        else
        {
            Debug.LogError("JSON file not found in Resources!");
        }
    }

    private void SelectedPlanet(int planetID)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            if (planetID == i)
            {
                planets[i].transform.position = new Vector3(-4.5f, 1.4f, 0);
            }
            else
            {
                planets[i].transform.position = new Vector3(-4.5f, screenHeight + 10f, 0);
            }
        }
    }

    private void AddText(int planetID)
    {
        planetName.text = planetData.planets[planetID].planetName;
        planetDescription.text = planetData.planets[planetID].description;
    }
}
