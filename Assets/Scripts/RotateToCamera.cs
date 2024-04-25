using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    // ����� ��� ������������ �������� ������ �����������
    private bool is2DModeDisplaying;
    private bool is3DModeDisplaying;

    // ������������� �� ������� ������������ ������� ��� ��������� �������
    private void OnEnable()
    {
        MainCameraScript.onSwitchTo2D += SwitchTo2DModeDisplaying;
        MainCameraScript.onSwitchTo3D += SwitchTo3DModeDisplaying;
    }

    // ������������ �� ������� ��� ����������� �������
    private void OnDisable()
    {
        MainCameraScript.onSwitchTo2D -= SwitchTo2DModeDisplaying;
        MainCameraScript.onSwitchTo3D -= SwitchTo3DModeDisplaying;
    }

    // ������������� ����� 3D ����������� �� ��������� ��� ������
    private void Start()
    {
        if (Camera.main.GetComponent<MainCameraScript>().is3DMode)
        {
            is3DModeDisplaying = true;
            is2DModeDisplaying = false;
        }
        if (Camera.main.GetComponent<MainCameraScript>().is2DMode)
        {
            is3DModeDisplaying = false;
            is2DModeDisplaying = true;
        }
    }

    // ��������� ������� ������� � ������ �����
    void Update()
    {
        if (is3DModeDisplaying)
        {
            // ���� ������� ����� 3D �����������, ������������ ������ � ������
            transform.LookAt(Camera.main.transform);
        }
        if (is2DModeDisplaying)
        {
            // ������������� ������ ������� ��� 2D ������
            transform.localRotation = Quaternion.Euler(-90, -180, 90);
        }
    }

    // ������������� � ����� 2D �����������
    private void SwitchTo2DModeDisplaying()
    {       
        // ��������� ����� ������� �����������
        is2DModeDisplaying = true;
        is3DModeDisplaying = false;
    }

    // ������������� � ����� 3D �����������
    private void SwitchTo3DModeDisplaying()
    {
        // ��������� ����� ������� �����������
        is2DModeDisplaying = false;
        is3DModeDisplaying = true;
    }
}
