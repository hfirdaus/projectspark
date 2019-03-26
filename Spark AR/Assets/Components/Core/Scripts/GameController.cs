using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // State
    public bool isInfoVisible = true;
    public Planet planetPlayingFor = null; 
    public List<Planet> planets;
    public int planetsCollected
    {
        get { return planets.Count(p => p.isCollected); }
    }


    // UI Elements
    public Text scoreText;
    public GameObject questionDisplay;
    public GameObject infoDisplay;
    public List<GameObject> answerButtons;

    void Start()
    {

    }

    void Update()
    {

    }

    // when tag is scanned, show question for planet
    void ShowQuestion()
    {
        // update planetPlayingFor, show question element, hide score element
    }

    void OnAnswerClicked()
    {
        // if answer.isCorrect
        // mark UI element green
        // mark planet as collected
        // update score text

        // else
        // make answer red, then remove from UI
    }

    void OnInfoClick()
    {
        this.isInfoVisible = !this.isInfoVisible;
        infoDisplay.SetActive(this.isInfoVisible);
        // hide any other visible elements
    }

}
