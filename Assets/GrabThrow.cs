using UnityEngine;
using UnityEngine.UI;

public class GrabThrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Transform holdPoint;
    [SerializeField]
    private Image reticle;

    [Header("Settings")]
    [SerializeField]
    private LayerMask grabbableLayer;
    [SerializeField]
    private float pickupRange = 3f;
    [SerializeField]
    private float power = 6f;

    [Header("Reticle Color")]
    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField]
    private Color targetColor = Color.yellow;
    [SerializeField]
    private Color grabColor = Color.cyan;

    private Rigidbody heldRigidbody;
    private bool heldUseGravity;

    void Update()
    {
        UpdateReticle();

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(heldRigidbody == null)
            {
                TryGrab();
            }
            else
            {
                Release();
            }
        }

        if(heldRigidbody != null && Input.GetMouseButtonDown(0))
        {
            Throw();
        }
    }

    private void FixedUpdate()
    {
        if(heldRigidbody == null)
        {
            return;
        }

        heldRigidbody.MovePosition(holdPoint.position);
        heldRigidbody.MoveRotation(holdPoint.rotation);
    }

    private void TryGrab()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if(!Physics.Raycast(ray, out RaycastHit hit, pickupRange, grabbableLayer, QueryTriggerInteraction.Ignore))
        {
            return;
        }

        Rigidbody target = hit.rigidbody;
        if(target == null)
        {
            return;
        }

        heldRigidbody = target;
        heldUseGravity = heldRigidbody.useGravity;

        heldRigidbody.isKinematic = true;
        heldRigidbody.useGravity = false;
    }

    private void Release()
    {
        if(heldRigidbody == null)
        {
            return;
        }

        heldRigidbody.isKinematic = false;
        heldRigidbody.useGravity = heldUseGravity;
        heldRigidbody = null;
    }

    private void Throw()
    {
        if(heldRigidbody == null)
        {
            return;
        }

        Rigidbody target = heldRigidbody;
        Release();

        Vector3 direction = playerCamera.transform.forward + playerCamera.transform.up * 0.1f;

        target.AddForce(direction.normalized * power, ForceMode.Impulse);
    }

    private void UpdateReticle()
    {
        if(reticle == null)
        {
            return;
        }

        if(heldRigidbody != null)
        {
            reticle.color = grabColor;
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        bool canGrab = Physics.Raycast(ray, out RaycastHit hit, pickupRange, grabbableLayer, QueryTriggerInteraction.Ignore) && hit.rigidbody != null;
        reticle.color = canGrab ? targetColor : normalColor;
    }
}