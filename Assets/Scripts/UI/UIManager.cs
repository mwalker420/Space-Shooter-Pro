﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _reloadLevelText;


    [SerializeField]
    private Indicator _shieldIndicator;

    [SerializeField]
    private Indicator _ammoIndicator;
    [SerializeField]
    private Text _ammoLabel;

    [SerializeField]
    private Indicator _thrusterIndicator;
    [SerializeField]
    private Text _thrusterLabel;

    [SerializeField]
    private Text _debugText;

    [SerializeField]
    private Text _instructionsText;

    private static UIManager _instance;
    public static UIManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }


    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _reloadLevelText.gameObject.SetActive(false);
        _shieldIndicator.ShowIndicator(false);
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[Mathf.Clamp(currentLives, 0, 3)];
        if (currentLives <= 0)
        {
            GameOverSequence();
        }
    }

    public void GameWinSequence()
    {
        _gameOverText.text = "Mission Accomplished";
        GameOverSequence();
    }
    public void GameLoseSequence()
    {
        _gameOverText.text = "Game Over";
        GameOverSequence();
    }
    private void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _reloadLevelText.gameObject.SetActive(true);
        StartCoroutine(FlickerGameOverTextRoutine());
    }

    IEnumerator FlickerGameOverTextRoutine()
    {
        bool show = true;
        while (true)
        {
            _gameOverText.gameObject.SetActive(show);
            show = !show;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SetShieldStrength(float strength)
    {
        _shieldIndicator.Percentage = strength;
    }

    public void ShowShieldIndicator(bool show)
    {
        _shieldIndicator.ShowIndicator(show);
    }

    public void SetAmmoIndicator(int current, int max)
    {
        _ammoLabel.text = "Ammo: " + current + "/" + max;
        _ammoIndicator.Percentage = (float)current / max;

    }

    public void SetThrusterIndicator(int current, int max)
    {
        int percentage = (int)((float)current / max * 100);
        _thrusterLabel.text = String.Format("Thruster: {0}%", percentage);
        _thrusterIndicator.Percentage = (float)current / max;
    }

    public void SetDebugText(string text)
    {
        _debugText.text = text;
    }

    public void ShowInstructions(bool show)
    {
        _instructionsText.gameObject.SetActive(show);
    }


}
