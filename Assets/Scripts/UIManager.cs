using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    //[SerializeField] private TMP_Text _scoreText;
    [SerializeField] Image _livesImg;
    [SerializeField] Sprite[] _liveSprites;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int playerLife) 
    {
        _livesImg.sprite = _liveSprites[playerLife];
    }
}
