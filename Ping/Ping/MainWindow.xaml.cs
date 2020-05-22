using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading; 


namespace Ping
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int Time; // отсчет времени

        Ellipse O; // объект шарика

        double R = 10; // радиус шарика
         
        double x, y; // положение шарика

        double V = 3; // начальная скорость шарика
        double vx, vy; // направление скорости шарика


        Rectangle Plate; // объект ракетки

        double H = 100; // ширина ракетки
        double Px; // положение ракетки
        double Pv = 25; // скорость движения ракетки

        DispatcherTimer Timer; // таймер

        public MainWindow()
        {
            InitializeComponent();

            Restart(); // Инициализировать переменные, задающие положение

            O = new Ellipse(); // создать объект - шарик
            O.Fill = Brushes.Red; // цвет - красный
            O.Width = 2 * R; // ширина
            O.Height = 2 * R; // высота
            O.Margin = new Thickness(x, y, 0, 0); // положение шара 
            g.Children.Add(O); // добавить на холст


            Plate = new Rectangle(); // создать объект - ракетка
            Plate.Fill = Brushes.Blue; // цвет - синий
            Plate.Width = H; // ширина ракетки
            Plate.Height = 5; // высота ракетки
            Px = g.Width / 2 - H / 2; // начальное положение ракетки
            Plate.Margin = new Thickness(Px, g.Height, 0, 0); // положение ракетки
            g.Children.Add(Plate); // добавить на холст

            Timer = new DispatcherTimer(); // создать таймер
            Timer.Tick += new EventHandler(onTick); // подключить обрабочик таймера
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 10); // установить таймер на 10 миллисекунд
            Timer.Start(); // включить таймер
        }

        void Restart()
        {
            x = g.Width / 2 - R; // начальное положение шарика - центр холста
            y = g.Height / 2 - R;

            Random rnd = new Random(); // создать генератор случайных чисел

            double alpha = rnd.NextDouble() * Math.PI / 2 + Math.PI / 4; // установить начальный угол полета

            vx = V * Math.Cos(alpha); // задать направление скорости полета
            vy = V * Math.Sin(alpha); // переменная V - это модуль скорости

            Px = g.Width / 2 - H / 2; // начальное положение ракетки

            Time = 0; // сбросить время
        }

        // этот метод вызывается таймером в каждый тик
        void onTick(object sender, EventArgs e)
        {
            Time++; // увеличить отсчет времени

            if ((x < 0) || (x > g.Width - 2 * R))
            {
                vx = -vx; // если шарик ударился о вертикальную стенку, то отразить 
            }

            if ((y < 0) || (y > g.Height - 2 * R))
            {
                vy = -vy; // если шарик ударился о горизонтальную стенку, то отразить 
            }

            // если ударились о нижнюю стенку
            if (y > g.Height - 2 * R)
            {
                double c = x + R; // x-координата центра шарика

                // проверить отразили ли мы ракеткой
                if ((c >= Px) && (c <= Px + H))
                {
                    vx *= 1.1; // если отразили, то немного увеличить скорость
                    vy *= 1.1;
                }
                else
                {
                    MessageBox.Show("A-a-a-a-a-a"); // если не отразили, то сообщение
                    Restart(); // и начать все занова
                    Plate.Margin = new Thickness(Px, g.Height, 0, 0); // положение ракетки
                }
            }

            x += vx; // изменить координаты шарика
            y += vy;

            O.Margin = new Thickness(x, y, 0, 0); // изменить положение шарика

            tbTime.Text = (Time / 100).ToString(); // обновить отсчет времени
        }

        // обработка нажатия клавиши
        private void cmKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) 
            {
                Px -= Pv; // если нажата стрелка влево
            }

            if (e.Key == Key.Right)
            {
                Px += Pv; // если нажата стрелка вправо
            }

            if (Px < 0)
            {
                Px = 0; // предотвратить выход ракетки за игровое поле
            }

            if (Px > g.Width - H)
            {
                Px = g.Width - H; // предотвратить выход ракетки за игровое поле
            }

            Plate.Margin = new Thickness(Px, g.Height, 0, 0); // изменить положение ракетки

        }
    }
}
