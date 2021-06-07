using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Personagem
{
    [Header ("Sons do Player")]
    [Tooltip ("Objeto com componente AudioSource correspondente ao áudio de Pulo do personagem")]
    [SerializeField] private AudioSource somPulo;

    [Tooltip ("Objeto com componente AudioSource correspondente ao áudio quando o personagem coleta um cristal")]
    [SerializeField] public AudioSource somCristal;

    [Header("Controle de Movimentação do Player")]
    [Tooltip ("Determina a velocidade de movimento do personagem")]
    public float velAndar;

    [Tooltip ("Determina a força do pulo do personagem")]
    public float forcaPulo;

    [Tooltip ("Determina a posicao do pé do personagem para validação do pulo")]
    public Transform groundCheck;

    [Tooltip ("Determina a distancia para validar o pulo do personagem")]
    public float raio;

    [Tooltip ("Determina a camada referente ao Chao para validar o pulo do personagem")]
    public LayerMask camadaChao;
    

    [Header("Componentes do Player")]
    [Tooltip("Componente Image responsável em mostrar a vida atual do jogador")]
    public Image barraVida;

    [Tooltip ("Objeto responsável por exibir os itens coletados na fase pelo jogador")]
    public Transform areaItensColetados;

    [Tooltip ("Lista de itens coletados pelo jogador")]
    public List<GameObject> inventario = new List<GameObject> ();

    [Tooltip("Componente responsável pelas animações do personagem")]
    [SerializeField] private Animator anim;

    [Tooltip ("Componente responsável pela física do personagem")]
    public Rigidbody2D rb2d;

    private float andar; //Armazena informação do quanto o jogador deve se mover horizontalmente
    private bool podePular; //Controla se o personagem pode pular

    [Header ("Sistema de Ataque")]
    [Tooltip ("Determina o dano que o jogador poderá causar ao inimigo com seu ataque corpo a corpo")]
    [SerializeField] private float danoCorpoACorpo;

    [Tooltip ("Determina o tempo entre ataques do jogador")]
    [SerializeField] private float tempoEntreAtaques;

    [Tooltip ("Determina a posição do ataque")]
    [SerializeField] private Transform posAtaque;

    [Tooltip ("Determina o raio de alcance do ataque do jogador")]
    [SerializeField] private float alcanceAtaque;

    [Tooltip ("Determina qual é a camada do Inimigo")]
    [SerializeField] private LayerMask camadaInimigo;

    //Controla a hora do último ataque
    private float horaUltimoAtaque;

    //Controla a quantidade de tempo que o jogador está parado
    private float tempoParado = 0;

    //QUantide de segundos para entrar na animação de Idle Especial
    private float tempoAnimIdleEspecial = 4f;

    private void Start ()
    {
        barraVida.fillAmount = VidaAtual / VidaMaxima;
    }

    private void Update ()
    {

        AtualizaBarraVida (VidaAtual / VidaMaxima);

        AnimaPlayer ();

        if(Morto)
        {
            andar = 0;
            podePular = false;
            return;
        }

        //Verifica se o jogador pressionou alguma tecla para se movimentar e armazena essa informação
        andar = Input.GetAxisRaw ("Horizontal") * velAndar;
        FlipHorizontal();

        if ( Input.GetKeyDown (KeyCode.Space) && NoChao())
        {
            podePular = true;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= horaUltimoAtaque + tempoEntreAtaques)
        {
            horaUltimoAtaque = Time.time;
            AtaqueCorpoACorpo ();
        }
    }

    private void FixedUpdate ()
    {            
        Movimento ();

        if(podePular) AplicaForcaVertical (forcaPulo);
    }

    public void AtualizaBarraVida(float vida)
    {        
        barraVida.fillAmount = Mathf.Lerp (barraVida.fillAmount, vida, 6f * Time.deltaTime);
    }
    public void Movimento()
    {
        Vector2 movimento = new Vector2 (andar, rb2d.velocity.y);
        rb2d.velocity = movimento;
    }

    //Método que aplica uma força vertical para simular o pulo do personagem
    public void AplicaForcaVertical (float forca)
    {
        if(!Morto) somPulo.Play ();
        podePular = false;
        rb2d.AddForce (Vector2.up * forca);
    }

    public bool NoChao()
    {
        return Physics2D.OverlapCircle (groundCheck.position, raio, camadaChao);
    }

    public void AdicionaItemColetado(string nome, GameObject itemColetado, float quant)
    {
        //Emite o som de coleta do cristal
        somCristal.Play ();

        for (int i = 0; i < quant; i++)
        {
            GameObject item = Instantiate (itemColetado, areaItensColetados);
            item.name = nome;
            inventario.Add (item);
        }
        
    }


    public void AnimaPlayer()
    {
        anim.SetBool("NoChao", NoChao());
        
        anim.SetBool("Parado", andar == 0f);

        anim.SetBool("Morto", Morto);
    }

    
    private void FlipHorizontal()
    {
        Vector2 scale = transform.localScale;

        if (andar < 0)
        {            
            scale.x = -2;
            transform.localScale = scale;
        }
        else if (andar > 0)
        {
            scale.x = 2;
            transform.localScale = scale;
        }
    }

    public override void Morte ()
    {
        somMorte.Play ();
        Invoke ("GameOver", anim.GetCurrentAnimatorStateInfo (0).length);        
    }

    private void GameOver()
    {
        GameController.instance.ExibeTelaGameOver (true);
    }

    //Efetua o ataque corpo a corpo do jogador
    private void AtaqueCorpoACorpo()
    {
        Debug.Log ("AtaqueCorpoACorpo");

        anim.SetTrigger ("AtaqueCorpoACorpo");

        Collider2D[] inimigos = Physics2D.OverlapCircleAll (posAtaque.position, alcanceAtaque, camadaInimigo);


        if(inimigos.Length > 0)
        {
            foreach (var inimigo in inimigos)
            {
                inimigo.GetComponent<InimigoController> ().RecebeDano (danoCorpoACorpo);
                Debug.Log (inimigo.GetComponent<InimigoController>().VidaAtual);
            }
        }
        

    }
}
