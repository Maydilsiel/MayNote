using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MayPad
{
    public partial class EncodingSelectorForm : Form
    {
        public Encoding SelectedEncoding { get; private set; }

        public EncodingSelectorForm()
        {
            InitializeComponent();
            LoadEncodings();
        }

        public void LoadEncodings()
        {
           List<EncodingItem> encodings = new List<EncodingItem>
         {
        new EncodingItem(Encoding.UTF8, "UTF-8"),
        new EncodingItem(Encoding.Unicode, "UTF-16"),
        new EncodingItem(Encoding.ASCII, "ASCII"),
        new EncodingItem(Encoding.GetEncoding("windows-1251"), "Windows-1251"),
        new EncodingItem(Encoding.GetEncoding("iso-8859-1"), "ISO-8859-1"),
        new EncodingItem(Encoding.GetEncoding("shift_jis"), "Shift-JIS"),
        new EncodingItem(Encoding.GetEncoding("euc-jp"), "EUC-JP"),
        new EncodingItem(Encoding.GetEncoding("ks_c_5601-1987"), "KS_C_5601-1987"),
        new EncodingItem(Encoding.GetEncoding("big5"), "Big5"),
        new EncodingItem(Encoding.GetEncoding("iso-8859-2"), "ISO-8859-2"),
        new EncodingItem(Encoding.GetEncoding("iso-8859-5"), "ISO-8859-5"),
        // Добавьте другие кодировки по необходимости
           };

            comboBox1.DataSource = encodings;
            comboBox1.DisplayMember = "DisplayName";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is EncodingItem encodingItem)
            {
                SelectedEncoding = encodingItem.Encoding;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Выберите кодировку.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    public class EncodingItem
    {
        public Encoding Encoding { get; private set; }
        public string DisplayName { get; private set; }

        public EncodingItem(Encoding encoding, string displayName)
        {
            Encoding = encoding;
            DisplayName = displayName;
        }
    }
}
