/*!
 * I am Karo  😊👍
 *
 * Contact me:
 *     https://www.karo.link/
 *     https://github.com/iamkaro
 *     https://www.linkedin.com/in/iamkaro
 *
 * Leitner Box  (app)
 * https://github.com/iamkaro/Leitner-Box.git
 * Copyright © 2014 developed.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AllLanguages
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KaroTicket());
        }
    }
}
