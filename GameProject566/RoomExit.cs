﻿using System;

namespace GameProject566
{
	public class RoomExit
	{
		public Tile tileA { get ; set; }
		//attaching to an exit that is vertical.
		public bool isVertical { get; set; }

		public RoomExit (Tile a, bool isVertical){
			this.tileA = a;
			this.isVertical = isVertical;
		}

	}
}

