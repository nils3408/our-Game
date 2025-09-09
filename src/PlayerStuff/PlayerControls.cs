using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public enum PlayerAction
{
    Left = 0,
    Right = 1,
    Jump = 2,
    Special = 3,
    PowerUp_1 = 4,
    PowerUp_2 = 5,
    Lupfer = 6,
    Diagonal = 7,
    Horizontal = 8
    }
public class PlayerControls
{


    //maps the Controlls to the Keys, standartwerte, müssen angepasst werden
    private Keys[] controlKeys = new Keys[9];
    private Dictionary<Keys, PlayerAction> keyToAction = new Dictionary<Keys, PlayerAction>();

    public Keys Left{ get; private set; }
    public Keys Right{ get; private set; }
    public Keys Jump{ get; private set; }
    public Keys Special{ get; private set; }
    public Keys PowerUp_1{ get; private set; }
    public Keys PowerUp_2{ get; private set; }
    public Keys Lupfer{get; private set; }
    public Keys Diagonal{get; private set; }
    public Keys Horizontal{get; private set; }

    public PlayerControls(Keys[] keys)
    {
        this.controlKeys = keys;
        for (int i = 0; i < keys.Length; i++)
        {
            this.keyToAction[keys[i]] = (PlayerAction)i;
        }
        syncPublicFields(controlKeys);
    }

    public void editKey(PlayerAction action, Keys key)
    {
        controlKeys[(int)action] = key;
        if (keyToAction.ContainsKey(key))
        {
            keyToAction[key] = action;
        }
        syncPublicFields(controlKeys);
    }


    public static PlayerControls getStandartLeft()
    {
        Keys[] keys = { Keys.A, Keys.D, Keys.W, Keys.E, Keys.R, Keys.F, Keys.C, Keys.S, Keys.X};
        return new PlayerControls(keys);
    }

    //standart besetzung wie zuvor in GameLogic.handel_player_movement()
    public static PlayerControls getStandartRight()
    {
        Keys[] keys = { Keys.J, Keys.L, Keys.I, Keys.O, Keys.P, Keys.OemPeriod, Keys.M, Keys.K, Keys.OemComma};
        return new PlayerControls(keys);
    }

    public PlayerAction GetAction(Keys key)
    {
        return keyToAction[key];
    }

    public Keys getKey(PlayerAction action)
    {
        return controlKeys[(int)action];
    }

    private void syncPublicFields(Keys[] keys)
    {
        Left = keys[(int)PlayerAction.Left];
        Right = keys[(int)PlayerAction.Right];
        Jump = keys[(int)PlayerAction.Jump];
        Special = keys[(int)PlayerAction.Special];
        PowerUp_1 = keys[(int)PlayerAction.PowerUp_1];
        PowerUp_2 = keys[(int)PlayerAction.PowerUp_2];
        Lupfer = keys[(int)PlayerAction.Lupfer];
        Diagonal = keys[(int)PlayerAction.Diagonal];
        Horizontal = keys[(int)PlayerAction.Horizontal];
    }
}

public class ControlsEditor : StackContainer
{
    public PlayerControls controls { get; }


    public ControlsEditor(PlayerControls controls) : base()
    {
        this.controls = controls;

        AddKeyEditor(PlayerAction.Left, "Move Left");

        AddKeyEditor(PlayerAction.Right, "Move Right");

        AddKeyEditor(PlayerAction.Jump, "Jump Up");

        AddKeyEditor(PlayerAction.Special, "Special Move");

        AddKeyEditor(PlayerAction.PowerUp_1, "PowerUp 1");

        AddKeyEditor(PlayerAction.PowerUp_2, "PowerUp 2");

        AddKeyEditor(PlayerAction.Lupfer, "Lupfer");

        AddKeyEditor(PlayerAction.Diagonal, "Shoot high");

        AddKeyEditor(PlayerAction.Horizontal, "Shoot horiz.");
    }
    
    private void AddKeyEditor(PlayerAction action, String text)
    {
        KeyEditor editor = new KeyEditor(controls.getKey(action), text);
        editor.selector.OnKeySelected += () =>
        {
            controls.editKey(action, editor.selector.key);
        };
        base.Add(editor);
    }
    

}

public class KeyEditor : HorizontalContainer
{
    String description;

    public Point textfieldSize;
    public Point keyFieldSize;

    public KeySelector selector { get; }

    public KeyEditor(Keys key, String description) : base()
    {
        this.description = description;

        textfieldSize = new Point(350, 100);
        keyFieldSize = new Point(200, 100);

        base.SetSpacing(7);

        Textfield text = new Textfield(description, textfieldSize);
        text.backgroundColor = Color.White;
        text.IsSetToDrawOutline = true;
        base.Add(text);

        selector = new KeySelector(keyFieldSize, key);
        selector.IsSetToDrawOutline = true;
        base.Add(selector);

    }

}