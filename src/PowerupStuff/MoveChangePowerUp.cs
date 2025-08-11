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



public class MoveChangePowerUp: PowerUp
{
    public MoveChangePowerUp(Ball ball, ContentManager content) : base(ball, content) {
        cooldown = 7f;
        powerUp_texture = content.Load<Texture2D>("powerUp_textures/movement_changing");
    }

    
    public override void activate(Player player, int powerUp)
    {
        player.otherPlayer.MoveChange_powerup_in_use = true;
        player.otherPlayer.activation_time_MoveChange_powerup = DateTime.Now;
        player.otherPlayer.cooldown_MoveChange_powerup = cooldown;
    }

}