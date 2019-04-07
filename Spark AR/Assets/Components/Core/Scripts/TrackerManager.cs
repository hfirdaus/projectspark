using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerManager : Singleton<TrackerManager>
{
	public event Action<PlanetName> OnPlanetTracked = new Action<PlanetName>(p => { });

	public void PlanetTracked(string planetName)
	{
		if (Enum.TryParse(planetName, out PlanetName planet))
		{
			PlanetTracked(planet);
		}
		else
		{
			if (planetName == "Saturn")
				PlanetTracked(PlanetName.Saturn);
		}
	}

	public void PlanetTracked(PlanetName planet)
	{
		OnPlanetTracked?.Invoke(planet);
	}
}
