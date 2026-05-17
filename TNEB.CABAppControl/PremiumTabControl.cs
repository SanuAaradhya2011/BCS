using System;
using System.Drawing;
using System.Windows.Forms;

namespace CAB.UI.Controls
{
    public class PremiumTabControl : TabControl
    {
        public PremiumTabControl()
        {
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.ItemSize = new Size(120, 30); // Slightly larger tabs for modern look
            this.SizeMode = TabSizeMode.Normal;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle tabRect = this.GetTabRect(e.Index);
            bool isSelected = (this.SelectedIndex == e.Index);
            
            tabRect.Inflate(2, 2);

            if (isSelected)
            {
                g.FillRectangle(Brushes.White, tabRect);
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 120, 215)), tabRect.X, tabRect.Bottom - 3, tabRect.Width, 3);
            }
            else
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(240, 242, 245)), tabRect);
            }

            string tabText = this.TabPages[e.Index].Text;
            Font tabFont = isSelected ? new Font("Segoe UI", 9F, FontStyle.Bold) : new Font("Segoe UI", 9F, FontStyle.Regular);
            Brush textBrush = isSelected ? new SolidBrush(Color.FromArgb(0, 120, 215)) : new SolidBrush(Color.FromArgb(15, 23, 42));

            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString(tabText, tabFont, textBrush, tabRect, stringFormat);
        }
    }
}
