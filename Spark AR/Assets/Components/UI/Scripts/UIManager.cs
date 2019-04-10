using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

	public event Action<PlanetName> OnPlanetCollected = new Action<PlanetName>(p => { });

	void Awake()
	{
		Application.targetFrameRate = 60;

		UIPlanetIcon.OnClick += UIPlanetIcon_OnClick;

		TriviaPanel.Instance.OnCorrectAnswerSelected += OnCorrectAnswerSelected;

		TrackerManager.Instance.OnPlanetTracked += PlanetScanned;

		PlanetTray.Instance.OnCollectedAllPlanets += DoWin;

		KeyboardTestManager.Instance.Numpad1 += () => { PlanetScanned((PlanetName)0); };
		KeyboardTestManager.Instance.Numpad2 += () => { PlanetScanned((PlanetName)1); };
		KeyboardTestManager.Instance.Numpad3 += () => { PlanetScanned((PlanetName)2); };
		KeyboardTestManager.Instance.Numpad4 += () => { PlanetScanned((PlanetName)3); };
		KeyboardTestManager.Instance.Numpad5 += () => { PlanetScanned((PlanetName)4); };
		KeyboardTestManager.Instance.Numpad6 += () => { PlanetScanned((PlanetName)5); };
		KeyboardTestManager.Instance.Numpad7 += () => { PlanetScanned((PlanetName)6); };
		KeyboardTestManager.Instance.Numpad8 += () => { PlanetScanned((PlanetName)7); };
	}

	void OnCorrectAnswerSelected(PlanetName planet)
	{
		PlanetTray.Instance.PlanetCollected(planet);
		OnPlanetCollected?.Invoke(planet);
	}

	public void DoWin()
	{
		LeanTween.delayedCall(0.2f, () =>
		{
			PopupPanel.Instance.ShowWin("Wow!", "You collected all those planets!\n\nGood job!.");
		});
	}

	void UIPlanetIcon_OnClick(PlanetName planet)
	{
		bool collected = PlanetTray.Instance[planet].Collected;
		string location = "Location:\n\n" + TriviaManager.Instance[planet].location_hint;

		PopupPanel.Instance.Show(planet.ToString(), collected ? PlanetInfo.Info[planet] : location, collected ? PlanetInfo.Colors[planet] : Color.grey);
	}

	public void PlanetScanned(PlanetName planet)
	{
		if (!PlanetTray.Instance[planet].Collected)
			TriviaPanel.Instance.Show(planet);

		else
			PopupPanel.Instance.Show(planet.ToString(), "You've already collected this planet!\nWow!", PlanetInfo.Colors[planet]);
	}
}
