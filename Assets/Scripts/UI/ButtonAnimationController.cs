using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimationController : MonoBehaviour
{
    public Image img;
    public TMPro.TextMeshProUGUI buttonText;
    public Color idleTextColor;
    public Color highlightedTextColor;

    public void OnHighlight() {
        img.color = idleTextColor;
        buttonText.color = highlightedTextColor;
    }

    public void OnIdle() {
        img.color = highlightedTextColor;
        buttonText.color = idleTextColor;
    }
}
