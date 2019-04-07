using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PanelBase<T> : Singleton<T> where T : MonoBehaviour
{
	CanvasGroup m_group;
	public CanvasGroup Group => m_group ?? (m_group = GetComponent<CanvasGroup>());

	public bool Visible { get; protected set; } = true;

	protected List<int> baseTweens = new List<int>();

	protected virtual void Awake()
	{
		SetVisibility(false, 0f);
	}

	public virtual void Show(float delay = 0f)
	{
		SetVisibility(true, 0.2f, delay);
	}

	public virtual void Hide()
	{
		SetVisibility(false);
	}

	public virtual void SetVisibility(bool visible, float time = 0.2f, float delay = 0f)
	{
		if (Visible == visible)
			return;

		Visible = visible;

		CancelAnimation();

		baseTweens.Add(Group.LerpToAlpha(visible ? 1f : 0f, time, delay));
	}

	public void CancelAnimation()
	{
		baseTweens.ForEach(t => LeanTween.cancel(t));
		baseTweens.Clear();
	}
}
