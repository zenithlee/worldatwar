using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour {

  public bool IsPlacing = false;
  public enum Types { Barracks, VehicleFactory, Building, Assault };
  public Types Type = Types.Building;

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
