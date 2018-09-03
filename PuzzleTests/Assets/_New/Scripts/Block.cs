using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Advertisements;
public class Block : MonoBehaviour
{
    public bool hasPlayer = false;
    public bool clockwise = true;
    public float rotation_speed = 1, current_rotation_speed = 1, shot_force = 10;

    private GameObject _menu;
    private Transform _transform;
    private Vector2 start, direction;
    private Vector3 rotation_angles;
    private Quaternion _starting_rotation;
    private LineRenderer _line_rnd;
    private RaycastHit2D[] _sighttest;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        if (!clockwise)
            rotation_speed = Mathf.Abs(rotation_speed);
        else
            rotation_speed = -Mathf.Abs(rotation_speed);
        _transform = transform;
        _starting_rotation = _transform.rotation;
        current_rotation_speed = rotation_speed;
        //Gambiarra detected:
        if (_transform.name.Contains("white"))
            _particleSystem = transform.parent.GetChild(0).GetComponent<ParticleSystem>();

        else if (_transform.name.Contains("black"))
            _particleSystem = transform.parent.GetChild(1).GetComponent<ParticleSystem>();
        //
        rotation_angles = new Vector3(0, 0, rotation_speed);
        _menu = Player.player._canvas.transform.GetChild(0).gameObject;
        _line_rnd = GetComponent<LineRenderer>();
        _line_rnd.SetPosition(0, _transform.position);
        SeeForward(); //Para pegar os valores de start e direction
        SetPlayer(false);
    }

    private void FixedUpdate()
    {
        if (hasPlayer && !_menu.activeSelf)
        {
            SeeForward();
            rotation_angles.z = current_rotation_speed;    //Para mudar a rotacao no playmode, pode tirar mais tarde eu acho
            _transform.Rotate(rotation_angles);
        }
    }

    private void Update()
    {
        //Só deixa o player atirar caso o canvas não esteja ativo, não tenha ad sendo visto e o bloco está com o player agora.
        if (hasPlayer && !_menu.activeSelf && !Advertisement.isShowing)
        {
            CheckInputs();
        }
    }

    void SeeForward()
    {
        start = _transform.position;
        direction = _transform.right;

        _sighttest = Physics2D.RaycastAll(start, direction);

        foreach (RaycastHit2D target in _sighttest)  //Para cada objeto no Ray, compara pra ver se não é ele mesmo e nem o player(que esta dentro dele)
        {
            if (target.collider.gameObject != gameObject && !target.collider.CompareTag("Player"))
            {
                _line_rnd.SetPosition(1, target.point);
                EmitParticlesOnLaserBeamEnd(target.point);
                break;
            }
        }

    }

    public void SetPlayer(bool state)
    {
        hasPlayer = state;
        _line_rnd.enabled = state;
        _particleSystem.Play();
        _particleSystem.gameObject.SetActive(state);
    }

    void CheckInputs()
    {
        if (_transform.name.Contains("white"))
        {
            Debug.Log("Dentro do branco.");
            if (Input.GetMouseButtonDown(0))
            {
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))                    //Se não tiver clicado em um elemento de UI...não faça o imput !
                        return;
                }
                else
                {
                    if (EventSystem.current.IsPointerOverGameObject())                                              //Se não tiver clicado em um elemento de UI...não faça o imput !
                        return;
                }
                InvertRotation();
                SetPlayer(false);
                Player.player.AddVelocity(direction * shot_force);
            }
        }
        else if (_transform.name.Contains("black"))
        {
            Debug.Log("Dentro do preto.");
            if (!Input.anyKey)
            {
                InvertRotation();
                SetPlayer(false);
                Player.player.AddVelocity(direction * shot_force);
            }
        }
    }

    public void EmitParticlesOnLaserBeamEnd(Vector3 position)
    {
        if (!_particleSystem.isPlaying)
            _particleSystem.Play();
        _particleSystem.transform.position = position;
    }

    public void ResetRotation()
    {
        transform.rotation = _starting_rotation;
    }

    public void InvertRotation()
    {
        current_rotation_speed *= -1;
    }
}