using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  
    [Tooltip ("Objeto que mostra a tela de GameOver")]
    [SerializeField] private GameObject menuGameOver;

    public static GameController instance;

    private void Start()
    {
        instance = this;
        ExibeTelaGameOver (false);
        
    }

    public void ExibeTelaGameOver(bool valor)
    {
        menuGameOver.SetActive (valor);
    }

    public void Btn_JogarNovamente()  // O botão de recarregar a fase
    {
        string nome = SceneManager.GetActiveScene ().name;
        SceneManager.LoadScene (nome);
    }

    public void Btn_Sair()  // O botão (ao morrer) para sair do jogo
    {
        Application.Quit ();
    }

}
