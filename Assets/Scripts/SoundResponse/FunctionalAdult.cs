using UnityEngine;
using UnityEngine.AI;

public class FunctionalAdult : MonoBehaviour, IHear
{
    [SerializeField] private NavMeshAgent agent = null;

    void Awake()
    {
        if (!TryGetComponent(out agent))
        {
            Debug.LogWarning(name + " doesnt have an agent");
        }
    }

    public void RespondToSound(Sound sound)
    {
        MoveTo(sound.pos);
    }

    public void MoveTo(Vector3 pos)
    {
        agent.SetDestination(pos);
        agent.isStopped = false;
    }
}
