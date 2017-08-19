using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joystick_Simulator
{
    public class Asteroid : Sprite
    {
        public Vector2 velocity;
        public bool isVisible;
        public bool enteredScreen = false;
    
        Random random;

        public Asteroid(Texture2D newTexture, float speed, Viewport viewport, Vector2 shipLocation)
            : base(newTexture, new Vector2 (0,0))
        {
            random = new Random();
            origin = new Vector2 (Texture.Height, Texture.Width) / 2;

            int side = random.Next(1, 5);
            
            if(side == 1)
            {
                Position = new Vector2(random.Next(0, viewport.Width), -Texture.Height);
            }
            if(side == 2)
            {
                Position = new Vector2(-Texture.Width, random.Next(0, viewport.Height));
            }
            if(side == 3)
            {
                Position = new Vector2(random.Next(0, viewport.Width), viewport.Height + Texture.Height);
            }
            if(side == 4)
            {
                Position = new Vector2(Texture.Width + viewport.Width, random.Next(0, viewport.Height));
            }

            velocity = shipLocation - Position;
            velocity.Normalize();
            velocity *= speed;
            
            
            
            isVisible = false;
        }

        public void Update()
        {
            Position += velocity;
            rotation += 0.1f;
        }

    }
}