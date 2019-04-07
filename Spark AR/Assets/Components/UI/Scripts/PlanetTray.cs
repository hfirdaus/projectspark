using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetTray : Singleton<PlanetTray>
{
	List<UIPlanetIcon> icons;
	public List<UIPlanetIcon> Icons => icons ?? (icons = GetComponentsInChildren<UIPlanetIcon>().ToList());

	public void PlanetCollected(PlanetName planet)
	{
		this[planet].SetCollected(true);
	}

	public bool IsCollected(PlanetName planet)
	{
		return this[planet].Collected;
	}

	public UIPlanetIcon this[PlanetName planet]
	{
		get => Icons.Where(i => i.PlanetName == planet).First();
	}
}
