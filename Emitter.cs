using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp1.Particle;

namespace WindowsFormsApp1
{
    internal class Emitter
    {
        public List<Point> gravityPoints = new List<Point>(); // тут буду хранится точки притяжения
        List<Particle> particles = new List<Particle>();
        public int MousePositionX;
        public int MousePositionY;
        public float GravitationX = 0;
        public float GravitationY = 0; // пусть гравитация будет силой один пиксель за такт, нам хватит

        public void UpdateState()
        {
            foreach (var particle in particles)
            {
                particle.Life -= 1;  // не трогаем
                if (particle.Life < 0)
                {
                    // тоже не трогаем
                    particle.Life = 20 + Particle.rand.Next(100);
                    particle.X = MousePositionX;
                    particle.Y = MousePositionY;

                    /* это убираем
                    particle.Direction = Particle.rand.Next(360);
                    particle.Speed = 1 + Particle.rand.Next(10);
                    */

                    /* ЭТО ДОБАВЛЯЮ, тут сброс состояния частицы */
                    var direction = (double)Particle.rand.Next(360);
                    var speed = 1 + Particle.rand.Next(10);

                    particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;

                    // это не трогаем
                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;

                    // это не трогаем
                    particle.Radius = 2 + Particle.rand.Next(10);
                }
                else
                {
                    // каждая точка по-своему воздействует на вектор скорости
                    foreach (var point in gravityPoints)
                    {
                        float gX = point.X - particle.X;
                        float gY = point.Y - particle.Y;
                        float r2 = (float)Math.Max(100, gX * gX + gY * gY); // ограничил
                        float M = 100;

                        particle.SpeedX += (gX) * M / r2;
                        particle.SpeedY += (gY) * M / r2;
                    }

                    // это не трогаем
                    particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;

                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;
            }
            }
            for (var i = 0; i < 10; ++i)
            {
                if (particles.Count < 500)
                {
                    // а у тут уже наш новый класс используем
                    var particle = new ParticleColorful();
                    // ну и цвета меняем
                    particle.FromColor = Color.Yellow;
                    particle.ToColor = Color.FromArgb(0, Color.Magenta);
                    particle.X = MousePositionX;
                    particle.Y = MousePositionY;
                    particles.Add(particle);
                }
                else
                {
                    break;
                }
            }
        }
            

        public void Render(Graphics g)
        {
            // это не трогаем
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }

            // рисую точки притяжения красными кружочками
            foreach (var point in gravityPoints)
            {
                g.FillEllipse(
                    new SolidBrush(Color.Red),
                    point.X - 5,
                    point.Y - 5,
                    10,
                    10
                );
            }
        }
    }
}
