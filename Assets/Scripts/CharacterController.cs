using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    private bool stepOn = true;
    public int blood = 5;

    private NavMeshAgent mMeshAgent;

    private Brick mPreviousBrick;

    private Brick mCurrentBrick;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        mMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                GameObject hitObject = hit.transform.gameObject;
                Brick brick = hitObject.GetComponent<Brick>();
                if (brick != null) {
                    mMeshAgent.SetDestination(hit.transform.position);
                }
            }
        }

        DetectMine();
    }

    private void ColorChange(int value)
    {
        Color[] colors = {Color.red,Color.magenta,Color.yellow,Color.blue,Color.green};
        // Color[] colors = {Color.red};
        if (value > 0){
            GetComponent<Renderer>().material.SetColor("_Color", colors[value]);
        }
    }
    private void DetectMine()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.2f, out hit)) {
            GameObject hitObject = hit.transform.gameObject;
            Brick brick = hitObject.GetComponent<Brick>();
            if (brick != null) {
                brick.ShowSecret();

                if (brick.mine && mPreviousBrick != null && stepOn) {
                    mMeshAgent.SetDestination(mPreviousBrick.transform.position);
                    blood -= 1;
                    stepOn = false;
                    ColorChange(blood);
                } else 
                if (!brick.mine)
                {
                    stepOn = true;
                }

                if (brick != mCurrentBrick) {
                    mPreviousBrick = mCurrentBrick;
                    mCurrentBrick = brick;
                }
            }
        }
    }
    // private void OnMouseEnter()
	// {
    //     GetComponent<Renderer>().material.SetColor("_Color", Color.red);
	// }
	// private void OnMouseExit()
	// {
    //     GetComponent<Renderer>().material.SetColor("_Color", Color.green);
	// }
}
