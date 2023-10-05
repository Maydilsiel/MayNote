using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MayPad
{
    public partial class settings : Form
    {
        Form1 adminForm;
        public settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminForm = (Form1)this.Owner;

            if (comboBox1.SelectedItem != null)
            {
                ///Themes
                string theme = comboBox1.SelectedItem.ToString();
                adminForm = (Form1)this.Owner;
                switch (theme)
                {
                    case "Cветлая":
                        theme0();
                        break;
                    case "Темная":
                        theme1();
                        break;
                    case "Темная с зелённым текстом":
                        theme2();
                        break;
                    case "Indigo":
                        themeindigo();
                        break;
                    default:
                        theme0();
                        break;
                }

                if (comboBox2.SelectedItem != null)
                {
                    ///Themes
                    string line = comboBox2.SelectedItem.ToString();
                    adminForm = (Form1)this.Owner;
                    switch (line)
                    {
                        case "Включить":
                            enablelinenumber();
                            break;
                        case "Отключить":
                            disablelinenumber();
                            break;
                        default:
                            enablelinenumber();
                            break;
                    }
                }

                if (comboBox6.SelectedItem != null)
                {
                    ///Themes
                    string statusb = comboBox6.SelectedItem.ToString();
                    adminForm = (Form1)this.Owner;
                    switch (statusb)
                    {
                        case "Включить":
                            enablestatus();
                            break;
                        case "Отключить":
                            disablestatus();
                            break;
                        default:
                            enablestatus();
                            break;
                    }
                }

                

                ///Saving
                Properties.Settings.Default.settingssave = comboBox1.SelectedItem.ToString();
                Properties.Settings.Default.settingssave2 = comboBox2.SelectedItem.ToString();
                Properties.Settings.Default.settingssave3 = comboBox6.SelectedItem.ToString();
                Properties.Settings.Default.ComboBox1SelectedIndex = comboBox1.SelectedIndex;
                Properties.Settings.Default.ComboBox2SelectedIndex = comboBox2.SelectedIndex;
                Properties.Settings.Default.ComboBox6SelectedIndex = comboBox6.SelectedIndex;
                Properties.Settings.Default.Save();
            }
            else
            {
                // Обязательные элементы не выбраны, вывод ошибки
                MessageBox.Show("Пожалуйста, выберите все обязательные элементы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void theme0()
        {
            adminForm = (Form1)this.Owner;
            adminForm.txtbox.BackColor = Color.White;
            adminForm.txtbox.ChangedLineColor = Color.White;
            adminForm.txtbox.IndentBackColor = Color.White;
            adminForm.txtbox.LineNumberColor = Color.Black;
            adminForm.txtbox.ForeColor = Color.Black;
        }

        public void theme1()
        {
            adminForm = (Form1)this.Owner;
            adminForm.txtbox.BackColor = Color.Black;
            adminForm.txtbox.ChangedLineColor = Color.Black;
            adminForm.txtbox.IndentBackColor = Color.Black;
            adminForm.txtbox.LineNumberColor = Color.White;
            adminForm.txtbox.ForeColor = Color.White;
        }

        public void theme2()
        {
            adminForm = (Form1)this.Owner;
            adminForm.txtbox.BackColor = Color.Black;
            adminForm.txtbox.ChangedLineColor = Color.Black;
            adminForm.txtbox.IndentBackColor = Color.Black;
            adminForm.txtbox.LineNumberColor = Color.GreenYellow;
            adminForm.txtbox.ForeColor = Color.GreenYellow;
        }

        public void themeindigo()
        {
            adminForm = (Form1)this.Owner;
            adminForm.txtbox.BackColor = Color.DarkBlue;
            adminForm.txtbox.ChangedLineColor = Color.DarkBlue;
            adminForm.txtbox.IndentBackColor = Color.DarkBlue;
            adminForm.txtbox.LineNumberColor = Color.Yellow;
            adminForm.txtbox.ForeColor = Color.Yellow;
        }

        public void enablelinenumber() {
            adminForm = (Form1)this.Owner;
            adminForm.txtbox.ShowLineNumbers = true;
        }

        public void disablelinenumber()
        {
            adminForm = (Form1)this.Owner;
            adminForm.txtbox.ShowLineNumbers = false;
        }

        public void enablestroka()
        {
            adminForm = (Form1)this.Owner;
            adminForm.statusBarPanel1.Style = StatusBarPanelStyle.Text;
        }

        public void disablestroka()
        {
            adminForm = (Form1)this.Owner;
            adminForm.statusBarPanel1.Style = StatusBarPanelStyle.OwnerDraw;
        }

        public void enablestatus()
        {
            adminForm = (Form1)this.Owner;
            adminForm.statusBar1.Visible = true;
        }

        public void disablestatus()
        {
            adminForm = (Form1)this.Owner;
            adminForm.statusBar1.Visible = false;
        }


        private void settings_Load(object sender, EventArgs e)
        {
            // Проверьте, есть ли сохраненный индекс в настройках
            if (Properties.Settings.Default.ComboBox1SelectedIndex >= 0 &&
                Properties.Settings.Default.ComboBox1SelectedIndex < comboBox1.Items.Count)
            {
                // Установите сохраненный индекс для ComboBox
                comboBox1.SelectedIndex = Properties.Settings.Default.ComboBox1SelectedIndex;
            }

            // Проверьте, есть ли сохраненный индекс в настройках
            if (Properties.Settings.Default.ComboBox2SelectedIndex >= 0 &&
                Properties.Settings.Default.ComboBox2SelectedIndex < comboBox2.Items.Count)
            {
                // Установите сохраненный индекс для ComboBox
                comboBox2.SelectedIndex = Properties.Settings.Default.ComboBox2SelectedIndex;
            }

            // Проверьте, есть ли сохраненный индекс в настройках
            if (Properties.Settings.Default.ComboBox6SelectedIndex >= 0 &&
                Properties.Settings.Default.ComboBox6SelectedIndex < comboBox2.Items.Count)
            {
                // Установите сохраненный индекс для ComboBox
                comboBox6.SelectedIndex = Properties.Settings.Default.ComboBox6SelectedIndex;
            }
        }
    }
}
