using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Win32;
using Novacode;
using Phonebook.Models;

namespace Phonebook
{
    public static class WordHelper
    {
        private static Formatting TitleFormatting()
        {
            return new Formatting
            {
                FontFamily = new FontFamily("Arial Black"),
                Size = 20D,
                Bold = true
            };
        }

        private static Formatting EnterpriseFormatting()
        {
            return new Formatting
            {
                FontFamily = new FontFamily("Arial Black"),
                Size = 14D,
                Bold = true,
            };
        }

        private static Formatting DeptFormatting()
        {
            return new Formatting
            {
                FontFamily = new FontFamily("Calibri"),
                Size = 14D,
                Bold = true,
                UnderlineStyle = UnderlineStyle.singleLine
            };
        }

        private static Formatting InfoFormatting()
        {
            return new Formatting
            {
                FontFamily = new FontFamily("Calibri"),
                Size = 10D,
            };
        }

        public static void CreateDocument(List<Person> personnel)
        {
            string fileName = @"D:\1.docx";

            var doc = DocX.Create(fileName);
            doc.MarginTop = 40F;
            doc.MarginLeft = 50F;
            doc.MarginRight = 50F;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Word2007+ files (*.docx)|*.docx",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            Table t = doc.AddTable(1, 3);
            t.AutoFit = AutoFit.Window;
            t.SetBorder(TableBorderType.InsideV, new Border(BorderStyle.Tcbs_none, BorderSize.one, 0, Color.White));
            InsertTitle(t.Rows[0]);


            string enterprise = "";
            string dept = "";
            foreach (var person in personnel)
            {
                if (!person.EnterpriseName.Equals(enterprise))
                {
                    InsertEnterprise(t.InsertRow(), person.EnterpriseName);
                    enterprise = person.EnterpriseName;
                }
                if (!person.DeptName.Equals(dept))
                {
                    InsertDept(t.InsertRow(), person.DeptName);
                    dept = person.DeptName;
                }
                InsertPerson(t.InsertRow(), person);

            }

            doc.InsertTable(t);

            if (saveFileDialog1.ShowDialog() == true)
            {
                fileName = saveFileDialog1.FileName;
                doc.SaveAs(fileName);
                // Open in Word:
                Process.Start("WINWORD.EXE", fileName);
            }
        }

        private static void InsertTitle(Row row)
        {
            row.MergeCells(0, 2);
            row.RemoveParagraphAt(0);
            row.RemoveParagraphAt(0);
            row.RemoveParagraphAt(0);
            row.Cells[0].InsertParagraph("Справочник", false, TitleFormatting()).Alignment = Alignment.center;
        }

        private static void InsertEnterprise(Row row, string name)
        {
            row.MergeCells(0, 2);
            row.RemoveParagraphAt(0);
            row.RemoveParagraphAt(0);
            row.RemoveParagraphAt(0);
            row.Cells[0].InsertParagraph(name, false, EnterpriseFormatting()).Alignment = Alignment.center;
        }

        private static void InsertDept(Row row, string name)
        {
            row.MergeCells(0, 2);
            row.RemoveParagraphAt(0);
            row.RemoveParagraphAt(0);
            row.RemoveParagraphAt(0);
            row.Cells[0].InsertParagraph(name, false, DeptFormatting()).Alignment = Alignment.center;
        }

        private static void InsertPerson(Row row, Person person)
        {
            row.Cells[0].RemoveParagraphAt(0);
            row.Cells[0].InsertParagraph(person.GetFIO(), false, InfoFormatting());
            row.Cells[1].RemoveParagraphAt(0);
            row.Cells[1].InsertParagraph(person.JobName, false, InfoFormatting());
            row.Cells[2].RemoveParagraphAt(0);
            row.Cells[2].InsertParagraph(person.LandlineNumbers, false, InfoFormatting()).Alignment = Alignment.right;
        }
    }
}
