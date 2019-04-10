using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemPlanetManager : MonoBehaviour
{
    public static Dictionary<PlanetName, bool> RenderedPlanets = new Dictionary<PlanetName, bool>()
    {
        { PlanetName.Mercury, false },
        { PlanetName.Venus,   false },
        { PlanetName.Earth,   false },
        { PlanetName.Mars,    false },
        { PlanetName.Jupiter, false },
        { PlanetName.Saturn,  false },
        { PlanetName.Uranus,  false },
        { PlanetName.Neptune, false }
    };
    
    void Awake()
    {
        UIManager.Instance.OnPlanetCollected += AddToSolarSystem;
    }

    public void AddToSolarSystem(PlanetName planet)
    {
        Debug.Log("Add a planet to solar system");
        RenderedPlanets[planet] = true;
    }
}
