using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIArmy : MonoBehaviour {

  LimitsMan Limits;
  BuildMan Builder;
  Team MyTeam;

	// Use this for initialization
	void Start () {
    Limits = GetComponent<LimitsMan>();
    Builder = GetComponent<BuildMan>();
    MyTeam = GetComponent<Team>();
    Invoke("BuildArmy", 0.5f);
	}


  void BuildArmy()
  {
    if ( Limits.CheckBuild(Types.ConstructionTypes.Base) )
    {
      Build(MyTeam.BaseProto);
    }
  }

  void Build(GameObject proto)
  {
    GameObject go = Instantiate(proto, this.transform.position, this.transform.rotation);
    go.transform.parent = this.transform;
    go.GetComponent<Selectable>().Data.Team = MyTeam.Data.TeamID;
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
