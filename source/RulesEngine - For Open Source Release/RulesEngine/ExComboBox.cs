using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

public class ExComboBox : ComboBox
{
    private bool m_Unselectable = false;
    DblPanel pnl = new DblPanel();
    private const int WM_MOUSEWHEEL = 256;
    private const int WM_LBUTTONDOWN = 0x201;
    private const int WM_LBUTTONDBLCLK = 0x0203;
    private const int VK_SHIFT = 0x10;

    public ExComboBox()
    {
        pnl.Width = 17;
        pnl.Height = this.Height - 2;
        pnl.Left = this.Width - 18;
        pnl.Top = 1;
        this.Controls.Add(pnl);
        pnl.BringToFront();
        pnl.Visible = false;
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        if (m_Unselectable == true)
            e.Handled = true;
        else
            base.OnKeyPress(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (m_Unselectable == true)
        {
            if ((int)e.KeyData == 131139)
                if (this.SelectedText != null)
                    Clipboard.SetText(this.SelectedText);
            e.Handled = true;
        }
        else
            base.OnKeyDown(e);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        pnl.Left = this.Width - 18;
    }

    public bool ReadOnly
    {
        get
        {
            return m_Unselectable;
        }
        set
        {
            m_Unselectable = value;
            MakeUnselectable(m_Unselectable);
        }
    }

    private void MakeUnselectable(bool Unselectable)
    {
        if (m_Unselectable == true && this.DropDownStyle != ComboBoxStyle.Simple)
        {
            pnl.Visible = true;
        }
        else
        {
            pnl.Visible = false;
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (m_Unselectable == true)
        {
            if (this.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                if (keyData != Keys.Tab)
                    return true;
            }
            else
            {
                if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.PageUp || keyData == Keys.PageDown)
                    return true;
            }
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
    }

    protected override void WndProc(ref Message m)
    {
        if (this.m_Unselectable == true)
        {
            if (m.Msg == WM_MOUSEWHEEL || m.Msg == WM_LBUTTONDBLCLK)
                return;
            if (m.Msg == WM_LBUTTONDOWN)
            {
                this.Focus();
                return;
            }
        }

        base.WndProc(ref m);
    }

    protected override void OnDropDownStyleChanged(EventArgs e)
    {
        if (this.DropDownStyle == ComboBoxStyle.Simple)
            pnl.Visible = false;
        else
            if (m_Unselectable == true)
                pnl.Visible = true;
            else
                pnl.Visible = false;

        base.OnDropDownStyleChanged(e);
    }

    protected class DblPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Visible == true)
            {
                //ComboBoxRenderer.DrawDropDownButton(e.Graphics, e.ClipRectangle, System.Windows.Forms.VisualStyles.ComboBoxState.Disabled);
                Pen pen = new Pen(Color.DarkGray);
                Pen penBR = new Pen(Color.LightGray);
                Pen penArrow = new Pen(Color.LightGray);
                //Pen pen = new Pen(SystemColors.ControlText);
                //Pen penBR = new Pen(SystemColors.Window);
                //Pen penBR = new Pen(Color.White);
                //Pen penArrow = new Pen(SystemColors.ControlText);
                
                penArrow.Width = 2;
                penArrow.EndCap = LineCap.Square;

                Graphics g = e.Graphics;
                LinearGradientBrush lgb = new LinearGradientBrush(new Point(0, 0), new Point(0, this.Height + 1), Color.LightGray, Color.Red);
                //LinearGradientBrush lgb = new LinearGradientBrush(new Point(0, 0), new Point(0, this.Height + 1), Color.White, Color.Black);
                g.FillRectangle(lgb, new Rectangle(new Point(0, 0), this.Size));
                g.DrawLine(penBR, new Point(0, 0), new Point(this.Width - 1, 0));
                g.DrawLine(penBR, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height - 1));
                g.DrawLine(penBR, new Point(0, this.Height - 1), new Point(this.Width - 1, this.Height - 1));
                g.DrawLine(penBR, new Point(0, 0), new Point(0, this.Height - 1));
                g.DrawLine(pen, new Point(1, 0), new Point(this.Width - 2, 0));
                g.DrawLine(pen, new Point(this.Width - 1, 1), new Point(this.Width - 1, this.Height - 2));
                g.DrawLine(pen, new Point(1, this.Height - 1), new Point(this.Width - 2, this.Height - 1));
                g.DrawLine(pen, new Point(0, 1), new Point(0, this.Height - 2));

                g.DrawLine(penArrow, new Point(4, 7), new Point(8, 11));
                g.DrawLine(penArrow, new Point(8, 11), new Point(11, 8));

                pen.Dispose();
                penBR.Dispose();
                penArrow.Dispose();
                g = null;
                lgb.Dispose();
            }
        }
    }

}

