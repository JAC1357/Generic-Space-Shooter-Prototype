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
    private IEnumerator _spawnRoutine;
    [SerializeField]
    private GameObject _enemyObject;


    // Start is called before the first frame update
    void Start()
    {
        _spawnRoutine = SpawnRoutine(_enemyObject); 
        StartCoroutine(_spawnRoutine);
        //StopCoroutine(spawnRoutine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine(GameObject gameObject)
    {
        while (true)
        {
            Instantiate(gameObject, EnemySpawnVector(_xWidth, _ySpawnPoint), Quaternion.identity);
            Debug.Log("Created new enemy object.");
            yield return _spawnTime;
        }
    }

    public Vector3 EnemySpawnVector(float width, float ySpawn)
    {
        return new Vector3(Random.Range(width * -1, width), ySpawn, 0);
        //return new Vector3(Random.Range(_xWidth * -1, _xWidth), 7, 0);
    }
}
