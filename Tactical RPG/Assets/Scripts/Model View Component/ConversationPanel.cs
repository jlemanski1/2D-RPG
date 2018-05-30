using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ConversationPanel : MonoBehaviour {

    public Text message;
    public Image speaker;       // Speaker sprite
    public GameObject arrow;    // Next page arrow
    public Panel panel;

    private void Start() {
        Vector3 pos = arrow.transform.localPosition;    // Use current position as base point
        arrow.transform.localPosition = new Vector3(pos.x, pos.y + 5, pos.z);   // Move up by 5
        Tweener t = arrow.transform.MoveToLocal(new Vector3(pos.x, pos.y - 5, pos.z),
            0.5f, EasingEquations.EaseInQuad);                                  // Tween down by 5
        t.easingControl.loopType = EasingControl.LoopType.PingPong;             // Replay in reverse
        t.easingControl.loopCount = -1;     // Keep tweening indefinitely as there is more to read
    }

    public IEnumerator Display (SpeakerData sd) {
        speaker.sprite = sd.speaker;
        speaker.SetNativeSize();

        for (int i = 0; i < sd.messages.Length; i++) {
            message.text = sd.messages[i];
            arrow.SetActive(i + 1 < sd.messages.Length);
            yield return null;
        }
    }

}
