using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovementSolo : MonoBehaviour {

	public float speed, jumpForce;
	[SerializeField] private GameObject tete;

	private float mh, mv;
	private Vector3 jump;
	private Animator ator;
	private Rigidbody rb;
	private float currenthealth;

    [SerializeField] public bool auSol, enMarche;

    //Sons
    [SerializeField] private AudioClip FootstepsSound;
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private AudioClip JumpPlayerSound;

    private AudioSource AudioSource;

	// Use .this for initialization
	void Start () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		rb = GetComponent<Rigidbody> ();
		ator = GetComponent<Animator> ();
		//Contrainte de rotation en X, Z activée
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		jump = new Vector3 (0f, 1.5f, 0f);
        enMarche = false;
        AudioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

		if(!GestionMenu.pause){
			RotateCameraX ();

			Deplacement ();

			Saut();
		}
	}

	void FixedUpdate(){
		if((!GestionMenu.pause)&&(!GestionMenu.InvStats))
			RotateCameraY ();
	}


	void Deplacement()
    {
        mv = Input.GetAxis("Vertical");
        mh = Input.GetAxis("Horizontal");

        if (mv != 0)
        {
            transform.Translate(mv * transform.forward * speed, Space.World);
            ator.SetBool("walking", true);
            PlayFootstepsSound();
        }
        else
        {
            ator.SetBool("walking", false);

        }
        if (mh != 0)
        {
            transform.Translate(mh * transform.right * speed, Space.World);
            ator.SetBool("walking", true);
            PlayFootstepsSound();
        }
	}

	void Saut()
    {
	    if (Input.GetAxis ("Jump") > 0 && auSol)
        {
			rb.AddForce (new Vector3 (0, jumpForce, 0), ForceMode.Impulse);
			//Le joueur n'est donc plus sur le sol
			auSol = false;
            PlayJumpSound();
            PlayJumpPlayerSound();
        }
	}

    void PlayFootstepsSound()
    {
        if (auSol == false)
        {
            return;
        }
        else
        {
                AudioSource.clip = FootstepsSound;
                AudioSource.PlayDelayed(0.1f);
        }
    }

    private void PlayJumpSound()
    {
        AudioSource.clip = JumpSound;
        AudioSource.Play();
    }

    private void PlayJumpPlayerSound()
    {
        AudioSource.clip = JumpPlayerSound;
        AudioSource.Play();
    }

	void RotateCameraX(){
		if (!GestionMenu.InvStats)
            transform.Rotate(0, Input.GetAxis("Mouse X") * 5f, 0);
	}

	float NegativeEuler(float angle){
		return (angle > 180) ? angle - 360 : angle;
	}

	void RotateCameraY(){
		int up = 65, down = -70;
		// Si t'es l'angle de rotation de la tete est copmprise entre les valeurs desirées
		if (NegativeEuler(tete.transform.rotation.eulerAngles.z) < up && NegativeEuler(tete.transform.rotation.eulerAngles.z) > down) {
			if(Input.GetAxis ("Mouse Y") != 0)
				tete.transform.Rotate (0, 0, ((Input.GetAxis ("Mouse Y") < 0) ? -0.1f : 0.1f) * 25f);
		} else if (NegativeEuler(tete.transform.rotation.eulerAngles.z) > up) {
			if(Input.GetAxis ("Mouse Y") != 0)
				tete.transform.Rotate (0, 0, ((Input.GetAxis ("Mouse Y") < 0) ? -0.1f : 0f) * 25f);

		} else if (NegativeEuler(tete.transform.rotation.eulerAngles.z) < down) {
			if(Input.GetAxis ("Mouse Y") != 0)
				tete.transform.Rotate (0, 0, ((Input.GetAxis ("Mouse Y")> 0) ? 0.1f : 0f) * 25f);
		}
	}

	void OnCollisionStay(Collision co){
		foreach (ContactPoint contact in co.contacts) {
			if (contact.normal == new Vector3 (0, 1, 0)) {
				auSol = true;
			}
		}
	}
		
}
