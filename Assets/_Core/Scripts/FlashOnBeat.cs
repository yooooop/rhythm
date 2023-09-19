using UnityEngine;

public class FlashOnBeat : OnBeatBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteToFlash;

    private bool flash;

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnBeat()
    {
        flash = !flash;
        if (flash)
        {
            spriteToFlash.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            spriteToFlash.color = new Color(0f, 0f, 0f, 1f);
        }
    }
}
