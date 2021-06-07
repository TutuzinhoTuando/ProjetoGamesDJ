using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espinho : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            if (!outro.GetComponent<PlayerController>().Morto)
            {
                float dano = outro.GetComponent<PlayerController>().VidaAtual;
                outro.GetComponent<PlayerController>().RecebeDano(dano);

                outro.GetComponent<PlayerController>().rb2d.velocity = Vector2.zero;
                outro.GetComponent<PlayerController>().AplicaForcaVertical(350);
            }
        }
    }


}

