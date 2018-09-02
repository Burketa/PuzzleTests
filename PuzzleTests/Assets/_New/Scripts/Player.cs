using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player player;

    public TextMesh score_obj, highscore_obj, sum_obj;
    public int health_max = 10, health_current, remaining_revives = 3, score = 0, highscore = 0, sum = 0, player_progress = 1, rotation_scale = 1;

    [Header("References")]
    public UI _canvas;
    public TextMesh _health_obj;
    public Rigidbody2D _rgdbdy;
    //public CircleCollider2D _collider;
    public BoxCollider2D _collider;
    public SpriteRenderer _sprt;
    public Transform _transform;
    //Caching
    private Vector2 _spawn_point, _last_valid_block_position;
    private Transform _last_valid_block;

    private void Awake()    //Coloca as variaveis e faz o caching ao acordar
    {
        player = GetComponent<Player>();
        health_current = health_max;
        _spawn_point = _transform.position;
        _last_valid_block_position = _transform.position;
        if (!Application.isEditor)
            SceneManager.LoadScene(player_progress, LoadSceneMode.Additive);
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
            trigger_obj.GetComponent<Block>().current_rotation_speed = trigger_obj.GetComponent<Block>().rotation_speed * (1 + (health_current * rotation_scale / health_max)); //Melhorar essa linha porcaria !
            if (trigger_obj.name.Contains("white"))                         //Salva o bloco branco que fez contato como ultimo bloco valido.
            {
                _last_valid_block = trigger_obj.transform;
                _last_valid_block_position = _last_valid_block.position;
            }
            BreakPlayer();                                                  //Para o player no bloco
            _sprt.enabled = false;                                          //Da um disable no sprite renderer para não ficar aparecendo por cima do bloco em que esta
            _transform.position = trigger_obj.transform.position;            //Seta a posição para ficar certinho em cima do bloco
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
        _canvas.ShowDieMenu();

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
    
    public void ReviveClicked()
    {
        remaining_revives--;
        AddScore(-1);   //TODO: melhorar isso
        _last_valid_block.GetComponent<Block>().SetPlayer(false);
        _transform.position = _last_valid_block_position;                    //Seta a posicao para o ultimo bloco valido
    }

    public void TryAgainClicked()
    {
        //_transform.position = _spawn_point;
        _transform.position = Vector2.zero;
        remaining_revives = 3;

        _sprt.enabled = true;
        
        if(_last_valid_block)
            _last_valid_block.GetComponent<Block>().SetPlayer(false);
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
        Block block = _last_valid_block.GetComponent<Block>();

        block.ResetRotation();
    }
}
