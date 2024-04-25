using System;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    private Camera camera; // Камера, которой управляет скрипт

    [Header("Move settings")]
    [SerializeField] private float speed2DMode; // Скорость движения в 2D режиме
    [SerializeField] private float speed3DModeCoefficient; // Коэффициент скорости в 3D режиме
    [SerializeField] private float speedOfRotation; // Коэффициент скорости поворота в 3D режиме
    [SerializeField] private Vector3 cameraMoveLimit; // Ограничения на перемещение камеры

    private Vector3 movement; // Вектор перемещения
    public Vector3 input; // Ввод пользователя

    [Header("Mode settings")]
    public bool is2DMode; // Флаг 2D режима
    public bool is3DMode; // Флаг 3D режима

    [Header("Action settings")]
    public static Action onSwitchTo2D; // Действие при переключении в 2D режим
    public static Action onSwitchTo3D; // Действие при переключении в 3D режим


    public float smoothTime = 0.2f; // Время сглаживания движения камеры
       private Vector2 currentRotation;


    private void Start()
    {
        camera = Camera.main; // Получаем главную камеру
        is3DMode = true; // Устанавливаем 3D режим по умолчанию
        input = Vector2.zero; // Обнуляем ввод пользователя
        Cursor.lockState = CursorLockMode.Confined; // Ограничиваем перемещение курсора пределами окна
        currentRotation = new Vector2(camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.x);
    }


    void Update()
    {
        movement = Vector3.zero; // Обнуляем вектор перемещения

        if (is3DMode)
        {
            MoveIn3D(); // Если активен 3D режим, вызываем соответствующий метод
        }

        if (is2DMode)
        {
            MoveIn2D(); // Если активен 2D режим, вызываем соответствующий метод
        }
    }

    private void MoveIn2D()
    {
        // Обрабатываем ввод пользователя и перемещаем камеру в соответствии с ним
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
        Vector3 newPosition = Vector3.zero; // Новая позиция камеры

        // Обрабатываем ввод пользователя и перемещаем камеру в соответствии с ним
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

                // Обновляем текущий поворот камеры
                currentRotation.x += mouseX;
                currentRotation.y -= mouseY;

                // Поворачиваем камеру
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

        // Обновляем позицию камеры с учетом ограничений
        newPosition = transform.position + movement;
        newPosition.x = Mathf.Clamp(newPosition.x, -cameraMoveLimit.x, cameraMoveLimit.x);
        newPosition.y = Mathf.Clamp(newPosition.y, 5, cameraMoveLimit.y);
        newPosition.z = Mathf.Clamp(newPosition.z, -cameraMoveLimit.z, cameraMoveLimit.z);

        camera.transform.position = newPosition; // Применяем новую позицию камеры
    }

    public void SwitchTo2D()
    {
        // Переключаем камеру в 2D режим
        transform.localRotation = Quaternion.Euler(90, 0, 0);
        camera.orthographic = true;
        camera.orthographicSize = 20.0f;
        is2DMode = true;
        is3DMode = false;

        onSwitchTo2D?.Invoke(); // Вызываем соответствующее действие
    }

    public void SwitchTo3D()
    {
        // Переключаем камеру в 3D режим
        camera.orthographic = false;
        is2DMode = false;
        is3DMode = true;
        onSwitchTo3D?.Invoke(); // Вызываем соответствующее действие
        currentRotation = new Vector2(camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.x);
    }
}