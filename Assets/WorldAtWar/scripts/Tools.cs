using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools {
  public static Vector3 GetSnap(GameObject go, Vector3 pos)
  {
    Selectable s = go.GetComponent<Selectable>();
    Vector3 v = new Vector3(
      Mathf.Round(pos.x / s.SnapSize.x) * s.SnapSize.x,
      Mathf.Round(pos.y / s.SnapSize.y) * s.SnapSize.y,
    Mathf.Round(pos.z / s.SnapSize.z) * s.SnapSize.z);

    return v;
  }

}
