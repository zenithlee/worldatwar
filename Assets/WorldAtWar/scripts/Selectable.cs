using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace WW
{

  [Serializable]
  public class SelectableData
  {
    [SerializeField]
    public Types.ConstructionTypes Type = Types.ConstructionTypes.Building;
    [SerializeField]
    public int Team = 0;
    [SerializeField]
    public float Cost = 100;

    [SerializeField]
    public float x,y,z = 0;    
    [SerializeField]
    public float rx,ry,rz = 0;
  }

  public class Selectable : MonoBehaviour
  {

    public SelectableData Data = new SelectableData();

    public bool IsPlacing = false;
    public Vector3 SnapSize = Vector3.one;

    public SelectableData Serialize()
    {
      Data.x = this.transform.position.x;
      Data.y = this.transform.position.y;
      Data.z = this.transform.position.z;
      Vector3 r = this.transform.rotation.eulerAngles;
      Data.rx = r.x;
      Data.ry = r.y;
      Data.rz = r.z;
      return Data;
    }

    void DoDie()
    {
      SendMessageUpwards("KillMe", this, SendMessageOptions.DontRequireReceiver);
    }

    void OnMouseUp()
    {
      if (!EventSystem.current.IsPointerOverGameObject())
      {
        //SendMessageUpwards("DeselectAll");
        Select();
      }
    }

    public void Select()
    {
      SendMessageUpwards("SelectMe", this);
      SendMessage("DoSelect");
    }

    public float GetRadius()
    {
      NavMeshAgent nm = GetComponent<NavMeshAgent>();
      if (nm != null)
      {
        return nm.radius;
      }
      else
      {
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc != null)
        {
          return bc.bounds.extents.x;
        }
      }
      return 1;
    }

    public void Place()
    {
      IsPlacing = false;
      SendMessage("DoPlace");
    }

    public void SetTarget(Vector3 v)
    {
      SendMessage("DoSetTarget", v);
    }

    public void MoveTo(Vector3 v)
    {
      SendMessage("DoMove", v);
    }

    public void Deselect()
    {

    }

   

    public void Action()
    {
      SendMessage("DoAction");
    }

    // Use this for initialization
    void Start()
    {

      BoxCollider b = GetComponent<BoxCollider>();
      if (b != null)
      {
        this.SnapSize = b.size;
      }
    }

    // Update is called once per frame
    void Update()
    {
      // Debug.Log(EventSystem.current.IsPointerOverGameObject());
    }
  }


}