using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SlidingUpgradesMenu : MonoBehaviour
{
    [SerializeField]
    private RectTransform upgradesMenuPanel;
    [SerializeField]
    private float slideSpeed = 5f;
    private bool isUpgradesMenuOpen = false;

    void Start()
    {
        upgradesMenuPanel.anchoredPosition = new Vector2(0, -upgradesMenuPanel.rect.height);
    }

    public void ToggleMenu()
    {
        isUpgradesMenuOpen = !isUpgradesMenuOpen;
    }
    private void Update()
    {
        if (isUpgradesMenuOpen)
        {
            upgradesMenuPanel.anchoredPosition = Vector2.Lerp(upgradesMenuPanel.anchoredPosition, Vector2.zero, Time.deltaTime * slideSpeed);
        }
        else
        {           
            upgradesMenuPanel.anchoredPosition = Vector2.Lerp(upgradesMenuPanel.anchoredPosition, new Vector2(0, -upgradesMenuPanel.rect.height), Time.deltaTime * slideSpeed);
        }
    }
}
