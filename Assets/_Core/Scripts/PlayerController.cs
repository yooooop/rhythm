using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerController : OnBeatBehaviour
{
    public int attackDamage = 1;
    
    [SerializeField]
    private float timeToMove;
    [SerializeField]
    private float absoluteLeniency;
    [SerializeField]
    private float judgementLeniency;
    [SerializeField]
    private TextMeshPro judgement;
    [SerializeField]
    private AudioClip perfectHitSound;
    [SerializeField]
    private AudioClip nearHitSound;

    [SerializeField]
    private Rigidbody2D rb2D;

    private bool _playerAction;
    private bool _isMoving;
    private Vector2 _origPos, _targetPos;

    private GameObject _boss;
    private Health _bossHealth;
    private Grid _grid;
    private SoundManager _soundManager;

    public float AbsoluteLeniency => absoluteLeniency;

    protected override void Start()
    {
        conductor = FindObjectOfType<Conductor>();
        _boss = GameObject.FindGameObjectWithTag("Boss");
        _bossHealth = _boss.GetComponent<Health>();
        _grid = FindObjectOfType<Grid>();
        _soundManager = FindObjectOfType<SoundManager>();
    }

    protected override void Update()
    {
        base.Update();

        // Find type of frame
        FrameType frameType;
        if (conductor.NextBeat - absoluteLeniency < conductor.GetSongPosition() && conductor.GetSongPosition() < conductor.NextBeat + absoluteLeniency
            || conductor.LastBeat - absoluteLeniency < conductor.GetSongPosition() && conductor.GetSongPosition() < conductor.LastBeat + absoluteLeniency)
        {
            if (conductor.NextBeat - absoluteLeniency < conductor.GetSongPosition() && conductor.GetSongPosition() < conductor.NextBeat - judgementLeniency)
            {
                frameType = FrameType.Early;
            }
            else if (conductor.LastBeat + judgementLeniency < conductor.GetSongPosition() && conductor.GetSongPosition() < conductor.LastBeat + absoluteLeniency)
            {
                frameType = FrameType.Late;
            }
            else
            {
                frameType = FrameType.Perfect;
            }
        }
        else
        {
            frameType = FrameType.Miss;
        }

        // Check for input
        Vector2 input = Vector2.zero;
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !_isMoving)
        {
            input = Vector2.up;
        }
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !_isMoving)
        {
            input = Vector2.left;
        }
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !_isMoving)
        {
            input = Vector2.down;
        }
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !_isMoving)
        {
            input = Vector2.right;
        }

        // Resolve input
        if (input == Vector2.zero) return;
        
        if (frameType == FrameType.Miss || !_playerAction)
        {
            DisplayFrameType(FrameType.Miss, conductor.GetSongPosition() + conductor.LengthOfBeat);
        }
        else
        {
            if (frameType == FrameType.Perfect)
            {
                _soundManager.AddSound(perfectHitSound);
            }
            else if (frameType == FrameType.Early || frameType == FrameType.Late) 
            {
                _soundManager.AddSound(nearHitSound);
            }
            StartCoroutine(MovePlayer(input));
            DisplayFrameType(frameType, conductor.NextBeat);
                
            // Attack
            var bossPos = _boss.transform.position;
            if (_grid.WorldToCell((Vector2)transform.position + input * 2) == _grid.WorldToCell(bossPos))
            {
                _bossHealth.TakeDamage(attackDamage, "boss");
            }
        }
    }

    private void DisplayFrameType(FrameType frameType, float end)
    {
        StopCoroutine("TextAnimation");
        StartCoroutine(TextAnimation(conductor.GetSongPosition(), end, frameType));
    }

    private IEnumerator TextAnimation(float start, float end, FrameType frameType)
    {
        while (conductor.GetSongPosition() < start + conductor.LengthOfBeat)
        {
            judgement.text = frameType.ToString();
            judgement.transform.localScale = Vector3.one - Vector3.one * (conductor.GetSongPosition() - start) / conductor.LengthOfBeat;
            yield return null;
        }
        judgement.text = "";
    }

    private IEnumerator MovePlayer(Vector2 direction)
    {
        _isMoving = true;
        _playerAction = false;

        float elapsedTime = 0;
        _origPos = transform.position;
        _targetPos = _origPos + direction;

        while (elapsedTime < timeToMove)
        {
            rb2D.MovePosition(Vector2.Lerp(_origPos, _targetPos, (elapsedTime / timeToMove)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb2D.MovePosition(_targetPos);

        _isMoving = false;
    }

    protected override void OnBeat()
    {
        StartCoroutine(RefreshPlayerAction(conductor.NextBeat));
    }

    private IEnumerator RefreshPlayerAction(float start)
    {
        yield return new WaitUntil(() => start - absoluteLeniency < conductor.GetSongPosition());
        if (_playerAction)
        {
            DisplayFrameType(FrameType.Miss, conductor.NextBeat);
        }
        _playerAction = true;
    }

    private enum FrameType
    {
        Early,
        Perfect,
        Late,
        Miss
    }
}
