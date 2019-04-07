using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerManager : MonoBehaviour
{
    public enum Planets
    {
        Earth=1,
        Jupiter,
        Mars,
        Mercury,
        Neptune,
        Saturn,
        Uranus,
        Venus,
    }

//    public static event PlanetTracket<PlanetName>()

    public void PlanetTracked(string planetName, int planetIndex)
    {
        // Call Game Controller to open question for that planet
        Debug.Log("I tracked " + planetName);
    }
}
