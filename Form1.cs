using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenShot
{
    public partial class Form1 : Form
    {
        private Point _startPoint;
        private Rectangle _selection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // Запоминаем начальную точку выделения
            _startPoint = e.Location;
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            // Рисуем выделенную область
            using (Brush brush = new SolidBrush(Color.FromArgb(128, 72, 145, 220)))
            {
                if (_selection.Width > 0 && _selection.Height > 0)
                {
                    e.Graphics.FillRectangle(brush, _selection);
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // Обновляем выделение при перемещении мыши
            if (e.Button == MouseButtons.Left)
            {
                int x = Math.Min(e.X, _startPoint.X);
                int y = Math.Min(e.Y, _startPoint.Y);
                int width = Math.Abs(e.X - _startPoint.X);
                int height = Math.Abs(e.Y - _startPoint.Y);
                _selection = new Rectangle(x, y, width, height);
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

            // Сохраняем скриншот выделенной области
            if (_selection.Width > 0 && _selection.Height > 0)
            {
                Bitmap bmp = new Bitmap(_selection.Width, _selection.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(_selection.Location, Point.Empty, _selection.Size);
                }
                Clipboard.SetImage(bmp);

                // Открываем новую форму и передаем ей изображение
                Edit edit = new Edit();
                edit.SetImage(bmp);
                edit.Show();

                // Скрываем текущую форму
                this.Hide();
            }
            _selection = new Rectangle(0, 0, 0, 0);
            Invalidate();
        }

    }

}
