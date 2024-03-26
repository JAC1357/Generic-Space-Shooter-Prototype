using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _laserOffset = 1.05f;
    [SerializeField]
    private float _fireRate = .5f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    public int Lives
    {
        get { return _lives; }
    }
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private WaitForSeconds _tripleshotCoolDowRate = new WaitForSeconds(5);
    private bool _tripleShotCanFire;
    private IEnumerator _stopTripleShot;
    [SerializeField]
    private WaitForSeconds _speedCoolDowRate = new WaitForSeconds(5);
    [SerializeField]
    private bool _speedActive = false;
    [SerializeField]
    private bool _hasShield = false;
    [SerializeField]
    private GameObject _shieldvisualizer;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField] private GameObject[] _playerDamage;
    [SerializeField] private AudioSource _laserAudioSource;
    [SerializeField] private AudioSource _explosionSource;
    [SerializeField] private AudioSource _powerUpPickAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;

        _stopTripleShot = TripleshotPowerDownRoutine();

        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is null.");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The uiManger is empty.");
        }

        foreach (GameObject damage in _playerDamage)
        {
            damage.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        ShootLaser(_laserPrefab, _tripleShotPrefab);
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), transform.position.z);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
        }
    }

    void ShootLaser(GameObject laser, GameObject tripleShot)
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;

            if (_tripleShotActive == true)
            {
                Instantiate(tripleShot, this.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(laser, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
                //Instantiate(laser, transform.position, Quaternion.identity);
            }

            _laserAudioSource.Play();
        }
    }

    public void Damage()
    {
        if (_hasShield == true)
        {
            _hasShield = false;
            _shieldvisualizer.SetActive(false);
            return;
        }

        _lives -= 1;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _explosionSource.Play();
            _playerDamage[0].SetActive(true);
        }
        else if (_lives == 1)
        {
            _explosionSource.Play();
            _playerDamage[1].SetActive(true);
        }
        if (_lives < 1)
        {
            _explosionSource.Play();
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void ItemPickup(GameObject gameObject)
    {
        switch (gameObject.tag) 
        {
            case "Powerup":
                _tripleShotActive = true;
                break;
        }
    
    }

    public void TripleShotPickUp()
    {
        _powerUpPickAudioSource.Play();
        _tripleShotActive = true;
        Debug.Log("_tripleShotActive = true");
        //Only Works once: collct powerup for true, timer runs out for false, collect powerupagain for true, time is supposed to run out but stays true
        //StartCoroutine(_stopTripleShot);
        //this works as expected
        StartCoroutine(TripleshotPowerDownRoutine());
    }

    private IEnumerator TripleshotPowerDownRoutine()
    {
        yield return _tripleshotCoolDowRate;
        _tripleShotActive = false;
        Debug.Log("_tripleShotActive = false");
    }

    public void SpeedPickUp()
    {
        _powerUpPickAudioSource.Play();
        _speedActive = true;
        _speed = 8f;
        Debug.Log("_speedActive = true");
        StartCoroutine(SpeedPowerDownRoutine());
    }

    private IEnumerator SpeedPowerDownRoutine()
    {
        yield return _speedCoolDowRate;
        _speedActive = false;
        _speed = 5f;
    }

    public void ShieldPickUp()
    {
        _powerUpPickAudioSource.Play();
        _hasShield = true;
        _shieldvisualizer.SetActive(true);
    }

    public void Score(int points)
    {
        _score = _score + points;
        _uiManager.UpdateScore(_score);
    }
}
