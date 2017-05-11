using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

  public Color color = Color.white;

  public Camera _camera;
  static Vector3[] _frustumPoints = new Vector3[8];
  public GameObject Frustrum;
  public Vector3 Mult = Vector3.one;

  void LateUpdate()
  {
    Gizmos.color = color;

    _frustumPoints[0] = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
    //_frustumPoints[1] = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.nearClipPlane));
    _frustumPoints[2] = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
    //_frustumPoints[3] = _camera.ViewportToWorldPoint(new Vector3(0, 1, _camera.nearClipPlane));

    //_frustumPoints[4] = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.farClipPlane));
    //_frustumPoints[5] = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.farClipPlane));
    //_frustumPoints[6] = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.farClipPlane));
    //_frustumPoints[7] = _camera.ViewportToWorldPoint(new Vector3(0, 1, _camera.farClipPlane));

    Frustrum.transform.localScale = new Vector3((_frustumPoints[2].x - _frustumPoints[0].x) * Mult.x, 
      (_frustumPoints[2].y - _frustumPoints[0].y)*Mult.y,
      1);
  }
}
