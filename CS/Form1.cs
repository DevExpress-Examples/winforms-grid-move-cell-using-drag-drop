using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;



namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            gridView1.OptionsView.ColumnAutoWidth = false;
            WindowState = FormWindowState.Maximized;
            gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            gridControl1.DataSource = CreateTable(20, 20);
            new GridViewHelper(gridView1);
        }



        private DataTable CreateTable(int columnCount, int rowCount)
        {
            DataTable tbl = new DataTable();
            for (int i = 0; i < columnCount; i++)
                tbl.Columns.Add(String.Format("Column{0}", i), typeof(string));
            for (int i = 0; i < rowCount; i++)
                tbl.Rows.Add(new object[] { });
            return tbl;
        }
    }
}