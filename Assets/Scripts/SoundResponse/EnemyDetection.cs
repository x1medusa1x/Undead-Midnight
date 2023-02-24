using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private FPSController fpsController = null;
    [SerializeField] private float soundRange = 25f;
    [SerializeField] private FieldOfView fov = null;

    private void Update()
    {
        if (source.isPlaying)
        {
            return;
        }

        if (fpsController.GetIsSprinting() || fov.canSeePlayer)
        {
            source.Play();

            var sound = new Sound(transform.position, soundRange);

            Sounds.MakeSound(sound);
        }
    }
}
