using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip ("Define o valor mínimo da posição X da camera")]
    [SerializeField] private float minX;

    [Tooltip ("Define o valor máximo da posição X da camera")]
    [SerializeField] private float maxX;

    [Tooltip ("Objeto do player para ser pego a sua posição")]
    [SerializeField] private Transform posPlayer;
           

    // Update is called once per frame
    void Update()
    {
        if(posPlayer.position.x >= minX && posPlayer.position.x <= maxX)
        {
            Vector3 posCamera = transform.position;
            posCamera.x = posPlayer.position.x;
            transform.position = Vector3.Lerp(transform.position, posCamera,1.5f * Time.deltaTime);
        }
        else if (posPlayer.position.x < minX)
        {
            Vector3 posCamera = transform.position;
            posCamera.x = minX;
            transform.position = Vector3.Lerp (transform.position, posCamera, 1.5f * Time.deltaTime);
        }
        else if (posPlayer.position.x > maxX)
        {
            Vector3 posCamera = transform.position;
            posCamera.x = maxX;
            transform.position = Vector3.Lerp (transform.position, posCamera, 1.5f * Time.deltaTime);
        }
    }
}
