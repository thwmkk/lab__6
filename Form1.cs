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
        Emitter emitter; // добавим поле для эмиттера

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            // Настройка эмиттеров
            bodyEmitter = CreateEmitter(picDisplay.Width / 2, picDisplay.Height / 2 + 50, 0, 360, 10, 20, Color.Sienna, Color.Sienna);
            wingEmitter = CreateEmitter(picDisplay.Width / 2, picDisplay.Height / 2, 0, 360, 10, 20, Color.LightBlue, Color.LightBlue);
            headEmitter = CreateEmitter(picDisplay.Width / 2, picDisplay.Height / 2 - 50, 0, 360, 10, 20, Color.Wheat, Color.Wheat);
            heartEmitter = CreateEmitter(picDisplay.Width / 2, picDisplay.Height / 2 - 20, 0, 360, 10, 20, Color.Red, Color.Red);

            // Добавление эмиттеров в список
            emitters.Add(bodyEmitter);
            emitters.Add(wingEmitter);
            emitters.Add(headEmitter);
            emitters.Add(heartEmitter);
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

            // Обновление угла крыльев
            wingAngle = (wingAngle + 5) % 360;
            theDirection.Value = wingAngle;
        }
        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            // а тут в эмиттер передаем положение мыфки
            emitter.MousePositionX = e.X;
            emitter.MousePositionY = e.Y;
        }

        private void theDirection_Scroll(object sender, EventArgs e)
        {
            emitter.Direction = theDirection.Value; // направлению эмиттера присваиваем значение ползунка 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }
        private Emitter CreateEmitter(int x, int y, int direction, int spreading, int speedMin, int speedMax, Color colorFrom, Color colorTo)
        {
            return new Emitter
            {
                X = x,
                Y = y,
                Direction = direction,
                Spreading = spreading,
                SpeedMin = speedMin,
                SpeedMax = speedMax,
                ColorFrom = colorFrom,
                ColorTo = colorTo,
                ParticlesPerTick = 3
            };
        }
    }
}
}
