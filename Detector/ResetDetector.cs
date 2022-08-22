using UnityEngine;

public class ResetDetector : MonoBehaviour
{
    public SceneChanger sceneChanger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            sceneChanger.DoRestart();
        }
    }
}
