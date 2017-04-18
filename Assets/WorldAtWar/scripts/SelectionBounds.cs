using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBounds : MonoBehaviour {

  List<Selectable> Units = new List<Selectable>();
  List<Selectable> KillList = new List<Selectable>();
  public GameObject SelectionHandle;

  public GameObject TopLeft;
  public GameObject TopRight;
  public GameObject BottomLeft;
  public GameObject BottomRight;

  public GameObject AcceptButtons;

  public Vector3 Center ;

  public void ButtonPress(string msg)
  {
    if ( msg == "Accept")
    {
      foreach (Selectable t in Units)
      {                 
        t.Place();        
      }
    }

    if (msg == "Cancel")
    {

      KillList.Clear();

      foreach (Selectable t in Units)
      {               
        KillList.Add(t);
      }

      foreach( Selectable t in KillList)
      {
        Units.Remove(t);
        GameObject.Destroy(t.gameObject, 0.1f);
      }
    }
  }

  public void SetUnits(List<Selectable> _units)
  {
    Units = _units;
  }

  public void CalculateSelectionBounds()
  {
    bool IsPlacing = false;

    if (Units.Count == 0)
    {
      SelectionHandle.SetActive(false);
    }
    else
    {
      SelectionHandle.SetActive(true);
      Center = new Vector3(0, 0, 0);
      Vector3 tl = new Vector3(999, 999, 999);     

      foreach (Selectable t in Units)
      {
        Center += t.transform.position;
        tl = Vector3.Min(t.transform.position, tl);

        if ( t.IsPlacing == true )
        {
          IsPlacing = true;
        }
      }



      Center /= Units.Count;
      Bounds b = new Bounds(Center, Vector3.one);

      foreach (Selectable t in Units)
      {
        b.Encapsulate(t.transform.position);
      }      

      SelectionHandle.transform.position = Center;
      b.Expand(new Vector3(10,0,10));

      TopLeft.transform.position = b.center - b.extents;
      float yp = TopLeft.transform.position.y; //grab the y coord from the first corner
      TopRight.transform.position = b.center + new Vector3(-b.extents.x, yp, +b.extents.z);
      TopRight.transform.position = new Vector3(TopRight.transform.position.x, yp, TopRight.transform.position.z);
      BottomRight.transform.position = b.center + b.extents;
      BottomRight.transform.position = new Vector3(BottomRight.transform.position.x,yp, BottomRight.transform.position.z);
      BottomLeft.transform.position = b.center + new Vector3(b.extents.x, yp, -b.extents.z);
      BottomLeft.transform.position = new Vector3(BottomLeft.transform.position.x, yp, BottomLeft.transform.position.z);
    }
    
    AcceptButtons.SetActive(IsPlacing);
  }

  // Use this for initialization
  void Start () {
    SelectionHandle = transform.FindChild("Handle").gameObject;
    TopLeft = transform.Find("Handle/TopLeft").gameObject;
    TopRight = transform.Find("Handle/TopRight").gameObject;
    BottomRight = transform.Find("Handle/BottomRight").gameObject;
    BottomLeft = transform.Find("Handle/BottomLeft").gameObject;

    AcceptButtons = transform.Find("Handle/TopRight/AcceptCancel").gameObject;    
  }
	
	// Update is called once per frame
	void Update () {
    if ( Units != null ) { 
      CalculateSelectionBounds();
    }
  }
}
