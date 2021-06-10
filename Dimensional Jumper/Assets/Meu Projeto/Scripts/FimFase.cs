using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FimFase : MonoBehaviour
{
    [Tooltip ("Objeto necessário para avançar de fase")]
    public GameObject itemNecessario;

    [Tooltip ("Quantidade necessária do item para avançar de fase")]
    public int qtdNecessaria;

    [Tooltip ("Nome da fase a ser carregada")]
    public string nomeProximaFase;

    private void OnTriggerEnter2D (Collider2D outro)
    {
        
        if(outro.CompareTag("Player"))
        {
            int quantidadeEncontrada = 0;

            foreach (var item in outro.GetComponent<PlayerController>().inventario)
            {
                    quantidadeEncontrada++;  
            }

            if(quantidadeEncontrada >= qtdNecessaria)
            {
                Debug.Log ("Pode passar de fase");
                SceneManager.LoadScene (nomeProximaFase);
            }
            else
            {
                Debug.Log ("Falta coletar mais itens");
            }
        }

    }

}
