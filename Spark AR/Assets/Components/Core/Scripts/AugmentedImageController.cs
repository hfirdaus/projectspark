//-----------------------------------------------------------------------
// <copyright file="AugmentedImageExampleController.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
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

using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.Examples.AugmentedImage;

/// <summary>
/// Controller for AugmentedImage example.
/// </summary>
public class AugmentedImageController : MonoBehaviour
{
	public GameObject FitToScanOverlay;
	public TrackerManager trackerManager;
	public List<AugmentedImageVisualizer> prefabs;

	Dictionary<int, AugmentedImageVisualizer> m_Visualizers = new Dictionary<int, AugmentedImageVisualizer>();
	List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

	string currentPlanetName = "";

	public void Update()
	{
		// Check that motion tracking is tracking.
		if (Session.Status != SessionStatus.Tracking)
			return;

		// Get updated augmented images for this frame.
		Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);

		// Create visualizers and anchors for updated augmented images that are tracking and do not previously
		// have a visualizer. Remove visualizers for stopped images.
		foreach (AugmentedImage image in m_TempAugmentedImages)
		{
			AugmentedImageVisualizer visualizer = null;
			m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);

			if (image.TrackingState == TrackingState.Tracking && visualizer == null)
			{
				// Create an anchor to ensure that ARCore keeps tracking this augmented image.
				Anchor anchor = image.CreateAnchor(image.CenterPose);

				visualizer = (AugmentedImageVisualizer)Instantiate(prefabs[image.DatabaseIndex], anchor.transform);
				visualizer.transform.localScale = Vector3.one * 0.1f;

				//TO DO CHECK THE ROTATION!!
				visualizer.Image = image;
				m_Visualizers.Add(image.DatabaseIndex, visualizer);

				if (image.DatabaseIndex > 0)
				{
					trackerManager.PlanetTracked(image.Name);
					currentPlanetName = image.Name;
				}
			}
			else if (image.TrackingState == TrackingState.Tracking)
			{
				if (image.DatabaseIndex > 0)
				{
					if (m_Visualizers.ContainsKey(image.DatabaseIndex))
					{
						if (currentPlanetName != image.Name)
						{
							DestroyImmediate(m_Visualizers[image.DatabaseIndex].gameObject);
							m_Visualizers.Remove(image.DatabaseIndex);
							//trackerManager.PlanetTracked(image.Name);
							//currentPlanetName = image.Name;
						}
					}
				}
			}
			else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
			{
				m_Visualizers.Remove(image.DatabaseIndex);
				Destroy(visualizer.gameObject);
			}
		}

		// Show the fit-to-scan overlay if there are no images that are Tracking.
		foreach (var visualizer in m_Visualizers.Values)
		{
			if (visualizer.Image.TrackingState == TrackingState.Tracking)
			{
				FitToScanOverlay.SetActive(false);
				return;
			}
		}

		FitToScanOverlay.SetActive(true);
	}
}