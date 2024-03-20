using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private static float _spawnWaitTime = 5.0f;
    private WaitForSeconds _spawnTime = new WaitForSeconds(_spawnWaitTime);
    //private WaitForSeconds _spawnTime = new WaitForSeconds(5f);
    [SerializeField]
    private float _xWidth = 8f;
    [SerializeField]
    private float _ySpawnPoint = 7f;
    private IEnumerator _spawnEnemyRoutine;
    [SerializeField]
    private GameObject _enemyObject;
    [SerializeField]
    private GameObject _enemyContainer; 
    private bool _stopEnemySpawning = false;
    private bool _stopPowerupSpawning = false;
    [SerializeField]
    private GameObject _powerupContainer;
    private IEnumerator _spawnPowerUpRoutine;
    [SerializeField]
    private GameObject _powerupObject;


    // Start is called before the first frame update
    void Start()
    {
        _spawnEnemyRoutine = SpawnEnemyRoutine(_enemyObject);
        _spawnPowerUpRoutine = SpawnPowerUpRoutine(_powerupObject);

        StartCoroutine(_spawnEnemyRoutine);
        StartCoroutine(_spawnPowerUpRoutine);
        //StopCoroutine(spawnRoutine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine(GameObject gameObject)
    {
        while (_stopEnemySpawning == false)
        {
            GameObject newEnemy = Instantiate(gameObject, EnemySpawnVector(_xWidth, _ySpawnPoint), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            Debug.Log("Created new enemy object.");
            yield return _spawnTime;
        }
    }

    IEnumerator SpawnPowerUpRoutine(GameObject gameObject)
    {
        while (_stopPowerupSpawning == false)
        {
            WaitForSeconds respawnTime = new WaitForSeconds(Random.Range(3, 8));
            GameObject newPowerup= Instantiate(gameObject, EnemySpawnVector(_xWidth, _ySpawnPoint), Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            Debug.Log("Created new powerup object.");
            yield return respawnTime;
        }
    }

    public void OnPlayerDeath()
    {
        _stopEnemySpawning = true;
    }

    public Vector3 EnemySpawnVector(float width, float ySpawn)
    {
        return new Vector3(Random.Range(width * -1, width), ySpawn, 0);
        //return new Vector3(Random.Range(_xWidth * -1, _xWidth), 7, 0);
    }
}
