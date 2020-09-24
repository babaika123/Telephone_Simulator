using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace SuperNokia
{
    abstract class Tele
    {

        public ConsoleKeyInfo k;
        public int WeightScreen { get; protected set; }
        public int HeightScreen { get; protected set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int SnakeHigscore { get; set; } = 0;
        public int SnakeWalls { get; set; }
        public ulong Number { get; protected set; }
        public string Operator { get; protected set; }
        public int Battery { get; protected set; }
        public int Xtimer { get; protected set; }
        public int Ytimer { get; protected set; }
        public int TimerSecNow { get; protected set; }
        public int SpeedOfDischarge { get; protected set; }
        public int SpeedOfCharge { get; protected set; }
        public Thread Background { get; protected set; }
        public bool Charging { get; set; }
        public abstract void Show();
        public abstract void Move(int side);
        public abstract void Dicharging();
        public abstract void Clear();
        public void Snake()
        {
            Global.SnakePlay = true;
            int size = 4;
            int middleX = PosX + WeightScreen / 2;
            int middleY = PosY + HeightScreen / 2;
            char direction = 'u'; //u,r,d,l
            char head = 'O', havka = '*', wall1 = '[', wall2 = ']';
            char[,] field = new char[WeightScreen - 2, HeightScreen - 2];
            int MsToSleep = 400;
            Clear();
            List<int> PosBodyX = new List<int>();
            List<int> PosBodyY = new List<int>();
            for (int i = size - 1; i > -1; i--)
            {
                PosBodyX.Add(middleX);
                PosBodyY.Add(middleY + i);
                Global.WriteAt("|", middleX, middleY + i);
            }
            Global.WriteAt(head, PosBodyX[size - 1], PosBodyY[size - 1]);
            bool generated = false;
            //Generation food
            int randX, randY;
            do
            {
                generated = true;
                randX = Global.rand.Next() % (WeightScreen - 2);
                randY = Global.rand.Next() % (HeightScreen - 2);
                for (int i = 0; i < size; i++)
                {
                    if (randX == PosBodyX[i] + PosX + 1 && randY == PosBodyY[i] + PosY + 1) generated = false;
                }
            } while (!generated);
            field[randX, randY] = '*';
            Global.WriteAt('*', randX + PosX + 1, randY + PosY + 1);
            //End of generation food
            //Begin generation walls
            for (int j = 0; j < SnakeWalls; j++)
            {
                do
                {
                    generated = true;
                    randX = Global.rand.Next() % (WeightScreen - 2);
                    randY = Global.rand.Next() % (HeightScreen - 2);
                    if (randX + PosX + 1 == PosBodyX[size - 1]) generated = false;
                    else if (field[randX, randY] == havka || field[randX, randY] == wall1 || field[randX, randY] == wall2) generated = false;
                } while (!generated);
                if (Global.rand.Next() % 2 == 0)
                {
                    field[randX, randY] = wall1;
                    Global.WriteAt(wall1, randX + PosX + 1, randY + PosY + 1);
                }
                else
                {
                    field[randX, randY] = wall2;
                    Global.WriteAt(wall2, randX + PosX + 1, randY + PosY + 1);
                }
            }
            //End of generation walls
            while (Global.SnakePlay)
            {
                Thread.Sleep(MsToSleep);
                switch (Global.SnakeKey.Key)
                {
                    case ConsoleKey.NumPad0:
                        {
                            Global.SnakePlay = false;
                            break;
                        }
                    case ConsoleKey.NumPad8:
                        {
                            if (direction != 'd') direction = 'u';
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            if (direction != 'd') direction = 'u';
                            break;
                        }
                    case ConsoleKey.NumPad4:
                        {
                            if (direction != 'r') direction = 'l';
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (direction != 'r') direction = 'l';
                            break;
                        }
                    case ConsoleKey.NumPad2:
                        {
                            if (direction != 'u') direction = 'd';
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (direction != 'u') direction = 'd';
                            break;
                        }
                    case ConsoleKey.NumPad6:
                        {
                            if (direction != 'l') direction = 'r';
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (direction != 'l') direction = 'r';
                            break;
                        }
                }
                //--------------------BEGIN MOVE-----------------------------
                //Proverka na havky
                if (field[PosBodyX[size - 1] - PosX - 1, PosBodyY[size - 1] - PosY - 1] == havka)
                {
                    field[PosBodyX[size - 1] - PosX - 1, PosBodyY[size - 1] - PosY - 1] = ' ';
                    //Begin generation food
                    do
                    {
                        generated = true;
                        randX = Global.rand.Next() % (WeightScreen - 2);
                        randY = Global.rand.Next() % (HeightScreen - 2);
                        for (int i = 0; i < size; i++)
                        {
                            if (randX + PosX + 1 == PosBodyX[i] && randY + PosY + 1 == PosBodyY[i]) generated = false;
                        }
                        if (field[randX, randY] == wall1 || field[randX, randY] == wall2) generated = false;
                    } while (!generated);
                    field[randX, randY] = havka;
                    Global.WriteAt(havka, randX + PosX + 1, randY + PosY + 1);
                    //End of generation food
                    size++;
                    PosBodyX.Add(0);
                    PosBodyY.Add(0);
                }
                //End proverki na havky
                else
                {
                    Global.WriteAt(" ", PosBodyX[0], PosBodyY[0]);
                    for (int i = 1; i < size; i++)
                    {
                        PosBodyX[i - 1] = PosBodyX[i];
                        PosBodyY[i - 1] = PosBodyY[i];
                    }
                }
                if (direction == 'u')
                {
                    PosBodyX[size - 1] = PosBodyX[size - 2];
                    PosBodyY[size - 1] = PosBodyY[size - 2] - 1;
                }
                else if (direction == 'l')
                {
                    PosBodyX[size - 1] = PosBodyX[size - 2] - 1;
                    PosBodyY[size - 1] = PosBodyY[size - 2];
                }
                else if (direction == 'r')
                {
                    PosBodyX[size - 1] = PosBodyX[size - 2] + 1;
                    PosBodyY[size - 1] = PosBodyY[size - 2];
                }
                else if (direction == 'd')
                {
                    PosBodyX[size - 1] = PosBodyX[size - 2];
                    PosBodyY[size - 1] = PosBodyY[size - 2] + 1;
                }
                for (int i = 0; i < size - 1; i++)
                {
                    if (PosBodyX[i] == PosBodyX[size - 1] && PosBodyY[i] == PosBodyY[size - 1]) Global.SnakePlay = false;
                }
                if (PosBodyX[size - 1] >= PosX + WeightScreen - 1 || PosBodyY[size - 1] >= PosY + HeightScreen - 1 || PosBodyY[size - 1] <= PosY || PosBodyX[size - 1] <= PosX) Global.SnakePlay = false;
                else if (field[PosBodyX[size - 1] - PosX - 1, PosBodyY[size - 1] - PosY - 1] == wall1 || field[PosBodyX[size - 1] - PosX - 1, PosBodyY[size - 1] - PosY - 1] == wall2) Global.SnakePlay = false;
                else
                {
                    if ((PosBodyY[size - 1] != PosBodyY[size - 2] && PosBodyX[size - 2] != PosBodyX[size - 3]) || (PosBodyX[size - 1] != PosBodyX[size - 2] && PosBodyY[size - 2] != PosBodyY[size - 3])) Global.WriteAt("o", PosBodyX[size - 2], PosBodyY[size - 2]);
                    else if (direction == 'u' || direction == 'd') Global.WriteAt("|", PosBodyX[size - 2], PosBodyY[size - 2]);
                    else Global.WriteAt("-", PosBodyX[size - 2], PosBodyY[size - 2]);
                    Global.WriteAt(head, PosBodyX[size - 1], PosBodyY[size - 1]);
                }
                //--------------------END MOVE-----------------------------
            }
            Thread.Sleep(200);
            Clear();
            Global.WriteAt("GameOver!", middleX - 4, middleY);
            Global.WriteAt("Score: ", middleX - 4, middleY + 1);
            Console.Write(size);
            if (size > SnakeHigscore) SnakeHigscore = size;
        }
        public void Timer()
        {
            int xSt = Xtimer;
            int ySt = Ytimer;
            short sec, min, hour;
            hour = (short)(TimerSecNow / 3600);
            min = (short)(TimerSecNow % 3600 / 60);
            sec = (short)(TimerSecNow % 60);
            Console.SetCursorPosition(xSt, ySt);
            if (hour < 10) Console.Write("0");
            Console.Write(hour + ":");
            if (min < 10) Console.Write("0");
            Console.Write(min + ":");
            if (sec < 10) Console.Write("0");
            Console.Write(sec);
            while (Global.time_go)
            {
                Console.SetCursorPosition(0, 0);
                Thread.Sleep(997);
                if (Global.time_go) sec++;
                if (min > 59)
                {
                    hour++;
                    sec = 0;
                    min = 0;
                    Console.SetCursorPosition(xSt + 3, ySt);
                    Console.Write("00:0");
                    if (hour < 10)
                        Console.SetCursorPosition(xSt + 1, ySt);
                    else
                        Console.SetCursorPosition(xSt, ySt);
                    Console.Write(hour);
                }
                if (sec > 59)
                {
                    min++;
                    sec = 0;
                    Console.SetCursorPosition(xSt + 6, ySt);
                    Console.Write("0");
                    if (min < 10)
                        Console.SetCursorPosition(xSt + 4, ySt);
                    else
                        Console.SetCursorPosition(xSt + 3, ySt);
                    Console.Write(min);
                }
                if (sec < 10)
                    Console.SetCursorPosition(xSt + 7, ySt);
                else
                    Console.SetCursorPosition(xSt + 6, ySt);
                Console.Write(sec);

            }
            Console.SetCursorPosition(0, 0);
            TimerSecNow = hour * 3600 + min * 60 + sec;
        }
        public void PrintScreen(int x, int y)
        {
            int i;
            string empt = "";
            for (i = 1; i < WeightScreen - 1; i++)
            {
                empt += " ";
            }
            //  ____________
            Global.WriteAt(" ", x, y);
            for (i = 1; i < WeightScreen - 1; i++)
            {
                Console.Write("_");
            }
            // |            |
            for (i = 1; i < HeightScreen - 1; i++)
            {
                Global.WriteAt("|", x, y + i);
                Console.Write(empt);
                Console.Write("|");
            }
            // \____________/
            Global.WriteAt("|", x, y + HeightScreen - 1);
            for (i = 1; i < WeightScreen - 1; i++)
            {
                Console.Write("_");
            }
            Console.WriteLine("|");
        }
        public void WriteText(string s, int beg) //номер строчки с которой пишется начиная с 1
        {
            beg--;
            WeightScreen -= 2;
            HeightScreen -= 2;
            int max, l = s.Length;
            if (l <= (HeightScreen - beg) * WeightScreen) max = l;
            else max = (HeightScreen - beg) * WeightScreen;
            //Console.SetCursorPosition(PosX+1, PosY+1+beg);
            for (int i = 0; i < max; i++)
            {
                if (i % WeightScreen == 0) Console.SetCursorPosition(PosX + 1, PosY + 1 + beg + (i / WeightScreen));
                Console.Write(s[i]);
            }
            WeightScreen += 2;
            HeightScreen += 2;
            Console.SetCursorPosition(0, 0);
        }
        public void WriteTextWithEnter2(string s, int beg, int begStrOfText) //номер строчки с которой пишется начиная с 1
        {
            s += '\n';
            int endY = HeightScreen - 1, endX = WeightScreen - 2, l = s.Length - 1, strnow = beg, lettnow;
            if (begStrOfText < 1) begStrOfText = 1;
            if (begStrOfText == 1) lettnow = -1;
            else
            {
                begStrOfText++;
                int i, CountSymbols = 0, maxi = (HeightScreen - beg) * (WeightScreen - 2), DistanceSlashN = 0;
                for (i = l; i > -1; i--)
                {
                    CountSymbols++;
                    if (s[i] == '\n')
                    {
                        CountSymbols += endX - (DistanceSlashN % endX);
                        DistanceSlashN = 0;
                    }
                    DistanceSlashN++;
                    if (CountSymbols >= maxi) break;
                }
                //i++;
                //lettnow = i;
                lettnow = 0; // i - индекс начального символа в случае если последние строки
                bool naznach_i = false;
                while (strnow < begStrOfText && !naznach_i)
                {
                    for (int j = 0; j < endX; j++)
                    {
                        lettnow++;
                        if (s[lettnow] == '\n') break;
                    }
                    strnow++;
                    if (lettnow > i) { naznach_i = true; lettnow = i; }
                }
                strnow = beg;
            }
            while (strnow < endY)
            {
                Console.SetCursorPosition(PosX + 1, PosY + strnow);
                for (int i = 0; i < endX; i++)
                {
                    lettnow++;
                    if (s[lettnow] == '\n') break;
                    else Console.Write(s[lettnow]);
                }
                strnow++;
                if (lettnow == l) break;
            }
            Console.SetCursorPosition(0, 0);
        }
        public bool WriteTextWithEnter(string s, int beg, int begStrOfText) // false - достигнут конец текста, true - ещё можно листать
        {
            begStrOfText--;
            int RowNow = 0, endY = PosY + HeightScreen - 1, endX = PosX + WeightScreen - 1, l = s.Length - 1;
            int LettNow = 0;
            while (RowNow < begStrOfText)
            {
                for (int j = 0; j < endX; j++)
                {
                    LettNow++;
                    if (s[LettNow] == '\n' && j!=0) break;
                }
                RowNow++;
            }
            //Сейчас LettNow показывает символ, с которого нужно писать
            if (beg < 1) beg = 1;
            Console.SetCursorPosition(PosX + 1, beg+PosY);
            for (; LettNow < l; LettNow++)
            {
                Console.Write(s[LettNow]);
                if (Console.CursorLeft <= PosX) Console.SetCursorPosition(PosX + 1, Console.CursorTop);
                else if (Console.CursorLeft >= endX && s[LettNow + 1] != '\n') Console.SetCursorPosition(PosX + 1, Console.CursorTop + 1);
                if (Console.CursorTop >= endY)
                {
                    Console.SetCursorPosition(0, 0);
                    return true;
                }
            }
            Console.Write(s[LettNow]);
            Console.SetCursorPosition(0, 0);
            return false;
        }
        public void Write2RowsOfText (string s, int beg, int begX) //begX - с какого X начать (начиная с 0)
        {
            s += "\n\n";
            int last = WeightScreen - 2, lettnow = -1;
            Console.SetCursorPosition(PosX+1+begX, beg+PosY);
            for (int i = begX; i<last; i++)
            {
                lettnow++;
                if (s[lettnow] == '\n') break;
                else Console.Write(s[lettnow]);
            }
            Console.SetCursorPosition(PosX + 1, beg + PosY+1);
            for (int i = 0; i < last; i++)
            {
                lettnow++;
                if (s[lettnow] == '\n') break;
                else Console.Write(s[lettnow]);
            }
            Console.SetCursorPosition(0,0);
        }
        public void Batt()
        {
            while (Global.now_nokia)
            {
                if (!Charging)
                    Thread.Sleep(SpeedOfDischarge);
                if (!Charging && Battery > 0)
                    Battery--;
                if (Battery == 0 && Charging==false)
                    Dicharging();
                if (!Charging)
                    Thread.Sleep(SpeedOfDischarge);
                if (Charging)
                    Thread.Sleep(SpeedOfCharge);
                if (Charging && Battery < 100)
                    Battery++;
                if (Charging)
                    Thread.Sleep(SpeedOfCharge);
            }
        }
    }
}
