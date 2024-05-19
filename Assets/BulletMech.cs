using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMech : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody bulletRigidbody;

    private void Awake() {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        float speed = 50f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other) {
     
        Destroy(gameObject);
    }
}
