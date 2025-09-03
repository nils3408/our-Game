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



public class Panzer1PowerUp : PowerUp
{
    public Panzer1PowerUp(Ball ball, ContentManager content) : base(ball, content) {
        cooldown = 1f;
        powerUp_texture = content.Load<Texture2D> ("powerUp_textures/panzer1");
    }

    public override void activate(Player player, int powerUp)
    {
        //error prevention
        if (player == null)                  { return; }
        if (player.GameLogic_object == null) { return; }

        player.GameLogic_object.add_Panzer(new Vector2(player.position.X, player.position.Y+30), 
                                           player, 
                                           player.moving_direction
                                           );
    }
}