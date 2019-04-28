using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitValueScript : MonoBehaviour
{
    [SerializeField] private Text hitValueText;

    public float Value { set {
            hitValueText.text =  "-" + value.ToString();
        }
    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime / 2f, transform.position.z);
        StartCoroutine(DestroyWait());
    }

    private IEnumerator DestroyWait() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
