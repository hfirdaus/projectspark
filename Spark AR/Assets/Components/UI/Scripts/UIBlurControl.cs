using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlurControl : Singleton<UIBlurControl>
{
	public Image blur1;
	public Image blur2;

	string blurShaderValue = "_blurSizeXY";

	public bool Visible { get; private set; }

	public float Blur => blur1?.material.GetFloat(blurShaderValue) / blurMultiplier ?? 0f;

	float blurMultiplier = 3f;

	[HideInInspector]
	public bool TemporaryBlurBypass;

	[Space]
	public bool DisableBlurShaders = false;

	int blurTween;

	void Start()
	{
		if (DisableBlurShaders)
		{
			Destroy(blur1.gameObject);
			Destroy(blur2.gameObject);
		}
		setBlur(0f);
	}

	public void SetBlurVisibility(bool visible, float time = 0.2f, float delay = 0f)
	{
		if (Visible == visible)
			return;

		if (TemporaryBlurBypass)
		{
			TemporaryBlurBypass = false;
			return;
		}

		Visible = visible;

		LeanTween.cancel(blurTween);

		float blur = Visible ? 1f : 0f;
		float start = Blur;

		blurTween = LeanTween.value(Blur, blur, time).setOnUpdate((float val) =>
		{
			setBlur(val);
		}).setEase(LeanTweenType.easeInOutSine).setDelay(delay).uniqueId;
	}

	void setBlur(float blur)
	{
		bool enabled = blur > 0.0001f;

		if (!DisableBlurShaders)
		{
			blur1.enabled = blur2.enabled = enabled;
			blur1?.material.SetFloat(blurShaderValue, blur * blurMultiplier);
			blur2?.material.SetFloat(blurShaderValue, blur * blurMultiplier);
		}
	}

}
