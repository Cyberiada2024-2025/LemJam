using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Attractor : MonoBehaviour
{
    public float AttractionForce = 1;
    // - kiedy gracz jest na skraju triggera, siła = 0
	// - kiedy gracz jest w odległości 0, siła = AttractionForce

    public float Radius = -1;  // radius skopiowany z collidera

    void Start() {
        Radius = GetComponent<SphereCollider>().radius;
    }
}
