using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Types {

  public enum ConstructionTypes {
    Building, Base, Barracks, VehicleFactory, Container, Mine, PowerStation, Radar, Runway, AirBase, Helipad, Wall,
    Defense, GunTurret, RocketTurrent, AA,
    Infantry, Assault, Gunner, Sniper, Engineer, Medic, Civilian,
    Vehicle, MobileBase, Jeep, Tank, LargeTank, SmallTank, RocketLauncher, APC, CargoTruck, BridgeBuilder, Harvester,
    Aircraft, Helicopter, AttackHelicopter, FighterPlane, Bomber,
    Ship, Warship, Boat, PatrolBoat, AttackBoat, Carrier};
  public enum GameState { Loading, Menu, Playing };
  public enum SelectionState { None, BuildAt, MoveTo, Attack }
  
}
