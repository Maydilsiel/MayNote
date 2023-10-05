using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Windows.Controls;
using System.IO;
using FastColoredTextBoxNS;
using Ude;
using System.Diagnostics;
using System.Net.Http;

namespace MayPad
{

    public partial class Form1 : Form

    {
        //Преднастройки MayNote
        private bool DataHasBeenModified = false;
        //private bool TextHasChanged = false;
        private string fileContent = string.Empty;
        public bool isFileChanged;
        private string currentFilePath;
        private int currentFontSize = 10;

        settings adminForm;
        public string SelectedTheme { get; set; }
        public string NumberLine { get; set; }
        public string StatusBar { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(string content)
        {
            InitializeComponent();
            fileContent = content;
            txtbox.Text = fileContent; // Здесь мы присваиваем содержимое файла полю txtbox
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Безымянный - MayNote";
            //txtbox.Font = new Font("Consolas", 10, FontStyle.Regular);
                adminForm = (settings)this.Owner;

                SelectedTheme = Properties.Settings.Default.settingssave;
                string theme = Properties.Settings.Default.settingssave;
                switch (theme)
                {
                    case "Светлая":
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

            NumberLine = Properties.Settings.Default.settingssave2;
            string line = Properties.Settings.Default.settingssave2;
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

            StatusBar = Properties.Settings.Default.settingssave3;
            string stbar = Properties.Settings.Default.settingssave3;
            switch (stbar)
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

        public void enablestatus()
        {
            statusBar1.Visible = true;
        }

        public void disablestatus()
        {
            statusBar1.Visible = false;
        }

        public void enablelinenumber()
        {
            txtbox.ShowLineNumbers = true;
        }

        public void disablelinenumber()
        {
            txtbox.ShowLineNumbers = false;
        }

        public void theme0()
        {
            txtbox.BackColor = Color.White;
            txtbox.ChangedLineColor = Color.White;
            txtbox.IndentBackColor = Color.White;
            txtbox.LineNumberColor = Color.Black;
            txtbox.ForeColor = Color.Black;
        }

        public void theme1()
        {
            txtbox.BackColor = Color.Black;
            txtbox.ChangedLineColor = Color.Black;
            txtbox.IndentBackColor = Color.Black;
            txtbox.LineNumberColor = Color.White;
            txtbox.ForeColor = Color.White;
        }

        public void theme2()
        {
            txtbox.BackColor = Color.Black;
            txtbox.ChangedLineColor = Color.Black;
            txtbox.IndentBackColor = Color.Black;
            txtbox.LineNumberColor = Color.GreenYellow;
            txtbox.ForeColor = Color.GreenYellow;
        }

        public void themeindigo()
        {
            txtbox.BackColor = Color.DarkBlue;
            txtbox.ChangedLineColor = Color.DarkBlue;
            txtbox.IndentBackColor = Color.DarkBlue;
            txtbox.LineNumberColor = Color.Yellow;
            txtbox.ForeColor = Color.Yellow;
        }

        private void menuItem35_Click(object sender, EventArgs e)
        {

        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtbox.Text))
            {
                DialogResult result = MessageBox.Show("Текущий документ содержит текст. Вы уверены, что хотите открыть новый файл?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return; // Прерываем открытие файла
                }
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;

                // Автоматически распознаем кодировку
                string fileContent = ReadTextFileWithAutoEncoding(selectedFileName, out string detectedEncoding);

                txtbox.Text = fileContent;
                //txtbox.SelectionLength = 0;
                UpdateFormTitle(selectedFileName);
                statusBarPanel6.Text = $"Кодировка: {detectedEncoding ?? "Неизвестно"}";
            }
        }

        private string ReadTextFileWithAutoEncoding(string filePath, out string detectedEncoding)
        {
            detectedEncoding = null;
            byte[] bytes = File.ReadAllBytes(filePath);
            CharsetDetector detector = new CharsetDetector();
            detector.Feed(new MemoryStream(bytes)); // Преобразуем byte[] в MemoryStream
            detector.DataEnd();
            if (detector.Charset != null)
            {
                try
                {
                    Encoding encoding = Encoding.GetEncoding(detector.Charset);
                    detectedEncoding = encoding.EncodingName;
                    return encoding.GetString(bytes);
                }
                catch (ArgumentException)
                {
                    // Если определенная кодировка недопустима, используем кодировку по умолчанию (например, UTF-8)
                    detectedEncoding = "Ошибка определения";
                    return Encoding.UTF8.GetString(bytes);
                }
            }
            else
            {
                // Если не удалось определить кодировку, используем кодировку по умолчанию (например, UTF-8)
                detectedEncoding = "Неизвестно";
                return Encoding.UTF8.GetString(bytes);
            }
        }


        private void UpdateFormTitle(string fileName)
        {
            // Устанавливаем заголовок формы с именем файла
            this.Text = $"{Path.GetFileName(fileName)} - MayNote";
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (DataHasBeenModified)
            {
                DialogResult result = MessageBox.Show("Текущие данные были изменены. Хотите сохранить изменения?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    // Настройте фильтр и сохранение файла

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(saveFileDialog.FileName, txtbox.Text);
                        txtbox.Clear();
                        DataHasBeenModified = false;
                    }
                }
                else if (result == DialogResult.No)
                {
                    txtbox.Clear();
                    DataHasBeenModified = false;
                    this.Text = $"Безымянный - MayNote";

                }
                // Если результат - DialogResult.Cancel, ничего не делаем
            }
            else
            {
                this.Text = $"Безымянный - MayNote";
                txtbox.Clear();
                DataHasBeenModified = false;
            }
        }

        private void txtbox_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (!DataHasBeenModified)
            {
                this.Text = this.Text.Replace('*', ' ');
                DataHasBeenModified = true;
                this.Text = "* " + this.Text;
            }
            DataHasBeenModified = true;
            int selectedStart = txtbox.Selection.Start.iChar;
            int selectedLine = txtbox.Selection.Start.iLine;

            int column = selectedStart + 1; // +1, так как индексы начинаются с 0

            statusBarPanel1.Text = $"Строка: {selectedLine + 1}"; // +1, так как индексы начинаются с 0
            statusBarPanel2.Text = $"Символы: {column}";
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = saveFileDialog1.FileName;

                // Получаем текст из TextBox
                string textToSave = txtbox.Text;

                try
                {
                    // Записываем текст в выбранный файл
                    File.WriteAllText(selectedFileName, textToSave);
                    MessageBox.Show("Файл успешно сохранен.");
                    DataHasBeenModified = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении файла: " + ex.Message);
                }
            }
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                // Если текущий файл не был сохранен, то сохраняем как новый файл
                SaveFileAs();
            }
            else
            {
                // Иначе, перезаписываем текущий файл
                SaveFile(currentFilePath);
            }
        }

        // Метод для сохранения файла как нового
        private void SaveFileAs()
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = saveFileDialog1.FileName;
                SaveFile(selectedFileName);
            }
        }

        // Метод для сохранения файла по указанному пути
        private void SaveFile(string filePath)
        {
            string textToSave = txtbox.Text;

            try
            {
                File.WriteAllText(filePath, textToSave);
                MessageBox.Show("Файл успешно сохранен.");
                DataHasBeenModified = false;
                currentFilePath = filePath; // Обновляем текущий путь к файлу
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении файла: " + ex.Message);
            }
        }

        private void menuItem38_Click(object sender, EventArgs e)
        {
            txtbox.Language = FastColoredTextBoxNS.Language.CSharp;
        }

        private void menuItem47_Click(object sender, EventArgs e)
        {
            txtbox.Language = FastColoredTextBoxNS.Language.Custom;
        }

        private void menuItem40_Click(object sender, EventArgs e)
        {
            txtbox.Language = FastColoredTextBoxNS.Language.HTML;
        }

        private void menuItem41_Click(object sender, EventArgs e)
        {
            txtbox.Language = FastColoredTextBoxNS.Language.XML;
        }

        private void menuItem42_Click(object sender, EventArgs e)
        {
            txtbox.Language = FastColoredTextBoxNS.Language.SQL;
        }

        private void menuItem43_Click(object sender, EventArgs e)
        {
            txtbox.Language = FastColoredTextBoxNS.Language.PHP;
        }

        private void menuItem44_Click(object sender, EventArgs e)
        {
            txtbox.Language = FastColoredTextBoxNS.Language.JS;
        }

        private void menuItem45_Click(object sender, EventArgs e)
        {
            txtbox.Language = FastColoredTextBoxNS.Language.Lua;
        }

        private void menuItem46_Click(object sender, EventArgs e)
        {
            txtbox.Language = FastColoredTextBoxNS.Language.JSON;
        }

        private void menuItem39_Click(object sender, EventArgs e)
        {
            txtbox.Language = FastColoredTextBoxNS.Language.VB;
        }

        private void menuItem13_Click(object sender, EventArgs e)
        {
            txtbox.Copy();
        }

        private void menuItem15_Click(object sender, EventArgs e)
        {
            txtbox.Paste();
        }

        private void menuItem14_Click(object sender, EventArgs e)
        {
            if (txtbox.SelectionLength > 0)
            {
                txtbox.Cut();
            }
        }

        private void menuItem17_Click(object sender, EventArgs e)
        {
            txtbox.Text += DateTime.Now;
        }

        private void menuItem19_Click(object sender, EventArgs e)
        {
            txtbox.SelectAll();
        }

        private void menuItem20_Click(object sender, EventArgs e)
        {
            txtbox.Undo();
        }

        private void menuItem21_Click(object sender, EventArgs e)
        {
            int cursorPosition = txtbox.SelectionStart;

            if (txtbox.SelectionLength > 0)
            {
                txtbox.SelectedText = "";
            }
            else if (cursorPosition < txtbox.Text.Length)
            {
                txtbox.Text = txtbox.Text.Remove(cursorPosition, 1);
                txtbox.SelectionStart = cursorPosition;
            }
        }

        private void check1()
        {
            if (!menuItem33.Checked)
            {
                menuItem33.Checked = true;
                txtbox.ReadOnly = true;
                MessageBox.Show("Активирован режим чтения.");
            }
            else if (menuItem33.Checked)
            {
                menuItem33.Checked = false;
                txtbox.ReadOnly = false;
                MessageBox.Show("Деактивирован режим чтения.");
            }
        }

        private void menuItem33_Click(object sender, EventArgs e)
        {
            check1();
        }

        private void txtbox_Load(object sender, EventArgs e)
        {

        }

        private void txtbox_SelectionChanged(object sender, EventArgs e)
        {
            int selectedStart = txtbox.Selection.Start.iChar;
            int selectedLine = txtbox.Selection.Start.iLine;

            int column = selectedStart + 1; // +1, так как индексы начинаются с 0

            statusBarPanel1.Text = $"Строка: {selectedLine + 1}"; // +1, так как индексы начинаются с 0
            statusBarPanel2.Text = $"Символы: {column}";
        }

        private void txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                // Сохраняем текущую позицию курсора
                int cursorPosition = txtbox.SelectionStart;

                // Проверяем, если курсор не находится в конце текста
                if (cursorPosition < txtbox.Text.Length)
                {
                    // Удаляем символ в текущей позиции курсора
                    txtbox.Text = txtbox.Text.Remove(cursorPosition, 1);

                    // Восстанавливаем позицию курсора
                    txtbox.SelectionStart = cursorPosition;
                }

                // Предотвращаем дальнейшую обработку клавиши "Delete"
                e.SuppressKeyPress = true;
            }
        }

        private void menuItem10_Click(object sender, EventArgs e)
        {
            // Открываем диалоговое окно для выбора кодировки
            EncodingSelectorForm encodingForm = new EncodingSelectorForm();
            if (encodingForm.ShowDialog() == DialogResult.OK)
            {
                Encoding selectedEncoding = encodingForm.SelectedEncoding;

                // Декодируем текст в выбранную кодировку
                byte[] encodedBytes = selectedEncoding.GetBytes(txtbox.Text);
                string decodedText = Encoding.UTF8.GetString(encodedBytes); // Декодируем в UTF-8 (можно заменить на нужную кодировку)

                // Обновляем текстовое поле с декодированным текстом
                txtbox.Text = decodedText;
            }
        }

        private void menuItem23_Click(object sender, EventArgs e)
        {
            search searchForm = new search(this);
            searchForm.Show();
        }

        private void menuItem35_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
            settings adminForm = new settings();
            this.AddOwnedForm(adminForm);
            adminForm.ShowDialog();
        }

        private void menuItem50_Click(object sender, EventArgs e)
        {
            using (var encodingSelectorForm = new EncodingSelectorForm())
            {
                if (encodingSelectorForm.ShowDialog() == DialogResult.OK)
                {
                    Encoding selectedEncoding = encodingSelectorForm.SelectedEncoding;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFileName = saveFileDialog1.FileName;

                        using (StreamWriter writer = new StreamWriter(selectedFileName, false, selectedEncoding))
                        {
                            writer.Write(txtbox.Text);
                        }
                        DataHasBeenModified = false;
                    }
                }
            }
        }

        private void menuItem49_Click(object sender, EventArgs e)
        {
            Form1 newwindow = new Form1();
            newwindow.Show();
        }

        private void menuItem36_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Содержимое справки \n22\n22", "Название загаловка", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void menuItem9_Click(object sender, EventArgs e)
        {
            aboutprogramms about = new aboutprogramms();
            about.ShowDialog();
        }

        private void menuItem51_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtbox.Text))
            {
                DialogResult result = MessageBox.Show("Текущий документ содержит текст. Рекомендуется сохранить документ, сохранить документ ? \n\n все несохраненные изменения будут утратчены", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Cancel)
                {
                    return; // Прерываем открытие файла
                }
                if (result == DialogResult.Yes)
                {
                    if (string.IsNullOrEmpty(currentFilePath))
                    {
                        // Если текущий файл не был сохранен, то сохраняем как новый файл
                        SaveFileAs();
                    }
                    else
                    {
                        // Иначе, перезаписываем текущий файл
                        SaveFile(currentFilePath);
                    }
                }
            }
            // Ваш HTML-код
            txtbox.Language = FastColoredTextBoxNS.Language.HTML;
            string htmlCode =
                @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Document</title>
    <link rel=""stylesheet"" href=""style.css"">
</head>
<body>
    
</body>
</html>";

            // Ваш TextBox
            txtbox.Text = htmlCode;
        }

        private void menuItem52_Click(object sender, EventArgs e)
        {
            currentFontSize += 2;
            txtbox.Font = new Font(txtbox.Font.FontFamily, currentFontSize);
        }

        private void menuItem54_Click(object sender, EventArgs e)
        {
            currentFontSize = 11;
            txtbox.Font = new Font(txtbox.Font.FontFamily, currentFontSize);
        }

        private void menuItem53_Click(object sender, EventArgs e)
        {
            // Уменьшаем размер шрифта на 2 пункта (задайте нужное значение)
            currentFontSize -= 2;

            // Проверяем, чтобы размер шрифта не стал меньше 1
            if (currentFontSize < 1)
            {
                currentFontSize = 1;
            }

            // Создаем новый шрифт с обновленным размером и применяем его к текстовому полю
            txtbox.Font = new Font(txtbox.Font.FontFamily, currentFontSize);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtbox.Text))
            {
                DialogResult result = MessageBox.Show("Текущий документ содержит текст. Перед выходом рекомендуется сохранить документ, сохранить документ ?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; // Прерываем открытие файла
                }
                if (result == DialogResult.Yes)
                {
                    if (string.IsNullOrEmpty(currentFilePath))
                    {
                        // Если текущий файл не был сохранен, то сохраняем как новый файл
                        SaveFileAs();
                    }
                    else
                    {
                        // Иначе, перезаписываем текущий файл
                        SaveFile(currentFilePath);
                    }
                }
            }
        }

        private async void menuItem28_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли выделенный текст
            if (txtbox.SelectionLength > 0)
            {
                string query = txtbox.SelectedText;
                string searchEngineUrl = "https://www.google.com/search?q=";

                await SearchOnSearchEngine(query, searchEngineUrl);
            }
        }

        private async void menuItem29_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли выделенный текст
            if (txtbox.SelectionLength > 0)
            {
                string query = txtbox.SelectedText;
                string searchEngineUrl = "https://www.yandex.ru/search/?text=";

                await SearchOnSearchEngine(query, searchEngineUrl);
            }
        }

        private async void menuItem30_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли выделенный текст
            if (txtbox.SelectionLength > 0)
            {
                string query = txtbox.SelectedText;
                string searchEngineUrl = "https://www.bing.com/search?q=";

                await SearchOnSearchEngine(query, searchEngineUrl);
            }
        }

        private async Task SearchOnSearchEngine(string query, string searchEngineUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Собираем URL для запроса
                    string searchUrl = searchEngineUrl + Uri.EscapeDataString(query);

                    // Выполняем HTTP-запрос
                    HttpResponseMessage response = await client.GetAsync(searchUrl);

                    // Проверяем успешность запроса
                    if (response.IsSuccessStatusCode)
                    {
                        // Открываем браузер с результатами поиска
                        Process.Start(new ProcessStartInfo(searchUrl));
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при выполнении запроса.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {

        }
    }
}
