using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent nav;
    private RaycastHit hit;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
}
