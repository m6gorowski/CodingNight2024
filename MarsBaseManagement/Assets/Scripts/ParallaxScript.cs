using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxScript : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = -2f;
    private float _singleTextureWidth;
    private RectTransform _rectTransform;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        SetupTexture();
    }

    void Update()
    {
        Scroll();
        CheckReset();
    }
    void SetupTexture()
    {
        Image image = GetComponent<Image>();
        if (image != null && image.sprite != null)
        {
            _singleTextureWidth = image.sprite.rect.width / image.pixelsPerUnit / 3f;
        }
        else
        {
            Debug.LogWarning("No Image or Sprite assigned to the UI element.");
        }
    }
    void Scroll()
    {
        float delta = _scrollSpeed * Time.deltaTime;
        _rectTransform.localPosition += new Vector3(delta, 0f, 0f);
    }

    void CheckReset()
    {
        if (Mathf.Abs(_rectTransform.localPosition.x) >= _singleTextureWidth)
        {
            _rectTransform.localPosition = new Vector3(0f, _rectTransform.localPosition.y, _rectTransform.localPosition.z);
        }
    }
}
