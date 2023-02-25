using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Animator Params")]
    private Animator animator;
    int moveZAnimationParameterId;
    int blendValue;
    [SerializeField] private EnemyDetection enemyDetection = null;
    private float currentSpeed = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");
        blendValue = Animator.StringToHash("Blend");
        animator.SetFloat(blendValue, 0f);
        animator.SetFloat(moveZAnimationParameterId, 0f);
    }

    private void Update()
    {
        if ((enemyDetection.fpsController.GetIsSprinting() && enemyDetection.fpsController.isWalking) || enemyDetection.fov.canSeePlayer)
        {
            if (enemyDetection.fpsController.charController.transform.position.x == gameObject.transform.position.x)
            {
                animator.SetFloat(moveZAnimationParameterId, 0);

                animator.SetFloat(blendValue, 0f);
                currentSpeed = 1f;
            }
            else
            {
                animator.SetFloat(moveZAnimationParameterId, 1);

                animator.SetFloat(blendValue, 1f);
            }
        }
        else
        {
            if (currentSpeed > 0)
            {
                StartCoroutine(Wait());
            }
            else
            {
                StopCoroutine(Wait());
                animator.SetFloat(moveZAnimationParameterId, 0);

            }

            animator.SetFloat(blendValue, 0f);
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        if (currentSpeed > 0)
        {
            currentSpeed -= Time.deltaTime;
            animator.SetFloat(moveZAnimationParameterId, currentSpeed);
        }
    }
}
