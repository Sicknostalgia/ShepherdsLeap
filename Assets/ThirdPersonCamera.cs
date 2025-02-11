using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("references")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public float rotationSpeed;
    public Vector3 moveCross;

    public Transform combatLookat;

    public GameObject basicCamera;
    public GameObject combatCamera;
    public GameObject topdownCamera;
    public CameraStyle cameraStyle;
    public enum CameraStyle
    {
        Basic,
        Combat,
        TopDown
    }
    private void OnDrawGizmos()
    {
        // Orientation (Cyan)
        if (orientation != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(orientation.position, 0.2f);
            Gizmos.DrawLine(transform.position, orientation.position);
        }
        // Player Object (Yellow)
        if (playerObj != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(playerObj.position, 0.25f);

            // Draw forward direction as a blue arrow
            Gizmos.color = Color.blue;
            Vector3 forwardPos = playerObj.position + playerObj.forward * 2f;
            Gizmos.DrawLine(playerObj.position, forwardPos);
            DrawArrowHead(playerObj.position, playerObj.forward, 2f, 0.3f);
        }
        // Rigidbody Position (Red)
        if (rb != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(rb.position, 0.2f);
        }

        // Rotation Speed Visualization (Magenta) - Represented as a circle
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, rotationSpeed * 0.1f);
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraStyle(CameraStyle.TopDown);
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);//ignore y axis
        orientation.forward = viewDir.normalized;

        if (cameraStyle == CameraStyle.Basic)
        {
            //rotate player object
            float horinzontalInput = Input.GetAxis("Horizontal");
            float verticalinput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalinput;
         moveCross = orientation.forward * verticalinput + orientation.right * horinzontalInput;
            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, moveCross.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (cameraStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookat.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            playerObj.forward = dirToCombatLookAt.normalized;
        }
        else if (cameraStyle == CameraStyle.TopDown)
        {

        }
    }
    // Draw an arrowhead at the end of the forward vector
    private void DrawArrowHead(Vector3 position, Vector3 direction, float length, float arrowSize)
    {
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 160, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -160, 0) * Vector3.forward;

        Gizmos.DrawLine(position + direction * length, position + direction * length + right * arrowSize);
        Gizmos.DrawLine(position + direction * length, position + direction * length + left * arrowSize);
    }


    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        basicCamera.SetActive(false);
        combatCamera.SetActive(false);
        topdownCamera.SetActive(false);

        if (newStyle == CameraStyle.Basic) basicCamera.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCamera.SetActive(true);
        if (newStyle == CameraStyle.TopDown) topdownCamera.SetActive(true);

        cameraStyle = newStyle;
    }
}
