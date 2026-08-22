using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] public AudioSource SFXSource;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        SFXSource.PlayOneShot(clip);
    }
}