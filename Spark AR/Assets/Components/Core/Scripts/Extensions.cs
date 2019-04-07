using ASMaterialIcon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static partial class Extensions
{
	public static float[] ToHSV(this Color color)
	{
		float h;
		float s;
		float v;
		Color.RGBToHSV(color, out h, out s, out v);
		return new float[] { h, s, v };
	}

	public static Color WithV(this Color color, float value)
	{
		float[] hsv = color.ToHSV();
		return Color.HSVToRGB(hsv[0], hsv[1], value);
	}

	public static void SetSquareSize(this RectTransform rect, float size)
	{
		rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
		rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
	}

	public static Color WithAlpha(this Color color, float alpha)
	{
		return new Color(color.r, color.g, color.b, alpha);
	}

	public static void DestroyChildren<T>(this T parent, float delay = 0f) where T : Component
	{
		for (int i = parent.transform.childCount - 1; i >= 0; i--)
			UnityEngine.Object.Destroy(parent.transform.GetChild(i).gameObject, delay);
	}

	public static List<int> StaggeredLerp(this Transform parent, float alpha, float time, float step, float delay = 0f, bool floor = false, bool useLayout = false)
	{
		List<int> t = new List<int>();

		for (int i = 0; i < parent.childCount; i++)
		{
			t.AddRange(parent.GetChild(i).StaggerElement(i, alpha, time, step, delay, floor, useLayout));
		}

		return t;
	}

	public static List<int> StaggerElement(this Transform element, int index, float alpha, float time, float step, float delay, bool floor = false, bool useLayout = false)
	{
		List<int> tweens = new List<int>();
		CanvasGroup group = element.GetComponent<CanvasGroup>();
		LayoutElement le = element.GetComponent<LayoutElement>();

		if (group)
		{
			group.blocksRaycasts = false;

			tweens.Add(LeanTween.value((floor) ? 0f : group.alpha, alpha, time).setOnUpdate((float val) =>
			{
				group.alpha = val;
			}).setEase(LeanTweenType.easeInOutSine).setDelay(delay + (index * step)).setOnComplete(() =>
			{
				if (alpha > 0.01f)
					group.blocksRaycasts = true;
			}).uniqueId);
		}
		if (le && useLayout)
		{
			if (floor)
			{
				le.preferredHeight = 0f;
				le.preferredWidth = 0f;
			}
			tweens.Add(LeanTween.value(le.preferredHeight, alpha * 50f, time).setOnUpdate((float val) =>
			{
				le.preferredHeight = val;
				le.preferredWidth = val;
			}).setEase((alpha > 0.01f) ? LeanTweenType.easeOutBack : LeanTweenType.easeInOutSine).setDelay(delay + (index * step)).uniqueId);
		}

		return tweens;
	}

	#region Animation

	public static int LerpToAlpha(this CanvasGroup group, float alpha, float time)
	{
		return LerpToAlpha(group, alpha, time, 0f, () => { });
	}

	public static int LerpToAlpha(this CanvasGroup group, float alpha, float time, float delay)
	{
		return LerpToAlpha(group, alpha, time, delay, () => { });
	}

	public static int LerpToAlpha(this CanvasGroup group, float alpha, float time, float delay, Action callback)
	{
		if (alpha < 0.99f)
			group.blocksRaycasts = false;
		else
			group.blocksRaycasts = true;

		return LeanTween.value(group.alpha, alpha, time).setOnUpdate((float val) =>
		{
			group.alpha = val;
		}).setEase(LeanTweenType.easeInOutSine).setOnComplete(callback).setDelay(delay).uniqueId;
	}

	public static int LerpRectToWidth(this RectTransform rect, float width, float time, float delay = 0f)
	{
		return LeanTween.value(rect.rect.width, width, time).setOnUpdate((float val) =>
		{
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, val);
		}).setEase(LeanTweenType.easeInOutSine).setDelay(delay).uniqueId;
	}

	public static int LerpRectToHeight(this RectTransform rect, float height, float time, float delay = 0f)
	{
		return LeanTween.value(rect.rect.height, height, time).setOnUpdate((float val) =>
		{
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, val);
		}).setEase(LeanTweenType.easeInOutSine).setDelay(delay).uniqueId;
	}

	public static void AddToList(this LTDescr tween, List<int> tweens)
	{
		tweens.Add(tween.uniqueId);
	}

	#endregion

	public static void StopTweens(this List<int> tweens)
	{
		tweens.ForEach(t => LeanTween.cancel(t));
		tweens.Clear();
	}

}