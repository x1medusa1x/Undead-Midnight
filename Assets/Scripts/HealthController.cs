using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Header("Health Values")]
    public float currentPlayerHealth = 100.0f;
    public bool dead = false;
    [SerializeField] private float maxHealth = 100.0f;
    private float breathOffset = 0;
    private float breathOffsetVolume = 0;
    private float breathTimer = 0;
    private bool isBreathing = false;

    [Header("Blood Image")]
    [SerializeField] private Image bloodBorder = null;
    [SerializeField] private float hurtTimer = 0.1f;

    [Header("Audio Effect")]
    [SerializeField] private AudioClip[] hurtAudio = null;
    [SerializeField] private AudioClip[] deathAudio = null;
    [SerializeField] private AudioClip breathAudio = null;
    public AudioSource hurtAudioSource;
    public AudioSource deathAudioSource;
    public AudioSource breatheAudioSource;

    private void Start()
    {
        hurtAudioSource = GetComponent<AudioSource>();
        deathAudioSource = GetComponent<AudioSource>();
        breatheAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        breathOffset = currentPlayerHealth <= 60.0f && currentPlayerHealth > 30.0f ?
    0.5f : currentPlayerHealth <= 30.0f && currentPlayerHealth > 1 ?
    0.15f : 0;
        breathOffsetVolume = currentPlayerHealth <= 60.0f && currentPlayerHealth > 30.0f ?
    0.7f : currentPlayerHealth <= 30.0f && currentPlayerHealth > 0 ?
    0.9f : 0;
        if (currentPlayerHealth > 0)
        {
            StartCoroutine(HurtFlash());
            if (currentPlayerHealth < 70)
            {
                if (!isBreathing)
                {
                    StartCoroutine(Breathing());
                    isBreathing = true;
                }
            }
            else
            {
                if (isBreathing)
                {
                    StopCoroutine(Breathing());
                    isBreathing = false;
                }
            }
        }
    }

    public void DecreaseHealth()
    {
        Color splatterAlpha = bloodBorder.color;
        splatterAlpha.a = 1 - (currentPlayerHealth / maxHealth);
        bloodBorder.color = splatterAlpha;
    }

    public void IncreaseHealth(float value)
    {
        Color splatterAlpha = bloodBorder.color;
        splatterAlpha.a = 1 - ((currentPlayerHealth + value) / maxHealth);
        bloodBorder.color = splatterAlpha;
    }

    public void TakeDamage()
    {
        if (currentPlayerHealth > 0)
        {
            DecreaseHealth();
        }
    }

    public void Death()
    {
        if (!dead)
        {
            StopCoroutine(Breathing());
            StartCoroutine(KillPlayer());
            StopCoroutine(KillPlayer());
            dead = true;
        }
        currentPlayerHealth = 1;
    }

    public void Heal(float value)
    {
        if (currentPlayerHealth > 0 && currentPlayerHealth <= 100)
        {
            IncreaseHealth(value);
        }
        else
        {
            if (currentPlayerHealth > 100)
            {
                currentPlayerHealth = 100;
            }
        }
    }

    public void playSound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void stopSound(AudioSource source)
    {
        source.Stop();
    }

    public IEnumerator HurtFlash()
    {
        bloodBorder.enabled = true;
        SoundManager.PlaySound(hurtAudio[UnityEngine.Random.Range(0, hurtAudio.Length - 1)], hurtAudioSource);
        yield return new WaitForSeconds(hurtTimer);
    }

    public IEnumerator KillPlayer()
    {
        bloodBorder.enabled = false;
        SoundManager.PlaySound(deathAudio[UnityEngine.Random.Range(0, deathAudio.Length - 1)], deathAudioSource);
        yield return new WaitForSeconds(hurtTimer);
    }

    public IEnumerator Breathing()
    {
        while (currentPlayerHealth <= 60.0f && currentPlayerHealth > 1)
        {
            breathTimer -= Time.deltaTime;

            if (breathTimer <= 0)
            {
                yield return new WaitForSeconds(0.7f);
                breatheAudioSource.PlayOneShot(breathAudio);
                yield return new WaitForSeconds(breathOffset);
                breathTimer = breathOffset;
            }
            hurtAudioSource.volume = breathOffsetVolume;
        }
    }
}
