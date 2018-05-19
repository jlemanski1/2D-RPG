using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Defines target positions and the n works with LayoutAnchor to snap or move
/// them when necessary
/// </summary>
[RequireComponent(typeof(LayoutAnchor))]
public class Panel : MonoBehaviour {
    [SerializeField] List<Position> positionList;
    Dictionary<string, Position> positionMap;
    LayoutAnchor anchor;

    public Position CurrentPosition { get; private set; }
    public Tweener Transition { get; private set; }
    public bool InTransition { get { return Transition != null; } }

    private void Awake() {
        anchor = GetComponent<LayoutAnchor>();
        positionMap = new Dictionary<string, Position>(positionList.Count);
        for (int i = positionList.Count - 1; i >= 0; i--) {
            AddPosition(positionList[i]);
        }
    }

    public Position this[string name] {
        get {
            if (positionMap.ContainsKey(name))
                return positionMap[name];
            return null;
        }
    }

    public void AddPosition(Position p) {
        positionMap[p.name] = p;
    }

    public void RemovePosition(Position p) {
        if (positionMap.ContainsKey(p.name))
            positionMap.Remove(p.name);
    }

    public Tweener SetPosition(string positionName, bool animated) {
        return SetPosition(this[positionName], animated);
    }

    public Tweener SetPosition(Position p, bool animated) {
        CurrentPosition = p;
        if (CurrentPosition == null)
            return null;

        if (InTransition)
            Transition.easingControl.Stop();

        if (animated) {
            Transition = anchor.MoveToAnchorPosition(p.anchor, p.parentAnchor, p.offset);
            return Transition;
        }
        else {
            anchor.SnapToAnchorPosition(p.anchor, p.parentAnchor, p.offset);
            return null;
        }
    }

    private void Start() {
        if (CurrentPosition == null && positionList.Count > 0)
            SetPosition(positionList[0], false);
    }

    [Serializable]
    public class Position {
        public string name;
        public TextAnchor anchor;
        public TextAnchor parentAnchor;
        public Vector2 offset;

        public Position(string name) {
            this.name = name;
        }

        public Position(string name, TextAnchor anchor, TextAnchor parentAnchor) : this(name) {
            this.anchor = anchor;
            this.parentAnchor = parentAnchor;
        }

        public Position(string name, TextAnchor anchor, TextAnchor parentAchor, Vector2 offset) :
            this(name, anchor, parentAchor) {
            this.offset = offset;
        }

    }
}



