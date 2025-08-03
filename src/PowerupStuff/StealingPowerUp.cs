//nils 



using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class StealingPowerUp : PowerUp
{
    public StealingPowerUp(Ball ball, ContentManager content) : base(ball, content) {
        cooldown = 10f; //value sollte egal sein
        powerUp_texture = content.Load<Texture2D>("powerUp_textures/dieb");
    }

    
    public override void activate(Player player, int powerUp)
    {
        
        if (powerUp != 1 && powerUp != 2) { return;  } //error handlinf, palyer only has slots 1 and 2


        PowerUp stolen = null;

        if (player.otherPlayer.powerup1 != null)
        {
            stolen = player.otherPlayer.powerup1;
            Debug.WriteLine("hier i am 1");
            player.otherPlayer.powerup1 = null;
        }

        else if (player.otherPlayer.powerup2 != null)
        {
            stolen = player.otherPlayer.powerup2;
            Debug.WriteLine("hier i am 2");
            player.otherPlayer.powerup2 = null;
        }

        if (powerUp == 1)
        {
            player.powerup1 = stolen;
            Debug.WriteLine("hier i am 3");
        }
        else
        {
            player.powerup2 = stolen;
            Debug.WriteLine("hier i am 4");
        }
    }
}
