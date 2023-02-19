using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private float dmg = 10.0f;
    [SerializeField] private HealthController healthController = null;
    [SerializeField] private AudioClip dmgAudio = null;
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
                healthController.Death();
                healthController.currentPlayerHealth = 1;
            }
            else
            {
                healthController.currentPlayerHealth -= dmg;
                healthController.TakeDamage();
            }
        }
    }

    private void Update()
    {
    }

}