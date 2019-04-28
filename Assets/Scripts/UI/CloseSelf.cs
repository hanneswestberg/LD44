using UnityEngine;

public class CloseSelf : MonoBehaviour
{
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
