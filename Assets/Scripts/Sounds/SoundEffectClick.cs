using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


public class SoundEffectClick : MonoBehaviour, IPointerUpHandler
{	    
    public void OnPointerUp(PointerEventData eventData)
    {
		CSSoundManager.instance.PlaySound(0);
    } 
}

