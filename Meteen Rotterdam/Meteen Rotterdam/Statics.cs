using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Meteen_Rotterdam {
	class Statics {
		//Checks if connection can be made with google.com -> returns bool
		public static bool hasInternet() {
			try {
					using (var web = new System.Net.WebClient()) {
						var read = web.OpenRead("http://www.google.com/");
			}
					return true;
				}
				catch {
					return false;
				}
		}

	}
}
