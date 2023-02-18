using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private float dmg = 10.0f;
    [SerializeField] private HealthController healthController = null;
    [SerializeField] private AudioClip dmgAudio = null;
    private bool playingAudio;
    private AudioSource dmgAudioSource;

    private void Start()
    {
        dmgAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dmgAudioSource.PlayOneShot(dmgAudio);
            if (healthController.currentPlayerHealth - dmg <= 0)
            {
                healthController.dead = true;
                healthController.stopSound(healthController.breatheAudioSource);
            }
            healthController.currentPlayerHealth -= dmg;
            healthController.TakeDamage();
            playingAudio = true;
        }
    }

    private void Update()
    {
        if (playingAudio)
        {
            ;
        }
    }

}
