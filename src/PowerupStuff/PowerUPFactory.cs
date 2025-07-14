//nils 


/*-------------------------------------------------------------------------------------------------------------
 * PowerUp that are integrated in the Map
 *     
 * 
 * ------------------------------------------------------------------------------------------------------------
 */

using System;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework;
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
        Fireball
    }


    public PowerUpFactory(Ball ball1)
    {
        this.ball = ball1;
    }

    public PowerUp random_powerUP()
    {
        Array values = Enum.GetValues(typeof(PowerUps));
        PowerUps randomType = (PowerUps)values.GetValue(random.Next(values.Length));

        switch (randomType)
        {
            case PowerUps.Fireball:
                return new Fireball(ball);
            // Weitere PowerUps hier behandeln
            default:
                throw new NotImplementedException($"PowerUp '{randomType}' ist nicht implementiert.");
        }
    }
}