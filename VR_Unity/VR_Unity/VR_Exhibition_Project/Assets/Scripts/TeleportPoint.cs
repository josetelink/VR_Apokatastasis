using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportPoint : MonoBehaviour
{
    // Crearem tres events
    public UnityEvent OnTeleportEnter;
    public UnityEvent OnTeleport;
    public UnityEvent OnTeleportExit;

    // Start is called before the first frame update
    void Start()
    {
        // Desactivarem la fletxa on s'indica la direcci� on es far� la teletransportaci�
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Es crean les tres funcions que cridarem amb el Gaze. Des de ah� cridarem els events
    public void OnPointerEnterXR()
    {
        OnTeleportEnter?.Invoke();
    }

    public void OnPointerClickXR()
    {
        ExecuteTeleportation();
        OnTeleport?.Invoke();

        // Desactivar el punt de teletransportaci�
        TeleportManager.Instance.DisableTeleportPoint(gameObject); 
    }

    public void OnPointerExitXR()
    {
        OnTeleportExit?.Invoke();
    }

    // Aquest m�tode executa la teletransportaci�
    private void ExecuteTeleportation()
    {
        // Cal tenir acc�s al Player. Ser� el Player qui ser� teletransportat
        GameObject player = TeleportManager.Instance.Player;
        player.transform.position = transform.position;

        //Rotaci� de la c�mera en direcci� a la fletxa
        Camera camera = player.GetComponentInChildren<Camera>();
        float rotationY = transform.rotation.eulerAngles.y - camera.transform.localEulerAngles.y;
        player.transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
