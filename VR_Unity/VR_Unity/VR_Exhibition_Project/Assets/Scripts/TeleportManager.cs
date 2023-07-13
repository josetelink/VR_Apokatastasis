using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    // Es crea un patró Singleton per poder accedir a aquest script 

    public static TeleportManager Instance;

    public GameObject Player;
    private GameObject _lastTeleportPoint;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

   
    // Crearem un mètode per desactivar el punto de teletransportació al qual hem anat
    public void DisableTeleportPoint(GameObject teleportPoint)
    {
        if (_lastTeleportPoint != null)
        {
            _lastTeleportPoint.SetActive(true);
        }

        //Desactivar el punt de teletransportació on estem
        teleportPoint.SetActive(false);
        _lastTeleportPoint = teleportPoint;

#if UNITY_EDITOR
        Player.GetComponent<CardboardSimulator>().UpdatePlayerPositonSimulator();
#endif
    }
}
