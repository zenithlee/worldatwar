using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WW
{

  public class UIMan : MonoBehaviour
  {

    public GameObject BarracksPanel;
    public GameObject VehicleFactoryPanel;
    public GameObject ConstructionPanel;
    public GameObject BasePanel;
    public GameObject MinePanel;
    public GameObject ContextPanel;

    public List<Selectable> Selection;
    public Report Reporter;


    public void SetSelection(List<Selectable> nl)
    {
      Selection = nl;
      if ( Selection.Count > 0 )
      {
        Selectable s = Selection[0];
        Reporter.SetSelection(s);
      }
      else
      {
        Reporter.SetSelection(null);
      }
    }


    void SelectMe(Selectable t)
    {
      Game g = GetComponent<Game>();
      if (t.Data.Team != g.MyTeam.Data.TeamID) return;


      if (t.IsPlacing == false)
      {
        if (t.Data.Type == Types.ConstructionTypes.Barracks)
        {
          ShowBarracks();
        }
        if (t.Data.Type == Types.ConstructionTypes.VehicleFactory)
        {
          ShowVehicleFactory();
        }
        if (t.Data.Type == Types.ConstructionTypes.Base)
        {
          ShowBase();
        }
        if (t.Data.Type == Types.ConstructionTypes.Mine)
        {
          ShowMine();
        }
      }
    }

    void DoDeselectAll()
    {
      HideAllPanels();
      ShowConstruction();
    }


    public void HideAllPanels()
    {
      BarracksPanel.SetActive(false);
      VehicleFactoryPanel.SetActive(false);
      ConstructionPanel.SetActive(false);
      BasePanel.SetActive(false);
      ContextPanel.SetActive(false);
    }

    public void ShowConstruction()
    {
      HideAllPanels();
      ConstructionPanel.SetActive(true);
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

    public void ShowBase()
    {
      HideAllPanels();
      BasePanel.SetActive(true);
    }

    public void ShowMine()
    {
      HideAllPanels();
      MinePanel.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
  }


}