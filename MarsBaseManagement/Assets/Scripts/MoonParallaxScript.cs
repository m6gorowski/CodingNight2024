using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoonParallaxScript : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = -50f; // Speed at which the moon moves
    [SerializeField] private int distanceMultiplier = 30; // How many times the moon's width it moves
    private float moonWidth; // The width of the moon image
    private RectTransform rectTransform; // Reference to the RectTransform component
    private Vector3 startPosition; // The initial position of the moon

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        SetupMoon();
    }

    void Update()
    {
        MoveMoon();
        CheckReset();
    }

    void SetupMoon()
    {
        Image moonImage = GetComponent<Image>();
        if (moonImage != null && moonImage.sprite != null)
        {
            moonWidth = moonImage.sprite.rect.width / moonImage.pixelsPerUnit;
        }
        else
        {
            Debug.LogWarning("No Image or Sprite assigned to the UI element.");
            moonWidth = rectTransform.rect.width;
        }
        startPosition = rectTransform.localPosition;
    }

    void MoveMoon()
    {
        float delta = scrollSpeed * Time.deltaTime;
        rectTransform.localPosition += new Vector3(delta, 0f, 0f);
    }

    void CheckReset()
    {
        float maxDistance = distanceMultiplier * moonWidth;
        if (startPosition.x - rectTransform.localPosition.x >= maxDistance)
        {
            rectTransform.localPosition = startPosition;
        }
    }
}
