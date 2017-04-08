using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBounds : MonoBehaviour {

  List<Selectable> Units;
  public GameObject SelectionHandle;

  public GameObject TopLeft;
  public GameObject TopRight;
  public GameObject BottomLeft;
  public GameObject BottomRight;

  public Vector3 Center ;

  public void SetUnits(List<Selectable> _units)
  {
    Units = _units;
  }

  public void CalculateSelectionBounds()
  {
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

  }

  // Use this for initialization
  void Start () {
    SelectionHandle = transform.FindChild("Handle").gameObject;
    TopLeft = transform.Find("Handle/TopLeft").gameObject;
    TopRight = transform.Find("Handle/TopRight").gameObject;
    BottomRight = transform.Find("Handle/BottomRight").gameObject;
    BottomLeft = transform.Find("Handle/BottomLeft").gameObject;
  }
	
	// Update is called once per frame
	void Update () {
    if ( Units != null ) { 
      CalculateSelectionBounds();
    }
  }
}
