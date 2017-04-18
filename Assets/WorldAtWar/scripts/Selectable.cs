using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[Serializable]
public class SelectableData
{
  public Types.ConstructionTypes Type = Types.ConstructionTypes.Building;
  public int Team = 0;
}

public class Selectable : MonoBehaviour {

  public SelectableData Data = new SelectableData();

  public bool IsPlacing = false;    
  public Vector3 SnapSize = Vector3.one;  

  void DoDie()
  {
    SendMessageUpwards("KillMe", this);
  }

  void OnMouseUp()
  {
   
    if (!EventSystem.current.IsPointerOverGameObject())
    {
      SendMessageUpwards("DeselectAll");
      Select();
    }
  }

  public float GetRadius()
  {
    NavMeshAgent nm = GetComponent<NavMeshAgent>();
    if ( nm != null ) {
      return nm.radius;
    }
    else
    {
      BoxCollider bc = GetComponent<BoxCollider>();
      return bc.bounds.extents.x;      
    }    
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

  public void MoveTo(Vector3 v) {
    SendMessage("DoMove", v);
  }

  public void Deselect()
  {
    
  }

  public void Select()
  {
    SendMessageUpwards("SelectMe", this);    
    SendMessage("DoSelect");
  }

  public void Action()
  {
    SendMessage("DoAction");
  }

  // Use this for initialization
  void Start () {        
  }
	
	// Update is called once per frame
	void Update () {
   // Debug.Log(EventSystem.current.IsPointerOverGameObject());
  }
}
