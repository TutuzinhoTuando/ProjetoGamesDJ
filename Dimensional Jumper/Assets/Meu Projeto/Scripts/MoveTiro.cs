using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTiro : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb2d;
    [SerializeField] public float velTiro;
    [SerializeField] private float dano;
    [SerializeField] private float tempoVida;

    private void Start ()
    {        
        Destroy (gameObject, tempoVida);        
    }

    private void OnTriggerEnter2D (Collider2D outro)
    {
        if(outro.CompareTag("Player"))
        {
            outro.GetComponent<PlayerController> ().RecebeDano (dano);
        }

        Destroy (gameObject);

    }
}
