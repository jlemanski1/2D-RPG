using System;
using System.Collections;
using UnityEngine;

public class RectTransformAnchorPositionTweener : Vector3Tweener {

    RectTransform rectTransform;

    protected override void Awake() {
        base.Awake();
        rectTransform = transform as RectTransform;
    }

    protected override void OnUpdate(object sender, EventArgs e) {
        base.OnUpdate(sender, e);
        rectTransform.anchoredPosition = currentValue;
    }

}
