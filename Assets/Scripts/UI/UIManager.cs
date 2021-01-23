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

    private GameManager _gameManager;

    [SerializeField]
    private Indicator _shieldIndicator;

    [SerializeField]
    private Indicator _ammoIndicator;
    [SerializeField]
    private Text _ammoLabel;


    void Start()
    {

        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _reloadLevelText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }

        _shieldIndicator.ShowIndicator(false);
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[Mathf.Clamp(currentLives, 0, 2)];
        if (currentLives <= 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _reloadLevelText.gameObject.SetActive(true);
        StartCoroutine(FlickerGameOverTextRoutine());
        _gameManager.GameOver();
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
}
