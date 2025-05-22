using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

namespace Flappy
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont spriteFont;
        Texture2D pixel;

        int flappyPositionX = 100;
        int score = 0;
        int flappyPositionY = 100;
        int flappyLaangeur = 50;
        bool estEnVie = true;
        int flappyHuteur = 50;
        float flappyVitesse = 0;

        List<int[]> tuyau = new List<int[]>();
        int largeurPipe = 50;
        int trou = 100;
        Random random = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            flappyPositionX = 50;
            flappyPositionY = 200;
            tuyau.Add(new int[] { 400, random.Next(120, 380) }); 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("test");
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            var clavier = Keyboard.GetState();
            if (clavier.IsKeyDown(Keys.Escape))
                Exit();
            if (clavier.IsKeyDown(Keys.R))
                reset();

            if (!estEnVie && clavier.IsKeyDown(Keys.Space))
                Initialize();

            if (!estEnVie)
                return;

            if (clavier.IsKeyDown(Keys.Space))
                flappyVitesse = -4.5f;

            flappyVitesse += 0.2f;
            flappyPositionY += (int)flappyVitesse;

            for (int i = 0; i < tuyau.Count; i++)
                tuyau[i][0] -= 2;

            if (tuyau[^1][0] < 200)
            {
                tuyau.Add(new int[] { 400, random.Next(120, 380) });
            }
            Rectangle flappy = new Rectangle(flappyPositionX, flappyPositionY, flappyLaangeur, flappyHuteur);

            foreach (var tuy in tuyau)
            {
                Rectangle tuyauHaut = new Rectangle(tuy[0], 0, largeurPipe, tuy[1] - trou);
                Rectangle tuyaubas = new Rectangle(tuy[0], tuy[1] + trou, largeurPipe, 500 - (tuy[1] + trou));
                if (flappy.Intersects(tuyauHaut) || flappy.Intersects(tuyaubas))
                    estEnVie = false;
            }
            if (tuyau.Count > score && flappyPositionX > tuyau[score][0] + largeurPipe)
            {
                score++;
   
            }
            if (flappyPositionY < 0 || flappyPositionY + flappyHuteur > 500)
                estEnVie = false;
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (var tuy in tuyau)
            {
                Rectangle tuyeauDuHaut = new Rectangle(tuy[0], 0, largeurPipe, tuy[1] - trou / 2);
                _spriteBatch.Draw(pixel, tuyeauDuHaut, Color.Green);

                Rectangle tuyeauDuBas = new Rectangle(tuy[0], tuy[1] + trou, largeurPipe, 500 - (tuy[1] + trou));
                _spriteBatch.Draw(pixel, tuyeauDuBas, Color.Green);
            }
            _spriteBatch.Draw(pixel, new Rectangle(flappyPositionX, flappyPositionY, flappyLaangeur, flappyHuteur), Color.Orange);

            _spriteBatch.Draw(pixel, new Rectangle(0, 495, 400, 5), Color.SaddleBrown);
            _spriteBatch.DrawString(spriteFont, "score" + score, new Vector2(50, 50), Color.White);
            _spriteBatch.End();
            base.Update(gameTime);
            base.Draw(gameTime);
        }
        private void reset()
        {
            flappyPositionX = 50;
            flappyPositionY = 200;
            flappyVitesse = 0;
            tuyau.Clear();
            tuyau.Add(new int[] { 400, random.Next(120, 380) });
            score = 0;
            estEnVie = true;
        }
    }
}
