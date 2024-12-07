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
        waterResource = new ResourceData("Water", 0, 0.1f, 0);
        oxygenResource = new ResourceData("Oxygen", 0, 0f, 0);
        oilResource = new ResourceData("Oil", 0, 0f, 0);
        energyResource = new ResourceData("Energy", 0, 0f, 0);
        resources.Add(waterResource);
        resources.Add(oxygenResource);
        resources.Add(oilResource);
        resources.Add(energyResource);

        waterUpgrades = new List<Upgrade>
        {
            new Upgrade("New Water Filter", 1, new Dictionary<string, float> { { "Water", 0.7f } }, 0.1f, "New Water Filter", upgradePrefab, resourceIcons[0]),
            new Upgrade("New Pump System", 2, new Dictionary<string, float> { { "Water", 5f }, { "Oil", 3f } }, 0.3f, "New Pump System", upgradePrefab, resourceIcons[0]),
            new Upgrade("New Pipes", 3, new Dictionary<string, float> { { "Water", 15f }, { "Oil", 5f } }, 0.5f, "New Pipes", upgradePrefab, resourceIcons[0])
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
        // Loop through each resource and its respective upgrade list
        List<(ResourceData resource, List<Upgrade> upgrades)> resourceUpgradePairs = new List<(ResourceData, List<Upgrade>)>
        {
            (waterResource, waterUpgrades),
            (oxygenResource, oxygenUpgrades),
            (oilResource, oilUpgrades),
            (energyResource, energyUpgrades)
        };

        // Loop through each resource and upgrade list
        foreach (var resourceUpgradePair in resourceUpgradePairs)
        {
            ResourceData resource = resourceUpgradePair.resource;
            List<Upgrade> upgrades = resourceUpgradePair.upgrades;

            // Loop through the upgrades of the current resource
            foreach (Upgrade upgrade in upgrades)
            {
                // Only display the upgrade that matches the next level
                if (upgrade.level == resource.upgradeLevel + 1) // Show only the next level upgrade
                {
                    // Instantiate the upgrade prefab
                    GameObject upgradeInstance = Instantiate(upgrade.prefab, scrollView.content);
                    Transform backgroundImage = upgradeInstance.transform.Find("BackgroundImage");

                    // Set the icon image
                    Image resourceImage = backgroundImage.transform.Find("ResourceImage").GetComponent<Image>();
                    resourceImage.sprite = upgrade.icon;

                    // Set the cost text
                    TextMeshProUGUI costText = backgroundImage.transform.Find("BuyButton").GetComponentInChildren<TextMeshProUGUI>();
                    costText.text = "Cost: ";
                    foreach (var resourceCost in upgrade.cost)
                    {
                        costText.text += resourceCost.Key + ": " + resourceCost.Value + " ";
                    }

                    // Set the description text
                    TextMeshProUGUI descriptionText = backgroundImage.transform.Find("Description").GetComponent<TextMeshProUGUI>();
                    descriptionText.text = upgrade.description;

                    // Find the buy button in the instantiated prefab and add the onClick listener
                    Button buyButton = backgroundImage.transform.Find("BuyButton").GetComponent<Button>();

                    // Ensure that the button triggers the correct method
                    buyButton.onClick.RemoveAllListeners(); // Clear any existing listeners to prevent duplicates
                    buyButton.onClick.AddListener(() => BuyUpgrade(upgrade, resource)); // Add the new listener
                    
                }
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

    private void BuyUpgrade(Upgrade upgrade, ResourceData resource)
    {
        // Check if player has enough resources to buy the upgrade
        foreach (var resourceCost in upgrade.cost)
        {
            string resourceName = resourceCost.Key;
            float cost = resourceCost.Value;

            // Find the resource data based on the resource name
            ResourceData playerResource = resources.First(r => r.resourceName == resourceName);
            if (playerResource.points < cost)
            {
                Debug.Log("Not enough " + resourceName + " to buy this upgrade.");
                return; // Exit if not enough resources
            }
        }

        // Deduct the cost from the player's resources
        foreach (var resourceCost in upgrade.cost)
        {
            string resourceName = resourceCost.Key;
            float cost = resourceCost.Value;
            ResourceData playerResource = resources.First(r => r.resourceName == resourceName);
            playerResource.points -= cost;
        }

        // Upgrade has been purchased, so increment the resource's upgrade level
        resource.upgradeLevel++;

        // Now, update the scroll view to show the next level upgrade
        PopulateUpgradeScrollView();
        UpdateUpgradeUI();
    }

}