using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InimigoController : Personagem
{
    [Header("Movimentação do Inimigo")]
    [Tooltip("Determina a velocidade de movimento do personagem")]
    public float velAndar;

    [Tooltip ("Determina a velocidade de movimento de retorno ao ponto inicial do personagem")]
    public float velRetornar;

    [Tooltip ("Componente responsável pela física do personagem")]
    public Rigidbody2D rb2d;

    [Tooltip ("Determina a posicao do pé do personagem para validação do pulo")]
    public Transform groundCheck;

    [Tooltip ("Determina a distancia para validar o pulo do personagem")]
    public float raio;

    [Tooltip ("Determina a camada referente ao Chao para validar o pulo do personagem")]
    public LayerMask camadaChao;

    [Tooltip ("Salva informação da posição inicial do Inimigo")]
    public Vector2 posInicial;

    [Tooltip ("Determina a distância máxima de patrulha do Inimigo")]
    public float distPatrulha;

    [Tooltip ("De quanto em quanto tempo o inimigo efetua patrulha")]
    public float intervaloPatrulha;

    [Tooltip ("De quanto em quanto tempo o inimigo efetua patrulha")]
    public float horaUltimaPatrulha;



    [Header("Compenentes e Objetos de Interface")]

    [Tooltip("Componente responsável pelas animações do personagem")]
    [SerializeField] private Animator anim;
    
    [Tooltip("Componente Image responsável em mostrar a vida atual do jogador")]
    public Image barraVida;
       



    [Header("Sistema de Ataque do Inimigo")]

    [Tooltip ("Objeto referente ao tiro do Inimigo")]
    public GameObject tiroPrefab;

    [Tooltip ("Local onde deverá ser criado o tiro disparado")]
    public Transform posAtaque;

    [Tooltip ("Quantidade de segundos entre ataques")]
    public float tempoEntreAtaques;

    [Tooltip ("Alcance de visão do inimigo")]
    public float distAgro;

    [Tooltip ("Alcance de ataque do inimigo")]
    public float alcanceAtaque;

    [Tooltip ("Distância máxima que o inimigo pode se afastar da posição inicial")]
    public float distMaximaPosInicial;

    //Controla quando foi o ultimo ataque
    private float horaUltimoAtaque;

    //Posição do Player
    private Transform posPlayer;

    private Vector3 novaPosicao;

    private bool retornando;

    // Start is called before the first frame update
    void Awake()
    {
        VidaAtual = VidaMaxima;
        retornando = false;
    }

    private void Start()
    {
        posPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        posInicial = transform.position;
        novaPosicao = posInicial;
    }

    // Update is called once per frame
    void Update()
    {
        AtualizaBarraVida (VidaAtual / VidaMaxima);

        //Efetua o tratamento das animações do inimigo
        AnimaInimigo ();

        //Verifica se o jogador está morto
        if(posPlayer.gameObject.GetComponent<PlayerController>().Morto)
        {
            //Se tiver morto, fica parado
            novaPosicao = transform.position;
            return;
        }

        //Vira o jogador para o lado correto quando está em movimento
        Flip ();

        if (retornando)
        {
            float dist = Mathf.Abs (transform.position.x - novaPosicao.x);
            if ( dist < 0.3f) retornando = false;
        }

        //Verifica se encontrou o jogador
        if(EncontrouJogador())
        {
            //Encontrou o jogador
            //Verifica se está longe da posição inicial?
            if (DevoRetornar() && !retornando)
            {
                // Se sim, retorna à posição inicial
                retornando = true;
                novaPosicao = new Vector2(posInicial.x, 0);
            }                
            else if (PodeAtacar() && !retornando)
            {
                //Senão, verifica se pode atacar e se não precisa retornar ao ponto inicial
                //Nesse caso, ajusta o inimigo e efetua o ataque
                novaPosicao = transform.position;
                                
                FlipParado ();
                Atacar ();
            }
            else if(!retornando)
            {
                //Senão, continua em direção ao jogador
                novaPosicao = posPlayer.position;
            }
        }
        else if (!retornando)
        {
            //Caso não encontre o jogador
            Patrulhar();
        }
        
    }

    private void FixedUpdate()
    {
        if ( Mathf.Abs(transform.position.x - novaPosicao.x ) > 0.1f ) Movimento ();
    }

    //Efetua a movimentação do inimigo quando este não encontrou o jogador
    private void Patrulhar()
    {
        if (Time.time > horaUltimaPatrulha + intervaloPatrulha)
        {
            horaUltimaPatrulha = Time.time;
            float novoX = Random.Range(posInicial.x - distPatrulha, posInicial.x + distPatrulha);
            novaPosicao = new Vector2(novoX, transform.position.y);
        }        
    }

    //Valida se o inimigo encontrou o jogador, ou seja, jogador está dentro da área de alcance de visão do inimigo
    private bool EncontrouJogador()
    {
        float distancia = Mathf.Abs (posPlayer.position.x - transform.position.x);

        return distAgro >= distancia;
    }

    //Valida se o inimigo está longe demais do ponto inicial
    private bool DevoRetornar()
    {
        float distancia = Mathf.Abs ( transform.position.x - posInicial.x );

        return distancia > distMaximaPosInicial;
    }

    //Valida se o inimigo está perto o suficiente do jogador para poder atacar
    private bool PodeAtacar()
    {
        float distancia = Mathf.Abs( transform.position.x - posPlayer.position.x );

        return distancia <= alcanceAtaque;
    }

    //Movimentação do Inimigo
    private void Movimento()
    {
        //Vector2 movimento = Vector2.Lerp(transform.position, novaPosicao, velAndar * Time.deltaTime);
        Vector3 direcao = (novaPosicao - transform.position).normalized;
        
        if (retornando)
        {
            direcao *= velRetornar;            
        }
        else
        {
            direcao *= velAndar;
        }

        direcao.y = rb2d.velocity.y;
        rb2d.velocity = direcao;
    }

    //Atualiza informações de animação no Animator
    private void AnimaInimigo()
    {
        bool parado = rb2d.velocity.x > -0.1f && rb2d.velocity.x < 0.1f;
        anim.SetBool("Parado",parado);
        anim.SetBool ("Morto", Morto);
    }

    //Vira o inimigo conforme velocidade de movimento
    private void Flip()
    {
        Vector3 scale = transform.localScale;

        if (rb2d.velocity.x > 0.1f)
        {
            scale.x = 1;
        }
        else if (rb2d.velocity.x < -0.1f)
        {
            scale.x = -1;
        }

        transform.localScale = scale;
    }

    //Vira o inimigo conforme posição do jogador
    private void FlipParado()
    {
        Vector3 scale = transform.localScale;

        //Verifica se o jogador está a direito do inimigo
        if (transform.position.x < posPlayer.position.x)
        {
            scale.x = 1;
        }
        else
        {
            scale.x = -1;
        }

        transform.localScale = scale;
    }

    //Tratamento do ataque do inimigo
    private void Atacar()
    {
        if(Time.time > horaUltimoAtaque + tempoEntreAtaques)
        {
            horaUltimoAtaque = Time.time;
            anim.SetTrigger ("Atacar");
            GameObject tiro = Instantiate (tiroPrefab, posAtaque.position, posAtaque.rotation);
            tiro.transform.localScale = transform.localScale;

            float vel = tiro.GetComponent<MoveTiro> ().velTiro * transform.localScale.x;
            tiro.GetComponent<MoveTiro> ().rb2d.velocity = Vector2.right * vel;
        }        
    }

    public override void Morte ()
    {
        base.Morte ();
        Destroy (gameObject, 1f);
    }

    public void AtualizaBarraVida (float vida)
    {
        barraVida.fillAmount = Mathf.Lerp (barraVida.fillAmount, vida, 6f * Time.deltaTime);
    }

}
