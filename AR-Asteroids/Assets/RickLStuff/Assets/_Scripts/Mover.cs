using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{

    public float speed;
	public AudioClip clip;
	public AudioSource src;

	void Start ()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
		src = GetComponent<AudioSource>();
		src.PlayOneShot(clip);
	}
	
}
