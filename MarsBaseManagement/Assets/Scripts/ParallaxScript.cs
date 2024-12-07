using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = -2f;
    private float _singleTextureWidth;
    void Start()
    {
        SetupTexture();
    }

    void Update()
    {
        Scroll();
        CheckReset();
    }
    void SetupTexture()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        _singleTextureWidth = sprite.texture.width / (sprite.pixelsPerUnit * 3);
    }
    void Scroll()
    {
        float delta = (_scrollSpeed * Time.deltaTime);
        transform.position += new Vector3(delta, 0f, 0f);
    }

    void CheckReset()
    {
        if ((Mathf.Abs(transform.position.x) - _singleTextureWidth) > 0)
        {
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        }
    }
}
