
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Robot : Player
{

    private GameLogic game;

    private Vector2 prevBallPos;

    public Robot(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, Texture2D special_move_texture1, int player, PlayerControls controls, GameLogic game)
              : base(graphicsDevice, position1, texture1, special_move_texture1, player, controls)
    {
        this.game = game;
    }

    public override void handle_input(float delta)
    {
        if (game.getBall().velocity.X < 0)
        {
            
        }
        else
        {

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