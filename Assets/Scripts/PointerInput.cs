using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.UI;

public class PointerInput : MonoBehaviour
{
    [SerializeField] private Image pointer;
    public float selectionTimer = 3f;
    public float speed = 3.5f;

    private float X;
    private float Y;
    private bool trigger = false;

    private Coroutine pointerSelection = null;
    // Start is called before the first frame update
    void Start()
    {
        pointer.fillAmount = 1.0f;
#if UNITY_EDITOR
        GetComponent<TrackedPoseDriver>().enabled = false;
#endif
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0)) // MOUSE CONTROL FOR DEBUG
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
        }
#endif

        RaycastHit hit;// Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("UI_btn") && !trigger)
            {
                trigger = true;
                pointerSelection = StartCoroutine(PointerSelection(hit.collider.GetComponent<Button>()));
            }
        }
        else
        {
            if (trigger)
            {
                ResetPointer();
                trigger = false;
            }
        }
    }

    public void ResetPointer()
    {
        if (pointerSelection != null)
        {
            StopCoroutine(pointerSelection);
            pointerSelection = null;
        }
        pointer.fillAmount = 1.0f;
    }

    IEnumerator PointerSelection(Button btn)
    {
        float time = selectionTimer;
        while (pointer.fillAmount > 0.0f)
        {
            pointer.fillAmount -= 1.0f / time * Time.deltaTime;
            yield return null;
        }
        btn.onClick.Invoke();
        ResetPointer();
    }
}
