using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Meteen_Rotterdam {
	class Rectangler {
		public static Texture2D makeRect(int width, int height, Color color, GraphicsDeviceManager graphics) {
			Color[] truecolor = new Color[width * height];
			for (int i = 0; i < truecolor.Length; i++) {
				if (i % width == 0 || i % width == width - 1 || i < width || i > ((width * height) - width))  {
					truecolor[i] = Color.Black;
				}
				else {
					truecolor[i] = color;
				}
			}
			var a = new Texture2D(graphics.GraphicsDevice, width, height);
			a.SetData(truecolor);
			return a;
		}
	}
}
