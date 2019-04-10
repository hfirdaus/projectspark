using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIPlanetRing : MonoBehaviour
{
	List<Image> planets = new List<Image>();
	List<float> planetSizes = new List<float>();

	CanvasGroup group;
	public CanvasGroup Group => group ?? (group = GetComponent<CanvasGroup>());

	void Awake()
	{
		for (int i = 0; i < 8; i++)
		{
			planets.Add(transform.GetChild(i).GetComponent<Image>());
			planetSizes.Add(planets[i].rectTransform.rect.width);
			//planets[i].color = PlanetInfo.Colors[(PlanetName)i];
		}
	}

	public void DoAnimation()
	{
		float time = 0.4f;
		float stagger = 0.08f;

		planets.ForEach(p => { p.rectTransform.SetSquareSize(0f); });

		for (int i = 0; i < planets.Count; i++)
		{
			RectTransform planet = planets[i].rectTransform;

			LeanTween.value(0f, planetSizes[i], time).setOnUpdate((float val) =>
			{
				planet.SetSquareSize(val);
			}).setEase(LeanTweenType.easeOutBack).setOvershoot(2f).setDelay(i * stagger);
		}

		LeanTween.delayedCall(stagger * 8f, () =>
		{
			ParticleManager.Instance.WinEffect.Play();
		});
	}

	public void SetVisibility(bool visible)
	{
		Group.alpha = visible ? 1f : 0f;
	}
}
