using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _xWidth = 8f;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

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

            Destroy(this.gameObject);
        }
        
        //if (other.tag.Equals("Laser"))
        if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            //Player player = other.transform.GetComponent<Player>();

            if (_player != null)
            {
                _player.Score(10);
            }
            Destroy(this.gameObject);
        }
    }
}
