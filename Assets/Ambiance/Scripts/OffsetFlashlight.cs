using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFlashlight : MonoBehaviour {

    private Vector3 vectOffset;
    private GameObject goFollow;
    [SerializeField] private float speed = 3.0f;

    [SerializeField] private Light flashlight;

    private bool lighted = false;

    [SerializeField] private AudioClip FlashlightSound;

    private AudioSource AudioSource;

    void Start()
    {
        flashlight.enabled = false;

        goFollow = Camera.main.gameObject;
        vectOffset = transform.position - goFollow.transform.position;

        AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.position = goFollow.transform.position + vectOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.T))
        {
            FlashlightOnOff();
        }
    }

    public void FlashlightOnOff()
    {
        lighted = !lighted;

        if(lighted == false)
        {
            flashlight.enabled = true;
        }
        if (lighted == true)
        {
            flashlight.enabled = false;
        }
        
        AudioSource.clip = FlashlightSound;
        AudioSource.Play();
    }
}
