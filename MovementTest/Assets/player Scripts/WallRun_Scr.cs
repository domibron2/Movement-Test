using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun_Scr : MonoBehaviour
{
    [SerializeField] float CHANGETHISVALUE = 1;

    [Header("Refernces")]
    [SerializeField] Transform orientation;
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;

    [Header("Wall Running checks")]
    [SerializeField] float wallDistance = .5f;
    [SerializeField] float minimumJumpHeight = 1.5f;

    [Header("Wall Running forces")]
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunJumpForce;
    [SerializeField] float wallRunSpeed;

    [Header("Camera settings")]
    [SerializeField] float fov;
    [SerializeField] float wallRunfov;
    [SerializeField] float wallRunfovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;

    public float tilt { get; private set; }

    bool wallLeft = false;
    bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    bool canWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    void FixedUpdate()
    {
        WallRun();
    }

    void WallRun()
    {
        CheckWall();

        if (canWallRun())
        {
            if (wallLeft || wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    void StartWallRun()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, CHANGETHISVALUE, rb.velocity.y);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, CHANGETHISVALUE, rb.velocity.y);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }

        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        if (wallLeft)
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (wallRight)
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);

        rb.AddForce(orientation.forward * wallRunSpeed, ForceMode.Acceleration);
    }

    void StopWallRun()
    {
        rb.useGravity = true;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
