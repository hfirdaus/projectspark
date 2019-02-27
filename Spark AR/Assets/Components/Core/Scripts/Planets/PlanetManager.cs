using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages planets in the scene.
/// </summary>
public class PlanetManager : Singleton<PlanetManager>
{
	public List<GameObject> PlanetPrefabs;

	void Start()
	{
		Planet.PlanetSelected += PlanetSelected;
	}

	public void PlanetSelected(Planet planet)
	{
		Debug.Log("Planet " + planet.Name + " selected.");

		// Do something with the planet here.


	}
}
