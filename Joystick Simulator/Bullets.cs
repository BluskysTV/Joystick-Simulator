using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Joystick_Simulator
{
    public class Bullets : Sprite 
    {

        public Texture2D texture;

       

        //public Vector2 position;
        public Vector2 velocity;
       
        //public Vector2 origin;

        public bool isVisible;

        public Bullets(Texture2D newTexture, Vector2 position, Vector2 velocity)
            :base(newTexture, position)
        {
            this.velocity = velocity;
            isVisible = false;            
        }

        public void Update ()
        {
            Position += velocity;

        }
   
    }
}
