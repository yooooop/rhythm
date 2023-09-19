using System.Collections;
using UnityEngine;

public class Laser : OnBeatBehaviour
{
    [SerializeField]
    private AudioClip soundAppear;
    [SerializeField]
    private AudioClip soundFire;

    public float startWidth = 0.1f;
    public float endWidth = 0.1f;
    public Color startColor;
    public Color endColor;
    public Sprite telegraph;
    public int beatsDelay = 1;
    
    private SpriteRenderer _spriteRenderer;
    private LineRenderer _lineRenderer;
    private GameObject _boss;
    private int _index = -1;

    private Health health;
    private SoundManager soundManager;
    private PlayerController playerController;
    private Grid grid;

    protected override void Start()
    {
        base.Start();
        playerController = FindObjectOfType<PlayerController>();
        soundManager = FindObjectOfType<SoundManager>();
        health = playerController.GetComponent<Health>();
        conductor = FindObjectOfType<Conductor>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boss = GameObject.FindGameObjectWithTag("Boss");
        grid = FindObjectOfType<Grid>();

        if (!_spriteRenderer)
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject.");
        }
        
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = startWidth;
        _lineRenderer.endWidth = endWidth;
        _lineRenderer.material.color = startColor;
        _lineRenderer.startColor = startColor;
        _lineRenderer.endColor = endColor;
        _lineRenderer.sortingOrder = 11;
        soundManager.AddSound(soundAppear);
    }
    
    protected override void OnBeat()
    {
        if (_index++ == beatsDelay)
        {
            var position = transform.position;
            var bossPosition = _boss.transform.position;
            _spriteRenderer.enabled = false;
            _lineRenderer.SetPosition(0, bossPosition);
            _lineRenderer.SetPosition(1, position);
            _lineRenderer.enabled = true;
            soundManager.AddSound(soundFire);
            StartCoroutine(CheckDanger(conductor.LastBeat));
        } else if (_index > beatsDelay)
        {
            Destroy(gameObject, 0f);
        }
    }

    private IEnumerator CheckDanger(float lastBeat)
    {
        yield return new WaitUntil(() => conductor.GetSongPosition() > lastBeat + playerController.AbsoluteLeniency);
        if (grid.WorldToCell(playerController.transform.position) == grid.WorldToCell(transform.position))
        {
            health.TakeDamage(1, "player");
        }
    }
}
