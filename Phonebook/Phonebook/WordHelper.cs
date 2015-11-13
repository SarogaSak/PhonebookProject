using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Novacode;

namespace Phonebook
{
    public static class WordHelper
    {
        public static void CreateDocument()
        {
            string fileName = @"D:\1.docx";
            string headlineText = "Constitution of the United States";
            string paraOne = String.Format("{0,-40}{1,20}{2,20}", "1111111", 22, 333);

            var headerFormat = new Formatting
            {
                FontFamily = new System.Drawing.FontFamily("Arial Black"),
                Size = 18D,
                Position = 12
            };

            var paraFormat = new Formatting
            {
                FontFamily = new System.Drawing.FontFamily("Calibri"),
                Size = 10D,
                Bold = true
            };

            var doc = DocX.Create(fileName);
            doc.InsertParagraph(headlineText, false, headerFormat);
            doc.InsertParagraph(paraOne, false, paraFormat);
            doc.Paragraphs[0].Append("qqqq").Bold();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Word2007+ files (*.docx)|*.docx";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == true)
            {
                fileName = saveFileDialog1.FileName.Replace(".docx", "") + ".docx";
                doc.SaveAs(fileName);
                // Open in Word:
                Process.Start("WINWORD.EXE", fileName);
            } 
        }
    }
}
