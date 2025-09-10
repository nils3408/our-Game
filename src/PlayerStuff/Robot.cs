
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Math = System.Math;
using Random = System.Random;
using DateTime = System.DateTime;

public class Robot : Player
{

    private GameLogic game;
    Random rnd = new Random();


    public Robot(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, Texture2D shoot_texture1, Texture2D special_move_texture1, int player, PlayerControls controls, GameLogic game)
              : base(graphicsDevice, position1, texture1, shoot_texture1, special_move_texture1, player, controls)
    {
        this.game = game;
        moving_direction = -1;
    }

    public override void handle_input(float delta)
    {
        Vector2 BV = game.getBall().velocity;
        Vector2 BP = game.getBall().position;
        Vector2 RP = base.position;
        Vector2 OP = otherPlayer.position;

        //Debug.WriteLine($"Ball Position: {BP}");

        if (BV.X < 0 && (BP.X + 60) < RP.X)
        {

            moveLeft(delta);
        }
        else
        {
            if (RP.X < 1600) moveRight(delta);


        }  
        
        if (Math.Abs(RP.X - BP.X) < 120)  // Bot is next to the ball and right of the ball
            {
                reset();   //reset before shoot, to make sure the right shoot will be executed
                shoot();

                //System.Diagnostics.Debug.WriteLine($"Condition to shoot is true");
            }
        //Debug.WriteLine($"Ball Position: {BP}, Robot Position: {RP} ");

        // Springt wenn der ball sich nach rechts bewegt, der ball nicht mehr als 230 pixel links vom roboter ist
        // und wenn der Ball auf mit der aktuellen beschleunigugn unter 450 (umgekerhtes Koordinatensystem) sein wird an der Stelle wo der Roboter steht
        if (BV.X > 0 && RP.X - BP.X < 230 && (BP.Y + BV.Y * ((RP.X - BP.X)/BV.X) ) < 450 )
        {
            jump(delta);
        }
        //System.Diagnostics.Debug.WriteLine($"Ball Y prediction : {(BP.Y + BV.Y * ((RP.X - BP.X) / (BV.X)+ 0.003f))}");

        activate_PowerUp();

    }


    private void moveLeft(float delta)
    {
        if (MoveChange_powerup_in_use == false) { move_helper(delta, -1); }
        else { move_helper(delta, 1); }
    }
    private void moveRight(float delta)
    {
        if (MoveChange_powerup_in_use == false) { move_helper(delta, 1); }
        else { move_helper(delta, -1); }
    }

    private void shoot()
    {
        int shootType = getShootType();

        switch (shootType)
        {
            case 1: { shoots_horizontal = true; break; }
            case 2: { shoots_lupfer = true; break; }
            case 3: { shoots_diagonal = true; break; }
        }
    }


    private int getShootType()
    {
        Vector2 BV = game.getBall().velocity;
        Vector2 BP = game.getBall().position;
        Vector2 RP = base.position;
        Vector2 OP = otherPlayer.position;

        if (((OP.Y + otherPlayer.RectangleHeight) <= game.getBall().position.Y && otherPlayer.velocity.Y < 0) //player is over the ball and is still going up
           || BP.Y <= OP.Y-70 )  // or wenn the Ball is above the other player                                                           
        {
            //System.Diagnostics.Debug.WriteLine($"Robot shoots horizontal");
            return 1;
        }
        float PlayerBallDistance = Math.Abs(OP.X - BP.X);
        if (PlayerBallDistance < 180 && BP.X > 550 && OP.X > 400) {
            //System.Diagnostics.Debug.WriteLine($"Robot shoots diagonal");
            return 3;
        }

        //System.Diagnostics.Debug.WriteLine($"Robot shoots random");
        int value = rnd.Next(1, 4); // 2 inklusiv, 4 exklusiv --> liefert 2 oder 3
        return value;
    }

    public void reset()
    {
        //reset shoots as id does not have the reset_when_key_is_released() as the other player do
        shoots_diagonal = false;
        shoots_horizontal = false;
        shoots_lupfer = false;
    }



    //--------------------------------------------------------
    // powerups
    public void activate_PowerUp()
    {
        //only activate powerup if robot contains it for >= xyz seconds
        float xyz = 3;

        if ((DateTime.Now - time_powerUp1_got_collected).TotalSeconds >= xyz)
        {
           activate_powerUp_helper(powerup1, 1);
        }

        if ((DateTime.Now - time_powerUp2_got_collected).TotalSeconds >= xyz)
        {
            activate_powerUp_helper(powerup2, 2);
        }
    }


    public void activate_powerUp_helper(PowerUp? powerUP, int slot)
    {
        //handle the switch case
        if (powerUP == null) { return; }

        switch (powerUP)
        {
            case BigPlayerPowerUp _:
                if (BigPlayerPowerUpActivationMakesSense())
                    activate_powerUP(slot);
                break;

            case SmallPlayerPowerUp _:
                if (SmallPlayerPowerUpActivationMakesSense())
                    activate_powerUP(slot);
                break;

            case FireballPowerUp _:
                if (FirePowerUpActivationMakesSense())
                    activate_powerUP(slot);
                break;

            case BiggerGoalPowerUp _:
                if (BiggerGoalPowerUpActivationMakesSense())
                    activate_powerUP(slot);
                break;

            case StealingPowerUp _:
                if (StealingPowerUpActivationMakesSense())
                    activate_powerUP(slot);
                break;

            case MoveChangePowerUp _:
                activate_powerUP(slot);
                break;

            case IceballPowerUp _:
                if(IcePowerUpActivationMakesSense())
                    activate_powerUP(slot);
                break;

            default:
                break;
        }
    }


    public bool BigPlayerPowerUpActivationMakesSense()
    {
        // makes sense if not currently activated
        // size <= normal_size -> he ist not big currently
        return RectangleHeight <= RectangleHeight_copy;
    }

    public bool SmallPlayerPowerUpActivationMakesSense()
    {
        // makes sense if not currently activated
        // otherPlayer.size >= normal_size -> he ist not small currently
        return otherPlayer.RectangleHeight >= RectangleHeight_copy;
    }

    public bool FirePowerUpActivationMakesSense()
    {
        //makes sense if
        //      the moves into the direction of the oponents goal 
        //      icepowerUp is not activated

        bool rightDirection = game.getBall().velocity.X < 0;
        bool icePowerup     = game.getBall().ice_powerUp_in_use;
        return (rightDirection && (!icePowerup)) ;
    }

    public bool BiggerGoalPowerUpActivationMakesSense()
    {
        //makes sense if the ball goes into the goal of the oponent
        bool rightDirection = game.getBall().velocity.X < 0;
        return rightDirection;
    }

    public bool StealingPowerUpActivationMakesSense()
    {
        //makes sense if the oponents contains at least one powerup the bot can steal
        return (otherPlayer.powerup1 != null || otherPlayer.powerup2 != null);
    }

    public bool IcePowerUpActivationMakesSense()
    {
        // makes sense when
        //      the ball goes into the direction of the own goal
        //      ball is closer to the own goal than bot 
        //      but ball is not in the goal
        //      no player intersects with it -> bug prevention 

        bool rightDirection = game.getBall().velocity.X > 0;
        bool isCloser       = game.getBall().position.X > position.X;
        bool notInGoal      = game.getBall().position.X < game.getRightGoalPosition().X - 20;


        bool noIntersection1 = !(game.getBall().getRect().Intersects(currentRect));
        bool noIntersection2 = !(game.getBall().getRect().Intersects(otherPlayer.currentRect));

        return (rightDirection && isCloser && notInGoal && noIntersection1 && noIntersection2);
    }


}
