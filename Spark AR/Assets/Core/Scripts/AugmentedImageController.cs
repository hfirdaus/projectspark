﻿//-----------------------------------------------------------------------
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
        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
//        public AugmentedImageVisualizer AugmentedImageVisualizerPrefab;

        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;

        public TrackerManager trackerManager;

        private Dictionary<int, AugmentedImageVisualizer> m_Visualizers
            = new Dictionary<int, AugmentedImageVisualizer>();

        public List<AugmentedImageVisualizer> prefabs;

        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
//            Debug.Log("Prefabs size: " + prefabs.Count);
            //
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Check that motion tracking is tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            // Get updated augmented images for this frame.
            Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);

            // Create visualizers and anchors for updated augmented images that are tracking and do not previously
            // have a visualizer. Remove visualizers for stopped images.
            foreach (var image in m_TempAugmentedImages)
            {
                Debug.Log("Planet " + image.Name + image.DatabaseIndex);
                AugmentedImageVisualizer visualizer = null;
                m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
            Debug.Log("yo1");
            Debug.Log(image.TrackingState == TrackingState.Tracking);
            Debug.Log(visualizer == null);
                if (image.TrackingState == TrackingState.Tracking && visualizer == null)
                {
                // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                Debug.Log("yo6");

                visualizer = (AugmentedImageVisualizer)Instantiate(prefabs[image.DatabaseIndex], anchor.transform);
                visualizer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                Debug.Log("yo7");

                //TO DO CHECK THE ROTATION!!
                //                visualizer.transform.localRotation = new Vector3(0f, 0f, 90f);
                visualizer.Image = image;
                Debug.Log("yo8");
                m_Visualizers.Add(image.DatabaseIndex, visualizer);
                    if (image.DatabaseIndex > 0) // Not the solar system
                {
                    Debug.Log("y09");

                    trackerManager.PlanetTracked(image.Name, image.DatabaseIndex);
                    }
            }
                else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
                {
                Debug.Log("yo10");

                m_Visualizers.Remove(image.DatabaseIndex);
                    GameObject.Destroy(visualizer.gameObject);
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