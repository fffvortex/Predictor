using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Predictor
{
    public partial class Form1 : Form
    {
        private const string APP_NAME = "PREDICT";
        static private string currentDirectory = Environment.CurrentDirectory;
        private string _predictions_path = currentDirectory + $"\\PredictionConfig.json";
        private string[] _predictions;
        Font myFont;
        public Form1()
        {
            InitializeComponent();
            LoadFont();
            button1.Font = myFont;

        }
        private void LoadFont()
        {
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile("Bender.otf");
            myFont = new Font(fontCollection.Families[0], 30);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            pictureBox1.Visible = true;
            await Task.Run(() =>
            {
                UpdateProgressBar();
            });
            pictureBox1.Visible = false;
            MessageBox.Show(GetRandomPredict());
            this.Text = APP_NAME;
            button1.Enabled = true;
        }
        private string GetRandomPredict()
        {
            var random = new Random();
            return _predictions[random.Next(_predictions.Length)];
        }
        private void UpdateProgressBar()
        {
            for (int i = 0; i <= 100; i++)
            {
                if (progressBar1.Maximum == i)
                {
                    progressBar1.Maximum = i + 1;
                    progressBar1.Value = i + 1;
                    progressBar1.Maximum = i;
                }
                else
                {
                    progressBar1.Value = i + 1;
                }
                this.Text = $"{i}%";
                Thread.Sleep(10);
                progressBar1.Value = i;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = APP_NAME;
            try
            {
                var data = File.ReadAllText(_predictions_path);
                _predictions = JsonConvert.DeserializeObject<string[]>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_predictions == null)
                {
                    Close();
                }
                else if (_predictions.Length == 0)
                {
                    MessageBox.Show("NOT ENOUGH PREDICTIONS");
                    Close();
                }
            }
        }
    }
}
