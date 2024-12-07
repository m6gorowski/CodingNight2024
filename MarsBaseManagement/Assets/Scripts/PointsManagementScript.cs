using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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

    public Sprite[] resourceIcons;

    // Single Upgrade Prefab 
    public GameObject upgradePrefab;

    // Scroll View 
    public ScrollRect scrollView;
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
    public class Upgrade
    {
        public string upgradeName;
        public int level;
        public Dictionary<string, float> cost;
        public float multiplierIncrease;
        public Sprite icon; 
        public string description;
        public GameObject prefab = new GameObject();
        public Upgrade(string name, int initialLevel, Dictionary<string, float> initialCost, float increase, string description, GameObject prefab, Sprite icon)
        {
            upgradeName = name;
            level = initialLevel;
            cost = initialCost;
            multiplierIncrease = increase;
            this.description = description;
            this.prefab = prefab;
            this.icon = icon;
        }
    }
    public List<Upgrade> waterUpgrades;
    public List<Upgrade> oxygenUpgrades;
    public List<Upgrade> energyUpgrades;
    public List<Upgrade> oilUpgrades;

    public List<ResourceData> resources;
    void Start()
    {
        resources = new List<ResourceData>();
        waterResource = new ResourceData("Water", 0f, 0.1f, 0);
        oxygenResource = new ResourceData("Oxygen", 0f, 0f, 0);
        oilResource = new ResourceData("Oil", 0f, 0f, 0);
        energyResource = new ResourceData("Energy", 0f, 0f, 0);
        resources.Add(waterResource);
        resources.Add(oxygenResource);
        resources.Add(oilResource);
        resources.Add(energyResource);

        waterUpgrades = new List<Upgrade>
        {
            new Upgrade("New Water Filter", 1, new Dictionary<string, float> { { "H20", 0.7f } }, 0.1f, "New Water Filter", upgradePrefab, resourceIcons[0]),
            new Upgrade("New Pump System", 2, new Dictionary<string, float> { { "H20", 5f }, { "Oil", 3f } }, 0.3f, "New Pump System", upgradePrefab, resourceIcons[0]),
            new Upgrade("New Pipes", 3, new Dictionary<string, float> { { "H20", 15f }, { "Oil", 5f } }, 0.5f, "New Pipes", upgradePrefab, resourceIcons[0])
        };

        PopulateUpgradeScrollView();

        UpdateUpgradeUI();
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
            resource.points = resource.points + resource.multiplier;
            resource.points = Mathf.Round(resource.points * 10.0f) * 0.1f;

        }
    }
    private void PopulateUpgradeScrollView()
    {
        foreach (List<Upgrade> upgradeList in new List<List<Upgrade>> { waterUpgrades, oxygenUpgrades, oilUpgrades, energyUpgrades })
        {
            // Loop through all upgrades in the list (e.g., waterUpgrades)
            foreach (Upgrade upgrade in waterUpgrades)
            {
                // Instantiate the prefab
                GameObject upgradeInstance = Instantiate(upgrade.prefab, scrollView.content);
                Transform backgroundImage = upgradeInstance.transform.Find("BackgroundImage");
                // Set the icon image
                Image resourceImage = backgroundImage.transform.Find("ResourceImage").GetComponent<Image>();
                resourceImage.sprite = upgrade.icon;

                // Set the cost text
                TextMeshProUGUI costText = backgroundImage.transform.Find("BuyButton").GetComponentInChildren<TextMeshProUGUI>();

                foreach (var resourceCost in upgrade.cost)
                {
                    costText.text += resourceCost.Key + ": " + resourceCost.Value + " ";
                }

                // Set the description text
                TextMeshProUGUI descriptionText = backgroundImage.transform.Find("Description").GetComponent<TextMeshProUGUI>();
                descriptionText.text = upgrade.description;
            }
        }
            
    }
    private void UpdateUpgradeUI()
    {
        // Loop through all upgrade lists
        foreach (List<Upgrade> upgradeList in new List<List<Upgrade>> { waterUpgrades, oxygenUpgrades, oilUpgrades, energyUpgrades })
        {
            // Loop through each upgrade in the list
            foreach (Upgrade upgrade in upgradeList)
            {
                // Find the corresponding upgrade instance in the scroll view
                GameObject upgradeInstance = FindUpgradeInstance(upgrade);

                // Update the cost text
                if (upgradeInstance != null)
                {
                    TextMeshProUGUI costText = upgradeInstance.transform.Find("BuyButton").GetComponentInChildren<TextMeshProUGUI>();
                    costText.text = "Cost: " + string.Join(", ", upgrade.cost.Select(c => c.Key + ": " + c.Value));

                    TextMeshProUGUI descriptionText = upgradeInstance.transform.Find("Description").GetComponent<TextMeshProUGUI>();
                    descriptionText.text = upgrade.description;
                }
            }
        }
    }

    // Helper function to find the upgrade instance in the scroll view
    private GameObject FindUpgradeInstance(Upgrade upgrade)
    {
        // You'll need to implement this function based on your specific scroll view structure.
        // Here's a possible approach:

        // Iterate through all child game objects of the scroll view content
        foreach (Transform child in scrollView.content)
        {
            // Check if the child's name matches the upgrade's name
            if (child.name == upgrade.upgradeName)
            {
                return child.gameObject;
            }
        }

        // If the upgrade instance is not found, return null
        return null;
    }

}