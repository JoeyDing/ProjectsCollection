using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TokenTestOnUI
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource cts;
        private List<Task> tasksList = new List<Task>();
        private double cancelAfterSecs = 40 * 1000;
        private Task task2;

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            try
            {
                await TestTask(() => Thread.Sleep(60 * 1000), cts.Token);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("\r\nDownload canceled.\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("\r\nDownload failed.\r\n");
            }
        }

        private async Task TestTask(Action action, CancellationToken ct)
        {
            await Task.Run(() =>
            {
                Thread thread = null;
                var task1 = Task.Run(() =>
                {
                    while (true)
                    {
                        //Thread.Sleep(1000);
                        ct.ThrowIfCancellationRequested();
                    }
                });

                task2 = Task.Run(() =>
               {
                   //do some heavy work
                   thread = Thread.CurrentThread;
                   action();
               });
                Task.WaitAny(task1, task2);
                if (task1.IsCompleted)
                {
                    thread.Abort();
                    //throw new OperationCanceledException();
                }
            });
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }
    }
}