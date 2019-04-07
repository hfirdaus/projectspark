//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Modified version
/// 2019-03-25
/// 
/// - Used for placing a solar system
/// -> First the user points the camera towards the ground.
/// -> The user then touches where they would like to place the system.
/// -> The system will then be anchoired to that point
/// 
/// -- Possible Future improvements:
/// -> live re-scaling of solar system?
/// -> 
/// </summary>

using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;

#if UNITY_EDITOR
// Set up touch input propagation while using Instant Preview in the editor.
using Input = GoogleARCore.InstantPreviewInput;
#endif

/// <summary>
/// Controls the HelloAR example.
/// </summary>
public class HelloARController : MonoBehaviour
{
	public DetectedPlaneGenerator dp;

	bool placed = false;

	/// <summary>
	/// The first-person camera being used to render the passthrough camera image (i.e. AR background).
	/// </summary>
	public Camera FirstPersonCamera;

	/// <summary>
	/// A prefab for tracking and visualizing detected planes.
	/// </summary>
	public GameObject DetectedPlanePrefab;

	/// <summary>
	/// A model to place when a raycast from a user touch hits a plane.
	/// </summary>
	public GameObject solarSystemPrefab;

	/// <summary>
	/// A model to place when a raycast from a user touch hits a feature point.
	/// </summary>
	public GameObject SearchingForPlaneUI;

	GameObject SolarInstance;

	/// <summary>
	/// The rotation in degrees need to apply to model when the Andy model is placed.
	/// </summary>
	const float k_ModelRotation = 90.0f;
	const float max_scale = 10f;
	const float min_scale = 0.1f;
	const float scale_mod = 0.1f;

	/// <summary>
	/// A list to hold all planes ARCore is tracking in the current frame. This object is used across
	/// the application to avoid per-frame allocations.
	/// </summary>
	List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

	/// <summary>
	/// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
	/// </summary>
	bool m_IsQuitting = false;

	/// <summary>
	/// The Unity Update() method.
	/// </summary>
	public void Update()
	{
		_UpdateApplicationLifecycle();

		// Hide snackbar when currently tracking at least one plane.
		Session.GetTrackables<DetectedPlane>(m_AllPlanes);
		bool showSearchingUI = true;

		for (int i = 0; i < m_AllPlanes.Count; i++)
		{
			if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
			{
				showSearchingUI = false;
				break;
			}
		}

		// For our purposes, we don't want the system to be replaced every update,
		// we want to make sure it just places it and stays where it is
		if (placed)
			return;

		SearchingForPlaneUI.SetActive(showSearchingUI);

		// If the player has not touched the screen, we are done with this update.
		Touch touch;
		if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
			return;

		// Raycast against the location the player touched to search for planes.
		TrackableHit hit;
		TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
			TrackableHitFlags.FeaturePointWithSurfaceNormal;

		if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
		{
			// Use hit pose and camera pose to check if hittest is from the
			// back of the plane, if it is, no need to create the anchor.
			if ((hit.Trackable is DetectedPlane) &&
				Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
					hit.Pose.rotation * Vector3.up) < 0)
			{
				Debug.Log("Hit at back of the current DetectedPlane");
			}
			else
			{
				GameObject prefab;
				if (hit.Trackable is FeaturePoint)
				{
					return;
				}
				else
				{
					prefab = solarSystemPrefab;
					placed = true;
					dp.Disable();
				}

				// Instantiate Andy model at the hit pose.
				var solarArray = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);

				// Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
				solarArray.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

				// Create an anchor to allow ARCore to track the hitpoint as understanding of the physical world evolves.
				var anchor = hit.Trackable.CreateAnchor(hit.Pose);

				// Make system model a child of the anchor.
				solarArray.transform.parent = anchor.transform;

				SolarInstance = solarArray;
			}
		}
	}

	public void IncTS()
	{
		if (SolarInstance != null)
			SolarInstance.GetComponentInChildren<SolarSystem>().IncrementTimeScale();
	}

	public void DecTS()
	{
		if (SolarInstance != null)
			SolarInstance.GetComponentInChildren<SolarSystem>().DecrementTimeScale();
	}

	public void IncrScale()
	{
		if (SolarInstance != null && SolarInstance.transform.localScale.x + scale_mod <= max_scale)
		{
			SolarSystem s = SolarInstance.GetComponentInChildren<SolarSystem>();
			Planet[] plan = s.GetComponentsInChildren<Planet>();
			foreach (Planet p in plan)
			{
				s.transform.localScale += Vector3.one * scale_mod;
				s.transform.localPosition += Vector3.one * scale_mod;
			}
		}
	}
	public void DecrScale()
	{
		if (SolarInstance != null && SolarInstance.transform.localScale.x - scale_mod > min_scale)
		{
			SolarSystem s = SolarInstance.GetComponentInChildren<SolarSystem>();
			Planet[] plan = s.GetComponentsInChildren<Planet>();
			foreach (Planet p in plan)
			{
				s.transform.localScale -= Vector3.one * scale_mod;
				s.transform.localPosition -= Vector3.one * scale_mod;
			}
		}
	}

	/// <summary>
	/// Check and update the application lifecycle.
	/// </summary>
	void _UpdateApplicationLifecycle()
	{
		// Only allow the screen to sleep when not tracking.
		if (Session.Status != SessionStatus.Tracking)
		{
			const int lostTrackingSleepTimeout = 15;
			Screen.sleepTimeout = lostTrackingSleepTimeout;
		}
		else
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

		if (m_IsQuitting)
			return;

		// Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
		if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
		{
			_ShowAndroidToastMessage("Camera permission is needed to run this application.");
			m_IsQuitting = true;
			Invoke("_DoQuit", 0.5f);
		}
		else if (Session.Status.IsError())
		{
			_ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
			m_IsQuitting = true;
			Invoke("_DoQuit", 0.5f);
		}
	}

	/// <summary>
	/// Actually quit the application.
	/// </summary>
	void _DoQuit()
	{
		Application.Quit();
	}

	/// <summary>
	/// Show an Android toast message.
	/// </summary>
	/// <param name="message">Message string to show in the toast.</param>
	void _ShowAndroidToastMessage(string message)
	{
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		if (unityActivity != null)
		{
			AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
			unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
			{
				AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
					message, 0);
				toastObject.Call("show");
			}));
		}
	}
}
