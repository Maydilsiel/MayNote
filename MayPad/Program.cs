using MayPad;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace notepad
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string fileContent = string.Empty;

            if (args.Length > 0)
            {
                string filePath = args[0];
                if (File.Exists(filePath))
                {
                    // Автоматически распознаем кодировку
                    fileContent = ReadTextFileWithAutoEncoding(filePath);
                }
            }

            Application.Run(new Form1(fileContent));
        }

        private static string ReadTextFileWithAutoEncoding(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fileStream, detectEncodingFromByteOrderMarks: true))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
