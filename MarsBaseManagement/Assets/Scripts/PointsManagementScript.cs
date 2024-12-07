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
            new Upgrade("New Water Filter", 1, new Dictionary<string, float> { { "Water", 5f } }, 0.1f, "New Water Filter", upgradePrefab, resourceIcons[0]),
            new Upgrade("New Pump System", 2, new Dictionary<string, float> { { "Water", 15f }, { "Oil", 3f } }, 0.3f, "New Pump System", upgradePrefab, resourceIcons[0]),
            new Upgrade("New Pipes", 3, new Dictionary<string, float> { { "Water", 30f }, { "Oil", 8f } }, 0.3f, "New Pipes", upgradePrefab, resourceIcons[0])
        };
        oxygenUpgrades = new List<Upgrade>
        {
            new Upgrade("Oxygen Purification", 1, new Dictionary<string, float> { { "Water", 40f } }, 0.1f, "Oxygen Purification", upgradePrefab, resourceIcons[1]),
            new Upgrade("New Pump System", 2, new Dictionary<string, float> { { "Water", 35f }, { "Energy", 10f } }, 0.3f, "New Pump System", upgradePrefab, resourceIcons[1])
        };
        oxygenUpgrades = new List<Upgrade>
        {
            new Upgrade("Batteries", 1, new Dictionary<string, float> { { "Water", 10f }, { "Oxygen", 4f } }, 0.1f, "Batteries", upgradePrefab, resourceIcons[2]),
            new Upgrade("Solar Panels", 2, new Dictionary<string, float> { { "Water", 15f }, { "Oxygen", 10f }, {"Energy", 5f } }, 0.3f, "Solar Panels", upgradePrefab, resourceIcons[2])
        };
        oilUpgrades = new List<Upgrade>
        {
            new Upgrade("Coal Mine", 1, new Dictionary<string, float> { { "Water", 20f }, { "Oxygen", 5f } }, 0.1f, "Coal Mine", upgradePrefab, resourceIcons[3]),
            new Upgrade("Basic Oil Refining", 2, new Dictionary<string, float> { { "Water", 25f }, { "Oxygen", 10f }, { "Energy", 8f } }, 0.2f, "Basic Oil Refining", upgradePrefab, resourceIcons[3]),
            new Upgrade("Oil Pumping", 3, new Dictionary<string, float> { { "Water", 50f } , { "Oxygen", 20f }, { "Oil", 20f }, { "Energy", 15f }}, 0.2f, "Oil Pumping", upgradePrefab, resourceIcons[3]),
            new Upgrade("Mega Oil Refinery", 4, new Dictionary<string, float> { { "Water", 35f }, { "Oxygen", 10f }, { "Oil", 50f }, { "Energy", 45f } }, 0.5f, "Mega Oil Refinery", upgradePrefab, resourceIcons[3])

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
        // List of resources and their respective upgrade lists
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
                // Only instantiate and display the upgrade if its level matches the next upgrade for this resource
                if (upgrade.level == resource.upgradeLevel + 1) // Show only the next level upgrade
                {
                    // Check if this upgrade has already been instantiated
                    if (!IsUpgradeInstantiated(upgrade))
                    {
                        // Instantiate the upgrade prefab and add it to the scroll view
                        GameObject upgradeInstance = Instantiate(upgrade.prefab, scrollView.content);
                        upgradeInstance.name = upgrade.upgradeName; // Set the name of the instantiated upgrade to avoid duplication

                        // Set the icon image
                        Transform backgroundImage = upgradeInstance.transform.Find("BackgroundImage");
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
                        buyButton.onClick.AddListener(() => BuyUpgrade(upgrade, resource, upgradeInstance)); // Pass the instance to disable it later
                    }
                }
            }
        }
    }

    // Helper function to check if an upgrade has already been instantiated
    private bool IsUpgradeInstantiated(Upgrade upgrade)
    {
        // Iterate through all instantiated upgrades and check if this upgrade already exists
        foreach (Transform child in scrollView.content)
        {
            if (child.name == upgrade.upgradeName)
            {
                return true; // Upgrade has already been instantiated
            }
        }
        return false; // Upgrade has not been instantiated
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
    private void BuyUpgrade(Upgrade upgrade, ResourceData resource, GameObject upgradeInstance)
    {
        // Check if player has enough resources to buy the upgrade
        foreach (var resourceCost in upgrade.cost)
        {
            string resourceName = resourceCost.Key;
            float cost = resourceCost.Value;

            // Find the resource data based on the resource name
            ResourceData playerResource = resources.FirstOrDefault(r => r.resourceName == resourceName);

            if (playerResource == null)
            {
                Debug.LogError("Resource not found: " + resourceName);
                return; // Exit if the resource is not found
            }

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

            ResourceData playerResource = resources.FirstOrDefault(r => r.resourceName == resourceName);
            if (playerResource == null)
            {
                Debug.LogError("Resource not found: " + resourceName);
                return; // Exit if the resource is not found
            }
            playerResource.points -= cost;
            resource.points = Mathf.Round(resource.points * 10.0f) * 0.1f;
        }

        // Upgrade has been purchased, so increment the resource's upgrade level
        resource.upgradeLevel++;

        // Apply the multiplier increase from the upgrade
        resource.multiplier += upgrade.multiplierIncrease; // Update the multiplier for the resource

        // Now, disable the old upgrade and make it semi-transparent
        if (upgradeInstance != null)
        {
            CanvasGroup canvasGroup = upgradeInstance.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = upgradeInstance.AddComponent<CanvasGroup>(); // Add CanvasGroup if it doesn't exist
            }
            canvasGroup.alpha = 0.5f; // Make it semi-transparent
            canvasGroup.interactable = false; // Disable interaction
            canvasGroup.blocksRaycasts = false; // Disable interaction with the UI
        }

        // Now, update the scroll view to show the next level upgrade
        PopulateUpgradeScrollView();
        UpdateUpgradeUI();
    }
}