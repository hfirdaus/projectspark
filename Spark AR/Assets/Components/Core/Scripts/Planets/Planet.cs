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

    private const float daysToSeconds = 60.0f * 60.0f * 24.0f;
    private const float hoursToSeconds = 60 * 60;
    private const float sun_diameter = 1392000;
    public Transform star;
    public float diameter;
    public float tilt;

    public float orbit_radius;
    public float orbit_per;
    public float orbit_incl;
    public float rotation_per;

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

    public void UpdateRotation(float time_scale)
    {
        // 1 period = orbit_period
        //

        float rotation = daysToSeconds * time_scale * (1 / orbit_per);
        transform.RotateAround(star.transform.position, star.transform.up, rotation);

        float planet_rotation = 360 * 60 * time_scale;

        transform.RotateAround(transform.position, transform.up, planet_rotation / rotation_per);

        //keep planet in same orientation
        transform.RotateAround(transform.position, Vector3.up, -rotation);

    }


    public void Init(float orbit_scale, float radius_scale)
    {


        //set the orbit position
        float offset = 3 * orbit_scale + 1;
        float pos = orbit_scale * Mathf.Log(orbit_radius);
        transform.position = new Vector3(pos - offset, 0, 0);


        //set a random orbit position

        float rotation = UnityEngine.Random.Range(0, 360);
        transform.RotateAround(star.transform.position, star.transform.up, rotation);
        //keep tilt
        transform.RotateAround(transform.position, Vector3.up, -rotation);

        float scale = 1 / Mathf.Log10(sun_diameter / diameter);
        scale = scale * radius_scale;

        //float scale = 1 / Mathf.Log10(9.0f * (diameter / star.diameter));

        print(name + ": " + scale);
        transform.localScale = new Vector3(scale, scale, scale);

        transform.RotateAround(transform.position, -Vector3.forward, tilt);


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
