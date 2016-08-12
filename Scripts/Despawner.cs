using UnityEngine;

public class Despawner : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            Destroy(other.gameObject);
            print(other + " was destroyed!");
        }
    }
}