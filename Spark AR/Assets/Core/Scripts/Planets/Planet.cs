using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// To be placed on a planet prefab, and instantiated by the planet manager.
/// </summary>
[RequireComponent(typeof(Collider), typeof(Renderer))]
public class Planet : MonoBehaviour
{
	#region Properties & Variables

	//public Sprite TooltipSprite;
	Material collectedMaterial;
	Material ghostMaterial => SolarSystem.Instance.GhostMaterial;
    OrbitVisualizer orb;

	public float diameter;
	public float tilt;
	public float orbit_radius;
	public float orbit_per;
	public float orbit_incl;
	public float rotation_per;
<<<<<<< HEAD:Spark AR/Assets/Core/Scripts/Planets/Planet.cs
    public float offset;


=======
    
>>>>>>> Hadiya:Spark AR/Assets/Core/Scripts/Planets/Planet.cs
    public string name;
    public bool isGhost = false;
    #endregion

    #region Fields

    Collider m_collider;
	Renderer m_renderer;

	public Collider Collider => m_collider ?? (m_collider = GetComponent<Collider>());
	public Renderer Renderer => m_renderer ?? (m_renderer = GetComponent<Renderer>());
    private LineRenderer l;

	#endregion

	void Awake()
	{
        l = GetComponent<LineRenderer>();
		collectedMaterial = Renderer.material;
	}

	public void SetGhost(bool isAGhost)
	{
        isGhost = isAGhost;
		// Animate the transition later
		Renderer.material = isGhost ? collectedMaterial : ghostMaterial;
	}
    
	public void SetAlpha(float alpha)
	{
		Renderer.material.color = Renderer.material.color.WithAlpha(alpha);
	}
}
