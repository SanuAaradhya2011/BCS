using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CABApplication.Reports.Forms
{

    public static class ReportThemeHelper
    {
        // ─── Blue Theme Colors ─── base: RGB(29, 70, 150) ───────────────────────
        public static readonly Color HeaderBlue    = Color.FromArgb( 29,  70, 150); // exact brand color
        public static readonly Color SubHeaderBlue = Color.FromArgb( 50,  95, 175); // lighter variant
        public static readonly Color AccentBlue    = Color.FromArgb( 18,  50, 115); // darker for borders
        public static readonly Color TitleBlue     = Color.FromArgb( 29,  70, 150); // same as HeaderBlue


        // ─── Report Font ──────────────────────────────────────────────────────────
        private const string ReportFont = "Times New Roman";

        // ─── Known green/yellow-green RGB values in the legacy .rpt files ───────
        private static readonly int[,] GreenColors =
        {
            {157, 180,   0},
            {176, 196,   0},
            {128, 176,   0},
            {  0, 128,   0},
            {166, 196,   0},
            {148, 166,   0},
            {  0, 153,   0},
            {  0, 170,  17},
        };

     
        private static readonly string[] LandisStrings =
        {
            "Landis+Gyr", "Landis + Gyr", "Landis Gyr", "LANDIS+GYR",
            "manage energy better", "Manage Energy Better",
        };

        // ─── Public Entry Point ──────────────────────────────────────────────────

        public static void Apply(ReportDocument report)
        {
            if (report == null) return;
            try { ApplyToReport(report); }
            catch {  }
        }

        // ─── Internal Helpers ────────────────────────────────────────────────────

        private static void ApplyToReport(ReportDocument report)
        {
            foreach (Section section in report.ReportDefinition.Sections)
            {
                ApplyToSection(report, section);
            }
        }

        private static void ApplyToSection(ReportDocument report, Section section)
        {
            try
            {
                foreach (ReportObject obj in section.ReportObjects)
                {
                    ApplyToObject(report, obj);
                }
            }
            catch { }
        }

        private static void ApplyToObject(ReportDocument report, ReportObject obj)
        {
            try
            {
                switch (obj.Kind)
                {
                    case ReportObjectKind.BoxObject:
                        ApplyToBox((BoxObject)obj);
                        break;

                    case ReportObjectKind.FieldObject:
                        ApplyToField((FieldObject)obj);
                        break;

                    case ReportObjectKind.TextObject:
                        ApplyToText((TextObject)obj);
                        break;

                    case ReportObjectKind.PictureObject:

                        break;

                    case ReportObjectKind.SubreportObject:
                        SubreportObject sub = (SubreportObject)obj;
                        try
                        {
                            ReportDocument subDoc = sub.OpenSubreport(sub.SubreportName);
                            ApplyToReport(subDoc);
                        }
                        catch { }
                        break;

                    case ReportObjectKind.LineObject:
                        ApplyToLine((LineObject)obj);
                        break;
                }
            }
            catch { }
        }

        // ── Box (filled rectangle – used for header bars) ─────────────────────
        private static void ApplyToBox(BoxObject box)
        {
            try
            {
                if (IsGreenish(box.FillColor))
                    box.FillColor = HeaderBlue;

                if (IsGreenish(box.LineColor))
                    box.LineColor = AccentBlue;
            }
            catch { }
        }

        // ── Field (data field) ────────────────────────────────────────────────
        private static void ApplyToField(FieldObject field)
        {
            try
            {
                if (IsGreenish(field.Color))
                    field.Color = Color.White;

                // Change font family to Times New Roman, preserve size and style
                SetFont(field, field.Font);
            }
            catch { }
        }

        // ── Text object ───────────────────────────────────────────────────────
        private static void ApplyToText(TextObject text)
        {
            try
            {
                // Replace  branding text with "Cabcon"
                if (!string.IsNullOrEmpty(text.Text))
                {
                    bool isLandis = false;
                    foreach (string landisStr in LandisStrings)
                    {
                        if (text.Text.IndexOf(landisStr, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            isLandis = true;
                            break;
                        }
                    }

                    if (isLandis)
                    {
                        text.Text  = "Cabcon";
                        text.Color = HeaderBlue;
                        SetFont(text, text.Font);
                        return;
                    }
                }

                // Replace green foreground text color with blue
                if (IsGreenish(text.Color))
                    text.Color = TitleBlue;

                // Change font family to Times New Roman, preserve size and style
                SetFont(text, text.Font);
            }
            catch { }
        }

        // ── Font setter (Font property is read-only on CR objects; use reflection) ─
        private static void SetFont(object crObj, Font current)
        {
            try
            {
                Font newFont = new Font(ReportFont, current.Size, current.Style);
                Type t       = crObj.GetType();

                // Crystal Reports exposes font change via ApplyFont(Font) in some builds
                MethodInfo mi = t.GetMethod("ApplyFont", new[] { typeof(Font) })
                             ?? t.GetMethod("set_Font",   new[] { typeof(Font) });
                if (mi != null) { mi.Invoke(crObj, new object[] { newFont }); return; }

                // Fall back to property setter if the runtime actually allows it
                PropertyInfo pi = t.GetProperty("Font");
                if (pi != null && pi.CanWrite) pi.SetValue(crObj, newFont);
            }
            catch { }
        }

        // ── Line object ──────────────────────────────────────────────────────
        private static void ApplyToLine(LineObject line)
        {
            try
            {
                if (IsGreenish(line.LineColor))
                    line.LineColor = AccentBlue;
            }
            catch { }
        }

      
        private static void SuppressPicture(PictureObject picture)
        {
            try
            {
               
                picture.ObjectFormat.EnableSuppress = true;
            }
            catch { }
        }

        // ─── Color Mapping Helpers ────────────────────────────────────────────────

        private static bool IsGreenish(Color c)
        {
            for (int i = 0; i < GreenColors.GetLength(0); i++)
            {
                if (c.R == GreenColors[i, 0] &&
                    c.G == GreenColors[i, 1] &&
                    c.B == GreenColors[i, 2])
                    return true;
            }

            // Yellow-green heuristic (G dominates, B near zero)
            if (c.G > 100 && c.B < 30 && c.G > c.R && c.G > c.B)
                return true;

            // Pure saturated green
            if (c.R < 20 && c.G > 100 && c.B < 30)
                return true;

            // Lime / chartreuse
            if (c.R > 80 && c.R < 230 && c.G > 180 && c.B < 30)
                return true;

            return false;
        }
    }
}
