using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player player;
    private Rigidbody2D _rgdbdy;
    private CircleCollider2D _collider;
    private SpriteRenderer _sprt;
    public UI _canvas;
    public TextMesh score_obj, highscore_obj, sum_obj;


    private int health_max = 10, health_current;

    private Vector2 _spawn_point, _last_valid_block;
    private bool _revive_clicked = false, _continue_clicked = false;
    private int remaining_revives = 3;
    private int score = 0, highscore = 0, sum = 0;

    private void Awake()
    {
        _rgdbdy = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _sprt = GetComponent<SpriteRenderer>();
        player = this;

        health_current = health_max;
        _spawn_point = transform.position;
        _last_valid_block = transform.position;
    }

    public Vector2 AddVelocity(Vector2 velocity)
    {
        _sprt.enabled = true;
        _collider.enabled = true;
        _rgdbdy.velocity = velocity;
        return _rgdbdy.velocity;
    }

    public void OnTriggerEnter2D(Collider2D trigger_obj)
    {
        if(trigger_obj.tag.Equals("Block"))
        {
            _rgdbdy.gravityScale = 0;
            if(trigger_obj.name.Contains("white"))
                _last_valid_block = transform.position;
            _rgdbdy.velocity = Vector3.zero;
            _rgdbdy.angularVelocity = 0;
            _sprt.enabled = false;
            transform.position = trigger_obj.transform.position;
            trigger_obj.gameObject.GetComponent<Block>().SetPlayer(true);
        }

        else if(trigger_obj.tag.Equals("Star"))
        {
            AddScore(10);
            Destroy(trigger_obj.gameObject);
        }
    }

    public int HurtPlayer(int damage)
    {
        health_current -= damage;
        Mathf.Clamp(health_current, 0, health_max);
        if(health_current == 0)
        {
            Die();
        }
        return health_current;
    }

    public void Die()
    {
        StartCoroutine(ShowMenu());

        //Precisa dar um enable e um disable para resetar o OnTriggerEnter caso morra muito perto do inimigo.
        _collider.enabled = false;
        _collider.enabled = true;

        //transform.position = _spawn_point;
        health_current = health_max;

        _rgdbdy.velocity = Vector3.zero;
        _rgdbdy.angularVelocity = 0;
    }

    public bool isHurtable()
    {
        return _sprt.enabled;
    }

    public IEnumerator ShowMenu()
    {
        Debug.Log("Showing Menu");
        _canvas.ChangeRemainingRevives(remaining_revives);
        if (remaining_revives <= 0)
        {
            _canvas.button_revive.interactable = false;
            //TODO: Arrumar um jeito de resetar o tanto de restarts que ele tem.
            //remaining_revives = 3;
        }
        else
            _canvas.button_revive.interactable = true;
        _canvas.gameObject.SetActive(true);
        yield return StartCoroutine(WaitButtonPress());
        _canvas.gameObject.SetActive(false);
        Debug.Log("Disabling menu");
    }

    public IEnumerator WaitButtonPress()
    {
        Debug.Log("Esperando Clique");
        while (!_revive_clicked && !_continue_clicked)
        {
            yield return null;
        }
        _revive_clicked = _continue_clicked = false;
        Debug.Log("Clicado");
    }

    public void ReviveClicked()
    {
        _revive_clicked = true;
        remaining_revives--;
        AddScore(-1);
        transform.position = _last_valid_block;
        Camera.main.GetComponent<AdManager>().ShowRewardedAd();
    }

    public void ContinueClicked()
    {
        _continue_clicked = true;
        transform.position = _spawn_point;
        SceneManager.LoadScene(0);
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
}
