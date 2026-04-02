using UnityEngine;

public class UnlockableTrigger : MonoBehaviour
{
    [SerializeField] private int progressScorePoints;

    [SerializeField] private BoxCollider triggerCollider;
    [SerializeField] private Outline objectOutline;

    private void Awake()
    {
        LockTrigger();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LockTrigger();
            AudioManager.instance.PlaySFX(AudioManager.instance.file);
            GameManager.instance.UpdateProgressScore(progressScorePoints);
        }
    }

    public void UnlockTrigger()
    {
        triggerCollider.enabled = true;
        objectOutline.enabled = true;
    }

    private void LockTrigger()
    {
        triggerCollider.enabled = false;
        objectOutline.enabled = false;
    }
}
