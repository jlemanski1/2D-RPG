using System.Collections;
using System;
using UnityEngine;


/// <summary>
/// Handles the process of a conversation. Makes sure the appropriate panel is used and 
/// positioned correctly, tweening the panel into view, showing hte speaker and the
/// speaker's messages one at a time based on user input, tweening the panel out when
/// the speaker is finished speaking, and then tweening in a new panel for a new speaker
/// </summary>
public class ConversationController : MonoBehaviour {

    // Panel Positions
    const string ShowTop = "Show Top";
    const string ShowBottom = "Show Bottom";
    const string HideTop = "Hide Top";
    const string HideBottom = "Hide Bottom";

    [SerializeField] ConversationPanel leftPanel;
    [SerializeField] ConversationPanel rightPanel;

    public static event EventHandler completeEvent;

    Canvas canvas;

    IEnumerator conversation;   // Steps through the speaker and messages in a conversation
    Tweener transition;


    /// <summary>
    /// Set up references, set default panel position off screen, disable canvas
    /// </summary>
    private void Start() {
        canvas = GetComponentInChildren<Canvas>();
        if (leftPanel.panel.CurrentPosition == null)
            leftPanel.panel.SetPosition(HideBottom, false);
        if (rightPanel.panel.CurrentPosition == null)
            rightPanel.panel.SetPosition(HideBottom, false);
        canvas.gameObject.SetActive(false);
    }


    public void Show(ConversationData data) {
        canvas.gameObject.SetActive(true);
        conversation = Sequence(data);
        conversation.MoveNext();
    }


    public void Next() {
        if (conversation == null || transition != null)
            return;

        conversation.MoveNext();
    }


    /// <summary>
    /// Loops over all the speakers in a conversation, in a nested loop, iterates over each
    /// speaker's messages via another IEnumerator from the current panel
    /// </summary>
    /// <param name="data">ConversationData</param>
    IEnumerator Sequence (ConversationData data) {
        for (int i = 0; i < data.speakerDataList.Count; i++) {
            SpeakerData sd = data.speakerDataList[i];

            ConversationPanel currentPanel = (sd.anchor == TextAnchor.UpperLeft ||
                sd.anchor == TextAnchor.MiddleLeft || sd.anchor == TextAnchor.LowerLeft) ?
                leftPanel : rightPanel;
            IEnumerator presenter = currentPanel.Display(sd);
            presenter.MoveNext();

            string show, hide;
            if (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.UpperCenter ||
                sd.anchor == TextAnchor.UpperRight) {
                show = ShowTop;
                hide = HideTop;
            }
            else {
                show = ShowBottom;
                hide = HideBottom;
            }

            currentPanel.panel.SetPosition(hide, false);
            MovePanel(currentPanel, show);

            yield return null;     // Pause in the sequence 

            while (presenter.MoveNext())
                yield return null;

            MovePanel(currentPanel, hide);
            transition.easingControl.completedEvent += delegate (object sender, EventArgs e) {
                conversation.MoveNext();
            };

            yield return null;      // Pause in the sequence
        }

        canvas.gameObject.SetActive(false);
        if (completeEvent != null)
            completeEvent(this, EventArgs.Empty);
    }


    /// <summary>
    /// Move panel offscreen
    /// </summary>
    private void MovePanel(ConversationPanel obj, string pos) {
        transition = obj.panel.SetPosition(pos, true);
        transition.easingControl.duration = 0.5f;
        transition.easingControl.equation = EasingEquations.EaseOutQuad;
    }
}
