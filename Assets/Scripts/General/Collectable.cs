using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int progressScorePoints;

    [SerializeField] private UnlockableTrigger triggerToUnlock;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.UpdateProgressScore(progressScorePoints);
            AudioManager.instance.PlaySFX(AudioManager.instance.button);
            if (triggerToUnlock != null) triggerToUnlock.UnlockTrigger();
            Destroy(gameObject);
        }
    }
}
