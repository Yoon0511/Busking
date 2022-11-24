using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveSelf : MonoBehaviour {

    [SerializeField]
    float _Time;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(_Time);

        Destroy(gameObject);
    }
}
