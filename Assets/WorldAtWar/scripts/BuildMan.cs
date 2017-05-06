using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WW
{

  public class BuildMan : MonoBehaviour
  {

    float TimeLeft = 5;
    public Team MyTeam;
    public GameObject MyArmy;
    public LimitsMan Limits;
    public GameObject GameTerrain;

    public void Build(GameObject proto)
    {
      Selectable sel = proto.GetComponent<Selectable>();
      GameObject go = Instantiate(proto);
      SendMessage("PlaceMe", go);

      MyTeam.Data.Credits -= Limits.CostOf(sel.Data.Type);
      Limits.RegisterTransaction();
    }

    public void MarkerAt(Transform parent, Vector3 v)
    {
      GameObject go = Instantiate(MyTeam.Marker1Proto);
      go.transform.parent = parent;
      go.transform.position = v;
    }

    public void BuildBase()
    {
      if ( Limits.CheckBuild( Types.ConstructionTypes.Base)) {
        Build(MyTeam.BaseProto);  
      }
    }

    public void BuildMine()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.Mine))
      {
        Build(MyTeam.MineProto);
      }      
    }

    public void BuildAssault()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.Assault))
      {
        Build(MyTeam.AssaultProto);
      }     
    }

    public void BuildGunner()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.Gunner))
      {
        Build(MyTeam.GunnerProto);
      }
    }

    public void BuildSniper()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.Sniper))
      {
        Build(MyTeam.SniperProto);
      }
    }

    public void BuildEngineer()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.Engineer))
      {
        Build(MyTeam.EngineerProto);
      }
    }

    public void BuildJeep()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.Jeep))
      {
        Build(MyTeam.JeepProto);
      }
    }

    public void BuildPowerStation()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.PowerStation))
      {
        Build(MyTeam.PowerStationProto);
      }
    }

    public void BuildWall()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.Wall))
      {
        Build(MyTeam.WallProto);
      }
    }

    public void BuildTank()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.Tank))
      {
        Build(MyTeam.TankProto);
      }
    }

    public void BuildBarracks()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.Barracks))
      {
        Build(MyTeam.BarracksProto);
      }
    }

    public void BuildVehicleFactory()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.VehicleFactory))
      {
        Build(MyTeam.VehicleFactoryProto);
      }
    }

    public void BuildContainer()
    {
      if (Limits.CheckBuild(Types.ConstructionTypes.Container))
      {
        Build(MyTeam.ContainerProto);
      }
    }

    Team GetTeamByID(int id)
    {
      Team[] teams = GetComponentsInChildren<Team>();
      foreach (Team t in teams)
      {
        if (t.Data.Team == id) return t;
      }
      return null;
    }

    void PlaceMeNear(GameObject go)
    {
      NavMeshAgent nm = go.GetComponent<NavMeshAgent>();
      Selectable gs = go.GetComponent<Selectable>();
      gs.SnapSize = new Vector3(gs.GetRadius() * 5, 1, gs.GetRadius() * 5);
      if (nm != null)
      {
        float r = gs.GetRadius() * 5;
        Vector3 pos = Tools.GetSnap(go, go.transform.position + new Vector3(Random.value * r - r / 2, 0, Random.value * r - r / 2));
        go.GetComponent<NavMeshAgent>().Warp(pos);
      }
      else
      {
        gs.IsPlacing = true;
        float r = gs.GetRadius() * 5;
        Vector3 pos = Tools.GetSnap(go, go.transform.position + new Vector3(Random.value * r - r / 2, 0, Random.value * r - r / 2));
        go.transform.position = pos;
        gs.IsPlacing = false;
      }

      CreateRadarDot(go);
    }

    void PlaceMe(GameObject go)
    {
      Random.InitState((int)(Time.frameCount));
      Selectable gs = go.GetComponent<Selectable>();
      Team team = GetTeamByID(gs.Data.Team);
      go.transform.parent = team.transform;

      if (gs.Data.Team == MyTeam.Data.Team)
      {
        team.GetComponent<LimitsMan>().CheckBuild(gs.Data.Type);
      }

      RaycastHit hit;

      Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.5f, Screen.height / 2, 0));
      if (GameTerrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
      {
        NavMeshAgent nm = go.GetComponent<NavMeshAgent>();
        if (nm != null)
        {
          float r = gs.GetRadius() * 5;
          Vector3 pos = Tools.GetSnap(go, hit.point + new Vector3(Random.value * r - r / 2, 0, Random.value * r - r / 2));
          go.GetComponent<NavMeshAgent>().Warp(pos);
        }
        else
        {
          gs.IsPlacing = true;
          float r = gs.GetRadius() * 2;
          Vector3 pos = Tools.GetSnap(go, hit.point + new Vector3(Random.value * r - r / 2, 0, Random.value * r - r / 2));
          go.transform.position = pos;
        }

      }
    }

    void CreateRadarDot(GameObject go)
    {
      Selectable sel = go.GetComponent<Selectable>();
      Team t = GetTeamByID(sel.Data.Team);
      GameObject Proto = null;
      GameObject dot = null;
      switch( sel.Data.Type )
      {
        case Types.ConstructionTypes.Base:
        case Types.ConstructionTypes.Barracks:
          Proto = t.RadarDotBuildingProto;
          break;
        case Types.ConstructionTypes.Assault:
        case Types.ConstructionTypes.Gunner:
        case Types.ConstructionTypes.Sniper:
        case Types.ConstructionTypes.Engineer:
          Proto = t.RadarDotInfantryProto;
          break;
        case Types.ConstructionTypes.Jeep:
        case Types.ConstructionTypes.Tank:
        case Types.ConstructionTypes.SmallTank:
        case Types.ConstructionTypes.LargeTank:
        case Types.ConstructionTypes.CargoTruck:
        case Types.ConstructionTypes.Boat:
        case Types.ConstructionTypes.AttackHelicopter:
        case Types.ConstructionTypes.FighterPlane:
        case Types.ConstructionTypes.Bomber:
        case Types.ConstructionTypes.Aircraft:
        case Types.ConstructionTypes.APC:
        case Types.ConstructionTypes.AA:
          Proto = t.RadarDotVehicleProto;
          break;
        default:
          Proto = t.RadarDotBuildingProto;
          break;
      }

      dot = Instantiate(Proto);
      dot.transform.parent = go.transform;
      dot.transform.position = new Vector3(go.transform.position.x, 100, go.transform.position.z);
    }

    // Use this for initialization
    void Start()
    {
      MyArmy = GetComponent<Game>().MyArmy;
      MyTeam = MyArmy.GetComponent<Team>();
      Limits = MyTeam.GetComponent<LimitsMan>();
      GameTerrain = GetComponent<Game>().GameTerrain;
    }

    // Update is called once per frame
    void Update()
    {

    }
  }


}