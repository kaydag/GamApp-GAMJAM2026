using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Audio Source")]
    [SerializeField] public AudioSource SFXSource;
    [Header("BGM Sources")]
    [SerializeField] private AudioSource BGMSource1;
    [SerializeField] private AudioSource BGMSource2;
    [Header("BGM")]
    [SerializeField] public AudioClip dayBGM;
    [SerializeField] public AudioClip nightBGM;

    [Header("BGM Settings")]
    [SerializeField] private float bgmVolume = 0.4f;
    [SerializeField] private float fadeDuration = 1f;
    [Header("Enemy")]
    [SerializeField] public AudioClip enemyHitSound;
    [SerializeField] public AudioClip enemyDieSound;
    [SerializeField] public AudioClip enemyAttackSound;
    [Header("Tree Monster")]
    [SerializeField] public AudioClip woodThrowSound;
    [Header("Boss")]
    [SerializeField] public AudioClip rockThrowSound;
    [SerializeField] public AudioClip rollSound;
    [Header("Projectile")]
    [SerializeField] public AudioClip woodBreakSound;
    [SerializeField] public AudioClip rockBreakSound;
    [Header("UI")]
    [SerializeField] public AudioClip click;
    private AudioSource currentBGMSource;
    private Coroutine bgmCoroutine;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (BGMSource1 != null)
        {
            BGMSource1.loop = true;
            BGMSource1.playOnAwake = false;
            BGMSource1.volume = 0f;
        }
        if (BGMSource2 != null)
        {
            BGMSource2.loop = true;
            BGMSource2.playOnAwake = false;
            BGMSource2.volume = 0f;
        }
        currentBGMSource = BGMSource1;
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        if (SFXSource == null) return;
        SFXSource.PlayOneShot(clip);
    }
    public void PlayDayBGM()
    {
        PlayBGM(dayBGM);
    }
    public void PlayNightBGM()
    {
        PlayBGM(nightBGM);
    }

    public void PlayBGM(AudioClip newClip)
    {
        if (newClip == null) return;
        if (currentBGMSource != null && currentBGMSource.clip == newClip && currentBGMSource.isPlaying)
            return;
        if (bgmCoroutine != null) StopCoroutine(bgmCoroutine);
        bgmCoroutine =StartCoroutine(CrossFadeBGM(newClip));
    }
    private IEnumerator CrossFadeBGM(AudioClip newClip)
    {
        AudioSource oldSource = currentBGMSource;
        AudioSource newSource;
        if (currentBGMSource == BGMSource1) newSource = BGMSource2;
        else newSource = BGMSource1;
        if (newSource == null) yield break;

        newSource.clip = newClip;
        newSource.loop = true;
        newSource.playOnAwake = false;
        newSource.volume = 0f;
        newSource.Play();
        float time = 0f;
        float oldVolume = 0f;
        if (oldSource != null) oldVolume = oldSource.volume;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            if (oldSource != null)
            {
                oldSource.volume =Mathf.Lerp(oldVolume,0f,t);
            }
            newSource.volume =Mathf.Lerp(0f,bgmVolume,t);
            yield return null;
        }
        if (oldSource != null)
        {
            oldSource.Stop();
            oldSource.volume = 0f;
        }
        newSource.volume = bgmVolume;
        currentBGMSource = newSource;
        bgmCoroutine = null;
    }

    public void StopBGM()
    {
        if (bgmCoroutine != null)
        {
            StopCoroutine(bgmCoroutine);
            bgmCoroutine = null;
        }
        if (BGMSource1 != null)
        {
            BGMSource1.Stop();
            BGMSource1.volume = 0f;
            BGMSource1.clip = null;
        }

        if (BGMSource2 != null)
        {
            BGMSource2.Stop();
            BGMSource2.volume = 0f;
            BGMSource2.clip = null;
        }

        currentBGMSource = BGMSource1;
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (currentBGMSource != null)
        {
            currentBGMSource.volume =
                bgmVolume;
        }
    }
}