using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace MouseMoving
{
    public partial class Form1 : Form
    {

        public const uint ES_CONTINUOUS = 0x80000000;
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;
        public const uint ES_DISPLAY_REQUIRED = 0x00000002;
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SetThreadExecutionState([In] uint esFlags);

        static System.Drawing.Point TargetPoint;
        static double Angle = 0d;

        private TextBox textBox1;
        private Cursor cursor;

        bool exit = false;
        private Keys keyData;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Lazy";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)

        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Start_Click(object sender, EventArgs e)
        {


            SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED);

            Start.Enabled = false;
            this.Cursor = new Cursor(Cursor.Current.Handle);

            exit = false;
            Task.Run(async () =>
            {
                while (!exit && keyData != Keys.Escape)
                {
                    // do the work in the loop
                    string newData = DateTime.Now.ToLongTimeString();
                    double Radius = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 3;
                    Angle += Math.PI / 30;
                    
                    Cursor.Position = new System.Drawing.Point((int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2 + Math.Cos(Angle) * Radius), (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2 + Math.Sin(Angle) * Radius));
                    // update the UI on the UI thread
                    //Dispatcher.Invoke(() => txtTicks.Text = "TASK - " + newData);

                     //don't run again for at least 200 milliseconds
                    await Task.Delay(10000);
                }
            });
       
        }
       
        
        private void stop_Click(object sender, EventArgs e)
        {
            Start.Enabled = true;
            exit = true;
            SetThreadExecutionState(ES_CONTINUOUS);
        }
    }
}
