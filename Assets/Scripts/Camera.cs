using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private float _speed = 2;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = gameObject.transform.localPosition;
        Debug.Log(originalPos.ToString());//(0,1,-10)
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Coroutine shakeCamera = StartCoroutine(Shake());
            
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            transform.position = originalPos;
        }
    }

    public IEnumerator Shake()//back and forth
    {
        float shakeTime = 4f;
        while (shakeTime > 0)
        {
            transform.Translate(Vector3.left * 2 * Time.deltaTime);
            yield return new WaitForSeconds(.1f);
            transform.Translate(Vector3.right * 2 * Time.deltaTime);
            shakeTime--;
            yield return new WaitForSeconds(.1f);
        }
        transform.position = originalPos;
    }
}
