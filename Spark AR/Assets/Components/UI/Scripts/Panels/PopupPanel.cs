using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupPanel : PanelBase<PopupPanel>
{
	RectTransform rect;
	Text Title;
	Text Message;
	Graphic Accent;
	Action Callback;

	UIAnimatedPlanetIcon animIcon;
	UIPlanetRing planetWinRing;

	CanvasGroup wrongGroup;

	float timeGoal;

	void Start()
	{
		rect = transform.Find("Holder").GetComponent<RectTransform>();
		Title = transform.Find("Holder/Title").GetComponent<Text>();
		Message = transform.Find("Holder/Message").GetComponent<Text>();
		Accent = transform.Find("Holder/Accent").GetComponent<Graphic>();
		wrongGroup = transform.Find("Holder/WrongIcon").GetComponent<CanvasGroup>();
		animIcon = GetComponentInChildren<UIAnimatedPlanetIcon>();
		planetWinRing = GetComponentInChildren<UIPlanetRing>();
	}

	public void Show(string title, string message, Color color, Action callback = null, bool wrong = false)
	{
		Title.text = title;
		Message.text = message;
		Accent.color = color;
		Callback = callback;
		LayoutRebuilder.MarkLayoutForRebuild(rect);
		UIBlurControl.Instance.SetBlurVisibility(true);
		planetWinRing.SetVisibility(false);

		animIcon.SetVisibility(false);
		wrongGroup.alpha = wrong ? 1f : 0f;

		base.Show();
	}

	public void Show(string title, string message, Color color, PlanetName planet, Action callback = null, bool correct = false)
	{
		Title.text = title;
		Message.text = message;
		Accent.color = color;
		Callback = callback;
		LayoutRebuilder.MarkLayoutForRebuild(rect);
		UIBlurControl.Instance.SetBlurVisibility(true);
		planetWinRing.SetVisibility(false);

		if (correct)
		{
			wrongGroup.alpha = 0f;

			animIcon.SetVisibility(true);
			animIcon.InitIcon(planet);
		}
		else
		{
			wrongGroup.alpha = 1f;
			animIcon.SetVisibility(false);
		}

		base.Show();
	}

	public void ShowWin(string title, string message, Action callback = null)
	{
		Title.text = title;
		Message.text = message;
		Callback = callback;

		wrongGroup.alpha = 0f;
		animIcon.SetVisibility(false);
		UIBlurControl.Instance.SetBlurVisibility(true);
		Accent.color = Color.white;

		planetWinRing.SetVisibility(true);
		planetWinRing.DoAnimation();

		base.Show();
	}

	public override void SetVisibility(bool visible, float time = 0.25F, float delay = 0)
	{
		base.SetVisibility(visible, time, delay);

		baseTweens.Add(LeanTween.value(visible ? Screen.width / 4f : 10f, visible ? 10f : 300f, time).setOnUpdate((float val) =>
		{
			rect.offsetMax = Vector2.left * val;
			rect.offsetMin = Vector2.left * val;
		}).setEase(visible ? LeanTweenType.easeOutBack : LeanTweenType.easeInSine).setOvershoot(0.75f).uniqueId);

		timeGoal = Time.realtimeSinceStartup + 0.5f;
	}

	public override void Hide()
	{
		UIBlurControl.Instance.SetBlurVisibility(false);
		base.Hide();
		Callback?.Invoke();
	}

	void Update()
	{
		if (!Visible)
			return;

		if (Time.realtimeSinceStartup < timeGoal)
			return;

		if (Input.GetMouseButtonDown(0))
		{
			if (!RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
			{
				Hide();
			}
		}
	}
}
