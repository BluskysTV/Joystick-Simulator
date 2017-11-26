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

        public float Rotation;

        Vector2 originalVelocity;
        //public Vector2 position;
        public Vector2 velocity;
       
        //public Vector2 origin;

        public bool isVisible;

        public Bullets(Texture2D newTexture, Vector2 position, Vector2 velocity, float rotation)
            :base(newTexture, position)
        {
            Rotation = rotation - 90f;
            originalVelocity = velocity;
            isVisible = false;     
        }

        public void Update ()
        {
            //Rotation += 0.5f;
            velocity.X = (float)(Math.Cos(MathHelper.ToRadians(Rotation)) * originalVelocity.X - Math.Sin(MathHelper.ToRadians(Rotation)) * originalVelocity.Y);
            velocity.Y = (float)(Math.Sin(MathHelper.ToRadians(Rotation)) * originalVelocity.X + Math.Cos(MathHelper.ToRadians(Rotation)) * originalVelocity.Y);
            Position += velocity;

        }
   
    }
}
