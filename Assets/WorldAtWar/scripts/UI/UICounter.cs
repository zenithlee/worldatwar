using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WW
{

  public class UICounter : MonoBehaviour
  {

    public double CurrentValue = 1000;
    public double TargetValue = 0;
    public double Steps = 100;
    public double Diff = 1;

    public Text text;
    public string CurrencySymbol = "$";


    public void SetValue(float newVal)
    {
      TargetValue = newVal;      
    }

    // Use this for initialization
    void Start()
    {
      text = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      Diff = (TargetValue - CurrentValue) / Steps;
      CurrentValue += Diff * 5;
        //if (CurrentValue > TargetValue) CurrentValue = TargetValue;        
        text.text = CurrencySymbol + CurrentValue.ToString("N2");     
    }
  }

}