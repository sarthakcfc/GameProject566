﻿using System;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;
using SlimDX.Design;
using SlimDX.RawInput;
using SlimDX.Multimedia;
using SlimDX.DirectSound;
//using SlimDX.DirectInput;
using SlimDX.XInput;

namespace GameProject566
{

	public class GameStart
	{

		//Our graphics device
		static SlimDX.Direct3D9.Device device9;

		//We use this to load sprites.
		static Sprite sprite;
		static Sprite sprite2;
        static Sprite sprite3;
		static Sprite sprite4;

        //tiles
        //static Texture tiles;

        //array of tiles
        static Texture[,] bgTiles = new Texture[15, 15];

		//Absolute starting location for player

		static float characterX = 420;
		static float characterY = 300;

        //Beginning location for tile
        static float tileX = 0;
        static float tileY = 0;

		static float tileX2 = 0;
		static float tileY2 = 0;

        //fileLocation for tiles
        static string tiles = "..\\..\\sprites\\tile.png";
		static string wall = "..\\..\\sprites\\Wall.png";
        static string entryTile = "..\\..\\sprites\\entry.png";
        static string exitTile = "..\\..\\sprites\\exit.png";
        // Beginning location for monster
        static float monster1X = 300;
        static float monster1Y = 240;

        //object for player
        static PlayerChar player = new PlayerChar(null, characterX, characterY,0,0);
        //gets all the sprite location for the player
        static string pback = "..\\..\\sprites\\pback.png";
        static string pback1 = "..\\..\\sprites\\pback1.png";
        static string pfront = "..\\..\\sprites\\pfront.png";
        static string pfront1 = "..\\..\\sprites\\pfront1.png";
        static string pleft = "..\\..\\sprites\\pleft.png";
        static string pleft1 = "..\\..\\sprites\\pleft1.png";
        static string pright = "..\\..\\sprites\\pright.png";
        static string pright1 = "..\\..\\sprites\\pright1.png";
        static bool changePlayerBack = false;
        static bool changePlayerFront = false;
        static bool changePlayerLeft = false;
        static bool changePlayerRight = false;

		static SoundBuffer music;


        //object for monster
		static Monsterchar monsterChar = new Monsterchar(null, monster1X, monster1Y,0,0);
        //gets all the sprite location for the monster
		static string monsterCharSprite = "..\\..\\sprites\\monster.png";

        // create new form
        static RenderForm form;

        //Random Number Generator
        static Random rand = new Random();

        //object for graphics
        static Graphics graphics = new Graphics();

        //enum for status
		static GameStatus status = GameStatus.mainMenu;

		static Tile[,] worldTiles;

		const int WORLDSIZE = 100; //<- Grid size

		public static void Main ()
		{
			//using allows cleanup of form afterwards
			/* using -> New concept, Basically it creates objects that if disposable will
			* get rid of the object when no longer being managed.
			* The rest creates a standard windows form that we tell the application to run.
			*/
			using (form = new RenderForm ("Dreadnought KamZhor")) {
				

				//Window resolution is 1024 x 768
				form.Width = 1024;
				form.Height = 768;
				//No resizing
				form.FormBorderStyle = FormBorderStyle.Fixed3D;
				form.MaximizeBox = false;


				Icon icon = Graphics.createIcon ();

				//set the form's icon.
				form.Icon = icon;

				//Create our device, textures and sprites

				device9 = graphics.initializeGraphics (form);

                //initialize player
				//player1 = graphics.createPlayer (pback, device9);

                player.texture = (Graphics.createTexture(device9, pback));
				player.health = 10;
                //initialize monster
                //monster1 = graphics.createMonster(device9);
				monsterChar.texture = (Graphics.createTexture(device9, monsterCharSprite));


				//Intialize the world
				World world = new World ();
				world.wall = Graphics.createTexture (device9, wall);
				world.tile = Graphics.createTexture (device9, tiles);


				//create world grid
				worldTiles = world.makeWorld (WORLDSIZE);

				player.xGridLocation = 6;
				player.yGridLocation = 8;

				//Make starting room.
				Tile[,] startingRoom = world.makeStartingRoom (player);

				//place the room on the world grid.
				worldTiles = world.PlaceRoomOnWorld (worldTiles, startingRoom, 50);


				//


                //initialize tiles
						
                //fill the array with tiles
				/*
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 15; y++)
                    {
						bgTiles[x, y] = Graphics.createTexture(device9, tiles);
                    }
                }
					*/
				sprite = new Sprite (device9);
                sprite2 = new Sprite(device9);
                sprite3 = new Sprite(device9);
                sprite4 = new Sprite(device9);

				//Gimme da keyboards
				SlimDX.RawInput.Device.RegisterDevice (UsagePage.Generic, UsageId.Keyboard, SlimDX.RawInput.DeviceFlags.None);
				SlimDX.RawInput.Device.KeyboardInput += new EventHandler <KeyboardInputEventArgs> (Device_keyboardInput);

                //Mouse
                SlimDX.RawInput.Device.RegisterDevice(UsagePage.Generic, UsageId.Mouse, SlimDX.RawInput.DeviceFlags.None);
                SlimDX.RawInput.Device.MouseInput +=new EventHandler<MouseInputEventArgs>(Device_mouseInput);



				//SOUND STUFF/////////////////
				DirectSound directsound = new DirectSound();
				directsound.IsDefaultPool = false;
				directsound.SetCooperativeLevel (form.Handle, SlimDX.DirectSound.CooperativeLevel.Priority);
				WaveStream wave = new WaveStream("..\\..\\sprites\\music1.wav");

				SoundBufferDescription description = new SoundBufferDescription();
				description.Format = wave.Format;
				description.SizeInBytes = (int)wave.Length;
				description.Flags = BufferFlags.ControlVolume;

				// Create the buffer.
				music = new SecondarySoundBuffer(directsound, description);

				byte[] data = new byte[description.SizeInBytes];
				wave.Read(data, 0, (int)wave.Length);
				music.Write(data, 0,SlimDX.DirectSound.LockFlags.None);


				music.Volume = 0;

				//music.Play(0, PlayFlags.Looping);

				////////////////////////////////////



				//create main menu textures

				Graphics.createMainMenuTextures (device9);


				//Application loop

				MessagePump.Run (form, GameLoop);

				//Dispose no longer in use objects.
				Cleanup ();
			}
		}
			

		private static void GameLoop ()
		{

			//Logic then render then loop
			GameLogic ();
			RenderFrames ();

			//Example change to offset to move picture accross

		}


		private static void GameLogic ()
		{
			//z += 0.0001f;
			//This is where would place game logic for a game
		}


		//Sprites and textures CANNOT be created here, as it must retrieve textures
		private static void RenderFrames ()
		{
		
			//Clear the whole screen
			device9.Clear (ClearFlags.Target, Color.GhostWhite, 1.0f, 0);

			//Render whole frame
			device9.BeginScene ();

            sprite.Begin(SpriteFlags.AlphaBlend);
            sprite2.Begin(SpriteFlags.AlphaBlend);
            sprite3.Begin(SpriteFlags.AlphaBlend);
            sprite4.Begin(SpriteFlags.AlphaBlend);

			//not sure why we need this yet...
			SlimDX.Color4 color = new SlimDX.Color4 (Color.White);

			if (status == GameStatus.mainMenu)
            {
				Graphics.renderMainMenu (color, device9, sprite);
            }

            //Console.WriteLine(status);
			if (status == GameStatus.map)
            {
				renderGameRoom(color);
            }

			//end render
			sprite.End ();
			sprite2.End ();
            sprite3.End();
            sprite4.End();

			// End the scene.
			device9.EndScene ();

			// Present the backbuffer contents to the screen.
			device9.Present ();

		}
			

        public static void renderGameRoom(SlimDX.Color4 color)
        {
			status = GameStatus.map;
            //renders sprite for tile and player


            //renders tile texture
            makeTiles(sprite, color);

            //renders player texture

            sprite.Transform = Matrix.Translation(player.xLocation, player.yLocation, 0);
            sprite.Draw(player.texture, color);


            //renders monster sprite

			sprite2.Transform = Matrix.Translation(monsterChar.xLocation, monsterChar.yLocation, 0);
			sprite2.Draw(monsterChar.texture, color);

            //Translate the sprite with a 3d matrix with no z change.
            //return currStatus;
        }
    
        private static void makeTiles(Sprite sprite, SlimDX.Color4 color)
        {

            tileX = 0;
            tileY = 0;
			for (int x = 0; x < WORLDSIZE ; x++)
            {
                tileX += 60;
                tileY = 0;
				for (int y = 0; y < WORLDSIZE; y++)
                {
                    tileY += 60;

					if (worldTiles[x,y].texture != null) {
						sprite.Transform = Matrix.Translation (worldTiles [x, y].xLocation + tileX2, worldTiles [x, y].yLocation + tileY2, 0);
						sprite.Draw (worldTiles [x, y].texture, color);

						//System.Console.Out.WriteLine (worldTiles [x, y].xLocation + " y: " + worldTiles [x, y].yLocation);
					}
					if (worldTiles [x, y].wObject != null) {
						if (worldTiles [x, y].wObject.texture != null && worldTiles [x, y].wObject.texture != player.texture) {
							sprite.Transform = Matrix.Translation (worldTiles [x, y].xLocation + tileX2, worldTiles [x, y].yLocation + tileY2, 0);
							sprite.Draw (worldTiles [x, y].wObject.texture, color);
						}
					}
                }
            }
            
        }
			
        public static Boolean arrowOrNot(KeyboardInputEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Up: return true;
                case Keys.Down: return true;
                case Keys.Right: return true;
                case Keys.Left: return true;
            }
            return false;
        }

		public static void Device_mouseInput(object sender, MouseInputEventArgs m)
		{
			//Coordinates of the mouse point
			int cursorX = form.PointToClient(RenderForm.MousePosition).X;
			int cursorY = form.PointToClient(RenderForm.MousePosition).Y;

			if (status == GameStatus.mainMenu) {

				//Button positions
				if (m.ButtonFlags == MouseButtonFlags.LeftDown && cursorX >= 500 && cursorY >= 200 && cursorX <= 1000 && cursorY <= 310) {
					Console.WriteLine ("X Position: " + cursorX + "Y Position: " + cursorY);
					//Switch to game map
					status = GameStatus.map;
				} else if (m.ButtonFlags == MouseButtonFlags.LeftDown && cursorX >= 500 && cursorY >= 470 && cursorX <= 1000 && cursorY <= 580) {
					Console.WriteLine ("X Position: " + cursorX + " | Y Position: " + cursorY);
					//Exit the game
					Cleanup ();
				}

			}

		}

		public static void  Device_keyboardInput (object sender, KeyboardInputEventArgs e)
		{
			//Runs twice for some reason....
			//Console.Out.WriteLine ("Key pressed: " + e.Key + ". x value: " + p1.getXLocation() + ". y value: " + p1.getYLocation());
			//First if is probably redundant but whatever
			//Everything else is self explainatory.
			if (e.State == KeyState.Pressed)
			{
				if (e.Key == Keys.Down && worldTiles[player.xGridLocation,player.yGridLocation-1].wObject.texture == null 
					&& worldTiles[player.xGridLocation,player.yGridLocation-1].wObject != null)
				{
					//characterY = characterY + 60f;
					//monster1Y -= 60f;
					//m1.setYLocation(-60f);
					//monsterChar.move(0, -60f);
					tileY2 -= 60f;
					if (changePlayerFront)
						player.texture = (Graphics.createTexture(device9, pfront1));
					else
						player.texture = (Graphics.createTexture(device9, pfront));

					worldTiles [player.xGridLocation,player.yGridLocation].wObject = new WorldObject();
					player.yGridLocation = worldTiles [player.xGridLocation,player.yGridLocation - 1].yGrid;
	
					worldTiles [player.xGridLocation,player.yGridLocation].wObject = player;

					changePlayerFront = !changePlayerFront;
				}
				else if (e.Key == Keys.Up && worldTiles[player.xGridLocation,player.yGridLocation+1].wObject.texture == null 
					&& worldTiles[player.xGridLocation,player.yGridLocation+1].wObject != null)
				{
					//characterY = characterY - 60f;
					//monster1Y += 60f;
					//m1.setYLocation(60f);
				//	monsterChar.move(0, 60f);
					tileY2 += 60f;
					if (changePlayerBack)
						player.texture = (Graphics.createTexture(device9, pback1));
					else
						player.texture = (Graphics.createTexture(device9, pback));

					worldTiles [player.xGridLocation,player.yGridLocation].wObject = new WorldObject();
					player.yGridLocation = worldTiles [player.xGridLocation,player.yGridLocation + 1].yGrid;

					worldTiles [player.xGridLocation,player.yGridLocation].wObject = player;


					changePlayerBack = !changePlayerBack;
				}
				else if (e.Key == Keys.Left && worldTiles[player.xGridLocation-1,player.yGridLocation].wObject.texture == null 
					&& worldTiles[player.xGridLocation-1,player.yGridLocation].wObject != null)
				{

					//monster1X += 60f;
					//m1.setXLocation(60f);
				//	monsterChar.move(60f, 0);
					tileX2 += 60f;
					//characterX = characterX - 60f + tileX2;
					if (changePlayerLeft)
						player.texture = (Graphics.createTexture(device9, pleft1));
					else
						player.texture =(Graphics.createTexture(device9, pleft));
					changePlayerLeft = !changePlayerLeft;

					worldTiles [player.xGridLocation,player.yGridLocation].wObject = new WorldObject();
					player.xGridLocation = worldTiles [player.xGridLocation-1,player.yGridLocation].xGrid;

					worldTiles [player.xGridLocation,player.yGridLocation].wObject = player;

				}
				else if (e.Key == Keys.Right && worldTiles[player.xGridLocation+1,player.yGridLocation].wObject.texture == null 
					&& worldTiles[player.xGridLocation+1,player.yGridLocation].wObject != null)
				{
					//monster1X -= 60f;
					//m1.setXLocation(-60f);
				//	monsterChar.move(-60f, 0);
					tileX2 -= 60f;
					//characterX = characterX + 60f - tileX2;
					if (changePlayerRight)
						player.texture = (Graphics.createTexture(device9, pright1));
					else
						player.texture = (Graphics.createTexture(device9, pright));
					changePlayerRight = !changePlayerRight;

					worldTiles [player.xGridLocation,player.yGridLocation].wObject = new WorldObject();
					player.xGridLocation = worldTiles [player.xGridLocation+1,player.yGridLocation].xGrid;

					worldTiles [player.xGridLocation,player.yGridLocation].wObject = player;

				}

				if (arrowOrNot(e))
				{
					int XorY = rand.Next(1, 3);
					//Console.WriteLine(XorY);
					if (XorY == 1)
					{
						if (monsterChar.xLocation > player.xLocation)// && m1.getXLocation() <= (tileX + tileX2))//(monster1X > characterX && monster1X <= (tileX + tileX2))
						{
							//monster1X -= 60f;
							//m1.setXLocation(-60f);
							//monsterChar.move(-60f, 0);
							//Console.Out.WriteLine("C1: XorY: " + XorY + ". x value: " + m1.getXLocation() + ". Tile X + X2: " + (tileX + tileX2));
						}
						else if (monsterChar.xLocation < player.xLocation)// && m1.getXLocation() > (tileX + tileX2))//(monster1X < characterX && monster1X < (tileX + tileX2))
						{
							//monster1X += 60f;
							//m1.setXLocation(60f);
							//monsterChar.move(60f, 0);
							//Console.Out.WriteLine("C2: XorY: " + XorY + ". x value: " + m1.getXLocation() + ". Tile X + X2: " + (tileX + tileX2));
						}
						else
						{
							if (monsterChar.yLocation > player.yLocation)// && m1.getYLocation() <= (tileY + tileY2))//(monster1Y > characterY && monster1Y <= (tileY + tileY2))
							{
								// monster1Y -= 60f;
								//m1.setYLocation(-60f);
								//monsterChar.move(0, -60f);
								//Console.Out.WriteLine("C3: XorY: " + XorY + ". y value: " + m1.getYLocation() + ". Tile Y + Y2: " + (tileY + tileY2));
							}
							else if (monsterChar.yLocation < player.yLocation)// && m1.getYLocation() < (tileY + tileY2))//(monster1Y < characterY && monster1Y < (tileY + tileY2))
							{
								//monster1Y += 60f;
								//m1.setYLocation(60f);
							//	monsterChar.move(0, 60f);
								//Console.Out.WriteLine("C4: XorY: " + XorY + ". y value: " + m1.getYLocation() + ". Tile Y + Y2: " + (tileY + tileY2));
							}
						}
					}
					else
					{
						if (monsterChar.yLocation > player.yLocation)// && m1.getYLocation() <= (tileY + tileY2))//(monster1Y > characterY && monster1Y <= (tileY + tileY2))
						{
							// monster1Y -= 60f;
							//m1.setYLocation(-60f);
							//monsterChar.move(0, -60f);
							//Console.Out.WriteLine("C5: XorY: " + XorY + ". y value: " + m1.getYLocation() + ". Tile Y + Y2: " + (tileY + tileY2));
						}
						else if (monsterChar.yLocation < player.yLocation)// && m1.getYLocation() < (tileY + tileY2))//(monster1Y < characterY && monster1Y < (tileY + tileY2))
						{
							//monster1Y += 60f;
							//m1.setYLocation(60f);
							//monsterChar.move(0, 60f);
							//Console.Out.WriteLine("C6: XorY: " + XorY + ". y value: " + m1.getYLocation() + ". Tile Y + Y2: " + (tileY + tileY2));
						}
						else
						{
							if (monsterChar.xLocation > player.xLocation)// && m1.getXLocation() <= (tileX + tileX2))//(monster1X > characterX && monster1X <= (tileX + tileX2))
							{
								//monster1X -= 60f;
								//m1.setXLocation(-60f);
								//monsterChar.move(-60f, 0);
								//Console.Out.WriteLine("C7: XorY: " + XorY + ". x value: " + m1.getXLocation() + ". Tile X + X2: " + (tileX + tileX2));
							}
							else if (monsterChar.xLocation < player.xLocation)// && m1.getXLocation() < (tileX + tileX2))//(monster1X < characterX && monster1X < (tileX + tileX2))
							{
								//monster1X += 60f;
								//m1.setXLocation(60f);
								//monsterChar.move(60f, 0);
								//Console.Out.WriteLine("C8: XorY: " + XorY + ". x value: " + m1.getXLocation() + ". Tile X + X2: " + (tileX + tileX2));
							}
						}
					}
				}
			}
		}

		//Dispose unused
		private static void Cleanup ()
		{
			if (device9 != null)
				device9.Dispose ();

			sprite.Dispose ();
			sprite2.Dispose ();
			sprite3.Dispose();
			sprite4.Dispose();

			Graphics.disposeMainMenu ();

			music.Dispose ();

			//player1.Dispose ();
			//monster1.Dispose();
			//tiles.Dispose();

			Application.Exit ();
		}
			
	}


}
