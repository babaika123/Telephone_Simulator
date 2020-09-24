using System;
using System.Collections.Generic;
using System.Text;

namespace SuperNokia
{
    public static class Global
    {
        public static Random rand = new Random();
        public static bool time_go = true;
        public static bool now_nokia = true;
        public static ConsoleKeyInfo SnakeKey;
        public static bool SnakePlay;
        public static void WriteAt(object s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
        public static int Handling(ConsoleKeyInfo key)
        {
            int numb = 0;
            if (key.KeyChar > '0' && key.KeyChar <= '9')
            {
                numb = key.KeyChar - '0';
            }
            return numb;
        }
        public static int FirstDayOfWeekInMonth (int Month, int Year, bool Leap) //возвращает день недели первого дня в месяце
        {
            int Day = 1;//первое число
            int DayOfWeek;
            int CodeOfMonth, CodeOfYear, CodeOfCentury, century = Year / 100;
            if (Month == 1 || Month == 10) CodeOfMonth = 1;
            else if (Month == 5) CodeOfMonth = 2;
            else if (Month == 8) CodeOfMonth = 3;
            else if (Month == 6) CodeOfMonth = 5;
            else if (Month == 12 || Month == 9) CodeOfMonth = 6;
            else if (Month == 4 || Month == 7) CodeOfMonth = 0;
            else CodeOfMonth = 4;
            if (century % 4 == 0) CodeOfCentury = 6;
            else if (century % 4 == 1) CodeOfCentury = 4;
            else if (century % 4 == 2) CodeOfCentury = 2;
            else CodeOfCentury = 0;
            CodeOfYear = (CodeOfCentury + (Year % 100) + ((Year % 100) / 4)) % 7;
            DayOfWeek = (Day + CodeOfMonth + CodeOfYear) % 7;
            if (Month < 3 && Leap) DayOfWeek++;
            DayOfWeek--;
            if (DayOfWeek < 1) DayOfWeek += 7;
            return DayOfWeek;
        }
    }
}
