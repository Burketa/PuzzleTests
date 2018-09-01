using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player player;

    public UI _canvas;
    public TextMesh score_obj, highscore_obj, sum_obj;
    public TextMesh _health_obj;


    private bool _revive_clicked = false, _continue_clicked = false;
    private int health_max = 10, health_current, remaining_revives = 3, score = 0, highscore = 0, sum = 0;
    //Caching
    private Rigidbody2D _rgdbdy;
    private CircleCollider2D _collider;
    private SpriteRenderer _sprt;
    private Vector2 _spawn_point, _last_valid_block_position;
    private Transform _last_valid_block;

    private void Awake()    //Coloca as variaveis e faz o caching ao acordar
    {
        player = this;

        health_current = health_max;

        _rgdbdy = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _sprt = GetComponent<SpriteRenderer>();
        _spawn_point = transform.position;
        _last_valid_block_position = transform.position;
    }

    public void AddVelocity(Vector2 velocity)
    {
        _sprt.enabled = true;
        _collider.enabled = true;
        _rgdbdy.velocity = velocity;
    }

    public void OnTriggerEnter2D(Collider2D trigger_obj)
    {
        if(trigger_obj.tag.Equals("Block"))
        {
            _rgdbdy.gravityScale = 0;
            if (trigger_obj.name.Contains("white"))                         //Salva o bloco branco que fez contato como ultimo bloco valido.
            {
                _last_valid_block = trigger_obj.transform;
                _last_valid_block_position = _last_valid_block.position;
            }
            BreakPlayer();                                                  //Para o player no bloco
            _sprt.enabled = false;                                          //Da um disable no sprite renderer para não ficar aparecendo por cima do bloco em que esta
            transform.position = trigger_obj.transform.position;            //Seta a posição para ficar certinho em cima do bloco
            trigger_obj.gameObject.GetComponent<Block>().SetPlayer(true);   //Chama a funcao SetPlayer no bloco colidido
        }

        else if(trigger_obj.tag.Equals("Star"))
        {
            AddScore(1);
            trigger_obj.gameObject.SetActive(false);                        //Melhor destruir ou desativar ?
        }
    }

    public void BreakPlayer()
    {
        _rgdbdy.velocity = Vector3.zero;
        _rgdbdy.angularVelocity = 0;
    }

    public int HurtPlayer(int damage)
    {
        health_current -= damage;                                          //Da o dano no player
        Mathf.Clamp(health_current, 0, health_max);                        //Clampa o valor para não ter valores negativos
        _health_obj.text = health_current.ToString();                      //Atualiza o texto da vida
        if(health_current <= 0)                                            //Se tiver chegado a 0 de vida, morre
            Die();
        return health_current;
    }

    public void Die()
    {
        StartCoroutine(ShowMenu());

        //Precisa dar um enable e um disable para resetar o OnTriggerEnter caso morra muito perto do inimigo.
        _collider.enabled = false;
        _collider.enabled = true;
        
        health_current = health_max;                                            //Reseta a vida para o valor da vida maxima
        _health_obj.text = health_current.ToString();                           //Atualiza os valores no texto

        BreakPlayer();
    }

    public bool isHurtable()
    {
        return _sprt.enabled;
    }

    public IEnumerator ShowMenu()
    {
        Debug.Log("Showing Menu");
        _canvas.ChangeRemainingRevives(remaining_revives);                     //Atualiza a quantidade de revives restantes no UI
        if (remaining_revives <= 0)
        {
            _canvas.button_revive.interactable = false;
            //TODO: Arrumar um jeito de resetar o tanto de restarts que ele tem.
            //remaining_revives = 3;
        }
        else
            _canvas.button_revive.interactable = true;
        _canvas.gameObject.SetActive(true);                                  //Ativa o canvas
        yield return StartCoroutine(WaitButtonPress());                      //Espera o retorno da corrotina pra só continuar depois de algum botao ser apertado
        _canvas.gameObject.SetActive(false);                                 //Desativa o canvas
        Debug.Log("Disabling menu");
    }

    public IEnumerator WaitButtonPress()
    {
        Debug.Log("Esperando Clique");
        while (!_revive_clicked && !_continue_clicked)
        {
            yield return null;
        }
        _revive_clicked = _continue_clicked = false;                        //Depois de capturado o toque, reseta as variaveis
        Debug.Log("Clicado");
    }

    public void ReviveClicked()
    {
        _revive_clicked = true;
        remaining_revives--;
        AddScore(-1);   //TODO: melhorar isso
        transform.position = _last_valid_block_position;                    //Seta a posicao para o ultimo bloco valido
        Camera.main.GetComponent<AdManager>().ShowRewardedAd();             //Mostra um anuncio para reviver
    }

    public void ContinueClicked()
    {
        _continue_clicked = true;
        transform.position = _spawn_point;
        SceneManager.LoadScene(0);      //É mesmo nescessario ? Acho que quando arrumar um jeito de dar um enable nas estrelas ja pegas é possivel tirar isso
    }

    public void AddScore(int qtd)
    {
        score += qtd;
        //Gambiarras começando...
        if (qtd == -1)
        {
            score = 0;
            qtd = 0;
        }
        if (score > highscore)
            highscore += qtd;
        sum += qtd;

        score_obj.text = "Pontos\n" + score.ToString();
        highscore_obj.text = highscore.ToString();
        sum_obj.text = sum.ToString();
    }

    public void ResetBlockRotation()
    {
        _last_valid_block.GetComponent<Block>().ResetRotation();
    }
}
