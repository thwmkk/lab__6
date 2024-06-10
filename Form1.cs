using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Particle> particles = new List<Particle>();

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            }

        private void UpdateState()
        {
            foreach (var particle in particles)
            {
                particle.Life -= 1; // уменьшаю здоровье
                                    // если здоровье кончилось
                if (particle.Life < 0)
                {
                    // восстанавливаю здоровье
                    particle.Life = 20 + Particle.rand.Next(100);
                    // перемещаю частицу в центр
                    particle.X = picDisplay.Image.Width / 2;
                    particle.Y = picDisplay.Image.Height / 2;
                    // делаю рандомное направление, скорость и размер
                    particle.Direction = Particle.rand.Next(360);
                    particle.Speed = 1 + Particle.rand.Next(10);
                    particle.Radius = 2 + Particle.rand.Next(10);
                    particle.Life = 20 + Particle.rand.Next(100); // это не трогаем
                    // новое начальное расположение частицы — это то, куда указывает курсор
                    particle.X = MousePositionX;
                    particle.Y = MousePositionY;
                }
                else
                {
                    // а это наш старый код
                    var directionInRadians = particle.Direction / 180 * Math.PI;
                    particle.X += (float)(particle.Speed * Math.Cos(directionInRadians));
                    particle.Y -= (float)(particle.Speed * Math.Sin(directionInRadians));
                }
            }
            for (var i = 0; i < 10; ++i)
            {
                if (particles.Count < 500) // пока частиц меньше 500 генерируем новые
                {
                    var particle = new Particle();
                    particle.X = MousePositionX;
                    particle.Y = MousePositionY;
                    particles.Add(particle);
                }
                else
                {
                    break; // а если частиц уже 500 штук, то ничего не генерирую
                }
            }
        }

        // функция рендеринга
        private void Render(Graphics g)
        {
            // утащили сюда отрисовку частиц
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
        }

        // ну и обработка тика таймера, тут просто декомпозицию выполнили
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateState(); // каждый тик обновляем систему

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.White);
                Render(g); // рендерим систему
            }

            picDisplay.Invalidate();
        }
        private int MousePositionX = 0;
        private int MousePositionY = 0;
        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            // в обработчике заносим положение мыши в переменные для хранения положения мыши
            MousePositionX = e.X;
            MousePositionY = e.Y;
        }
    }
}
