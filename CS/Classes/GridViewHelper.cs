using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base.ViewInfo;



namespace WindowsApplication1
{
    public class GridViewHelper
    {

        private GridView gridView;
        private GridCell sourceGridCell;
        private GridCell lastMouseDownMoveCell;


        public GridViewHelper(GridView gridView)
        {
            this.gridView = gridView;
            this.gridView.MouseDown += new MouseEventHandler(gridView_MouseDown);
            this.gridView.MouseMove += new MouseEventHandler(gridView_MouseMove);
            this.gridView.MouseUp += new MouseEventHandler(gridView_MouseUp);
            this.gridView.CustomDrawCell += new RowCellCustomDrawEventHandler(gridView_CustomDrawCell); 
        }



        private void gridView_MouseDown(object sender, MouseEventArgs e)
        {
            BaseView view = gridView.GridControl.GetViewAt(e.Location);
            (view as GridView).GridControl.Cursor = Cursors.Default;
            BaseHitInfo baseHI = view.CalcHitInfo(e.Location);
            GridHitInfo gridHI = baseHI as GridHitInfo;

            if (gridHI.HitTest == GridHitTest.RowEdge)
            {
                sourceGridCell = null;
                lastMouseDownMoveCell = null;
                baseHI = view.CalcHitInfo(new Point(e.X, e.Y - 3));
                gridHI = baseHI as GridHitInfo;
                if (gridHI.HitTest == GridHitTest.RowCell)
                {
                    object val = (view as GridView).GetRowCellValue(gridHI.RowHandle, gridHI.Column);
                    if (val == null || val.ToString() == string.Empty) return;
                    sourceGridCell = new GridCell(gridHI.RowHandle, gridHI.Column);
                    lastMouseDownMoveCell = new GridCell(sourceGridCell.RowHandle, sourceGridCell.Column);
                }
            }
        }



        private void gridView_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (lastMouseDownMoveCell == null || sourceGridCell == null) return;
            if (e.RowHandle == lastMouseDownMoveCell.RowHandle && e.Column == lastMouseDownMoveCell.Column)
                e.Graphics.DrawRectangle(new Pen(Color.Gray), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
        }



        private void gridView_MouseMove(object sender, MouseEventArgs e)
        {
            BaseView view = gridView.GridControl.GetViewAt(e.Location);
            BaseHitInfo baseHI = view.CalcHitInfo(e.Location);
            GridHitInfo gridHI = baseHI as GridHitInfo;

            if (gridHI.HitTest == GridHitTest.RowCell && sourceGridCell != null)
            {
                bool is_changed = false;

                if (lastMouseDownMoveCell.RowHandle != gridHI.RowHandle || lastMouseDownMoveCell.Column.FieldName != gridHI.Column.FieldName)
                {
                    is_changed = true;
                    int rowHandle = lastMouseDownMoveCell.RowHandle;
                    GridColumn gridCol = lastMouseDownMoveCell.Column;
                    lastMouseDownMoveCell = new GridCell(gridHI.RowHandle, gridHI.Column);
                    (view as GridView).InvalidateRowCell(rowHandle, gridCol);                        
                }

                if (is_changed) (view as GridView).InvalidateRowCell(lastMouseDownMoveCell.RowHandle, lastMouseDownMoveCell.Column);

            }
            else if (gridHI.HitTest == GridHitTest.RowEdge && sourceGridCell == null)
            {
                baseHI = view.CalcHitInfo(new Point(e.X, e.Y - 3));
                gridHI = baseHI as GridHitInfo;
                if (gridHI.HitTest == GridHitTest.RowCell)
                    if (gridHI.RowHandle == (view as GridView).FocusedRowHandle && gridHI.Column == (view as GridView).FocusedColumn)
                       (view as GridView).GridControl.Cursor = Cursors.SizeAll;
            }
            else (view as GridView).GridControl.Cursor = Cursors.Default;
        }



        private void gridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (sourceGridCell != null && lastMouseDownMoveCell != null)
            {
                BaseView view = gridView.GridControl.GetViewAt(e.Location);
                object val = (view as GridView).GetRowCellValue(sourceGridCell.RowHandle, sourceGridCell.Column);
                (view as GridView).SetRowCellValue(sourceGridCell.RowHandle, sourceGridCell.Column, "");
                (view as GridView).SetRowCellValue(lastMouseDownMoveCell.RowHandle, lastMouseDownMoveCell.Column, val);               
                (view as GridView).ClearSelection();
                (view as GridView).SelectCell(lastMouseDownMoveCell.RowHandle, lastMouseDownMoveCell.Column);
                (view as GridView).FocusedRowHandle = lastMouseDownMoveCell.RowHandle;
                (view as GridView).FocusedColumn = lastMouseDownMoveCell.Column;
                sourceGridCell = null;
                int rowHandle = lastMouseDownMoveCell.RowHandle;
                GridColumn gridCol = lastMouseDownMoveCell.Column;
                lastMouseDownMoveCell = null;
                (view as GridView).InvalidateRowCell(rowHandle, gridCol);
            }
        }
    }
}
