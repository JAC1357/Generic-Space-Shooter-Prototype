using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
