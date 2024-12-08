using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale; // Store the original scale of the button
    public float hoverScaleMultiplier = 1.2f; // Factor by which the size increases on hover
    public float scaleSpeed = 10f; // Speed of the scaling effect

    private Vector3 targetScale; // The scale to interpolate towards

    private void Start()
    {
        // Save the original scale
        originalScale = transform.localScale;
        targetScale = originalScale; // Initial target scale
    }

    private void Update()
    {
        // Smoothly interpolate to the target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    // Called when the mouse enters the button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Set the target scale to the hover size
        targetScale = originalScale * hoverScaleMultiplier;
    }

    // Called when the mouse exits the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert the target scale to the original size
        targetScale = originalScale;
    }
}
