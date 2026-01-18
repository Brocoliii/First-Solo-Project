using UnityEngine;
using UnityEngine.Events; 

public class Interactable : MonoBehaviour
{
    [Header("Settings")]
    public string promptMessage = "กด E เพื่อใช้งาน";
    public bool useHighlight = true; 
    public Color highlightColor = Color.yellow; 
    public AudioClip interactSound;

    [Header("สิ่งที่เกิดขึ้นเมื่อกด")]
    public UnityEvent onInteract;

    private Renderer myRenderer;
    private Color originalColor;
    private bool isHovered = false;

        void Start()
    {
        myRenderer = GetComponent<Renderer>();
        if (myRenderer != null)
        {
            if (myRenderer.material.HasProperty("_BaseColor")) 
                originalColor = myRenderer.material.GetColor("_BaseColor");
            else
                originalColor = myRenderer.material.color;
        }
    }
    public void BaseInteract()
    {
        if (interactSound != null)
        {
            AudioSource.PlayClipAtPoint(interactSound, transform.position);
        }

        onInteract.Invoke();
    }

    public void OnFocus()
    {
        if (!useHighlight || myRenderer == null || isHovered) return;

        isHovered = true;
        if (myRenderer.material.HasProperty("_BaseColor"))
            myRenderer.material.SetColor("_BaseColor", highlightColor);
        else
            myRenderer.material.color = highlightColor;
    }

    public void OnLoseFocus()
    {
        if (!useHighlight || myRenderer == null || !isHovered) return;

        isHovered = false;
        if (myRenderer.material.HasProperty("_BaseColor"))
            myRenderer.material.SetColor("_BaseColor", originalColor);
        else
            myRenderer.material.color = originalColor;
    }
}