using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInputManager : InputManager {

    [SerializeField]
    private string _playerAxisPrefix = "";

    [SerializeField]
    private int _maxNumberOfPlayers = 1;

    [Header("Unity Axis Mappings")]
    [SerializeField]
    private string XAxis = "X";
    [SerializeField]
	private string SquareAxis = "Square";
    [SerializeField]
	private string TriangleAxis = "Triangle";
    [SerializeField]
	private string CircleAxis = "Circle";
    [SerializeField]
	private string L1Axis = "L1";
    [SerializeField]
	private string L2Axis = "L2";
    [SerializeField]
	private string R1Axis = "R1";
    [SerializeField]
	private string R2Axis = "R2";
    [SerializeField]
	private string MoveHorizontalAxis = "MoveHorizontal";
    [SerializeField]
	private string MoveVerticalAxis = "MoveVertical";
    [SerializeField]
	private string ShareAxis = "Share";
    [SerializeField]
	private string OptionsAxis = "Options";
    [SerializeField]
	private string L3Axis = "L3";
    [SerializeField]
	private string R3Axis = "R3";
    [SerializeField]
	private string PSAxis = "PS";
    [SerializeField]
	private string TouchpadAxis = "Touchpad";
    [SerializeField]
	private string LeftHorizontalAxis = "LeftHorizontal";
    [SerializeField]
	private string LeftVerticalAxis = "LeftVertical";
    [SerializeField]
	private string RightHorizontalAxis = "RightHorizontal";
    [SerializeField]
	private string RightVerticalAxis = "RightVertical";
    [SerializeField]
	private string DPadHorizontalAxis = "DPadHorizontal";
    [SerializeField]
	private string DPadVerticalAxis = "DPadVertical";

    private Dictionary<int, string>[] _actions;

    protected override void Awake() {
        base.Awake();
        if (InputManager.instance != null) {
            isEnabled = false;
            return;
        }

        SetInstance(this);

        _actions = new Dictionary<int, string>[_maxNumberOfPlayers];

        for (int i = 0; i < _maxNumberOfPlayers; i++) {
            Dictionary<int, string> playerActions = new Dictionary<int, string>();
            _actions[i] = playerActions;
            string prefix = !string.IsNullOrEmpty(_playerAxisPrefix) ? _playerAxisPrefix + i : string.Empty;
            AddAction(InputAction.X, prefix + XAxis, playerActions);
            AddAction(InputAction.Square, prefix + SquareAxis, playerActions);
            AddAction(InputAction.Triangle, prefix + TriangleAxis, playerActions);
            AddAction(InputAction.Circle, prefix + CircleAxis, playerActions);
            AddAction(InputAction.L1, prefix + L1Axis, playerActions);
            AddAction(InputAction.L2, prefix + L2Axis, playerActions);
            AddAction(InputAction.R1, prefix + R1Axis, playerActions);
            AddAction(InputAction.R2, prefix + R2Axis, playerActions);
            AddAction(InputAction.MoveHorizontal, prefix + MoveHorizontalAxis, playerActions);
            AddAction(InputAction.MoveVertical, prefix + MoveVerticalAxis, playerActions);
            AddAction(InputAction.Share, prefix + ShareAxis, playerActions);
            AddAction(InputAction.Options, prefix + OptionsAxis, playerActions);
            AddAction(InputAction.L3, prefix + L3Axis, playerActions);
            AddAction(InputAction.R3, prefix + R3Axis, playerActions);
            AddAction(InputAction.PS, prefix + PSAxis, playerActions);
            AddAction(InputAction.Touchpad, prefix + TouchpadAxis, playerActions);
            AddAction(InputAction.LeftHorizontal, prefix + LeftHorizontalAxis, playerActions);
            AddAction(InputAction.LeftVertical, prefix + LeftVerticalAxis, playerActions);
            AddAction(InputAction.RightHorizontal, prefix + RightHorizontalAxis, playerActions);
            AddAction(InputAction.RightVertical, prefix + RightVerticalAxis, playerActions);
            AddAction(InputAction.DPadHorizontal, prefix + DPadHorizontalAxis, playerActions);
            AddAction(InputAction.DPadVertical, prefix + DPadVerticalAxis, playerActions);
        }
    }

    private static void AddAction(InputAction action, string actionName, Dictionary<int, string> actions) {
        if (string.IsNullOrEmpty(actionName)) {
            return;
        }
        actions.Add((int)action, actionName);
    }

    public override bool GetButton(int playerID, InputAction action) {
        bool value = Input.GetButton(_actions[playerID][(int)action]);
        return value;
    }

    public override bool GetButtonDown(int playerID, InputAction action) {
        bool value = Input.GetButtonDown(_actions[playerID][(int)action]);
        return value;
    }

    public override bool GetButtonUp(int playerID, InputAction action) {
        bool value = Input.GetButtonUp(_actions[playerID][(int)action]);
        return value;
    }

    public override float GetAxis(int playerID, InputAction action) {
        float value = Input.GetAxisRaw(_actions[playerID][(int)action]);
        return value;
    }
}
