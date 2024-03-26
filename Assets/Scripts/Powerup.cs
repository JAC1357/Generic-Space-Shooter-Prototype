using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.UI;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private PowerupTypes.Types.PowerupType _powerupID;
    //[SerializeField] private AudioSource _powerUpPickAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                //_powerUpPickAudioSource.Play();
                switch (_powerupID)
                {
                    case PowerupTypes.Types.PowerupType.TripleShot:
                        player.TripleShotPickUp();
                        break;
                    case PowerupTypes.Types.PowerupType.Speed:
                        player.SpeedPickUp();
                        //Debug.Log("Power up speed collected");
                        break;
                    case PowerupTypes.Types.PowerupType.Shield:
                        player.ShieldPickUp();
                        break;
                    default:
                        Debug.Log("No power up given.");
                        break;
                }
                
            }
            Destroy(this.gameObject);
        }
        
    }
}
