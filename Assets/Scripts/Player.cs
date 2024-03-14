using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _laserOffset = .8f;
    [SerializeField]
    private float _fireRate = .5f;
    private float _canFire = -1f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        Debug.Log("This is a log");
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        ShootLaser(_laserPrefab);
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

    void ShootLaser(GameObject laser)
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(laser, transform.position  + new Vector3(0, _laserOffset, 0), Quaternion.identity);
        }
    }
}
