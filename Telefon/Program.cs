//Main menu: C,D,NumPad5,Arrows
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace SuperNokia
{
    class Telefon
    {

        public static void Main()
        {
            Console.CursorVisible=false;
            Nokia3310 Phone = new Nokia3310();
            Global.now_nokia = true;
            Phone.Show();
            Phone.Home();
            Console.SetCursorPosition(0, 50);
        }
    }
}