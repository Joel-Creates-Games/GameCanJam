using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DespawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
