using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp1.Emitter;
using static WindowsFormsApp1.Particle;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Emitter bodyEmitter, wingEmitter, headEmitter, heartEmitter;
        private int wingAngle = 0;
        private bool showBody = true, showWings = true, showHead = true, showHeart = true;
        List<Emitter> emitters = new List<Emitter>();

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            // Создание эмиттеров
            bodyEmitter = CreateEmitter(picDisplay.Width / 2, picDisplay.Height / 2 - 50, Color.Gold, 0);
            wingEmitter = CreateLineEmitter(picDisplay.Width / 2, picDisplay.Height / 2, 100, Color.LightBlue, Color.LightBlue);
            headEmitter = CreateEmitter(picDisplay.Width / 2, picDisplay.Height / 2, Color.Gold, -120);
            heartEmitter = CreateEmitter(picDisplay.Width / 2 + 20, picDisplay.Height / 2, Color.Red, -10);
            bodyEmitter.impactPoints.Add(new GravityPoint
            {
                Power = (int)Math.Pow((bodyEmitter.SpeedMax + bodyEmitter.SpeedMin) / 2, 2),
                X = bodyEmitter.X,
                Y = bodyEmitter.Y + 60,
            });
            headEmitter.impactPoints.Add(new GravityPoint
            {
                Power = (int)Math.Pow((headEmitter.SpeedMax + headEmitter.SpeedMin) / 2, 2),
                X = headEmitter.X,
                Y = headEmitter.Y + 30,
            });
            heartEmitter.impactPoints.Add(new GravityPoint
            {
                Power = (int)Math.Pow((heartEmitter.SpeedMax + heartEmitter.SpeedMin) / 2, 2),
                X = heartEmitter.X,
                Y = heartEmitter.Y + 2,
            });
            // Добавление эмиттеров в список
            emitters.Add(bodyEmitter);
            emitters.Add(wingEmitter);
            emitters.Add(headEmitter);
            emitters.Add(heartEmitter);
        }

        private Emitter CreateEmitter(int x, int y, Color color,int offset)
        {
            return new Emitter
            {
                GravitationY = 0, // отключил гравитацию
                Direction = 0, // направление 0
                Spreading = 10, // немного разбрасываю частицы, чтобы было интереснее
                SpeedMin = 10, // минимальная скорость 10
                SpeedMax = 10, // и максимальная скорость 10
                ColorFrom = color, // цвет начальный
                ParticlesPerTick = 3, // 3 частицы за тик генерю
                X = x, // x -- по центру экрана
                Y = y + offset, // y поднят вверх на offset
            };
        }
        private Emitter CreateLineEmitter(int x, int y, int length, Color colorFrom, Color colorTo)
        {
            return new Emitter
            {
                X = x,
                Y = y,
                Direction = -90,
                Spreading = 10,
                SpeedMin = 5,
                SpeedMax = 15,
                ColorFrom = colorFrom,
                ColorTo = colorTo,
                ParticlesPerTick = 3,
                impactPoints = { }
            };
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var emitter in emitters)
            {
                emitter.UpdateState();
            }

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);

                if (showBody)
                    bodyEmitter.Render(g);
                if (showWings)
                {
                    wingEmitter.Direction = -wingAngle;
                    wingEmitter.Render(g);
                    wingEmitter.Direction = wingAngle;
                    wingEmitter.Render(g);
                }
                if (showHead)
                    headEmitter.Render(g);
                if (showHeart)
                    heartEmitter.Render(g);
            }

            picDisplay.Invalidate();
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            // Нет необходимости в этом методе, так как мы не используем MousePositionX и MousePositionY
        }

        private void theDirection_Scroll(object sender, EventArgs e)
        {
            wingAngle = theDirection.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            showHeart = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            showBody = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            showHead = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            showWings = checkBox4.Checked;
        }
    }
}
