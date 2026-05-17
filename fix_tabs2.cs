using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string[] files = Directory.GetFiles(@"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS", "*.Designer.cs", SearchOption.AllDirectories);
        
        foreach (string file in files)
        {
            if (file.Contains("CABSearchControl") || file.Contains("CABButton") || file.Contains("ChangePassword")) continue; // skip forms where blue is intentional on buttons/panels

            string content = File.ReadAllText(file);
            
            // To ensure we only target TabPages, let's extract all tabpage names first
            var tabMatches = Regex.Matches(content, @"this\.([a-zA-Z0-9_]+)\s*=\s*new\s+System\.Windows\.Forms\.TabPage\(\);");
            if (tabMatches.Count == 0) continue;
            
            int modCount = 0;
            foreach(Match m in tabMatches)
            {
                string tab = m.Groups[1].Value;
                
                string p1 = @"this\." + tab + @"\.BackColor\s*=\s*System\.Drawing\.Color\.FromArgb\(\(\(int\)\(\(\(byte\)\(0\)\)\)\),\s*\(\(\(int\)\(\(\(byte\)\(120\)\)\)\),\s*\(\(\(int\)\(\(\(byte\)\(215\)\)\)\)\);";
                string p2 = @"this\." + tab + @"\.BackColor\s*=\s*System\.Drawing\.Color\.FromArgb\(0,\s*120,\s*215\);";
                string p3 = @"this\." + tab + @"\.ForeColor\s*=\s*System\.Drawing\.Color\.White;";
                
                bool changed = false;
                if (Regex.IsMatch(content, p1))
                {
                    content = Regex.Replace(content, p1, "this." + tab + ".BackColor = System.Drawing.Color.Snow;");
                    changed = true;
                }
                if (Regex.IsMatch(content, p2))
                {
                    content = Regex.Replace(content, p2, "this." + tab + ".BackColor = System.Drawing.Color.Snow;");
                    changed = true;
                }
                if (changed && Regex.IsMatch(content, p3))
                {
                    content = Regex.Replace(content, p3, "this." + tab + ".ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);");
                    modCount++;
                }
            }
            
            if (modCount > 0)
            {
                File.WriteAllText(file, content);
                Console.WriteLine("Modified " + modCount + " tabs in " + Path.GetFileName(file));
            }
        }
    }
}
