using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    private bool canRotate = false; // Controla si se permite la rotación

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(LockRotationForSeconds(0.5f)); // Bloquea la rotación por 2 segundos
    }

    // Update is called once per frame
    void Update()
    {
        if (!canRotate) return; // Evita modificar la rotación si está bloqueada

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private IEnumerator LockRotationForSeconds(float seconds)
    {
        // Fija la rotación a (0, 0, 0)
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        xRotation = 0f;
        yRotation = 0f;

        // Espera el tiempo especificado
        yield return new WaitForSeconds(seconds);

        // Permite la rotación
        canRotate = true;
    }
}