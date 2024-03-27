﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    //[SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Text _GameOverText;
    [SerializeField] private Text _restartGameText;
    [SerializeField] Image _livesImg;
    [SerializeField] Sprite[] _liveSprites;
    [SerializeField] private WaitForSeconds _flickerTime = new WaitForSeconds(.5f);
    private IEnumerator _gameOverFlicker;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _restartGameText.gameObject.SetActive(false);
        _GameOverText.gameObject.SetActive(false);
        _gameOverFlicker = GameOverFlicker();

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int playerLife) 
    {
        _livesImg.sprite = _liveSprites[playerLife];
        if (playerLife == 0)
        {
            _GameOverText.gameObject.SetActive(true);
            StartCoroutine(_gameOverFlicker);
            _restartGameText.gameObject.SetActive(true);

            
        }
        else if (playerLife > 0)
        {
            StopCoroutine(_gameOverFlicker);
            _GameOverText.gameObject.SetActive(false);
        }
    }

    IEnumerator GameOverFlicker()
    {
        while (true)//playerLife <= 0)
        {
            /*
            _GameOverText.color = Color.white;
            yield return _flickerTime;
            _GameOverText.color = Color.black;
            */
            _GameOverText.gameObject.SetActive(true);
            yield return _flickerTime;
            _GameOverText.gameObject.SetActive(false);
            yield return _flickerTime;
        }
    }
}
