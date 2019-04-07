using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIPlanetIcon : MonoBehaviour, IPointerClickHandler
{
	public static event Action<PlanetName> OnClick = new Action<PlanetName>(p => { });

	public PlanetName PlanetName;

	Image image;
	float baseWidth;
	RectTransform rect;

	Sprite outline;
	public Sprite OutlineSprite => outline ?? (outline = GetOutlineSprite(PlanetName));

	Sprite collected;
	public Sprite CollectedSprite => collected ?? (outline = GetCollectedSprite(PlanetName));

	public static Sprite GetCollectedSprite(PlanetName planet)
	{
		return Resources.Load<Sprite>("Images/Planets/" + planet.ToString());
	}

	public static Sprite GetOutlineSprite(PlanetName planet)
	{
		return Resources.Load<Sprite>("Images/Planets/Outline" + planet.ToString());
	}

	public bool Collected { get; private set; }

	void Awake()
	{
		rect = GetComponent<RectTransform>();
		image = GetComponent<Image>();
		baseWidth = rect.rect.width;
	}

	public void SetCollected(bool collected)
	{
		Collected = collected;
		PlanetInfo.IsCollected[PlanetName] = collected;

		DoAnimation();
	}

	public void OnPointerClick(PointerEventData e)
	{
		OnClick?.Invoke(PlanetName);
	}

	public void DoAnimation()
	{
		image.color = Color.white;
		image.sprite = OutlineSprite;

		LeanTween.value(baseWidth, baseWidth - 10f, 0.3f).setOnUpdate((float val) =>
		{
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, val);
		}).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
		{
			image.sprite = CollectedSprite;
			image.color = PlanetInfo.Colors[PlanetName];

			LeanTween.value(baseWidth - 10f, baseWidth, 0.2f).setOnUpdate((float val) =>
			{
				rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, val);
			}).setEase(LeanTweenType.easeOutBack).setOvershoot(2f);
		}).setDelay(0.3f);
	}
}
