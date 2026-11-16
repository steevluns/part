using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GameBooster
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public partial class MainForm : Form
    {
        private TextBox gamePathBox;
        private ComboBox resolutionSelector;
        private Button browseButton;
        private Button saveButton;
        private Button defaultButton;
        private Button boostButton;
        private Label statusLabel;

        private Dictionary<string, string> gameSettings = new Dictionary<string, string>();

        public MainForm()
        {
            this.Text = "مسرّع ألعاب ويندوز";
            this.Size = new Size(500, 350);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblPath = new Label() { Text = "مسار ملف اللعبة التنفيذي (.exe):", Location = new Point(20, 20), AutoSize = true };
            gamePathBox = new TextBox() { Location = new Point(20, 50), Width = 370 };

            browseButton = new Button() { Text = "...", Location = new Point(400, 48), Width = 50 };
            browseButton.Click += BrowseButton_Click;

            Label lblRes = new Label() { Text = "مستوى دقة العرض:", Location = new Point(20, 90), AutoSize = true };
            resolutionSelector = new ComboBox() { Location = new Point(20, 120), Width = 200 };
            resolutionSelector.Items.AddRange(new string[] { "منخفضة جداً", "منخفضة", "متوسطة", "عالية", "عالية جداً" });
            resolutionSelector.SelectedIndex = 1;

            saveButton = new Button() { Text = "حفظ الإعدادات", Location = new Point(230, 120), Width = 100 };
            saveButton.Click += SaveButton_Click;

            defaultButton = new Button() { Text = "الوضع الافتراضي", Location = new Point(340, 120), Width = 110 };
            defaultButton.Click += DefaultButton_Click;

            boostButton = new Button() { Text = "تشغيل اللعبة مع التسريع", Location = new Point(20, 170), Width = 430, Height = 40 };
            boostButton.BackColor = Color.LightGreen;
            boostButton.Click += BoostButton_Click;

            statusLabel = new Label() { Text = "الحالة: جاهز", Location = new Point(20, 230), AutoSize = true };

            this.Controls.Add(lblPath);
            this.Controls.Add(gamePathBox);
            this.Controls.Add(browseButton);
            this.Controls.Add(lblRes);
            this.Controls.Add(resolutionSelector);
            this.Controls.Add(saveButton);
            this.Controls.Add(defaultButton);
            this.Controls.Add(boostButton);
            this.Controls.Add(statusLabel);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "ملفات الألعاب (*.exe)|*.exe";
                if (ofd.ShowDialog() == DialogResult.OK)
                    gamePathBox.Text = ofd.FileName;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gamePathBox.Text))
            {
                gameSettings[gamePathBox.Text] = resolutionSelector.SelectedItem.ToString();
                statusLabel.Text = "الحالة: تم حفظ الإعدادات للعبة.";
            }
            else
                MessageBox.Show("الرجاء اختيار مسار اللعبة أولاً.");
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gamePathBox.Text) && gameSettings.ContainsKey(gamePathBox.Text))
            {
                gameSettings.Remove(gamePathBox.Text);
                statusLabel.Text = "الحالة: تم إلغاء التغييرات والعودة للوضع الافتراضي.";
            }
        }

        private void BoostButton_Click(object sender, EventArgs e)
        {
            string path = gamePathBox.Text;
            if (System.IO.File.Exists(path))
            {
                try
                {
                    Process.Start(path);
                    statusLabel.Text = "الحالة: تم تشغيل اللعبة. ملاحظة: خيارات الدقة المحفوظة لا تُطبّق تلقائياً على إعدادات اللعبة.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ أثناء تشغيل اللعبة: " + ex.Message);
                }
            }
            else
                MessageBox.Show("مسار اللعبة غير صحيح.");
        }
    }
}