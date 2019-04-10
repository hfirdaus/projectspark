using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class TriviaPanel : Singleton<TriviaPanel>
{
	public event Action<PlanetName> OnCorrectAnswerSelected = new Action<PlanetName>(p => { });

	public CanvasGroup QuestionGroup;
	Text questionText;

	public CanvasGroup TitleGroup;
	Text titleText;
	Graphic titleAccent;

	List<UIAnswerElement> answerList = new List<UIAnswerElement>();
	List<int> tweens = new List<int>();

	public bool Visible { get; private set; } = true;

	PlanetName currentPlanet;
	PlanetTrivia currentTrivia;

	void Awake()
	{
		answerList = GetComponentsInChildren<UIAnswerElement>().ToList();

		questionText = QuestionGroup.GetComponentInChildren<Text>();
		titleText = TitleGroup.GetComponentInChildren<Text>();
		titleAccent = TitleGroup.transform.Find("Accent").GetComponent<Graphic>();

		answerList.ForEach(a => a.OnAnswerClick += OnAnswerClick);

		SetVisibility(false, 0f);
	}

	void OnAnswerClick(int answer)
	{
		if (currentTrivia.correct_index == answer)
		{
			PopupPanel.Instance.Show("Correct!", currentTrivia.responses[answer], PlanetInfo.Colors[currentPlanet], currentPlanet, () =>
			{
				OnCorrectAnswerSelected?.Invoke(currentPlanet);
				Hide();
			}, true);
		}
		else
		{
			PopupPanel.Instance.Show("Whoops!", TriviaManager.Instance[currentPlanet].responses[answer], Color.grey, null, true);
		}
	}

	public void Show(PlanetName planet)
	{
		currentPlanet = planet;
		currentTrivia = TriviaManager.Instance[planet];

		questionText.text = currentTrivia.question;
		titleText.text = planet.ToString();
		titleAccent.color = PlanetInfo.Colors[planet];

		for (int i = 0; i < answerList.Count; i++)
		{
			UIAnswerElement element = answerList[i];
			element.SetInfo(currentTrivia.answers[i], PlanetInfo.Colors[planet]);
		}

		SetVisibility(true);
	}

	public void Hide()
	{
		for (int i = 0; i < answerList.Count; i++)
		{
			UIAnswerElement element = answerList[i];
		}

		SetVisibility(false);
	}

	public void SetVisibility(bool visible, float time = 0.2f, float delay = 0f)
	{
		if (Visible == visible)
			return;

		Visible = visible;

		tweens.StopTweens();

		float stagger = (!visible && time < 0.01f) ? 0f : 0.1f;

		tweens.Add(QuestionGroup.LerpToAlpha(visible ? 1f : 0f, time, visible ? time + delay : delay));
		tweens.Add(TitleGroup.LerpToAlpha(visible ? 1f : 0f, time, !visible ? time + delay : delay));

		for (int i = 0; i < answerList.Count; i++)
		{
			UIAnswerElement element = answerList[i];
			element.SetVisibility(visible, time, i * stagger);
		}

	}
}
