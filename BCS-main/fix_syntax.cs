using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = @"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS\CABApplication\Manage Area\AreaMaster.cs";
        string content = File.ReadAllText(path);
        
        // Let's just fix the section after ApplyModernDarkLayout
        int startIdx = content.IndexOf("this.pnAddArea.Resize +=");
        int endIdx = content.IndexOf("Region_ID = regionEntity.RegionID;");
        
        if (startIdx != -1 && endIdx != -1)
        {
            string newSection = @"            this.pnAddArea.Resize += (s, e) => {
                this.pnNewAreaDefinition.Location = new System.Drawing.Point(Math.Max(0, (this.pnAddArea.Width - this.pnNewAreaDefinition.Width) / 2), 40);
            };
        }

        private void btnRegion_Click(object sender, EventArgs e)
        {
            this.StatusMessage = """";
            RegionMaster regionMaster = new RegionMaster();
            regionMaster.StartPosition = FormStartPosition.CenterScreen;
            regionMaster.ShowDialog();
            regionEntity = regionMaster.Region;
            txtCircle.Text = """";
            txtDivision.Text = """";

            if (regionEntity != null)
            {
                txtRegion.Text = regionEntity.RegionName;
                Region_ID = regionEntity.RegionID;";
            
            content = content.Substring(0, startIdx) + newSection + content.Substring(endIdx + "Region_ID = regionEntity.RegionID;".Length);
            File.WriteAllText(path, content);
            Console.WriteLine("Fixed syntax error.");
        }
    }
}
