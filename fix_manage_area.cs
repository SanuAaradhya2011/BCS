using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string[] files = new string[] {
            @"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS\CABApplication\Manage Area\RegionManager.cs",
            @"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS\CABApplication\Manage Area\CircleManager.cs",
            @"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS\CABApplication\Manage Area\DivisionManager.cs",
            @"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS\CABApplication\Manage Area\RegionMaster.cs",
            @"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS\CABApplication\Manage Area\CircleMaster.cs",
            @"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS\CABApplication\Manage Area\DivisionMaster.cs"
        };
        
        string layoutMethod = @"
        // --- DARK MODE LAYOUT AND STYLING (Added 2026) ---
        private void ApplyModernDarkLayout()
        {
            System.Drawing.Color darkBg = System.Drawing.Color.FromArgb(30, 35, 45);
            System.Drawing.Color cardBg = System.Drawing.Color.FromArgb(40, 45, 55);
            System.Drawing.Color textLight = System.Drawing.Color.FromArgb(240, 240, 245);
            System.Drawing.Color accentTeal = System.Drawing.Color.FromArgb(32, 114, 126);
            System.Drawing.Color borderDark = System.Drawing.Color.FromArgb(60, 65, 75);

            this.BackColor = darkBg;

            Action<System.Windows.Forms.Control.ControlCollection> styleControls = null;
            styleControls = (controls) =>
            {
                foreach (System.Windows.Forms.Control c in controls)
                {
                    if (c is System.Windows.Forms.TextBox tb)
                    {
                        tb.BackColor = darkBg;
                        tb.ForeColor = textLight;
                        tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    }
                    else if (c is System.Windows.Forms.ComboBox cb)
                    {
                        cb.BackColor = darkBg;
                        cb.ForeColor = textLight;
                    }
                    else if (c is System.Windows.Forms.ListBox lb)
                    {
                        lb.BackColor = darkBg;
                        lb.ForeColor = textLight;
                        lb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    }
                    else if (c is System.Windows.Forms.Label lbl)
                    {
                        lbl.ForeColor = textLight;
                        lbl.BackColor = System.Drawing.Color.Transparent;
                    }
                    else if (c is System.Windows.Forms.Button btn)
                    {
                        btn.BackColor = accentTeal;
                        btn.ForeColor = textLight;
                        btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 0;
                        if (btn.Text == ""Cancel"" || btn.Text == ""Close"") btn.BackColor = System.Drawing.Color.FromArgb(90, 95, 105);
                    }
                    else if (c is System.Windows.Forms.GroupBox gb)
                    {
                        gb.ForeColor = textLight;
                        gb.Paint += (s, e) => {
                            e.Graphics.Clear(cardBg);
                            using (var pen = new System.Drawing.Pen(borderDark, 1))
                            {
                                e.Graphics.DrawRectangle(pen, 0, 0, gb.Width - 1, gb.Height - 1);
                            }
                        };
                        styleControls(c.Controls);
                    }
                    else if (c is System.Windows.Forms.Panel pn)
                    {
                        pn.BackColor = darkBg;
                        styleControls(c.Controls);
                    }
                }
            };
            
            styleControls(this.Controls);
        }";
        
        foreach (string file in files)
        {
            if (!File.Exists(file)) continue;
            string content = File.ReadAllText(file);
            
            if (content.Contains("ApplyModernDarkLayout")) continue;
            
            // Insert call in constructor
            content = Regex.Replace(content, @"(public\s+[A-Za-z0-9_]+\s*\(\)\s*\{[^{}]*InitializeComponent\(\);)", "\r\n            ApplyModernDarkLayout();");
            
            // Append method before last brace
            int lastBrace = content.LastIndexOf('}');
            if (lastBrace != -1)
            {
                int secondLastBrace = content.LastIndexOf('}', lastBrace - 1);
                if (secondLastBrace != -1)
                {
                    content = content.Insert(secondLastBrace, layoutMethod + "\r\n");
                }
            }
            
            File.WriteAllText(file, content);
            Console.WriteLine("Modified " + Path.GetFileName(file));
        }
    }
}
