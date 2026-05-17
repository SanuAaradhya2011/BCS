using System.Resources;
using System.Windows.Forms;
using CABAppControl.Properties;

namespace CAB.UI.Controls
{
    public partial class CABLabel : Label
    {
        private string translationKey;
        public CABLabel()
        { 
        }

       
        public string TranslationKey
        {
            get { return translationKey; }
            set
            {
                translationKey = value;
                if (string.IsNullOrEmpty(translationKey))
                    return;
                ResourceManager resourceManager = new ResourceManager("CABAppControl.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                this.Text = resourceManager.GetString(translationKey);
            }
        } 
    }
}
