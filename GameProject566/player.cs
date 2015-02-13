﻿using System;
using System.Windows.Forms;
using SlimDX.RawInput;
using SlimDX.Direct3D9;
namespace GameProject566
{
    class player : character
    {
        private Texture pTexture;
        private float xLocation;
        private float yLocation;

        public player()
        { }
        public player (Texture pTexture, float xLocation, float yLocation) : base (pTexture, xLocation, yLocation)
        {
            this.pTexture = pTexture;
            this.xLocation = xLocation;
            this.yLocation = yLocation;
        }

        /*public void move(float x, float y)
        {
            this.xLocation += x;
            this.yLocation += y;
        }*/

    }
}
