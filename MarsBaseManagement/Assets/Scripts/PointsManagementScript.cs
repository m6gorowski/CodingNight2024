using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsManagementScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI waterScoreText;
    [SerializeField]
    private TextMeshProUGUI oxygenScoreText;
    [SerializeField]
    private TextMeshProUGUI oilScoreText;
    [SerializeField]
    private TextMeshProUGUI energyScoreText;
    public class ResourceData
    {
        public string resourceName; // Name of the resource (e.g., "Water")
        public float points; // Current points
        public float multiplier; // Multiplier for gaining points
        public int upgradeLevel; // Level of the upgrade

        // Constructor
        public ResourceData(string name, float initialPoints, float initialMultiplier, int initialUpgradeLevel)
        {
            resourceName = name;
            points = initialPoints;
            multiplier = initialMultiplier;
            upgradeLevel = initialUpgradeLevel;
        }
    }
    public ResourceData waterResource;
    public ResourceData oxygenResource;
    public ResourceData oilResource;
    public ResourceData energyResource;

    public List<ResourceData> resources;
    void Start()
    {
        resources = new List<ResourceData>();
        waterResource = new ResourceData("Water", 0f, 1f, 0);
        oxygenResource = new ResourceData("Oxygen", 0f, 0f, 0);
        oilResource = new ResourceData("Oil", 0f, 0f, 0);
        energyResource = new ResourceData("Energy", 0f, 0f, 0);
        resources.Add(waterResource);
        resources.Add(oxygenResource);
        resources.Add(oilResource);
        resources.Add(energyResource);

    }
    void Update()
    {
        waterScoreText.text = waterResource.points.ToString();
        oxygenScoreText.text = oxygenResource.points.ToString();
        oilScoreText.text = oilResource.points.ToString();
        energyScoreText.text = energyResource.points.ToString();
    }

    public void ClickAction()
    {
        foreach(ResourceData resource in resources)
        {
            resource.points += resource.multiplier;
        }
    }
}
