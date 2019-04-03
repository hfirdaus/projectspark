using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolarSystem : Singleton<SolarSystem>
{
	#region Fields

	public float OrbitScale = 3f;
	public float RadiusScale = 1f;
	public float TimeScale = 1f;
    public float planet_offset;

    private const float time_mod = 0.001f;
    private const float daysToSeconds = 60.0f * 60.0f * 24.0f;
	private const float hoursToSeconds = 60 * 60;
	private const float sun_diameter = 1392000;

	GameObject TheSun;
	List<Planet> Planets;
    List<OrbitVisualizer> orbits;
    public OrbitVisualizer ov;

	public bool Visible { get; private set; }

	public Material GhostMaterial;

	#endregion

	void Awake()
	{
		// Make sure everything is invisible!!!
		TheSun = transform.GetChild(0).gameObject;
		Planets = gameObject.GetComponentsInChildren<Planet>().ToList();
        orbits = new List<OrbitVisualizer>();
		Init(transform.position, transform.up);

	}

	void Update()
	{
        //if (!Visible)
        //    return;

        Planets.ForEach(p => UpdatePlanetRotation(p));
        
    }

    public void SetVisibility(bool visible)
	{
		if (Visible == visible)
			return;

		Visible = visible;

		Planets.ForEach(p => p.SetAlpha(visible ? 1f : 0f));

		// DO NOT FUCKING LEAVE THIS IN THE FINAL PROJECT
		TheSun.GetComponent<Renderer>().material.color = TheSun.GetComponent<Renderer>().material.color.WithAlpha(visible ? 1f : 0f);
	}

	/// <summary>
	/// Call from FlowManager when seeing the ground plane for the first time.
	/// </summary>
	/// <param name="pos">Positiion</param>
	/// <param name="up">Up direction</param>
	public void Init(Vector3 pos, Vector3 up)
	{
		transform.position = pos;
		transform.up = up;
        int i = 0;
		foreach (Planet planet in Planets)
		{
            print(i);
            i++;
            //planet.transform.position = new Vector3((Mathf.Log(planet.orbit_radius)) - (3f * OrbitScale + 1f), pos.y, 0);
            float xpos = Mathf.Log(planet.orbit_radius) * OrbitScale - planet.offset;
            if (xpos < 0)
            {
                xpos = 0;
            }
            planet.transform.position = new Vector3(xpos, pos.y, 0);
            //draw orbit
            //OrbitVisualizer t = Instantiate(ov);
            //t.transform.SetParent(TheSun.transform);
           // t.transform.position = Vector3.zero;
            //t.drawOrbit(planet.transform.position.x, t.transform.parent.position.y,  50);
            //orbits.Add(t);
           // planet.DrawOrbit();

            planet.transform.localScale = Vector3.one * (1f / Mathf.Log10(sun_diameter / planet.diameter)) * RadiusScale;
            float rand = Random.value * 359f;
			planet.transform.RotateAround(transform.position, Vector3.up, rand);
			planet.transform.Rotate(planet.transform.up, -rand);
            //print(planet.name + ":a " + planet.transform.position);


            planet.transform.Rotate(-Vector3.forward, planet.tilt);
            
        }
	}

	/// <summary>
	/// Updates a planet's rotation. Is based on WOAH
	/// </summary>
	/// <param name="planet">The planet. FUCK</param>
	public void UpdatePlanetRotation(Planet planet)
	{
		float rotation = daysToSeconds * TimeScale * (1f / planet.orbit_per);
		float planet_rotation = 360f * 60f * TimeScale;

		planet.transform.RotateAround(transform.position, transform.up, -rotation);

		planet.transform.RotateAround(planet.transform.position, planet.transform.up, planet_rotation / planet.rotation_per);
		//keep planet in same orientation
		planet.transform.RotateAround(planet.transform.position, Vector3.up, rotation);
	}

    public void IncrementTimeScale()
    {
        if (TimeScale + time_mod < 1){
            TimeScale += time_mod;
        }
        
    }
    public void DecrementTimeScale()
    {
        if (TimeScale - time_mod > 0){
            TimeScale -= time_mod;
        }
        else
        {
            TimeScale = 0;
        }
    }

    public void IncrementSize()
    {
        if (TimeScale + time_mod < 1)
        {
            TimeScale += time_mod;
        }

    }
    public void DecrementSize()
    {
        if (TimeScale - time_mod > 0)
        {
            TimeScale -= time_mod;
        }
        else
        {
            TimeScale = 0;
        }
    }



}
