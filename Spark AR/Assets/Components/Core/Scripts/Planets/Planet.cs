using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// To be placed on a planet prefab, and instantiated by the planet manager.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Planet : MonoBehaviour
{
	#region Events

	public static event Action<Planet> PlanetSelected = new Action<Planet>((p) => { });

	#endregion

	#region Properties & Variables

	Collider m_collider;
	public PlanetData Data;
	public Sprite TooltipSprite;

	#endregion

	#region Fields

	public Collider Collider => m_collider ?? (m_collider = GetComponent<Collider>());
	public string Name => Data?.Name ?? "";
	public int PlanetIndex => Data?.PlanetIndex ?? -1;
	public List<string> Facts => Data?.Facts;

	#endregion

	public Planet(PlanetData data)
	{
		// Clone the data instead of using it outright. 
		// May prevent some weird behaviour later that I really don't want to debug.
		Data = new PlanetData(data);
	}

	public void Select()
	{
		PlanetSelected(this);
	}

	void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		PlanetSelected(this);
	}
}

[Serializable]
public class PlanetData
{
	[Tooltip("Name of the planet. Duh.")]
	public string Name;

	[Tooltip("Planet position from the sun (Starts at 0).")]
	public int PlanetIndex;

	[Tooltip("The facts to be used in the trivia portion of the app.")]
	public List<string> Facts;

	public PlanetData(string name, int index, List<string> facts)
	{
		Name = name;
		PlanetIndex = index;
		Facts = facts;
	}

	/// <summary>
	/// Clone from exiting data.
	/// </summary>
	/// <param name="cloneFrom">The data to clone from.</param>
	public PlanetData(PlanetData cloneFrom)
	{
		Name = "" + cloneFrom.Name;
		PlanetIndex = cloneFrom.PlanetIndex;
		Facts = cloneFrom.Facts.ToList();
	}
}
