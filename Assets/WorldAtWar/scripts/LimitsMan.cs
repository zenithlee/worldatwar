using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitsMan : MonoBehaviour {

  [SerializeField]
  public Dictionary<Types.ConstructionTypes, int> Limits = new Dictionary<Types.ConstructionTypes, int>();
  public Dictionary<Types.ConstructionTypes, Button> Buttons = new Dictionary<Types.ConstructionTypes, Button>();

  public Button UI_BaseButton;
  public Button UI_MineButton;
  public Button UI_PowerStationButton;
  public Button UI_BarracksButton;
  public Button UI_WallButton;

  public Button UI_TankButton;
  public Button UI_JeepButton;

  // Use this for initialization
  void Start()
  {
    Limits[Types.ConstructionTypes.Base] = 1;
    Limits[Types.ConstructionTypes.Mine] = 2;
    Limits[Types.ConstructionTypes.PowerStation] = 5;
    Limits[Types.ConstructionTypes.Barracks] = 2;
    Limits[Types.ConstructionTypes.Wall] = 20;

    Limits[Types.ConstructionTypes.Tank] = 5;
    Limits[Types.ConstructionTypes.Jeep] = 5;


    Buttons[Types.ConstructionTypes.Base] = UI_BaseButton;
    Buttons[Types.ConstructionTypes.Mine] = UI_MineButton;
    Buttons[Types.ConstructionTypes.PowerStation] = UI_PowerStationButton;
    Buttons[Types.ConstructionTypes.Barracks] = UI_BarracksButton;
    Buttons[Types.ConstructionTypes.Wall] = UI_WallButton;

    Buttons[Types.ConstructionTypes.Tank] = UI_TankButton;
    Buttons[Types.ConstructionTypes.Jeep] = UI_JeepButton;
  }

  public int CountOf(Types.ConstructionTypes t)
  {

    Selectable[] items = transform.GetComponentsInChildren<Selectable>();
    int Count = 1;
    foreach( Selectable s in items)
    {
      if (s.Data.Type == t) Count++;
    }
    return Count;
  }

  public bool CheckBuild(Types.ConstructionTypes type)
  {
    if ( CountOf(type) > Limits[type])
    {
      SetUI(type, false);
      return false;
    }

    SetUI(type, true);
    return true;   
  }

  void SetUI(Types.ConstructionTypes type, bool b)
  {
    if ( Buttons.ContainsKey(type)) {
      if (Buttons[type] != null)
      {
        Buttons[type].interactable = b;
      }
    }
  }
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
