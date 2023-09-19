using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAttack : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> sprites;
    [SerializeField]
    private List<AudioClip> sounds;
    [SerializeField]
    private int framesPerBeat;
    [SerializeField]
    private int dangerBeat; // beat that summons danger zone for rest of animation
    [SerializeField]
    private List<Vector2> dangerZones; // tiles that are dangerous relative to position

    private int currentFrame;
    private Conductor conductor;
    private Health health;
    private SoundManager soundManager;
    private PlayerController playerController;
    private Grid grid;
    private float startBeat;
    private bool danger;

    private void Start()
    {
        conductor = FindObjectOfType<Conductor>();
        playerController = FindObjectOfType<PlayerController>();
        soundManager = FindObjectOfType<SoundManager>();
        health = playerController.GetComponent<Health>();
        grid = FindObjectOfType<Grid>();
        startBeat = conductor.LastBeat;
    }

    private void Update()
    {
        if (conductor.GetSongPosition() > startBeat + conductor.LengthOfBeat / framesPerBeat * currentFrame)
        {
            currentFrame++;

            if (currentFrame >= sprites.Count)
            {
                Destroy(this.gameObject);
            }
            else
            {
                spriteRenderer.sprite = sprites[currentFrame];
                if (currentFrame < sounds.Count && sounds[currentFrame] != null)
                {
                    soundManager.AddSound(sounds[currentFrame]);
                }
                if (dangerBeat * framesPerBeat == currentFrame)
                {
                    danger = true;
                }
                if (danger && currentFrame % framesPerBeat == 0)
                {
                    StartCoroutine(CheckDanger(currentFrame));
                }
            }
        }
    }

    private IEnumerator CheckDanger(int attackFrame)
    {
        yield return new WaitUntil(() => conductor.GetSongPosition() > startBeat + conductor.LengthOfBeat / framesPerBeat * attackFrame + playerController.AbsoluteLeniency);
        if (PlayerInDangerZone())
        {
            health.TakeDamage(1, "player");
        }
    }

    private bool PlayerInDangerZone()
    {
        for (int i = 0; i < dangerZones.Count; i++)
        {
            Vector3 dangerPosition = this.transform.position + (Vector3)dangerZones[i];
            if (grid.WorldToCell(playerController.transform.position) == grid.WorldToCell(dangerPosition))
            {
                return true;
            }
        }
        return false;
    }
}
