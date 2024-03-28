using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private PowerupTypes.Types.PowerupType _powerupID;
    //[SerializeField] private AudioSource _powerUpPickAudioSource;

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
                    case PowerupTypes.Types.PowerupType.Ammo:
                        player.AmmoPickUp(7);
                        break;
                    case PowerupTypes.Types.PowerupType.Health:
                        player.HealthPickUp(1);
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
