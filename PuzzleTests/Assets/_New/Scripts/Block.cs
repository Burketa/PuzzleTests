using UnityEngine;
using UnityEngine.Advertisements;

public class Block : MonoBehaviour
{
    public float rotation_speed = 1, shot_force = 10, ray_distance = 10;
    public bool hasPlayer = false;

    private Vector2 start, finish, direction;
    private Vector3 rotation_angles;
    private LineRenderer line_rnd;
    private RaycastHit2D[] sighttest;
    private ParticleSystem _particleSystem;
    public GameObject _canvas;

    private void Awake()
    {
        if(gameObject.name.Contains("white"))
        _particleSystem = transform.parent.GetChild(0).GetComponent<ParticleSystem>();

        else if (gameObject.name.Contains("black"))
            _particleSystem = transform.parent.GetChild(1).GetComponent<ParticleSystem>();

        rotation_angles = new Vector3(0, 0, rotation_speed);
        line_rnd = GetComponent<LineRenderer>();
        line_rnd.SetPosition(0, transform.position);
        SeeForward(); //Para pegar os valores de start, finish e direction
        SetPlayer(false);
    }

    private void FixedUpdate()
    {
        if (hasPlayer)
        {
            SeeForward();
            transform.Rotate(rotation_angles);
            rotation_angles = new Vector3(0, 0, rotation_speed);
        }
    }

    private void Update()
    {
        //Só deixa o player atirar caso o canvas não esteja ativo, não tenha ad sendo visto e o bloco está com o player agora.
        if (hasPlayer && !_canvas.activeSelf && !Advertisement.isShowing)
        {
            CheckInputs();
        }
    }

    void SeeForward()
    {
        start = transform.position;
        finish = transform.GetChild(0).transform.position;
        direction = (finish - start).normalized;
        
        sighttest = DoRaycast(start, direction, ray_distance, Color.red);

        foreach (RaycastHit2D target in sighttest)  //Para cada objeto no Ray, compara pra ver se não é ele mesmo e nem o player(que esta dentro dele)
        {
            if (target.collider.gameObject != gameObject && !target.collider.CompareTag("Player"))
            {
                line_rnd.SetPosition(1, target.point);
                EmmitParticlesOnLaserBeamEnd(target.point, target.normal);
                break;
            }
        }
        
    }

    public RaycastHit2D[] DoRaycast(Vector3 start, Vector3 direction, float distance, Color color)
    {
        //Debug.DrawRay(start, direction * distance, Color.red);
        RaycastHit2D[] sighttest = Physics2D.RaycastAll(start, direction, distance);
        return sighttest;
    }

    public void SetPlayer(bool state)
    {
        hasPlayer = state;
        line_rnd.enabled = state;
        _particleSystem.Play();
        _particleSystem.gameObject.SetActive(state);
    }

    void CheckInputs()
    {
        if (transform.name.Contains("white"))
        {
            Debug.Log("Dentro do branco.");
            if (Input.GetButtonDown("Fire1"))
            {
                Camera.main.GetComponent<AudioSource>().Play();
                SetPlayer(false);
                Player.player.AddVelocity(direction * shot_force);
            }
        }
        else if (transform.name.Contains("black"))
        {
            Debug.Log("Dentro do preto.");
            if (Input.GetButtonUp("Fire1") || !Input.anyKey)
            {
                Camera.main.GetComponent<AudioSource>().Play();
                SetPlayer(false);
                Player.player.AddVelocity(direction * shot_force);
            }
        }
    }

    public void EmmitParticlesOnLaserBeamEnd(Vector3 position, Vector3 key)
    {
        if (!_particleSystem.isPlaying)
            _particleSystem.Play();
        _particleSystem.transform.position = position;

        //_particleSystem.transform.rotation = Quaternion.Euler(key);
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
