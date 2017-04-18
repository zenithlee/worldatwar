using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitsMan : MonoBehaviour {

  public Dictionary<Types.ConstructionTypes, int> Limits = new Dictionary<Types.ConstructionTypes, int>();
  public int MaxBase = 1;
  public float BaseBuildTime = 3;

  public int MaxBarracks = 1;

  public int CountOf(Types.ConstructionTypes t)
  {

    Selectable[] items = transform.GetComponentsInChildren<Selectable>();
    int Count = 0;
    foreach( Selectable s in items)
    {
      if (s.Data.Type == t) Count++;
    }
    return Count;
  }

  public bool CanIBuild(Types.ConstructionTypes type)
  {
    if ( type == Types.ConstructionTypes.Base)
    {
      if (CountOf(Types.ConstructionTypes.Base) < MaxBase)
      {
        return true;
      }
    }
    return false;
  }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
