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

        public void SetDirection(Viewport viewport)
        {
            int side = random.Next(1, 5);

            if (side == 1)
            {
                Position = new Vector2(random.Next(0, viewport.Width), -Texture.Height);
            }
            if (side == 2)
            {
                Position = new Vector2(-Texture.Width, random.Next(0, viewport.Height));
            }
            if (side == 3)
            {
                Position = new Vector2(random.Next(0, viewport.Width), viewport.Height + Texture.Height);
            }
            if (side == 4)
            {
                Position = new Vector2(Texture.Width + viewport.Width, random.Next(0, viewport.Height));
            }
        }

        public Asteroid(Texture2D newTexture, float speed, Viewport viewport, Vector2 shipLocation, Random random)
            : base(newTexture, new Vector2 (0,0))
        {
            this.random = random;
            origin = new Vector2 (Texture.Height, Texture.Width) / 2;

            SetDirection(viewport);

            velocity = shipLocation - Position;
            velocity.Normalize();
            velocity *= speed;
            
            isVisible = false;
        }


        public Asteroid(Texture2D newTexture, float speed, Vector2 oldVelocity, Vector2 startPosition, Random random)
            : base(newTexture, new Vector2(0, 0))
        {
            this.random = random;
            origin = new Vector2(Texture.Height, Texture.Width) / 2;

            Position = startPosition;

            velocity = oldVelocity;
            velocity.Normalize();
            velocity *= speed;
            float angle = (float)Math.Atan2(velocity.X, velocity.Y);
            angle += (float)(random.NextDouble() * Math.PI / 2 - (Math.PI / 12) - 1250);
            velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;
            isVisible = false;
        }



        public void Update()
        {
            Position += velocity;
            rotation += 0.1f;
        }

    }
}