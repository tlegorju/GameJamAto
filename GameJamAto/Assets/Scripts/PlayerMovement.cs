using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerAction
{
    Idle,
    Moving,
    RefillTower,
    PickupCoin
}

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent nav;
    private RaycastHit hit;

    private PlayerAction currentAction;
    private GameObject currentTarget;

    private AudioSource audioSource;

    private GameObject pointedTower;

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
                for(int i=0;i<Input.touchCount;i++)
                {
                    if(Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        requireMovement = true;
                        screenPointerPosition = Input.GetTouch(i).position;
                    }
                }
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPointerPosition);
        if (requireMovement)
        {

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.GetComponent<Tower>())
                {
                    currentAction = PlayerAction.RefillTower;
                    currentTarget = hit.collider.gameObject;
                    nav.enabled = true;
                    nav.SetDestination(hit.collider.transform.position);
                }
                else if(hit.collider.GetComponent<Coins>())
                {
                    currentAction = PlayerAction.PickupCoin;
                    currentTarget = hit.collider.gameObject;
                    nav.enabled = true;
                    nav.SetDestination(hit.collider.transform.position);
                }
                else
                {
                    currentAction = PlayerAction.Moving;
                    currentTarget = null;
                    nav.enabled = true;
                    nav.SetDestination(hit.point);
                }
            }
        }

        switch(currentAction)
        {
            case PlayerAction.Idle:
                break;
            case PlayerAction.Moving:
                if(Vector3.Distance(transform.position, nav.destination) < .1f || nav.isStopped)
                {
                    currentAction = PlayerAction.Idle;
                    currentTarget = null;
                    nav.enabled = false;
                }
                break;
            case PlayerAction.RefillTower:
                if (Vector3.Distance(transform.position, currentTarget.transform.position) <= 8)
                {
                    currentTarget.GetComponent<Tower>().RefillCoin();
                    currentAction = PlayerAction.Idle;
                    currentTarget = null;
                    nav.enabled = false;
                }
                break;
            case PlayerAction.PickupCoin:
                if(currentTarget==null)
                {
                    currentAction = PlayerAction.Idle;
                    nav.enabled = false;
                }
                break;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Tower t = hit.collider.GetComponentInChildren<Tower>();
            if (t == null)
                t = hit.collider.GetComponentInParent<Tower>();
            if (t != null && t.gameObject != pointedTower)
            {
                if (pointedTower != null)
                {
                    pointedTower.GetComponentInChildren<OutlineController>()?.EnableOutline(false);
                }
                t.GetComponentInChildren<OutlineController>()?.EnableOutline(true);
                pointedTower = t.gameObject;
            }
            else if (t == null)
            {
                if (pointedTower != null)
                {
                    pointedTower.GetComponentInChildren<OutlineController>()?.EnableOutline(false);
                    pointedTower = null;
                }
            }
        }
        else
        {
            if (pointedTower != null)
            {
                pointedTower.GetComponentInChildren<OutlineController>()?.EnableOutline(false);
                pointedTower = null;
            }
        }

    }

    public void PlayPickUpCoin()
    {
        SoundManager.Instance.PlayPickupCoin(audioSource);
    }
}
