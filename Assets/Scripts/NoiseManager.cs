using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class NoiseManager : MonoBehaviour
{

    public TextMeshProUGUI noiseGauge;
    public Image noiseMeter;
    public double totalNoise;
    public double noiseFloor;

    // Start is called before the first frame update
    void Start()
    {
        //noiseGauge = this.GetComponent<TextMeshProUGUI>();
        noiseGauge.SetText("0.0%");
    }

    // Update is called once per frame
    void Update()
    {
        noiseGauge.SetText(totalNoise + "%");

        if (totalNoise >= 50.0)
        {
            noiseFloor = 50.0;
        }

        switch (totalNoise) {

            case var n when (n >= 0.0 && n <= 49.9):
                noiseMeter.color = new Color32(0, 125, 0, 255);
                break;

            case var n when (n >= 50.0 && n <= 99.9):
                noiseMeter.color = new Color32(235, 126, 39, 255);
                break;

            case var n when (n >= 100.0):
                noiseMeter.color = new Color32(218, 22, 5, 255);
                break;
        
        }
    }

    public void NoiseGain(double damage)
    {
        totalNoise += (damage * 0.5);
    }

    public void NoiseDecay()
    {

    }
}
