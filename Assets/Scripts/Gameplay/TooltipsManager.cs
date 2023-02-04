using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipsManager : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;
    [SerializeField] private TextMeshProUGUI tooltipText;

    public void ShowTooltip(Vector3 position, string text)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        tooltipText.text = text;
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
