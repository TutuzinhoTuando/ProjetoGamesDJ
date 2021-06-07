using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletavel : MonoBehaviour
{
    public enum TipoItem
    {
        ItemCura,
        Cristal
    }

    [Tooltip("Define o tipo do item")]
    public TipoItem tipo;

    [Tooltip("Define o valor do item conforme seu tipo")]
    public float valor;

    [Tooltip("Prefab para criação do item Coletável na HUD do player")]
    public GameObject itemPrefab;

    private void OnTriggerEnter2D (Collider2D outro)
    {
        if(outro.CompareTag("Player"))
        {
            if (outro.GetComponent<PlayerController> ().Morto) return;

            switch (tipo)
            {
                case TipoItem.ItemCura:
                    outro.GetComponent<PlayerController> ().RecebeCura (valor);
                    Destroy (gameObject);
                    break;

                case TipoItem.Cristal:
                    outro.GetComponent<PlayerController> ().AdicionaItemColetado (itemPrefab.name, itemPrefab, valor);
                    Destroy (gameObject);
                    break;

                default:
                    break;
            }
        }
    }

}
