using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonVisualBehaviour : MonoBehaviour, IPointerUpHandler,IPointerDownHandler {

	RectTransform rectTransform;
    Button button;
	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
	}

	public void OnPointerUp(PointerEventData eventData)
    {
		ButtonUnused();
    }

	public void OnPointerDown(PointerEventData eventData)
    {
		ButtonUsed();
    }

	private void ButtonUsed()
	{
        if (button.interactable)
            rectTransform.localScale = 0.95f * Vector2.one;
        else
            rectTransform.localScale = Vector2.one;
    }

	private void ButtonUnused()
    {
		rectTransform.localScale = Vector2.one;
    }
}
