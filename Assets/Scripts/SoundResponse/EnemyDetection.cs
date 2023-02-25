using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] public FPSController fpsController = null;
    [SerializeField] private float soundRange = 25f;
    [SerializeField] public FieldOfView fov = null;
    public Sound sound = null;

    private void Update()
    {
        if (source.isPlaying)
        {
            return;
        }

        if ((fpsController.GetIsSprinting() && fpsController.isWalking) || fov.canSeePlayer)
        {
            source.Play();

            sound = new Sound(transform.position, soundRange);

            Sounds.MakeSound(sound);
        }
    }
}
