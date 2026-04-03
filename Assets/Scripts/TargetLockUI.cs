using UnityEngine;

public class TargetLockUI : MonoBehaviour
{
    [Header("UI Refference")]
    public TargetDetector targetDetector;
    public GameObject lockStatusImg;

    void Update()
    {
        if (targetDetector.lockedTarget != null)
        {
            lockStatusImg.SetActive(true);
        }
        else
        {
            lockStatusImg.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}