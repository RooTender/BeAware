using System;
using System.Threading;
using System.Windows.Forms;

namespace BeAware
{
    public partial class Form1 : Form
    {
        private int _time = 0;
        private int limit;

        private void popup(double time)
        {
            limit = (int)Properties.Settings.Default["limit"];

            if (time % 30 == 0)
            {
                notify.BalloonTipText = "Another 30 minutes passed 💨!\n" + time / 60 + " hours elapsed.";
                notify.ShowBalloonTip(3);
            }

            if (time == limit - 5)
            {
                notify.BalloonTipText = "Only 5 minutes left 🙉!";
                notify.ShowBalloonTip(3);
            }

            if (time == limit - 3)
            {
                notify.BalloonTipText = "Only 3 minutes left 🙊!";
                notify.ShowBalloonTip(3);
            }

            if (time == limit - 1)
            {
                notify.BalloonTipText = "Only 1 MINUTE left 🙈!";
                notify.ShowBalloonTip(3);
            }

            if (time >= limit)
            {
                notify.BalloonTipText = "Time's up 💥💤!";
                notify.ShowBalloonTip(3);
                Thread.Sleep(5000);
                Application.SetSuspendState(PowerState.Suspend, true, true);
                Application.Exit();
            }
        }

        private void calcTime()
        {
            try
            {
                int max = int.Parse(textBox1.Text);

                if (max < 60)
                    label2.Text = max + " minutes";
                else
                    label2.Text = (max / 60) + " hours and " + (max % 60) + " minutes";
            }
            catch (Exception)
            {
                textBox1.Text = "0";
            }
        }

        public Form1()
        {
            InitializeComponent();
            notify.Visible = true;
            calcTime();
            timer1.Start();
            textBox1.Text = Properties.Settings.Default["limit"].ToString();
            notify.BalloonTipText = "BeAware is working ⏰!";
            notify.ShowBalloonTip(1);
            notify.BalloonTipText = "Double click the icon to show settings 🛠";
            notify.ShowBalloonTip(1);
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                notify.BalloonTipText = "Double click again to hide settings 😉";
                notify.ShowBalloonTip(3);
            }
            else
                WindowState = FormWindowState.Minimized;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _time++;
            label3.Text = "Time elapsed: ";

            if (_time < 60)
                label3.Text += _time + " minutes";
            else
                label3.Text += (_time / 60) + " hours and " + (_time % 60) + " minutes";

            popup(_time);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Properties.Settings.Default["limit"] = 120;
            Properties.Settings.Default.Save();
            timer1.Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            calcTime();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
                e.Handled = true;
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (int.Parse(textBox1.Text) < 10)
                MessageBox.Show("Time limit must be greater than 10 minutes!", "Time delay", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                timer1.Stop();
                notify.BalloonTipText = "Saved ✔";
                notify.ShowBalloonTip(2);
                Properties.Settings.Default["limit"] = int.Parse(textBox1.Text);
                Properties.Settings.Default.Save();
                timer1.Start();
            }
        }
    }
}
