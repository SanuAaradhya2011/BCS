using System.Resources;
using System.Windows.Forms;

namespace CAB.UI.Controls
{
    public partial class CABButton : Button
    {
        private string translationKey;
        public CABButton()
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
