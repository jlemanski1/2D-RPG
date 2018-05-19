using System.Collections;
using System;
using UnityEngine;

public static class RectTransformAnimationExtensions {

    public static Tweener AnchorTo(this RectTransform rt, Vector3 pos) {
        return AnchorTo(rt, pos, Tweener.DefaultDuration);
    }

    public static Tweener AnchorTo(this RectTransform rt, Vector3 pos, float duration) {
        return AnchorTo(rt, pos, duration, Tweener.DefaultEquation);
    }

    public static Tweener AnchorTo(this RectTransform rt, Vector3 pos, float duration, Func<float, float, float, float> equation) {
        RectTransformAnchorPositionTweener tweener = rt.gameObject.AddComponent<RectTransformAnchorPositionTweener>();
        tweener.startValue = rt.anchoredPosition;
        tweener.endValue = pos;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
	
}
