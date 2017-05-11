using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WW
{

  public class PowerMan : MonoBehaviour
  {

    public GameObject HealthBar;
    public Team MyTeam;
    public float TotalPowerRequired = 0;
    public float TotalPowerAvailable = 0;


    void CalculatePower()
    {
     // Debug.Log("CalculatePower");
      PowerStation[] pa = MyTeam.GetComponentsInChildren<PowerStation>();
      TotalPowerAvailable = 0;
      foreach (PowerStation p in pa)
      {
        TotalPowerAvailable += p.Power;
      }

      Building[] ba = MyTeam.GetComponentsInChildren<Building>();
      TotalPowerRequired = 0;
      foreach (Building b in ba)
      {
        TotalPowerRequired += b.Data.PowerRequired;
      }

      Image m = HealthBar.GetComponent<Image>();
      m.fillAmount = (float)TotalPowerAvailable / (float)TotalPowerRequired;
    }

    // Use this for initialization
    void Start()
    {
      MyTeam = GetComponent<Game>().MyTeam;
      InvokeRepeating("CalculatePower", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
  }


}