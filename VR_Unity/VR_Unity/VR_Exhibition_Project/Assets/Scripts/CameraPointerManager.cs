using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    // Es crea un patró Singleton per cridar aquesta classe des de qualsevol altra classe
    public static CameraPointerManager Instance;


    [SerializeField] private GameObject pointer;
    [SerializeField] private float maxDistancePointer = 4.5f;  // Distancia des de la càmera al Pointer

    [Range(0, 1)]
    [SerializeField] private float disPointerObject = 0.95f;

    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;


    private readonly string interactableTag = "Interactable"; // Objectes amb els qual s'interactua
    private float scaleSize = 0.025f; // Mida del Pointer
    [HideInInspector]
    public Vector3 hitPoint;

    // Així ens assegurem que només existeix una instància de CameraPointerManager
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Aquí es fa la subscripció al GazeSelection. 
    private void Start()
    {
        GazeManager.Instance.OnGazeSelection += GazeSelection;
    }


    private void GazeSelection()
    {
        _gazedAtObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
    }

    public void Update()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            hitPoint = hit.point;
            if (_gazedAtObject != hit.transform.gameObject)
            {

                _gazedAtObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnterXR", null, SendMessageOptions.DontRequireReceiver);
                GazeManager.Instance.StartGazeSelection();
            }
            //Es compara si el objecte té el Tag definit
            if (hit.transform.CompareTag(interactableTag))
            {
                PointerOnGaze(hit.point); // Si es toca un objecte Interactable crida aquest mètode
            }
            else
            {
                // Si no es toca un objecte Interactable es crida a aquest Mètode
                PointerOutGaze();
            }
        }
        else
        {

            _gazedAtObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
            _gazedAtObject = null;
        }


        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedAtObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    // Mètode per tomer com origen el punt d'impacte
    private void PointerOnGaze(Vector3 hitPoint)
    {
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitPoint);
        pointer.transform.localScale = Vector3.one * scaleFactor;  // Pointer té la mateixa mida
        //Ubicar la posició del pare del Pointer
        pointer.transform.parent.position = CalculatePointerPosition(transform.position, hitPoint, disPointerObject);
    }

    // Mètode quan es toca un objecte que no és Interactable
    private void PointerOutGaze()
    {
        pointer.transform.localScale = Vector3.one * 0.1f;
        pointer.transform.parent.transform.localPosition = new Vector3(0, 0, maxDistancePointer);
        pointer.transform.parent.parent.transform.rotation = transform.rotation;
        GazeManager.Instance.CancelGazeSelection(); // Es cancela la selecció
    }

    // Càlcul matemàtic per ubicar la posició del Pointer
    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        float x = p0.x + t * (p1.x - p0.x);
        float y = p0.y + t * (p1.y - p0.y);
        float z = p0.z + t * (p1.z - p0.z);

        return new Vector3(x, y, z);
    }
}
