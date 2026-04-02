using UnityEngine;

public class TargetLockUI : MonoBehaviour
{
    public TargetDetector targetDetector;
    public GameObject lockStatusText;

    void Update()
    {
        if (targetDetector.lockedTarget != null)
        {
            lockStatusText.SetActive(true);
        }
        else
        {
            lockStatusText.SetActive(false);
        }
    }
}