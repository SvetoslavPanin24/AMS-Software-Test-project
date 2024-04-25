using System;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    private Camera camera; // ������, ������� ��������� ������

    [Header("Move settings")]
    [SerializeField] private float speed2DMode; // �������� �������� � 2D ������
    [SerializeField] private float speed3DModeCoefficient; // ����������� �������� � 3D ������
    [SerializeField] private float speedOfRotation; // ����������� �������� �������� � 3D ������
    [SerializeField] private Vector3 cameraMoveLimit; // ����������� �� ����������� ������

    private Vector3 movement; // ������ �����������
    public Vector3 input; // ���� ������������

    [Header("Mode settings")]
    public bool is2DMode; // ���� 2D ������
    public bool is3DMode; // ���� 3D ������

    [Header("Action settings")]
    public static Action onSwitchTo2D; // �������� ��� ������������ � 2D �����
    public static Action onSwitchTo3D; // �������� ��� ������������ � 3D �����


    public float smoothTime = 0.2f; // ����� ����������� �������� ������
       private Vector2 currentRotation;


    private void Start()
    {
        camera = Camera.main; // �������� ������� ������
        is3DMode = true; // ������������� 3D ����� �� ���������
        input = Vector2.zero; // �������� ���� ������������
        Cursor.lockState = CursorLockMode.Confined; // ������������ ����������� ������� ��������� ����
        currentRotation = new Vector2(camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.x);
    }


    void Update()
    {
        movement = Vector3.zero; // �������� ������ �����������

        if (is3DMode)
        {
            MoveIn3D(); // ���� ������� 3D �����, �������� ��������������� �����
        }

        if (is2DMode)
        {
            MoveIn2D(); // ���� ������� 2D �����, �������� ��������������� �����
        }
    }

    private void MoveIn2D()
    {
        // ������������ ���� ������������ � ���������� ������ � ������������ � ���
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, speed2DMode);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, 0, speed2DMode);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(speed2DMode, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed2DMode, 0, 0);
        }
    }

    private void MoveIn3D()
    {
        Vector3 newPosition = Vector3.zero; // ����� ������� ������

        // ������������ ���� ������������ � ���������� ������ � ������������ � ���
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            if (mouseX != 0 || mouseY != 0)
            {
                mouseX *= speedOfRotation;
                mouseY *= speedOfRotation;

                input.x += mouseX;
                input.y -= mouseY;

                // ��������� ������� ������� ������
                currentRotation.x += mouseX;
                currentRotation.y -= mouseY;

                // ������������ ������
                transform.localRotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
            }
        }


        if (Input.GetKey(KeyCode.W))
        {
            movement += transform.forward.normalized / speed3DModeCoefficient;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement -= transform.forward.normalized / speed3DModeCoefficient;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement -= transform.right.normalized / speed3DModeCoefficient;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement += transform.right.normalized / speed3DModeCoefficient;
        }

        // ��������� ������� ������ � ������ �����������
        newPosition = transform.position + movement;
        newPosition.x = Mathf.Clamp(newPosition.x, -cameraMoveLimit.x, cameraMoveLimit.x);
        newPosition.y = Mathf.Clamp(newPosition.y, 5, cameraMoveLimit.y);
        newPosition.z = Mathf.Clamp(newPosition.z, -cameraMoveLimit.z, cameraMoveLimit.z);

        camera.transform.position = newPosition; // ��������� ����� ������� ������
    }

    public void SwitchTo2D()
    {
        // ����������� ������ � 2D �����
        transform.localRotation = Quaternion.Euler(90, 0, 0);
        camera.orthographic = true;
        camera.orthographicSize = 20.0f;
        is2DMode = true;
        is3DMode = false;

        onSwitchTo2D?.Invoke(); // �������� ��������������� ��������
    }

    public void SwitchTo3D()
    {
        // ����������� ������ � 3D �����
        camera.orthographic = false;
        is2DMode = false;
        is3DMode = true;
        onSwitchTo3D?.Invoke(); // �������� ��������������� ��������
        currentRotation = new Vector2(camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.x);
    }
}