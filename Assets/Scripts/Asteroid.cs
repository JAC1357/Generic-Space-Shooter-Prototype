using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float _rotaionSpeed = 9f;
    private Animator _anim;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotaionSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            _anim.SetTrigger("OnExplosion");
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 2.8f);
        }
    }
}
