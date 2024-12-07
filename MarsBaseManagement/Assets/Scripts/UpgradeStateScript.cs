using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStateScript : MonoBehaviour
{
    [SerializeField]
    private string[] _descriptions;
    [SerializeField]
    private int[] _levels;
    [SerializeField]
    private Button _buyButton;
    [SerializeField]
    private string[] _buyButtonDesc;

    private Color _redButtonHue = new Color(238, 144, 97);
    private Color _greenButtonHue = new Color(157, 238, 97);

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
