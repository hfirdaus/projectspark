﻿using System;
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

	Material collectedMaterial;
	Material ghostMaterial => SolarSystem.Instance.GhostMaterial;
    OrbitVisualizer orb;

	public float diameter;
	public float tilt;
	public float orbit_radius;
	public float orbit_per;
	public float orbit_incl;
	public float rotation_per;
    public float offset;

    public bool isGhost = false;

    #endregion

    #region Fields

    Collider m_collider;
	Renderer m_renderer;

	public Collider Collider => m_collider ?? (m_collider = GetComponent<Collider>());
	public Renderer Renderer => m_renderer ?? (m_renderer = GetComponent<Renderer>());

	#endregion

	void Awake()
	{
		collectedMaterial = Renderer.material;
        if (collectedMaterial.mainTexture == null)
        {
            Debug.Log("Main texture of collected material is null");
        }
	}

	public void SetGhost(bool isAGhost)
	{
        isGhost = isAGhost;
        
        Renderer.material = isGhost ? ghostMaterial : collectedMaterial;
	}
    
	public void SetAlpha(float alpha)
	{
		Renderer.material.color = Renderer.material.color.WithAlpha(alpha);
	}
}
