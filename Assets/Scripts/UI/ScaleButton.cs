using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleButton : Button, IPointerExitHandler, IPointerEnterHandler
{
    private bool isSelected = false;
    public float scaleCoef = 1.05f;

    private Vector3 startScale = new Vector3();

    protected override void Awake()
    {
        base.Awake();
        startScale = transform.localScale;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!isSelected)
        {
            transform.localScale *= scaleCoef;
            isSelected = true;
        }
        MainMenu.audioManager?.StopSound(AudioManager.Sound.Click);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (isSelected)
        {
            gameObject.transform.localScale = startScale;
            isSelected = false;
        }
    }
}
