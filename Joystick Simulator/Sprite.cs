using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Joystick_Simulator
{
    public class Sprite
    {
        public SpriteEffects SpriteEffects
        {
            get { return spriteEffects; }
            set { spriteEffects = value; }
        }
        public SpriteEffects spriteEffects;
        public Texture2D Texture;
        public Vector2 Position;
        public Color Tint;
        public Rectangle HitBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); }

        }

        public Vector2 origin;
        public float Layer = 1;
        public float rotation;
        private Color color;

        public Sprite(Texture2D texture, Vector2 position, Color tint, float layer, float rotation)
        {
            Texture = texture;
            Position = position;
            Tint = tint;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            //HitBox = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            Layer = layer;
            this.rotation = rotation;
        }

        public Sprite(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public Sprite(Vector2 position, Texture2D texture, Color color) : this(texture, position)
        {
            this.color = color;
        }

        public virtual void Update(GameTime gameTime)
        {
            //HitBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, HitBox, null, Color.White, rotation, origin, SpriteEffects.None, Layer);
        }
    }
}
