using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UIAnswerElement : MonoBehaviour, IPointerClickHandler
{
	public event Action<int> OnAnswerClick = new Action<int>(i => { });
	public Text Text;

	RectTransform bgRect;
	public RectTransform BGRect => bgRect ?? (bgRect = transform.Find("BG").GetComponent<RectTransform>());

	Graphic accent;
	public Graphic Accent => accent ?? (accent = BGRect.Find("Accent").GetComponent<Graphic>());

	public bool Visible { get; private set; } = true;

	List<int> tweens = new List<int>();

	CanvasGroup m_group;
	public CanvasGroup Group => m_group ?? (m_group = GetComponent<CanvasGroup>());

	RectTransform rect;
	public RectTransform Rect => rect ?? (rect = GetComponent<RectTransform>());

	void Awake()
	{
		SetInfo("", Color.white);
		//SetVisibility(false, 0f);
	}

	public void SetInfo(string text, Color accent)
	{
		Text.text = text;
		Accent.color = accent;
	}

	public void SetVisibility(bool visible, float time = 0.2f, float delay = 0f)
	{
		if (Visible == visible)
			return;

		Visible = visible;

		tweens.StopTweens();

		float textDelay = visible ? time + delay : delay;
		float bgDelay = visible ? delay : time + delay;

		tweens.Add(LeanTween.value(Text.color.a, visible ? 1f : 0f, time).setOnUpdate((float val) =>
		{
			Text.color = Text.color.WithAlpha(val);
		}).setEase(LeanTweenType.easeInOutSine).setDelay(textDelay).uniqueId);

		tweens.Add(Group.LerpToAlpha(visible ? 1f : 0f, time / 2f, bgDelay));

		float xOffset = visible ? 0f : -(Screen.width - 110);
		float xStart = BGRect.offsetMax.x;

		tweens.Add(LeanTween.value(xStart, xOffset, time).setOnUpdate((float val) =>
		{
			BGRect.offsetMax = Vector2.right * val;
		}).setDelay(bgDelay).setEase(visible ? LeanTweenType.easeOutBack : LeanTweenType.easeInOutSine).setOvershoot(0.3f).uniqueId);
	}

	public void OnPointerClick(PointerEventData e)
	{
		OnAnswerClick?.Invoke(transform.GetSiblingIndex());
	}
}
