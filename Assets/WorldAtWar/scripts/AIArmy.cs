using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WW
{

  public class AIArmy : MonoBehaviour
  {

    LimitsMan Limits;
    BuildMan Builder;
    Team MyTeam;
    //Make this lower for more advanced levels
    float DelayBetweenBuilds = 5;

    // Use this for initialization
    void Start()
    {
      Limits = GetComponent<LimitsMan>();
      Builder = GetComponent<BuildMan>();
      MyTeam = GetComponent<Team>();
      //StartCoroutine("BuildArmy");
      InvokeRepeating("CheckBuild", 1, DelayBetweenBuilds);
    }
    

    void Build(GameObject proto)
    {
      
      if ( MyTeam.Data.Credits >= Limits.CostOf(proto.GetComponent<Selectable>().Data.Type))
      { 
        GameObject go = Instantiate(proto, this.transform.position, this.transform.rotation);
        go.transform.parent = this.transform;
        Selectable sel = go.GetComponent<Selectable>();
        sel.Data.Team = MyTeam.Data.TeamID;        
        MyTeam.Data.Credits -= Limits.CostOf(sel.Data.Type);
        //send up to buildman
        SendMessageUpwards("PlaceMeNear", go);
      }
    }

    bool IHaveA(Types.ConstructionTypes type)
    {
      for (int i = 0; i < MyTeam.transform.childCount; i++)
      {
        Selectable s = MyTeam.transform.GetChild(i).GetComponent<Selectable>();
        if (s.Data.Type == type) return true;
      }
      return false;
    }

    void CheckBuild()
    {
      //Base
      if (Limits.CheckBuild(Types.ConstructionTypes.Base))
      {
        Build(MyTeam.GetProtoByType(Types.ConstructionTypes.Base));
        return;
      }

      if (Limits.CheckBuild(Types.ConstructionTypes.Barracks))
      {
        Build(MyTeam.GetProtoByType(Types.ConstructionTypes.Barracks));
        return;
      }

      if (IHaveA(Types.ConstructionTypes.Barracks))
      {
        if (Limits.CheckBuild(Types.ConstructionTypes.Assault))
        {
          Build(MyTeam.GetProtoByType(Types.ConstructionTypes.Assault));
        }

        if (Limits.CheckBuild(Types.ConstructionTypes.Gunner))
        {
          Build(MyTeam.GetProtoByType(Types.ConstructionTypes.Gunner));
        }
      }

      if (Limits.CheckBuild(Types.ConstructionTypes.VehicleFactory))
      {
        Build(MyTeam.GetProtoByType(Types.ConstructionTypes.VehicleFactory));
        return;
      }

    }

    // Update is called once per frame
    void Update()
    {

    }
  }


}