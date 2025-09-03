//nils 


/*-------------------------------------------------------------------------------------------------------------
 * PowerUp that are integrated in the Map
 * central managing point where we have access to all PowerUps    
 * 
 * ------------------------------------------------------------------------------------------------------------
 */

using System;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class PowerUpFactory
{
    Ball ball;
    private static Random random = new Random();
    enum PowerUps
    {
        FireballPowerUp,
        IceballPowerUp,
        BigPlayerPowerUp,
        SmallPlayerPowerUp,
        BigGoalPowerUp,
        StealingPowerUp,
        MoveChangePowerUp,
        Panzer2PowerUp
    }


    public PowerUpFactory(Ball ball1)
    {
        this.ball = ball1;
    }

    public PowerUp random_powerUP(ContentManager content)
    {
        //return a random PowerUp

        Array values = Enum.GetValues(typeof(PowerUps));
        PowerUps randomType = (PowerUps)values.GetValue(random.Next(values.Length));

        switch (randomType)
        {
            case PowerUps.FireballPowerUp:
                return new FireballPowerUp(ball, content);  
            case PowerUps.BigPlayerPowerUp:
                return new BigPlayerPowerUp(ball, content);
            case PowerUps.IceballPowerUp:
                return new IceballPowerUp(ball, content);
            case PowerUps.BigGoalPowerUp:
                return new BiggerGoalPowerUp(ball, content);
            case PowerUps.SmallPlayerPowerUp:
                return new SmallPlayerPowerUp(ball, content);
            case PowerUps.StealingPowerUp:
                return new StealingPowerUp(ball, content);
            case PowerUps.MoveChangePowerUp:
                return new MoveChangePowerUp(ball, content);
            case PowerUps.Panzer2PowerUp:
                return new Panzer2PowerUp(ball, content);
            default:
                throw new NotImplementedException($"PowerUp '{randomType}' ist nicht implementiert.");
        }
    }
}