using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriviaManager : Singleton<TriviaManager>
{
	string triviaJson => Resources.Load<TextAsset>("Data/trivia").text;

	List<PlanetTrivia> m_questions;
	public List<PlanetTrivia> Questions => m_questions ?? (m_questions = JsonUtility.FromJson<TriviaHolder>(triviaJson).trivia);

	public PlanetTrivia this[string body]
	{
		get => Questions.Where(q => q.celestial_body == body).FirstOrDefault();
	}

	public PlanetTrivia this[PlanetName body]
	{
		get => Questions.Where(q => q.celestial_body == body.ToString()).FirstOrDefault();
	}
}

[Serializable]
public class TriviaHolder
{
	public List<PlanetTrivia> trivia = new List<PlanetTrivia>();
}

[Serializable]
public class PlanetTrivia
{
	public string celestial_body;
	public string question;
	public string[] answers;
	public string[] responses;
	public int correct_index;
	public string location;
	public string location_hint;
}
