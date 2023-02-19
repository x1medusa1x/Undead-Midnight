using System.Collections;
using UnityEngine;
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
    private float beatTimer = 0;
    private bool isBeating = false;
    public AudioSource beatAudioSource;

    void Start()
    {
        beatAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        beatOffset = currentStamina <= 60.0f && currentStamina > 30.0f ?
            0.55f : currentStamina <= 30.0f && currentStamina > -1 ?
            0.35f : 0;
        beatOffsetVolume = currentStamina <= 60.0f && currentStamina > 30.0f ?
            0.7f : currentStamina <= 30.0f && currentStamina > -1 ?
            0.9f : 0;
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
        while (currentStamina <= 60.0f && currentStamina > -1 && !healthController.dead)
        {
            beatTimer -= Time.deltaTime;
            if (beatTimer <= 0)
            {
                yield return new WaitForSeconds(beatOffset);
                beatAudioSource.PlayOneShot(beatAudio);
                beatTimer = beatOffset;
            }
            beatAudioSource.volume = beatOffsetVolume;
        }

    }
}
