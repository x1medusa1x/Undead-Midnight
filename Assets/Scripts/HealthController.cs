using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Header("Health Values")]
    public float currentPlayerHealth = 100.0f;
    public bool dead = false;
    [SerializeField] private float maxHealth = 100.0f;
    private bool isBreathing = false;
    public bool isHurting = false;

    [Header("Blood Image")]
    [SerializeField] private Image bloodBorder = null;
    [SerializeField] private float hurtTimer = 0.1f;

    [Header("Audio Effect")]
    [SerializeField] private AudioClip[] hurtAudio = null;
    [SerializeField] private AudioClip[] deathAudio = null;
    [SerializeField] private AudioClip breathAudio = null;
    [SerializeField] private AudioMixer audioMixer = null;
    public AudioSource hurtAudioSource;
    public AudioSource deathAudioSource;
    public AudioSource beatAudioSource;

    public IEnumerator coroutine = null;

    private void Start()
    {
        hurtAudioSource = GetComponent<AudioSource>();
        deathAudioSource = GetComponent<AudioSource>();
        beatAudioSource = gameObject.AddComponent<AudioSource>();
        beatAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[1];
    }

    private float offsets(float value1, float value2)
    {
        return currentPlayerHealth <= 60.0f && currentPlayerHealth > 30.0f ?
    value1 : currentPlayerHealth <= 30.0f && currentPlayerHealth > 1 ?
    value2 : 0;
    }

    private void Update()
    {
        beatAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[1];
        beatAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("HeartPitch", offsets(1f / 1.7f, 1f / 2.1f));
        beatAudioSource.pitch = offsets(1.7f, 2.1f);
        beatAudioSource.volume = offsets(0.2f, 0.4f);
        coroutine = Breathing();
        if (currentPlayerHealth > 0)
        {
            if (isHurting == true)
            {
                StartCoroutine(HurtFlash());
                if (currentPlayerHealth < 70)
                {
                    if (!isBreathing)
                    {
                        StartCoroutine(coroutine);
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
            isHurting = false;
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
        beatAudioSource.Stop();
        if (!dead)
        {
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

    public IEnumerator HurtFlash()
    {
        bloodBorder.enabled = true;
        hurtAudioSource.PlayOneShot(hurtAudio[UnityEngine.Random.Range(0, hurtAudio.Length - 1)]);
        yield return new WaitForSeconds(hurtTimer);
    }

    public IEnumerator KillPlayer()
    {
        bloodBorder.enabled = false;
        deathAudioSource.PlayOneShot(deathAudio[UnityEngine.Random.Range(0, deathAudio.Length - 1)]);
        yield return new WaitForSeconds(hurtTimer);
    }

    public IEnumerator Breathing()
    {
        beatAudioSource.clip = breathAudio;
        beatAudioSource.loop = true;
        beatAudioSource.Play();
        yield return new WaitForSeconds(0.7f);
    }
}
