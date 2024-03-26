using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _xWidth = 8f;
    private Player _player;
    private Animator _anim;
    [SerializeField] private AudioSource _explosionSource;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The player is empty.");
        }

        _anim = this.gameObject.GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The animator is empty.");
        }

        _explosionSource = GetComponent<AudioSource>();
        if (_explosionSource == null)
        {
            Debug.LogError("The _explosionSource is empty.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(_xWidth * -1, _xWidth);
            transform.position = new Vector3(randomX, 7, transform.position.z);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionSource.Play();
            Destroy(this.gameObject, 2.8f);
        }
        
        //if (other.tag.Equals("Laser"))
        if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            _explosionSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            if (_player != null)
            {
                _player.Score(10);
            }
            Destroy(this.gameObject, 2.8f);
        }
    }
}
