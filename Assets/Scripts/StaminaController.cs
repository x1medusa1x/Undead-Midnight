using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Parameters")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaUseMultiplier = 100;
    [SerializeField] private float timeBeforeRegenStarts = 5;
    [SerializeField] private float staminaValueIncrement = 2;
    [SerializeField] private float staminaTimeIncrement = 0.1f;
    [SerializeField] private FPSController fpsController = null;
    [SerializeField] private HealthController healthController = null;
    private float currentStamina = 100;
    private Coroutine regeneratingStamina;

    [Header("Audio Effect")]
    [SerializeField] private AudioClip beatAudio = null;
    [SerializeField] private float beatOffset = 0;
    [SerializeField] private float beatOffsetVolume = 0;
    [SerializeField] private AudioMixer audioMixer = null;
    private bool isBeating = false;
    public AudioSource heartAudioSource;

    void Start()
    {
        heartAudioSource = gameObject.AddComponent<AudioSource>();
        heartAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[2];
    }

    private float offsets(float value1, float value2)
    {
        return currentStamina <= 60.0f && currentStamina > 30.0f ?
    value1 : currentStamina <= 30.0f && currentStamina > -1 ?
    value2 : 0;
    }

    void Update()
    {
        heartAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[2];
        heartAudioSource.pitch = offsets(1f, 1.5f);
        heartAudioSource.volume = offsets(0.1f, 0.3f);
        heartAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("HeartPitch", offsets(1f / 1f, 1f / 1.5f));
        HandleStamina();

        if (currentStamina > 0)
        {
            if (currentStamina <= 60 && currentStamina > -1)
            {
                if (!isBeating)
                {
                    StartCoroutine(HeartBeat());
                    isBeating = true;
                }
            }
            else
            {
                if (isBeating)
                {
                    StopCoroutine(HeartBeat());
                    isBeating = false;
                }
            }
        }

        if (healthController.currentPlayerHealth <= 0 && healthController.dead == true)
        {
            StopCoroutine(HeartBeat());
            isBeating = false;
        }
    }

    private void HandleStamina()
    {
        if (fpsController.GetIsSprinting() && fpsController.currentInput != Vector2.zero)
        {
            if (regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }
            currentStamina -= staminaUseMultiplier * Time.deltaTime;
            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
            if (currentStamina <= 0)
            {
                fpsController.canSprint = false;
            }
        }

        if (!fpsController.GetIsSprinting() && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
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

    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(timeBeforeRegenStarts);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaTimeIncrement);
        while (currentStamina < maxStamina)
        {
            if (currentStamina > 0)
            {
                fpsController.canSprint = true;
            }

            currentStamina += staminaValueIncrement;
            if (currentStamina > 60)
            {
                isBeating = false;
            }

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }

            yield return timeToWait;
        }

        regeneratingStamina = null;
    }

    public IEnumerator HeartBeat()
    {
        heartAudioSource.clip = beatAudio;
        heartAudioSource.loop = true;
        heartAudioSource.Play();
        yield return new WaitForSeconds(beatOffset);

    }
}
