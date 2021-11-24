using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent nav;
    private RaycastHit hit;
    public CameraShake cameraShake;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool requireMovement = false;
        Vector3 screenPointerPosition = Vector3.zero;

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            requireMovement = Input.GetMouseButtonDown(0);
            screenPointerPosition = Input.mousePosition;
        }
        else if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if(Input.touchCount>0)
            {
                requireMovement = true;
                screenPointerPosition = Input.GetTouch(0).position;
            }
        }

        if (requireMovement)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPointerPosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.GetComponent<Tower>())
                {
                    if(Vector3.Distance(transform.position, hit.collider.transform.position) <= 4)
                    {
                        hit.collider.GetComponent<Tower>().RefillCoin();
                        return;
                    }
                }
                
                nav.SetDestination(hit.point);
            }
        }
        
    }

    public void PlayPickUpCoin()
    {
        SoundManager.Instance.PlayPickupCoin(audioSource);
    }
}
