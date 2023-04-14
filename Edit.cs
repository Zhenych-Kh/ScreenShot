using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenShot
{
    public partial class Edit : Form
    {
        private bool isDragging = false;
        private Point lastCursor;
        private Point lastForm;
        public Edit()
        {
            InitializeComponent();
            button1.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.BorderSize = 0;
        }

        public void SetImage(Image image)
        {
            pictureBox1.Image = image;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|BMP Image|*.bmp";
                saveFileDialog.Title = "Save an Image File";
                saveFileDialog.FileName = "image";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(saveFileDialog.FileName);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(pictureBox1.Image);

            // Открываем Paint
            Process paintProcess = Process.Start("mspaint");

            // Даем Paint некоторое время, чтобы он успел запуститься
            System.Threading.Thread.Sleep(1000);

            // Получаем дескриптор окна Paint
            IntPtr paintHandle = paintProcess.MainWindowHandle;

            // Устанавливаем фокус на окно Paint
            SetForegroundWindow(paintHandle);

            // Отправляем команду Ctrl+V, чтобы вставить скопированное изображение
            SendKeys.Send("^v");
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private void button4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Edit_Load(object sender, EventArgs e)
        {
            PictureBox_Border(pictureBox1);
            // Подписываемся на события мыши
            this.MouseDown += Edit_MouseDown;
            this.MouseMove += Edit_MouseMove;
            this.MouseUp += Edit_MouseUp;
        }
        
        private void PictureBox_Border(PictureBox pictureBox)
        {
            pictureBox1.Paint += (sender, e) =>
            {
                var picBox = sender as PictureBox;
                var g = e.Graphics;
                var pen = new Pen(Color.DarkGray, 3); // задаем цвет и толщину рамки
                g.DrawRectangle(pen, picBox.DisplayRectangle.X, picBox.DisplayRectangle.Y,
                    picBox.DisplayRectangle.Width - 2, picBox.DisplayRectangle.Height - 2);
            };
        }

        private void Edit_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastCursor = Cursor.Position;
                lastForm = this.Location;
            }
        }

        private void Edit_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                {
                    int deltaX = Cursor.Position.X - lastCursor.X;
                    int deltaY = Cursor.Position.Y - lastCursor.Y;
                    this.Location = new Point(lastForm.X + deltaX, lastForm.Y + deltaY);
                }
            }
        }

        private void Edit_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }
    }
}
