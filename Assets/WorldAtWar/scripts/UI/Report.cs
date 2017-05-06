using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Report : MonoBehaviour {

  public WW.Selectable Target;
  public Text HealthText;
  public Text CostText;

	// Use this for initialization
	void Start () {
    InvokeRepeating("SlowUpdate", 1.1f, 1.1f);
    HealthText = transform.FindChild("HealthText").GetComponent<Text>();
    CostText = transform.FindChild("CostText").GetComponent<Text>();
  }

  public void SetSelection( WW.Selectable ns)
  {
    Target = ns;
  }

  void SlowUpdate()
  {
    if (Target != null)
    {
      Health h = Target.GetComponent<Health>();
      HealthText.text = Mathf.RoundToInt(h.HealthValue / h.InitialHealth * 100).ToString() + "%";
      CostText.text = Target.Data.Cost.ToString();
    }
    else
    {
      HealthText.text = "???";
      CostText.text = "???";
    }
}
	
	// Update is called once per frame
	void Update () {
    
  }
}
