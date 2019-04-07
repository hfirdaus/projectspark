using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class UIAnimatedPlanetIcon : MonoBehaviour
{
	public Image PlanetImage;
	public ProceduralImage Accent;

	Sprite outlineSprite;
	Sprite collectedSprite;

	CanvasGroup group;

	PlanetName currentPlanet;

	List<int> tweens = new List<int>();

	float imageSize = 150f;
	float imageAnimSize = 120f;

	public ParticleSystem effect;

	void Awake()
	{
		group = GetComponent<CanvasGroup>();
		SetVisibility(false);
	}

	public void SetVisibility(bool visible)
	{
		group.alpha = visible ? 1f : 0f;
	}

	public void InitIcon(PlanetName planet)
	{
		currentPlanet = planet;

		outlineSprite = UIPlanetIcon.GetOutlineSprite(planet);
		collectedSprite = UIPlanetIcon.GetCollectedSprite(planet);

		DoAnimation();
	}

	public void DoAnimation()
	{
		var col = effect.colorOverLifetime;
		col.enabled = true;

		Color planetColor = PlanetInfo.Colors[currentPlanet];

		Gradient grad = new Gradient();
		grad.SetKeys(new GradientColorKey[]
		{
			new GradientColorKey(planetColor, 0.5f), new GradientColorKey(planetColor.WithV(0.3f), 1f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });

		col.color = grad;

		LeanTween.delayedCall(0.55f, () => { effect.Play(); });

		PlanetImage.color = Color.white;
		PlanetImage.sprite = outlineSprite;

		Accent.color = Color.white;
		Accent.fillAmount = 0f;

		LeanTween.color(Accent.gameObject, planetColor, 0.4f).setOnUpdate((Color val) =>
		{
			Accent.color = val;
		}).setEase(LeanTweenType.easeInOutQuad).setFromColor(Color.white).setDelay(0.25f);

		LeanTween.value(0f, 1f, 0.55f).setOnUpdate((float val) =>
		{
			Accent.fillAmount = val;
		}).setEase(LeanTweenType.easeInOutQuint).setDelay(0.2f);

		LeanTween.value(imageSize, imageAnimSize, 0.4f).setOnUpdate((float val) =>
		{
			PlanetImage.rectTransform.SetSquareSize(val);
		}).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
		{
			PlanetImage.sprite = collectedSprite;
			PlanetImage.color = planetColor;

			LeanTween.value(imageAnimSize, imageSize, 0.2f).setOnUpdate((float val) =>
			{
				PlanetImage.rectTransform.SetSquareSize(val);
			}).setEase(LeanTweenType.easeOutBack).setOvershoot(2f);
		}).setDelay(0.1f);
	}
}
