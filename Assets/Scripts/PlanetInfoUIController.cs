using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

public class PlanetInfoUIController : MonoBehaviour
{
    public GameObject[] planets;
    public TextMeshProUGUI planetName;
    public TextMeshProUGUI planetDescription;
    public ParticleSystem fogPS;
    public ParticleSystem starPS;
    
    private PlanetInfoList planetData;
    private float screenHeight;

    private Dictionary<int, Color> fogColors = new Dictionary<int, Color>()
    {
        { 2, new Color(0x9D / 255f, 0x6C / 255f, 0x37 / 255f, 16f / 255f) }, // Venus
        { 1, new Color(0x82 / 255f, 0x82 / 255f, 0x82 / 255f, 16f / 255f) }, // Mercury
        { 0, new Color(0x42 / 255f, 0x5E / 255f, 0xA6 / 255f, 16f / 255f) }, // Earth
        { 3, new Color(0x80 / 255f, 0x4B / 255f, 0x40 / 255f, 16f / 255f) }, // Mars
        { 4, new Color(0x46 / 255f, 0x25 / 255f, 0x8F / 255f, 16f / 255f) }, // Neptune
        { 5, new Color(0x2F / 255f, 0x6B / 255f, 0x6A / 255f, 16f / 255f) }, // Uranus
        { 6, new Color(0x7A / 255f, 0x6A / 255f, 0x35 / 255f, 16f / 255f) }, // Saturn
        { 7, new Color(0x7A / 255f, 0x62 / 255f, 0x35 / 255f, 16f / 255f) }  // Jupiter
    };
    
    private Dictionary<int, (Color colorMin, Color colorMax)> starColors = new Dictionary<int, (Color, Color)>()
    {
        { 2, (new Color(1f, 1f, 0.843f), new Color(0.925f, 0.502f, 0.302f)) }, // Venus (FFFFD7, EC804D)
        { 1, (new Color(1f, 1f, 1f), new Color(0.565f, 0.608f, 0.694f)) }, // Mercury (FFFFFF, 909BB1)
        { 0, (new Color(0.643f, 0.851f, 1f), new Color(0.267f, 0.506f, 0.988f)) }, // Earth (A4D9FF, 4481FC)
        { 3, (new Color(1f, 0.851f, 0.737f), new Color(0.98f, 0.455f, 0.290f)) }, // Mars (FFD9BC, FA744A)
        { 4, (new Color(0.776f, 0.831f, 1f), new Color(0.275f, 0.357f, 0.784f)) }, // Neptune (C6D4FF, 465BC8)
        { 5, (new Color(0.851f, 0.988f, 1f), new Color(0.396f, 0.675f, 0.82f)) }, // Uranus (D9FCFF, 65ACD1)
        { 6, (new Color(1f, 1f, 0.863f), new Color(0.761f, 0.631f, 0.388f)) }, // Saturn (FFFFDC, C2A163)
        { 7, (new Color(0.996f, 0.882f, 0.698f), new Color(0.827f, 0.553f, 0.306f)) }  // Jupiter (FEE1B2, D38D4E)
    };

    
    void Start()
    {
        screenHeight = Screen.height; // màn hình ở đây là ra khỏi world sceene thôi
        LoadJsonData();
        DisplayPlanetInfo(0);
    }
    
    public void DisplayPlanetInfo(int planetID = 0)
    {
        SelectedPlanet(planetID);
        UpdateText(planetID);
        UpdateFogColor(planetID);
        UpdateStarColor(planetID);
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
                planets[i].transform.position = new Vector3(-4.2f, 1.4f, -6);
            }
            else
            {
                planets[i].transform.position = new Vector3(-4.2f, screenHeight + 10f, -6);
            }
        }
    }

    private void UpdateText(int planetID)
    {
        planetName.text = planetData.planets[planetID].planetName;
        planetDescription.text = planetData.planets[planetID].description;
    }
    
    private void UpdateFogColor(int planetID)
    {
        if (fogPS != null && fogColors.ContainsKey(planetID))
        {
            fogPS.Stop();
            
            var mainModule = fogPS.main;
            Color colorMax = fogColors[planetID]; 
            Color colorMin = new Color(0f, 0f, 0f, 22f / 255f);

            mainModule.startColor = new ParticleSystem.MinMaxGradient(colorMin, colorMax);
            fogPS.Play();
        }
    }

    private void UpdateStarColor(int planetID)
    {
        if (starPS != null && starColors.ContainsKey(planetID))
        {
            starPS.Stop();
        
            var mainModule = starPS.main;
            
            mainModule.startColor = new ParticleSystem.MinMaxGradient(starColors[planetID].colorMin, starColors[planetID].colorMax);

            starPS.Play();
        }
    }

    
    public void GoToChapterMenu()
    {
        AudioManager.Instance.PlaySFX("Click");
        SceneManager.LoadScene("chapterMenu");
        // TransitionManager.Instance().Transition("planetInfo", transition, 0f);
    }
}
