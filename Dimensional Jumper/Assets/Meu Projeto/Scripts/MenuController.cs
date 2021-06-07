using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void JogarOJogo()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SairDoJogo ()
    {
        Debug.Log("Saiu do jogo.");
        Application.Quit();
    }
   
}
