using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    // Флаги для отслеживания текущего режима отображения
    private bool is2DModeDisplaying;
    private bool is3DModeDisplaying;

    // Подписываемся на события переключения режимов при активации объекта
    private void OnEnable()
    {
        MainCameraScript.onSwitchTo2D += SwitchTo2DModeDisplaying;
        MainCameraScript.onSwitchTo3D += SwitchTo3DModeDisplaying;
    }

    // Отписываемся от событий при деактивации объекта
    private void OnDisable()
    {
        MainCameraScript.onSwitchTo2D -= SwitchTo2DModeDisplaying;
        MainCameraScript.onSwitchTo3D -= SwitchTo3DModeDisplaying;
    }

    // Устанавливаем режим 3D отображения по умолчанию при старте
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

    // Обновляем поворот объекта в каждом кадре
    void Update()
    {
        if (is3DModeDisplaying)
        {
            // Если активен режим 3D отображения, поворачиваем объект к камере
            transform.LookAt(Camera.main.transform);
        }
        if (is2DModeDisplaying)
        {
            // Устанавливаем нужный поворот для 2D режима
            transform.localRotation = Quaternion.Euler(-90, -180, 90);
        }
    }

    // Переключаемся в режим 2D отображения
    private void SwitchTo2DModeDisplaying()
    {       
        // Обновляем флаги режимов отображения
        is2DModeDisplaying = true;
        is3DModeDisplaying = false;
    }

    // Переключаемся в режим 3D отображения
    private void SwitchTo3DModeDisplaying()
    {
        // Обновляем флаги режимов отображения
        is2DModeDisplaying = false;
        is3DModeDisplaying = true;
    }
}
