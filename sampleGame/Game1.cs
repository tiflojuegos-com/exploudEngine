﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tfj.exploudEngine;

namespace sampleGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        eSoundEngine engine;
        eSound sonido;
        eInstance instancia;
        float x = 0;
        int cooldown = 0;
        Random randomsito;
        eMusic yoshimusic;
        eMusic fossilMusic;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            engine = new eSoundEngine();
            randomsito = new Random();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.sonido = engine.loadSound("beam.mp3");
            this.instancia = this.sonido.play3d(0, 0, 0, loopMode.bidiLoop);
            this.yoshimusic = engine.loadMusic("yoshi.mp3");
            this.fossilMusic = engine.loadMusic("fossil.mp3");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            engine = null;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if(cooldown>0)
            {
                cooldown--;
            }

            this.instancia.x += 0.01f;
            if(this.instancia.x>10 )
            {
                this.instancia.x = -5;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Keyboard.GetState().IsKeyDown( Keys.Z) && cooldown <= 0)
            {
                eInstance yoshyPlaying = engine.loadSound("yoshi.wav").play3d(1, 0,0);
                yoshyPlaying.radius = 1.0f;
                yoshyPlaying.minDistance = 2.0f;
                yoshyPlaying.maxDistance = 50.0f;
                yoshyPlaying.oculusAtenuation = true;
                cooldown = 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X) && cooldown <= 0)
            {
                int pan = randomsito.Next(-10, 11);
                engine.loadSound("coin.wav").play((float)pan/10);
                cooldown = 2;
            }
            if(Keyboard.GetState().IsKeyDown( Keys.Y))
            {
                yoshimusic.play();
            }
            if(Keyboard.GetState().IsKeyDown( Keys.F))
            {
                fossilMusic.play();
            }
            if(Keyboard.GetState().IsKeyDown( Keys.Space))
            {
                yoshimusic.stop();
                fossilMusic.stop();

            }
            if(Keyboard.GetState().IsKeyDown( Keys.Left) && cooldown <= 0)
            {
                this.engine.listener.rotation -= 5;
                cooldown = 3;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && cooldown <= 0)
            {
                this.engine.listener.rotation += 5;
                cooldown = 3;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && cooldown <= 0)
            {
                this.engine.listener.z += 0.3F;
                cooldown = 15;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) && cooldown <= 0)
            {
                this.engine.listener.z -= 0.3f;
                cooldown = 15;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D) && cooldown <= 0)
            {
                this.engine.listener.x += 0.3F;
                cooldown = 15;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) && cooldown <= 0)
            {
                this.engine.listener.x -= 0.3f;
                cooldown = 15;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F3) && cooldown <= 0)
            {
                this.engine.defaultMusicGroup.volume -= 0.1f;
                cooldown = 5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F4) && cooldown <= 0)
            {
                this.engine.defaultMusicGroup.volume += 0.1f;
                cooldown = 5;
            }



            // TODO: Add your update logic here
            engine.update();
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
