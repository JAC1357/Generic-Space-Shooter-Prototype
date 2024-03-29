﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _laserOffset = 1.05f;
    [SerializeField] private float _fireRate = .5f;
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField] private bool _tripleShotActive = false;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private WaitForSeconds _tripleshotCoolDowRate = new WaitForSeconds(5);
    private bool _tripleShotCanFire;
    private IEnumerator _stopTripleShot;
    [SerializeField] private WaitForSeconds _speedCoolDowRate = new WaitForSeconds(5);
    [SerializeField] private bool _speedActive = false;
    [SerializeField] private bool _hasShield = false;
    [SerializeField] private GameObject _shieldvisualizer;
    [SerializeField] private int _score;
    private UIManager _uiManager;
    [SerializeField] private GameObject[] _playerDamage;
    [SerializeField] private AudioSource _laserAudioSource;
    [SerializeField] private AudioSource _explosionSource;
    [SerializeField] private AudioSource _powerUpPickAudioSource;
    [SerializeField] private int _shieldCount;
    [SerializeField] private int _ammoCount = 5;
    private GameObject _camera;
    private Coroutine _cameraShake;
    [SerializeField] private bool _hasRapidFire = false;
    [SerializeField] private bool _hasThruster = false;
    [SerializeField] private float _thrusterAmmount = 0;
    [SerializeField] private int _ThrusterLimit = 100;
    [SerializeField] private bool _leftShiftPressed = false;
    

    public int Lives
    {
        get { return _lives; }
    }

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
        _uiManager.UpdateAmmo(_ammoCount);

        _camera = GameObject.Find("Main Camera");
        if (_camera == null)
        {
            Debug.LogError("The _camera is empty.");
        }

        foreach (GameObject damage in _playerDamage)
        {
            damage.SetActive(false);
        }

        //_thrusterCoroutine = StartCoroutine(Thruster());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _thrusterAmmount > 0)
        {
            _leftShiftPressed = true;
            StartCoroutine(Thruster());
            _speed = 8;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _leftShiftPressed = false;
            StopCoroutine(Thruster());
            _speed = 5;
        }
        CalculateMovement();

        ShootLaser();
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

    void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount > 0)
        {
            if (_ammoCount > 0)
            {
                _ammoCount--;
                if (_hasRapidFire == true)
                {
                    _canFire = -1;
                    _ammoCount++;
                }
                else
                {
                    _canFire = Time.time + _fireRate;
                }
                
                if (_tripleShotActive == true)
                {
                    Instantiate(_tripleShotPrefab, this.transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
                }
                _laserAudioSource.Play();
                _uiManager.UpdateAmmo(_ammoCount);
                
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount == 0)
        {
            _uiManager.UpdateAmmo(_ammoCount);
        }
    }

    public void Damage()
    {
        if (_hasShield == true)
        {
            if (_shieldCount == 3)
            {
                _shieldvisualizer.GetComponent<SpriteRenderer>().color = Color.yellow;
                _shieldCount -= 1;
                return;
            }
            else if (_shieldCount == 2)
            {
                _shieldCount -= 1;
                _shieldvisualizer.GetComponent<SpriteRenderer>().color = Color.gray;
                return;
            }
            else if (_shieldCount == 1)
            {
                _shieldCount -= 1;
                _hasShield = false;
                _shieldvisualizer.SetActive(false);
                return;
            }   
        }

        _lives -= 1;
        _cameraShake = StartCoroutine(_camera.GetComponent<Camera>().Shake());
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

    public void TripleShotPickUp()
    {
        _powerUpPickAudioSource.Play();
        _tripleShotActive = true;
        Debug.Log("_tripleShotActive = true");
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
        _shieldCount = 3;
        _powerUpPickAudioSource.Play();
        _hasShield = true;
        _shieldvisualizer.SetActive(true);
        _shieldvisualizer.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Score(int points)
    {
        _score = _score + points;
        _uiManager.UpdateScore(_score);
    }

    public void AmmoPickUp(int ammoPickedup)
    {
        _powerUpPickAudioSource.Play();
        _ammoCount += ammoPickedup;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void HealthPickUp(int livesPickedUp)
    {
        _powerUpPickAudioSource.Play();
        if (_lives < 3)
        {
            _lives += livesPickedUp;
            if (_playerDamage[1].activeSelf == true)
            {
                _playerDamage[1].SetActive(false);
            }
            else if (_playerDamage[0].activeSelf == true)
            {
                _playerDamage[0].SetActive(false);
            }
            _uiManager.UpdateLives(_lives);
        }
    }

    public void RapidFirePickUp()
    {
        _powerUpPickAudioSource.Play();
        _hasRapidFire = true;
        StartCoroutine(RapidFire());
    }
    IEnumerator RapidFire()
    {
        yield return _speedCoolDowRate;
        _hasRapidFire = false;
    }

    public void ThrusterPickUp()
    {
        _powerUpPickAudioSource.Play();
        _hasThruster = true;
        _thrusterAmmount = _ThrusterLimit;
        _uiManager.ThrusterSliderUpdate(_thrusterAmmount);
    }

    IEnumerator Thruster()
    {
        Debug.Log("thruster coroutine started");
        while (_leftShiftPressed == true && _thrusterAmmount > 0)
        {
            Debug.Log("thruster coroutine while loop");
            _thrusterAmmount--;
            _uiManager.ThrusterSliderUpdate(_thrusterAmmount);
            yield return new WaitForSeconds(1);
            
        }
        if (_thrusterAmmount <= 0)
        {
            _speed = 5;
            _hasThruster = false;
        }

    }
}
