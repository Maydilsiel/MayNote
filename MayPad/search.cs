using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MayPad
{
    public partial class search : Form
    {
        private Form1 searchtext; // Обратите внимание, что я вернул это обратно, чтобы связать с существующим объектом Form1

        private bool firstSearch = true;
        private Place startPlace;
        private string lastSearchTerm = "";

        public search(Form1 form)
        {
            InitializeComponent();
            searchtext = form; // Инициализация searchtext при создании экземпляра search
        }
        public virtual void TextSearch(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    MessageBox.Show("Пожалуйста, введите текст для поиска.", "Пустое поле", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Если запрос повторяется, игнорируем ошибку и продолжаем выполнение
                if (searchTerm != lastSearchTerm)
                {
                    lastSearchTerm = searchTerm;
                }

                RegexOptions options = RegexOptions.IgnoreCase;

                Range currentRange = searchtext.txtbox.Selection.Clone();
                currentRange.Normalize();

                if (firstSearch)
                {
                    startPlace = currentRange.Start;
                    firstSearch = false;
                }

                currentRange.Start = currentRange.End;

                if (currentRange.Start >= startPlace)
                {
                    currentRange.End = new Place(searchtext.txtbox.GetLineLength(searchtext.txtbox.LinesCount - 1), searchtext.txtbox.LinesCount - 1);
                }
                else
                {
                    currentRange.End = startPlace;
                }

                using (IEnumerator<Range> enumerator = currentRange.GetRangesByLines(searchTerm, options).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        Range foundRange = enumerator.Current;
                        searchtext.txtbox.Selection = foundRange;
                        searchtext.txtbox.DoSelectionVisible();
                        searchtext.txtbox.Invalidate();
                        return;
                    }
                }

                if (currentRange.Start >= startPlace && startPlace > Place.Empty)
                {
                    searchtext.txtbox.Selection.Start = new Place(0, 0);
                    TextSearch(searchTerm);
                }
                else
                {
                    MessageBox.Show("Ничего не найдено для \"" + searchTerm + "\".", "Поиск завершен", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (lastSearchTerm == searchTerm) // Игнорируем ошибку только если запрос повторяется
                {
                    return;
                }
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }








        private void button1_Click(object sender, EventArgs e)
        {
            TextSearch(textBox1.Text);
        }
    }
}
