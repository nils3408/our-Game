using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class PlayerThies
{
    public string name;
    //int id;

    Vector2 velocity = new Vector2(0, 0);

    const float gravity = 10;
    const float jump_impuls = 50;
    Vector2 footForce = new Vector2(5, 10);

    PlayerControls controls = null;


    public PlayerThies(string name, PlayerControls controls)
    {
        this.name = name;
        this.controls = controls;
    }

    public void setControlls(PlayerControls controlls)
    {
        this.controls = controlls;
    }

    public Vector2 GetMoveDir(KeyboardState input)
    {

        Dictionary<int, Vector2> actionMovement = new Dictionary<int, Vector2>
        {
            [(int)PlayerAction.Left] = new Vector2(-1, 0),    //Left
            [(int)PlayerAction.Right] = new Vector2(1, 0),     //Right
            [(int)PlayerAction.Jump] = new Vector2(0, 1),     //Up
            //[(int)PlayerAction.Down] = new Vector2(0, -1)     //Down
        };

        Vector2 result = new Vector2(0, 0);
        Keys[] pushedKeys = input.GetPressedKeys();
        for (int i = 0; i < pushedKeys.Length; i++)
        {
            int action = (int)controls.GetAction(pushedKeys[i]);
            result += actionMovement[action];
        }
        return result;
    }

    public void handle_player_movement(GameTime gameTime)
    {
        KeyboardState input = Keyboard.GetState();

    }


}