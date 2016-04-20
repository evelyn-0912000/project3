﻿using System;

namespace Meteen_Rotterdam {
	enum Weather {
		raining,
		clear
	};
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
					using (var game = new Game1(1080, 720, false))
				
						game.Run();
								
						}
    }
#endif
}
