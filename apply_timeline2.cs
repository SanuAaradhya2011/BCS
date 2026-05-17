using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = @"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS\TNEB.CABApplication\ApplicationLogDetails.cs";
        string content = File.ReadAllText(path);

        if (content.Contains("SetupTimelineGrid"))
        {
            Console.WriteLine("Already processed.");
            return;
        }

        string timelineMethod = @"
        // --- MODERN TIMELINE RENDERER (2026) ---
        private void SetupTimelineGrid()
        {
            grdActivityLogDetails.ColumnHeadersVisible = false;
            grdActivityLogDetails.RowHeadersVisible = false;
            grdActivityLogDetails.CellBorderStyle = DataGridViewCellBorderStyle.None;
            grdActivityLogDetails.BackgroundColor = Color.FromArgb(245, 247, 250);
            grdActivityLogDetails.DefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            grdActivityLogDetails.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 247, 250);
            grdActivityLogDetails.DefaultCellStyle.SelectionForeColor = Color.Black;
            grdActivityLogDetails.AllowUserToResizeRows = false;
            grdActivityLogDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            grdActivityLogDetails.RowPrePaint -= GrdActivityLogDetails_RowPrePaint;
            grdActivityLogDetails.RowPrePaint += GrdActivityLogDetails_RowPrePaint;
            grdActivityLogDetails.RowPostPaint -= GrdActivityLogDetails_RowPostPaint;
            grdActivityLogDetails.RowPostPaint += GrdActivityLogDetails_RowPostPaint;
            grdActivityLogDetails.DataBindingComplete -= GrdActivityLogDetails_DataBindingComplete;
            grdActivityLogDetails.DataBindingComplete += GrdActivityLogDetails_DataBindingComplete;
        }

        private void GrdActivityLogDetails_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string prevDate = """";
            foreach (DataGridViewRow row in grdActivityLogDetails.Rows)
            {
                if (row.DataBoundItem is DataRowView drv)
                {
                    string loginDT = drv[""Login DateTime""].ToString();
                    DateTime dt;
                    string currDate = """";
                    if (DateTime.TryParseExact(loginDT, ""dd-MM-yyyy HH:mm:ss"", null, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        currDate = dt.ToString(""MMM dd, yyyy"").ToUpper();
                    }
                    else if (loginDT.Length >= 10)
                    {
                        currDate = loginDT.Substring(0, 10);
                    }
                    
                    if (currDate != prevDate)
                    {
                        row.Height = 110; // Extra height for date header
                        row.Tag = ""Header_"" + currDate;
                        prevDate = currDate;
                    }
                    else
                    {
                        row.Height = 65;
                        row.Tag = null;
                    }
                }
            }
            
            // Build the top header panel
            UpdateTopHeaderPanel();
        }

        private Panel topHeaderPanel;
        private void UpdateTopHeaderPanel()
        {
            if (topHeaderPanel == null)
            {
                topHeaderPanel = new Panel();
                topHeaderPanel.Height = 80;
                topHeaderPanel.Dock = DockStyle.Top;
                topHeaderPanel.BackColor = Color.FromArgb(10, 125, 230); // Bright Blue
                topHeaderPanel.Padding = new Padding(20);
                topHeaderPanel.Paint += (s, e) => {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    string username = ""UNKNOWN"";
                    string status = ""NO SESSION"";
                    
                    if (grdActivityLogDetails.Rows.Count > 0 && grdActivityLogDetails.Rows[0].DataBoundItem is DataRowView firstDrv)
                    {
                        username = firstDrv[""Login ID""].ToString();
                    }
                    
                    bool hasActive = false;
                    foreach (DataGridViewRow r in grdActivityLogDetails.Rows)
                    {
                        if (r.DataBoundItem is DataRowView drv)
                        {
                            if (drv[""Logout DateTime""].ToString() == ""----"")
                            {
                                hasActive = true; break;
                            }
                        }
                    }
                    if (hasActive) status = ""ACTIVE SESSION"";
                    else status = ""ALL SESSIONS COMPLETED"";
                    
                    e.Graphics.DrawString(""USERNAME: "" + username, new Font(""Segoe UI"", 14, FontStyle.Bold), Brushes.White, 20, 15);
                    e.Graphics.DrawString(""CURRENT STATUS: "" + status, new Font(""Segoe UI"", 10, FontStyle.Regular), Brushes.White, 20, 45);
                    
                    // Draw pulse icon for active
                    if (hasActive) {
                        e.Graphics.DrawString(""~v~"", new Font(""Consolas"", 12, FontStyle.Bold), Brushes.White, 280, 43);
                    }
                };
                
                // Add it above the grid inside groupBox1
                this.groupBox1.Controls.Add(topHeaderPanel);
                topHeaderPanel.BringToFront();
                this.groupBox1.Text = """"; // Hide group box text
                this.groupBox1.BackColor = Color.FromArgb(245, 247, 250);
                this.BackColor = Color.FromArgb(245, 247, 250);
            }
            topHeaderPanel.Invalidate();
        }

        private void GrdActivityLogDetails_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            // Clear the background to hide default cells
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(245, 247, 250)), e.RowBounds);
            e.Handled = true; 
        }

        private void GrdActivityLogDetails_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (!(grdActivityLogDetails.Rows[e.RowIndex].DataBoundItem is DataRowView drv)) return;
            
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            int yOff = e.RowBounds.Y;
            int width = Math.Min(900, e.RowBounds.Width - 40); // Card width
            int xOff = 20;
            
            string loginDT = drv[""Login DateTime""].ToString();
            string logoutDT = drv[""Logout DateTime""].ToString();
            string duration = drv[""Duration (DD:HH:MM:SS)""].ToString();
            
            // Extract just the time for the card
            string loginTime = loginDT.Length > 10 ? loginDT.Substring(11) : loginDT;
            string logoutTime = logoutDT.Length > 10 ? logoutDT.Substring(11) : logoutDT;
            
            // Draw Date Header if applicable
            string tag = grdActivityLogDetails.Rows[e.RowIndex].Tag as string;
            if (tag != null && tag.StartsWith(""Header_""))
            {
                string dateStr = tag.Substring(7);
                e.Graphics.DrawString(dateStr, new Font(""Segoe UI"", 14, FontStyle.Bold), new SolidBrush(Color.FromArgb(20, 28, 45)), xOff, yOff + 15);
                yOff += 50; // Shift down for the card
            }
            else
            {
                yOff += 5; // standard padding
            }
            
            int cardHeight = 50;
            Rectangle cardRect = new Rectangle(xOff, yOff, width, cardHeight);
            
            // Determine Card State
            Color bgColor, borderColor, textColor, badgeColor;
            string badgeText = """";
            
            if (logoutDT == ""----"")
            {
                // Active or Missing Logout
                DateTime parsedLogin;
                bool isToday = false;
                if (DateTime.TryParseExact(loginDT, ""dd-MM-yyyy HH:mm:ss"", null, System.Globalization.DateTimeStyles.None, out parsedLogin))
                {
                    isToday = (parsedLogin.Date == DateTime.Now.Date);
                }
                else
                {
                    isToday = true; // fallback
                }
                
                if (isToday)
                {
                    // Green ACTIVE
                    bgColor = Color.FromArgb(235, 248, 240);
                    borderColor = Color.FromArgb(46, 160, 67);
                    textColor = Color.FromArgb(20, 28, 45);
                    badgeColor = Color.FromArgb(46, 160, 67);
                    badgeText = ""LIVE"";
                }
                else
                {
                    // Orange NO LOGOUT DATA
                    bgColor = Color.FromArgb(253, 244, 230);
                    borderColor = Color.FromArgb(220, 140, 50);
                    textColor = Color.FromArgb(20, 28, 45);
                    badgeColor = Color.FromArgb(220, 50, 50);
                    badgeText = ""NO LOGOUT"";
                }
            }
            else
            {
                // Completed Slate
                bgColor = Color.FromArgb(232, 241, 255);
                borderColor = Color.FromArgb(160, 185, 220);
                textColor = Color.FromArgb(40, 50, 70);
                badgeColor = Color.FromArgb(140, 165, 190);
                badgeText = ""COMPLETED"";
            }
            
            // Draw Card Background
            using (var brush = new SolidBrush(bgColor))
            using (var pen = new Pen(borderColor, 1.5f))
            {
                // Draw rounded rect by drawing circles at corners
                int r = 10;
                e.Graphics.FillPath(brush, GetRoundedRect(cardRect, r));
                e.Graphics.DrawPath(pen, GetRoundedRect(cardRect, r));
            }
            
            // Draw Badge
            Rectangle badgeRect = new Rectangle(xOff + 15, yOff + 15, 80, 22);
            using (var brush = new SolidBrush(badgeColor))
            {
                e.Graphics.FillPath(brush, GetRoundedRect(badgeRect, 5));
                
                // Center text in badge
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(badgeText, new Font(""Segoe UI"", 8, FontStyle.Bold), Brushes.White, badgeRect, sf);
            }
            
            // Draw Session Text
            string sessionText;
            if (logoutDT == ""----"") {
                sessionText = $""Login: {loginTime}"";
                e.Graphics.DrawString(sessionText, new Font(""Segoe UI"", 11, FontStyle.Regular), new SolidBrush(textColor), xOff + 110, yOff + 14);
            } else {
                sessionText = $""Session: {loginTime} - {logoutTime}"";
                e.Graphics.DrawString(sessionText, new Font(""Segoe UI"", 11, FontStyle.Regular), new SolidBrush(textColor), xOff + 110, yOff + 14);
                
                // Draw Duration on the right
                e.Graphics.DrawString($""Duration: {duration}"", new Font(""Segoe UI"", 10, FontStyle.Regular), new SolidBrush(textColor), xOff + 400, yOff + 15);
            }
        }
        
        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            System.Drawing.Size size = new System.Drawing.Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }";

        // Inject the method at the end
        int lastBrace = content.LastIndexOf('}');
        int secondLastBrace = content.LastIndexOf('}', lastBrace - 1);
        content = content.Insert(secondLastBrace, timelineMethod + "\r\n");

        // Add call in constructor
        content = content.Replace("InitializeComponent();\r\n        }", "InitializeComponent();\r\n            SetupTimelineGrid();\r\n        }");
        
        File.WriteAllText(path, content);
        Console.WriteLine("Done.");
    }
}
