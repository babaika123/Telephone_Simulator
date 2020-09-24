using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SuperNokia
{

    class Nokia3310 : Tele
    {
        List<string> Sms;
        List<string> TutorialList;
        
        public Nokia3310()
        {
            WeightScreen = 19;
            HeightScreen = 9;
            Battery = 50;
            Number = (ulong)Global.rand.Next(0, 999999999);
            PosX = 10;
            PosY = 0;
            Xtimer = PosX + 1;
            Ytimer = PosY + 2;
            TimerSecNow = 0;
            Operator = "ГР";
            SnakeWalls = 2;
            //-------------------
            Sms = new List<string>();
            Sms.Add("Абонент \"Вася Сантехник\" снова на связи!");
            Sms.Add("Сегодня ночью вместо гудков на мобильный вам была подключена песня Н. Баскова \"Обниму тебя\". Первые полтора дня бесплатно, далее - 80 грн/час\nГромофон - мы точно знаем что ты хочешь");
            Sms.Add("Поздравляем! Вы являетесь предком шамаханского царя Рахира Эдуардовича и сейчас Вам пологается наследство в 5 миллионов долларов! Для получения вам нужно подтвердить транзакцию отправив сообщение на номер 565458525 и мы сразу же вышлем вам посылку с вашим наследством!");
            Sms.Add("Пользуйтесь 4G от Громофон!\nДо сих пор не пользуетесь 4G от Громофон и просите раздавать интернет прохожих?\nПодключайте 4G интрнет на Ваш Nokia3310 и наслаждайтесь фильмами в высоком качестве и не только!");
            Sms.Add("Вам была подключена услуга \"Анекдот дня\"!\nОплата: 30 грн/день\nПлатеж за месяц уже успешно снят с вашего счета\nГромофон - всегда на шаг впереди");
            Sms.Add("Теперь вам не нужно подключать услуги вручную, мы будем делать это за вас автоматически!\nГромофон - с заботой о Вас");
            //-------------------
            TutorialList = new List<string>();
            TutorialList.Add("Стрелки, Numpad2,8,5,0");
            TutorialList.Add("Уровень батареи:\nдля подклюяение зарядки к телефону нажмите С, находясь на главном экране");
            TutorialList.Add("Положение телефона в консоли:\nдля изменения положения телефона в кончоли используйте стрелки, находясь на главном экране");
            TutorialList.Add("Меню СМС:\nДля того, чтобы посмотреть весь текст, используйте стрелки или Numpad2,8. Вы также можете открывать функции СМС используя Numpad5");
            TutorialList.Add("Календарь:\nДля просмотра текущего месяца и года, нажмите 5, находясь в календаре. Листать месяцы: 4,6 или стрелка влево и вправо. Листать годы: 8,2 или вниз, вверх");
            //-------------------
            
            //-------------------
            SpeedOfDischarge = 30000;
            SpeedOfCharge = 30000;
            Charging = false;
            Background = new Thread(Batt);
            Background.Start();
        }
        //--------------------------methods----------------------------
        public override void Move(int side)
        {
            if (side == 1 && PosX > 0) PosX--;
            else if (side == 2) PosX++;
            else if (side == 3) PosY++;
            else if (side == 4 && PosY > 0) PosY--;
            Console.Clear();
            Show();
            if (Charging == true) Charge();
        }
        public override void Dicharging ()
        {
            Global.WriteAt("                 ", PosX + 1, PosY + 1);
            Global.WriteAt("                 ", PosX + 1, PosY + 2);
            Global.WriteAt("                 ", PosX + 1, PosY + 3);
            Global.WriteAt("   Low Battery   ", PosX + 1, PosY + 4);
            Global.WriteAt("        :(       ", PosX + 1, PosY + 5);
            Global.WriteAt("                 ", PosX + 1, PosY + 6);
            Global.WriteAt("                 ", PosX + 1, PosY + 7);

            k = Console.ReadKey(true);
            Charging = true;
            Charge();

        }
        public void UpperStrip()
        {
            Global.WriteAt(Operator + " ", PosX + 1, PosY + 1);
            Console.SetCursorPosition(PosX + WeightScreen - 11, PosY + 1);
            Console.Write(DateTime.Now.Hour + ":");
            if (DateTime.Now.Minute < 10) Console.Write("0");
            Console.Write(DateTime.Now.Minute + " ");
            if (Battery < 100) Console.Write(" ");
            if (Battery < 10) Console.Write(" ");
            Console.Write(Battery + "%");
        }
        public override void Clear()
        {
            Global.WriteAt("                 ", PosX + 1, PosY + 1);
            Global.WriteAt("                 ", PosX + 1, PosY + 2);
            Global.WriteAt("                 ", PosX + 1, PosY + 3);
            Global.WriteAt("                 ", PosX + 1, PosY + 4);
            Global.WriteAt("                 ", PosX + 1, PosY + 5);
            Global.WriteAt("                 ", PosX + 1, PosY + 6);
            Global.WriteAt("                 ", PosX + 1, PosY + 7);
        }
        public override void Show()
        {
            PrintScreen(PosX, PosY);
            Global.WriteAt("|  1  |  2  |  3  |", PosX, PosY + HeightScreen);
            Global.WriteAt("|_____|_____|_____|", PosX, PosY + HeightScreen + 1);
            Global.WriteAt("|  4  |  5  |  6  |", PosX, PosY + HeightScreen + 2);
            Global.WriteAt("|_____|_____|_____|", PosX, PosY + HeightScreen + 3);
            Global.WriteAt("|  7  |  8  |  9  |", PosX, PosY + HeightScreen + 4);
            Global.WriteAt("|_____|_____|_____|", PosX, PosY + HeightScreen + 5);
            Global.WriteAt("|  *  |  0  |  #  |", PosX, PosY + HeightScreen + 6);
           Global.WriteAt("\\_________________/", PosX, PosY + HeightScreen + 7);
        }
        public void Discharge()
        {
            if (Battery > 0)
            {
                Charging = false;
                Console.Clear();
                Show();
                Home();
            }
        }
        public void Charge()
        {
            Charging = true;
            Global.WriteAt("      |      |     ", PosX, PosY + HeightScreen + 8);
            Global.WriteAt("     /  ____/      ", PosX, PosY + HeightScreen + 9);
            /*WriteAt("    /    _/        ", PosX, PosY + HeightScreen + 10);
            WriteAt("   /   _/          ", PosX, PosY + HeightScreen + 11);
            WriteAt("  /  _/            ", PosX, PosY + HeightScreen + 12);
            WriteAt(" / _/              ", PosX, PosY + HeightScreen + 13);
            WriteAt("/ /                ", PosX, PosY + HeightScreen + 14);*/
            int y = 10;
            for (int i = PosX + 4; i > -1; i--)
            {
                Global.WriteAt("/ /", i, PosY + HeightScreen + y);
                y++;
            }
            Global.WriteAt(" /", 0, PosY + HeightScreen + y);
            Global.WriteAt("/", 0, PosY + HeightScreen + y + 1);
            Console.SetCursorPosition(0, 0);
        }
        public void Home()
        {
            do
            {
                Clear();
                UpperStrip();
                Global.WriteAt("                 ", PosX + 1, PosY + 2);
                Global.WriteAt("                 ", PosX + 1, PosY + 3);
                Global.WriteAt("      0   0      ", PosX + 1, PosY + 4);
                Global.WriteAt("        o        ", PosX + 1, PosY + 5);
               Global.WriteAt("      \\___/      ", PosX + 1, PosY + 6);
                Global.WriteAt("                 ", PosX + 1, PosY + 7);
                Console.SetCursorPosition(0, 0);
                k = Console.ReadKey(true);
                switch (k.Key)
                {
                    case ConsoleKey.NumPad5:
                        {
                            this.Menu();
                            break;
                        }
                    case ConsoleKey.C:
                        {
                            Charge();
                            break;
                        }
                    case ConsoleKey.D:
                        {
                            Discharge();
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            Move(1);
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            Move(2);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            Move(3);
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            Move(4);
                            break;
                        }
                }
            } while (k.Key != ConsoleKey.Enter);
        }
        public void Menu()
        {
            do
            {
            Clear();
            UpperStrip();
            Global.WriteAt("calc   SMS   tuto", PosX + 1, PosY + 2);
            Global.WriteAt("                 ", PosX + 1, PosY + 3);
            Global.WriteAt("tool   tel   cale", PosX + 1, PosY + 4);
            Global.WriteAt("                 ", PosX + 1, PosY + 5);
            Global.WriteAt("game   home  file", PosX + 1, PosY + 6);
            Global.WriteAt("                 ", PosX + 1, PosY + 7);
            Console.SetCursorPosition(0, 0);
                k = Console.ReadKey(true);
                switch (k.Key)
                {
                    case ConsoleKey.NumPad2:
                        {
                            this.Home();
                            break;
                        }
                    case ConsoleKey.NumPad0:
                        {
                            this.Home();
                            break;
                        }
                    case ConsoleKey.NumPad4:
                        {
                            this.Tool();
                            break;
                        }
                    case ConsoleKey.NumPad7:
                        {
                            this.Calculator();
                            break;
                        }
                    case ConsoleKey.NumPad1:
                        {
                            SuperSnake3000();
                            break;
                        }
                    case ConsoleKey.NumPad8:
                        {
                            SMSMenu();
                            break;
                        }
                    case ConsoleKey.NumPad9:
                        {
                            Tutorial();
                            break;
                        }
                    case ConsoleKey.NumPad6:
                        {
                            Calendar();
                            break;
                        }
                    case ConsoleKey.NumPad3:
                        {
                            AllFiles();
                            break;
                        }
                    case ConsoleKey.NumPad5:
                        {
                            DialingNumber();
                            break;
                        }
                }
            } while (k.Key != ConsoleKey.Enter);
        }
        public void Tutorial()
        {
            int choosenow = 1; // с 0
            int endchoose = 3;
            do
            {
                Clear();
                UpperStrip();
                if (choosenow < (endchoose - 2) || choosenow >= endchoose - 1)
                {
                    if (choosenow < 2) endchoose = 3;
                    else endchoose = choosenow + 2;
                    if (endchoose == TutorialList.Count + 1) endchoose--;
                }
                for (int i = endchoose - 3; i < endchoose; i++)
                {
                    if (i < TutorialList.Count)
                    {
                        if (choosenow == i) Console.ForegroundColor = ConsoleColor.Blue;
                        WriteText(i + ".", (i - endchoose + 4) * 2);
                        Write2RowsOfText(TutorialList[i], (i - endchoose + 4) * 2, 2);
                        if (choosenow == i) Console.ResetColor();
                    }
                }

                k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.NumPad2 || k.Key == ConsoleKey.DownArrow)
                {
                    if (choosenow + 1 < TutorialList.Count)
                        choosenow++;
                }
                else if (k.Key == ConsoleKey.NumPad8 || k.Key == ConsoleKey.UpArrow)
                {
                    if (choosenow > 1)
                        choosenow--;
                }
                else if (k.Key == ConsoleKey.NumPad5)
                {
                    int rownow = 1;
                    ConsoleKeyInfo arrow;
                    bool listability; //можно ли листать
                    do
                    {
                        Clear();
                        UpperStrip();
                        listability = WriteTextWithEnter(TutorialList[choosenow], 2, rownow);
                        arrow = Console.ReadKey(true);
                        if ((arrow.Key == ConsoleKey.DownArrow || arrow.Key == ConsoleKey.NumPad2) && listability)
                            rownow++;
                        else if ((arrow.Key == ConsoleKey.UpArrow || arrow.Key == ConsoleKey.NumPad8) && rownow > 1)
                            rownow--;
                    } while (arrow.Key != ConsoleKey.NumPad0);
                }
            } while (k.Key != ConsoleKey.NumPad0);
        }
        public void SuperSnake3000()
        {
            do
            {
                Clear();
                UpperStrip();
                Global.WriteAt("Any key-play     ", PosX + 1, PosY + 2);
                Global.WriteAt("Control:         ", PosX + 1, PosY + 3);
                Global.WriteAt("NumPad or arrows", PosX + 1, PosY + 4);
                Global.WriteAt("1-highscore      ", PosX + 1, PosY + 5);
                Global.WriteAt("2-difficult      ", PosX + 1, PosY + 6);
                Global.WriteAt("0-exit           ", PosX + 1, PosY + 7);
                k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.NumPad1)
                {
                    Clear();
                    UpperStrip();
                    Global.WriteAt("Your higscore:", PosX + 1, PosY + 2);
                    Console.Write(SnakeHigscore);
                    Console.ReadKey(true);
                }
                else if (k.Key == ConsoleKey.NumPad2)
                {
                    Clear();
                    UpperStrip();
                    Global.WriteAt("Press number", PosX + 1, PosY + 2);
                    Global.WriteAt("0-no walls", PosX + 1, PosY + 3);
                    Global.WriteAt("9-impossible", PosX + 1, PosY + 4);
                    ConsoleKeyInfo difficult;
                    difficult = Console.ReadKey(true);
                    if (difficult.KeyChar >= '0' && difficult.KeyChar <= '9')
                    SnakeWalls = (difficult.KeyChar - '0') * 2;
                }
                else if (k.Key == ConsoleKey.NumPad0)
                {
                    Menu();
                }
                else
                {
                    Thread SnakeThread = new Thread(Snake);
                    SnakeThread.Start();
                    Thread.Sleep(400);
                    while (Global.SnakePlay)
                    {
                        Global.SnakeKey = Console.ReadKey(true);
                    }
                }
            } while (k.Key != ConsoleKey.NumPad0);
        }
        public void Calculator()
        {
            Clear();
            UpperStrip();
            Global.WriteAt("+ - * / = <-", PosX + 1, PosY + 6);
            Global.WriteAt("Enter-выйти", PosX + 1, PosY + 7);
            Console.SetCursorPosition(PosX + 1, PosY + 2);
            long numb1 = 0, numbnow = 0;
            char operat = '+';
            bool new_calc=true;
            do
            {
                k = Console.ReadKey(true);
                if (new_calc)
                {
                    Global.WriteAt("                 ", PosX + 1, PosY + 2);
                    Global.WriteAt("                 ", PosX + 1, PosY + 3);
                    Global.WriteAt("                 ", PosX + 1, PosY + 4);
                    Global.WriteAt("                 ", PosX + 1, PosY + 5);
                    new_calc = false;
                }
                if (k.KeyChar >= '0' && k.KeyChar <= '9')
                {
                    if (numbnow < 10000000)
                    {
                        numbnow *= 10;
                        numbnow += k.KeyChar - '0';
                        if (numb1==0)
                            Console.SetCursorPosition(PosX + 1, PosY + 2);
                        else
                            Console.SetCursorPosition(PosX + 1, PosY + 4);
                            Console.Write(numbnow);
                    }
                }
                else if (k.Key == ConsoleKey.Backspace)
                {
                    numbnow/=10;
                    if (numb1 == 0)
                        Console.SetCursorPosition(PosX + 1, PosY + 2);
                    else
                        Console.SetCursorPosition(PosX + 1, PosY + 4);
                    Console.Write("                 ");
                    if (numb1 == 0)
                        Console.SetCursorPosition(PosX + 1, PosY + 2);
                    else
                        Console.SetCursorPosition(PosX + 1, PosY + 4);
                    Console.Write(numbnow);
                }
                else if (k.KeyChar == '/' || k.KeyChar == '*' || k.KeyChar == '-' || k.KeyChar == '+')
                {
                    operat = k.KeyChar;
                    Console.SetCursorPosition(PosX + 1, PosY + 3);
                    Console.Write(k.KeyChar);
                    if (numb1 == 0)
                    {
                        numb1 = numbnow;
                        numbnow = 0;
                    }
                    Console.SetCursorPosition(PosX + 1, PosY + 4);
                }
                else if (k.KeyChar == '=')
                {
                    Console.SetCursorPosition(PosX + 1, PosY + 5);
                    if (operat == '+') Console.Write(numb1 + numbnow);
                    else if (operat == '-') Console.Write(numb1 - numbnow);
                    else if (operat == '*') Console.Write(numb1 * numbnow);
                    else if (operat == '/') {
                        double n1=numb1;
                        double n2=numbnow;
                        Console.Write(Math.Round(n1/n2, 7));
                    }
                    numbnow = 0;
                    numb1 = 0;
                    new_calc = true;
                }
            } while (k.Key != ConsoleKey.Enter);
            Menu();
        }
        public void Tool()
        {
            do
            {
                Clear();
                UpperStrip();
                Global.WriteAt("1.Фонарик        ", PosX + 1, PosY + 2);
                Global.WriteAt("2.Анекдот дня    ", PosX + 1, PosY + 3);
                Global.WriteAt("3.Сенундомер     ", PosX + 1, PosY + 4);
                Global.WriteAt("4.Время          ", PosX + 1, PosY + 5);
                Global.WriteAt("5.Инфо           ", PosX + 1, PosY + 6);
                Global.WriteAt("0.выйти          ", PosX + 1, PosY + 7);
                Console.SetCursorPosition(0, 0);
                k = Console.ReadKey(true);
                switch (k.Key)
                {
                    case ConsoleKey.NumPad0:
                        {
                            this.Menu();
                            break;
                        }
                    case ConsoleKey.NumPad1:
                        {
                            Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            WriteText("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", 1);
                            Console.ResetColor();
                            k = Console.ReadKey(true);
                            break;
                        }
                    case ConsoleKey.NumPad2:
                        {
                            //Clear();
                            //UpperStrip();
                            int r = Global.rand.Next(3);
                            Clear();
                            UpperStrip();
                            switch (r)
                            {
                                case 0:
                                    {
                                        WriteText("А вот интересно, что будет делать кошка, если собаку облить валерьянкой?", 2);
                                        break;
                                    }
                                case 1:
                                    {
                                        WriteText("Встреча мальчика-одуванчика с ветряной подружкой оставила его без семени", 2);
                                        break;
                                    }
                                case 2:
                                    {
                                        WriteText("Видеть жениха в свадебном платье невесты до свадьбы - плохая примета", 2);
                                        break;
                                    }
                            }
                            Console.SetCursorPosition(0, 0);
                            k = Console.ReadKey(true);
                            break;
                        }
                    case ConsoleKey.NumPad3: // TIMER
                        {
                            Clear();
                            UpperStrip();
                            Global.WriteAt("1-сброс          ", PosX + 1, PosY + 6);
                            Global.WriteAt("0-выйти          ", PosX + 1, PosY + 7);
                            Thread thtimer;
                            Global.time_go = false;
                            Xtimer = PosX + 1;
                            Ytimer = PosY + 2;
                            thtimer = new Thread(Timer);
                            thtimer.Start();
                            Console.SetCursorPosition(0, 0);
                            do
                            {
                                k = Console.ReadKey(true);
                                if (k.Key == ConsoleKey.NumPad1)
                                {
                                    TimerSecNow = 0;
                                    Global.time_go = false;
                                    thtimer = new Thread(Timer);
                                    thtimer.Start();
                                    Console.SetCursorPosition(0, 0);
                                }
                                else if (k.Key != ConsoleKey.NumPad0)
                                {
                                    Global.time_go = true;
                                    thtimer = new Thread(Timer);
                                    thtimer.Start();
                                    k = Console.ReadKey(true);
                                    Global.time_go = false;
                                    Thread.Sleep(1000);
                                }
                            } while (k.Key != ConsoleKey.NumPad0);
                            break;
                        }
                    case ConsoleKey.NumPad4:
                        {
                            Clear();
                            UpperStrip();
                            Console.SetCursorPosition(PosX + 1, PosY + 2);
                            Console.Write("Дата:");
                            Console.SetCursorPosition(PosX + 1, PosY + 3);
                            Console.Write(DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year);
                            Console.SetCursorPosition(PosX + 1, PosY + 4);
                            Console.Write(DateTime.Now.DayOfWeek);
                            Console.SetCursorPosition(0, 0);
                            k = Console.ReadKey(true);
                            break;
                        }
                    case ConsoleKey.NumPad5:
                        {
                            Clear();
                            Global.WriteAt("Батарея: ", PosX + 1, PosY + 1);
                            Console.Write(Battery + "%");
                            Global.WriteAt("Оператор:", PosX + 1, PosY + 2);
                            Global.WriteAt("Громофон", PosX + 1, PosY + 3);
                            Global.WriteAt("Номер телефона:", PosX + 1, PosY + 4);
                            Global.WriteAt("", PosX + 1, PosY + 5);
                            Console.Write(Number);
                            Global.WriteAt("Расширение экрана", PosX + 1, PosY + 6);
                            Global.WriteAt("", PosX + 1, PosY + 7);
                            Console.Write((WeightScreen - 2) + "x" + (HeightScreen - 2));
                            Console.SetCursorPosition(0, 0);
                            k = Console.ReadKey(true);
                            break;
                        }
                }
            } while (k.Key != ConsoleKey.Enter);
        }
        public void SMSMenu()
        {
            int choosenow = 0; // с 0
            int endsms=3;
            do
            {
                Clear();
                UpperStrip();
                if (choosenow < (endsms - 2) || choosenow >= endsms-1)
                {
                    if (choosenow < 2) endsms = 3;
                    else endsms = choosenow + 2;
                    if (endsms == Sms.Count +1 && Sms.Count != 2) endsms--;
                }
                for (int i=endsms-3; i<endsms; i++)
                {
                    if (i < Sms.Count)
                    {
                        if (choosenow == i) Console.ForegroundColor = ConsoleColor.Blue;
                        WriteText(i + 1 + ".", (i - endsms + 4) * 2);
                        Write2RowsOfText(Sms[i], (i - endsms + 4) * 2, 2);
                        if (choosenow == i) Console.ResetColor();
                    }
                }

                k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.NumPad2 || k.Key == ConsoleKey.DownArrow)
                {
                    if (choosenow+1< Sms.Count)
                    choosenow++;
                }
                else if (k.Key == ConsoleKey.NumPad8 || k.Key == ConsoleKey.UpArrow)
                {
                    if (choosenow > 0)
                        choosenow--;
                }
                else if (k.Key == ConsoleKey.NumPad5 && choosenow < Sms.Count)
                    {
                        int rownow = 1;
                        ConsoleKeyInfo arrow;
                        bool listability; //можно ли листать
                        do
                        {
                            Clear();
                            UpperStrip();
                            listability = WriteTextWithEnter(Sms[choosenow], 2, rownow);
                            arrow = Console.ReadKey(true);
                            if ((arrow.Key == ConsoleKey.DownArrow || arrow.Key == ConsoleKey.NumPad2) && listability)
                                rownow++;
                            else if ((arrow.Key == ConsoleKey.UpArrow || arrow.Key == ConsoleKey.NumPad8) && rownow>1)
                                rownow--;
                            else if (arrow.Key == ConsoleKey.NumPad5)
                            {
                                ConsoleKeyInfo action;
                            do
                            {
                                Clear();
                                UpperStrip();
                                Global.WriteAt("1.удалить", PosX+1, PosY+2);
                                Global.WriteAt("2.удалить все", PosX + 1, PosY + 3);
                                Global.WriteAt("0.выйти", PosX + 1, PosY + 7);
                                action = Console.ReadKey(true);
                                if (action.Key==ConsoleKey.NumPad1)
                                {
                                    Sms.RemoveAt(choosenow);
                                }
                                if (action.Key == ConsoleKey.NumPad2)
                                {
                                    ConsoleKeyInfo DeleteAll;
                                    Clear();
                                    UpperStrip();
                                    WriteTextWithEnter("Вы уверены, что хотите удалить всё?\n1.да\n0.нет", 2, 0);
                                    DeleteAll = Console.ReadKey(true);
                                    if (DeleteAll.Key==ConsoleKey.NumPad1)
                                    {
                                        Sms.Clear();
                                    }
                                }
                            } while (action.Key != ConsoleKey.NumPad0 && action.Key != ConsoleKey.NumPad1 && Sms.Count>0);
                            if (choosenow>0) choosenow--;
                            break;
                            }
                        } while (arrow.Key != ConsoleKey.NumPad0);
                    }
            } while (k.Key != ConsoleKey.NumPad0);
        }
        public void Calendar()
        {
            ConsoleKeyInfo k;
            int DayOfWeek = (int)DateTime.Now.DayOfWeek;
            DayOfWeek--;
            int Day = DateTime.Now.Day-1;
            int Month = DateTime.Now.Month;
            int Year = DateTime.Now.Year;
            bool Leap;
            int Days;
            if (DayOfWeek < 0) DayOfWeek += 8;
            do
            {
                Clear();
                if (Year % 400 == 0) Leap = true;
                else if (Year % 100 == 0) Leap = false;
                else if (Year % 4 == 0) Leap = true;
                else Leap = false;
                if (Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10 || Month == 12) Days = 31;
                else if (Month != 2) Days = 30;
                else
                {
                    if (Leap) Days = 29;
                    else Days = 28;
                }
                Global.WriteAt("П", PosX + 1, PosY + 1);
                Global.WriteAt("В", PosX + 1, PosY + 2);
                Global.WriteAt("С", PosX + 1, PosY + 3);
                Global.WriteAt("Ч", PosX + 1, PosY + 4);
                Global.WriteAt("П", PosX + 1, PosY + 5);
                Global.WriteAt("С", PosX + 1, PosY + 6);
                Global.WriteAt("В", PosX + 1, PosY + 7);
                int DayOfWeekNow = Global.FirstDayOfWeekInMonth(Month, Year, Leap);
                int extraX = 3;
                bool RightMonth = false;
                if (Year == DateTime.Now.Year && Month == DateTime.Now.Month) RightMonth = true;
                for (int i=0; i<Days; i++)
                {
                    Global.WriteAt(i + 1, PosX + extraX, PosY + DayOfWeekNow);
                    if (RightMonth && i==Day)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Global.WriteAt(i + 1, PosX + extraX, PosY + DayOfWeekNow);
                        Console.ResetColor();
                    }
                    DayOfWeekNow++;
                    if (DayOfWeekNow>7)
                    {
                        DayOfWeekNow = 1;
                        if (i > 8) extraX += 3;
                        else extraX += 2;
                    }
                }
                k = Console.ReadKey(true);
                Clear();
                switch (k.Key)
                {
                    case ConsoleKey.NumPad5:
                        {
                            Console.SetCursorPosition(PosX + 1, PosY + 1);
                            Console.Write($"{Month}.{Year}");
                            if (Leap) WriteText("Высокосный год", 2);
                            else WriteText("Невысокосный год", 2);
                            Console.ReadKey(true);
                            break;
                        }
                    case ConsoleKey.NumPad6:
                        {
                            Month++;
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            Month++;
                            break;
                        }
                    case ConsoleKey.NumPad4:
                        {
                            Month--;
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            Month--;
                            break;
                        }
                    case ConsoleKey.NumPad2:
                        {
                            Year--;
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            Year--;
                            break;
                        }
                    case ConsoleKey.NumPad8:
                        {
                            Year++;
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            Year++;
                            break;
                        }
                }
                if (Month < 1)
                {
                    Month = 12;
                    Year--;
                }
                if (Month > 12)
                {
                    Month = 1;
                    Year++;
                }
            }
            while (k.Key != ConsoleKey.NumPad0);
        }
        public void AllFiles()
        {
            ConsoleKeyInfo k;
            do
            {
                Clear();
                UpperStrip();
                WriteTextWithEnter("1.Музыка", 2, 0);
                k = Console.ReadKey(true);
                switch (k.Key)
                {
                    case ConsoleKey.NumPad1:
                        {
                            MusicFiles();
                            break;
                        }
                }
            } while (k.Key != ConsoleKey.NumPad0);
        }
        public void MusicFiles()
        {
            Clear();
            UpperStrip();
            WriteTextWithEnter("1.StarWars\n2.Кузнечик\n3.Ёлочка\n4.Невыполнимо\n5.ДеньРожд", 2,0);
            ConsoleKeyInfo k;
            do
            {
                k = Console.ReadKey(true);
                switch(k.Key)
                {
                    case ConsoleKey.NumPad1:
                        {
                            Music.StarWars();
                            break;
                        }
                    case ConsoleKey.NumPad2:
                        {
                            Music.Grasshoper();
                            break;
                        }
                    case ConsoleKey.NumPad3:
                        {
                            Music.Tannenbaum();
                            break;
                        }
                    case ConsoleKey.NumPad4:
                        {
                            Music.MissionImpossible();
                            break;
                        }
                    case ConsoleKey.NumPad5:
                        {
                            Music.HappyBirthday();
                            break;
                        }
                }
            } while (k.Key != ConsoleKey.NumPad0);
        }
        public void DialingNumber()
        {
            Clear();
            UpperStrip();
            ConsoleKeyInfo k;
            string numb="";
            Console.SetCursorPosition(PosX + 1, PosY + 2);
            do
            {
                WriteText(numb,2);
                k = Console.ReadKey(true);
                if (k.KeyChar>='0' && k.KeyChar<='9')
                {
                    numb += k.KeyChar;
                    switch (k.KeyChar)
                    {
                        case '1':
                            {
                                Console.Beep(600,200);
                                goto case '*';
                            }
                        case '2':
                            {
                                Console.Beep(650, 200);
                                goto case '*';
                            }
                        case '3':
                            {
                                Console.Beep(700, 200);
                                goto case '*';
                            }
                        case '4':
                            {
                                Console.Beep(750, 200);
                                goto case '*';
                            }
                        case '5':
                            {
                                Console.Beep(800, 200);
                                goto case '*';
                            }
                        case '6':
                            {
                                Console.Beep(850, 200);
                                goto case '*';
                            }
                        case '7':
                            {
                                Console.Beep(900, 200);
                                goto case '*';
                            }
                        case '8':
                            {
                                Console.Beep(950, 200);
                                goto case '*';
                            }
                        case '9':
                            {
                                Console.Beep(1000, 200);
                                goto case '*';
                            }
                        case '0':
                            {
                                Console.Beep(1050, 200);
                                goto case '*';
                            }
                        case '*':
                            {
                                Thread.Sleep(50);
                                break;
                            }
                    }
                }

            } while (k.Key != ConsoleKey.Enter);
        }
    }
}
