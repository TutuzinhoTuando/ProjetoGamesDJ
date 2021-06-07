using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    [Header("Controle de Vida do Personagem")]
    [HideInInspector]public float VidaAtual; //Define o quanto de vida o personagem está no momento
    public float VidaMaxima; //Define o quanto de vida o personagem pode ter no máximo
    public bool Morto; //Controla se o personagem ainda está vivo ou não

    [Header("Sons do Personagem")]

    [Tooltip ("Objeto com componente AudioSource correspondente ao áudio quando o personagem recebe dano")]
    public AudioSource somRecebeDano;

    [Tooltip ("Objeto com componente AudioSource correspondente ao áudio  quando o personagem recebe cura")]
    public AudioSource somRecebeCura;

    [Tooltip ("Objeto com componente AudioSource correspondente ao áudio quando o personagem morre")]
    public AudioSource somMorte;

    //Método responsável em aplicar um dano ao personagem
    public void RecebeDano(float dano)
    {
        if (Morto) return;

        somRecebeDano.Play ();

        VidaAtual = Mathf.Max (VidaAtual - dano, 0);

        Morto = VidaAtual <= 0;

        if (Morto) Morte ();
    }

    //Método responsável em aplicar uma cura ao personagem
    public void RecebeCura(float cura)
    {
        somRecebeCura.Play ();
        VidaAtual = Mathf.Min (VidaAtual + cura, VidaMaxima);
    }

    public virtual void Morte()
    {
        somMorte.Play ();
    }

}
