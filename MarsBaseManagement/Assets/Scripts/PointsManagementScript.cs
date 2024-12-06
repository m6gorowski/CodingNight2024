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

    private float _waterScore;    
    private float _oxygenScore;
    private float _oilScore;
    private float _energyScore;



    void Start()
    {
        _waterScore = 0f;
        _oxygenScore = 0f;
        _oilScore = 0f;
        _energyScore = 0f;
    }
    void Update()
    {
        waterScoreText.text = _waterScore.ToString();
        oxygenScoreText.text = _oxygenScore.ToString();
        oilScoreText.text = _oilScore.ToString();
        energyScoreText.text = _energyScore.ToString();
    }

    public void ClickAction()
    {
        _waterScore += 1;
    }
}
