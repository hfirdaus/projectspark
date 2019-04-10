using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetTray : Singleton<PlanetTray>
{
	List<UIPlanetIcon> icons;
	public List<UIPlanetIcon> Icons => icons ?? (icons = GetComponentsInChildren<UIPlanetIcon>().ToList());

	public event Action OnCollectedAllPlanets = new Action(() => { });

	public void PlanetCollected(PlanetName planet)
	{
		this[planet].SetCollected(true);

		if (CollectedAllPlanets())
			OnCollectedAllPlanets?.Invoke();
	}

	public bool IsCollected(PlanetName planet)
	{
		return this[planet].Collected;
	}

	public bool CollectedAllPlanets()
	{
		return icons.All(i => IsCollected(i.PlanetName));
	}

	public UIPlanetIcon this[PlanetName planet]
	{
		get => Icons.Where(i => i.PlanetName == planet).First();
	}
}
