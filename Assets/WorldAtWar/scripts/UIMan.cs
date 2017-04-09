using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMan : MonoBehaviour {

  public GameObject BarracksPanel;
  public GameObject VehicleFactoryPanel;
  public GameObject ConstructionPanel;


  void SelectMe( Selectable t)
  {
    if (t.IsPlacing == false) { 
      if ( t.Type == Selectable.Types.Barracks)
      {
        ShowBarracks();
      }
      if (t.Type == Selectable.Types.VehicleFactory)
      {
        ShowVehicleFactory();
      }
    }
  }

  public void HideAllPanels()
  {
    BarracksPanel.SetActive(false);
    VehicleFactoryPanel.SetActive(false);
    ConstructionPanel.SetActive(false);
  }

  public void ShowBarracks()
  {
    HideAllPanels();
    BarracksPanel.SetActive(true);
  }

  public void ShowVehicleFactory()
  {
    HideAllPanels();
    VehicleFactoryPanel.SetActive(true);
  }

  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
