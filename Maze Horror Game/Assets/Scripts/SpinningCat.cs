using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SpinningCat : MonoBehaviour
{
    [SerializeField] private Transform spinningCatVisual;
    [SerializeField] private Volume volume;

    private Animator animator;
    private AudioSource spinAudio;
    private Coroutine musicCoroutine;
    private bool cancelMusic;
    private bool isMusicPlaying;
    private float hueVal;

    private const string SPIN_BOOL = "SpinBool";
    private const string MUSIC_TRIGGER = "MusicTrigger";

    private void Start()
    {
        animator = GetComponent<Animator>();
        spinAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!animator.GetBool(SPIN_BOOL))
            LookAtPlayer();

        if (isMusicPlaying)
            PlayHueShiftAnimation();
    }

    public void PlaySpinningAnimation()
    {
        if (musicCoroutine != null) return;

        spinAudio.Play();
        animator.SetBool(SPIN_BOOL, true);
        cancelMusic = false;
        musicCoroutine = StartCoroutine(PlayMusicAnimation());
    }

    public void StopSpinningAnimation()
    {
        spinAudio.Stop();
        animator.SetBool(SPIN_BOOL, false);
        StopMusicAnimation();
    }

    private void LookAtPlayer()
    {
        Vector3 target = new(Player.Instance.transform.position.x, spinningCatVisual.position.y, Player.Instance.transform.position.z);
        spinningCatVisual.LookAt(target);
    }

    private IEnumerator PlayMusicAnimation()
    {
        float timer = 0f;
        while (timer < 34f)
        {
            if (cancelMusic)
                yield break;

            timer += Time.deltaTime;
            yield return null;
        }

        animator.SetTrigger(MUSIC_TRIGGER);
        isMusicPlaying = true;

        if (volume.profile.TryGet(out Bloom bloom))
            bloom.threshold.value = 0;
    }

    private void StopMusicAnimation()
    {
        cancelMusic = true;

        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
            musicCoroutine = null;
        }

        isMusicPlaying = false;

        if (volume.profile.TryGet(out Bloom bloom))
            bloom.threshold.value = 1;

        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
            colorAdjustments.hueShift.value = 0;
    }

    private void PlayHueShiftAnimation()
    {
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            hueVal += 25;
            if (hueVal >= 180f)
            {
                hueVal = -180f;
            }
            else if (hueVal <= -180f)
            {
                hueVal = 180f;
            }
            colorAdjustments.hueShift.value = hueVal;
        }
    }
}
