using UnityEngine;
using UnityEngine.UI; // ถ้ามี Text UI บอกปุ่มกด
using TMPro; // ถ้าใช้ TextMeshPro

public class PlayerInteract : MonoBehaviour
{
    [Header("Settings")]
    public float rayDistance = 3f;
    public LayerMask interactLayer;

    [Header("UI (Optional)")]
    public TextMeshProUGUI promptText; 

    private Interactable currentInteractable; 
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactLayer))
        {
            Interactable newInteractable = hit.collider.GetComponent<Interactable>();

            if (newInteractable != null)
            {
                if (currentInteractable != newInteractable)
                {
                    ClearCurrentInteractable(); 
                    currentInteractable = newInteractable;
                    currentInteractable.OnFocus(); 

                    if (promptText) promptText.text = currentInteractable.promptMessage;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.BaseInteract();
                }
                return;
            }
        }
        ClearCurrentInteractable();
    }

    void ClearCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
            if (promptText) promptText.text = ""; 
        }
    }
}