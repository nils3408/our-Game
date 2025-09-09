
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Robot : Player
{

    private GameLogic game;

    public Robot(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, Texture2D special_move_texture1, int player, PlayerControls controls, GameLogic game)
              : base(graphicsDevice, position1, texture1, special_move_texture1, player, controls)
    {
        this.game = game;
    }

    public override void handle_input(float delta)
    {
        Vector2 BV = game.getBall().velocity;
        Vector2 BP = game.getBall().position;
        Vector2 RP = base.position;

        //Debug.WriteLine($"Ball Position: {BP}");

        if (BV.X < 0 && (BP.X+60) < RP.X)
        {

            moveLeft(delta);
        }
        else 
        {
            if(RP.X < 1600) moveRight(delta);
            
            if ( RP.X - BP.X < 10) {
                shoots_horizontal = true;

                Debug.WriteLine($"Ball Position: {BP}, Robot Position: {RP} ");
            }
        }

        if (BV.X > 0 &&  (BP.Y + 50 * BV.Y) < 300) {
            jump(delta);
        }

    }


    private void moveLeft(float delta)
    {
        if (MoveChange_powerup_in_use == false) { move_helper(delta, -1); }
        else { move_helper(delta, 1); }
    }
    private void moveRight(float delta)
    { 
        if (MoveChange_powerup_in_use == false)     { move_helper(delta, 1); }
        else                                        { move_helper(delta, -1);  }
    }
}