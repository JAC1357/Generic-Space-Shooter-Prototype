using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _player.Lives == 0)
        {

            Debug.Log("pressed r");
            SceneManager.LoadScene("Game");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
