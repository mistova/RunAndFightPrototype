using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    public static UIController Instance { get => instance; set => instance = value; }

    [SerializeField]
    Text guysCounterText;
    [SerializeField]
    Text GameOverText;
    [SerializeField]
    Slider slider;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        slider.value = 1;
    }

    internal void SetGuysCount(int guysCount)
    {
        guysCounterText.text = "Guys: " + guysCount;
        if (guysCount < 1)
            SetGameOver("Game Over");
    }

    internal void SetGameOver(string gameOver)
    {
        GameOverText.text = gameOver;
    }

    internal void SetSliderValue(float startingHealt, float currentHealth)
    {
        slider.value = currentHealth / startingHealt;
    }

    internal void SetSliderActive()
    {
        slider.gameObject.SetActive(true);
    }

}
