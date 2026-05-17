/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 10/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using System.Resources;
using System.Windows.Forms;

namespace CAB.UI.Controls
{
    public class CABMessageBox
    {

        public static DialogResult ShowMessage(string messageTranslationKey)
        {
            return MessageBox.Show(MessageConstant.GetText(messageTranslationKey));
        }

        public static DialogResult ShowMessage(string messageTranslationKey, string captionTranslationKey)
        {
            return MessageBox.Show(MessageConstant.GetText(messageTranslationKey), MessageConstant.GetText(captionTranslationKey));
        }

        public static DialogResult ShowMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons)
        {
            return MessageBox.Show(MessageConstant.GetText(messageTranslationKey), MessageConstant.GetText(captionTranslationKey), buttons);
        }

        public static DialogResult ShowMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(MessageConstant.GetText(messageTranslationKey), MessageConstant.GetText(captionTranslationKey), buttons, icon);
        }

        public static DialogResult ShowMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return MessageBox.Show(MessageConstant.GetText(messageTranslationKey), MessageConstant.GetText(captionTranslationKey), buttons, icon, defaultButton);
        }

        public static DialogResult ShowMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return MessageBox.Show(MessageConstant.GetText(messageTranslationKey), MessageConstant.GetText(captionTranslationKey), buttons, icon, defaultButton, options);
        }
        public static DialogResult ShowMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
        {
            return MessageBox.Show(MessageConstant.GetText(messageTranslationKey), MessageConstant.GetText(captionTranslationKey), buttons, icon, defaultButton, options, displayHelpButton);
        }
        public static DialogResult ShowMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
        {
            return MessageBox.Show(MessageConstant.GetText(messageTranslationKey), MessageConstant.GetText(captionTranslationKey), buttons, icon, defaultButton, options, helpFilePath);
        }
        public static DialogResult ShowMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            return MessageBox.Show(MessageConstant.GetText(messageTranslationKey), MessageConstant.GetText(captionTranslationKey), buttons, icon, defaultButton, options, helpFilePath, navigator);
        }
        public static DialogResult ShowMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
        {
            return MessageBox.Show(MessageConstant.GetText(messageTranslationKey), MessageConstant.GetText(captionTranslationKey), buttons, icon, defaultButton, options, helpFilePath, keyword);
        }
        public static DialogResult ShowFilterMessage(string messageTranslationKey)
        {
            return MessageBox.Show(MessageConstant.GetFilterText(messageTranslationKey));
        }

        public static DialogResult ShowFilterMessage(string messageTranslationKey, string captionTranslationKey)
        {
            return MessageBox.Show(MessageConstant.GetFilterText(messageTranslationKey), MessageConstant.GetFilterText(captionTranslationKey));
        }

        public static DialogResult ShowFilterMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons)
        {
            return MessageBox.Show(MessageConstant.GetFilterText(messageTranslationKey), MessageConstant.GetFilterText(captionTranslationKey), buttons);
        }

        public static DialogResult ShowFilterMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(MessageConstant.GetFilterText(messageTranslationKey), MessageConstant.GetFilterText(captionTranslationKey), buttons, icon);
        }

        public static DialogResult ShowFilterMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return MessageBox.Show(MessageConstant.GetFilterText(messageTranslationKey), MessageConstant.GetFilterText(captionTranslationKey), buttons, icon, defaultButton);
        }

        public static DialogResult ShowFilterMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return MessageBox.Show(MessageConstant.GetFilterText(messageTranslationKey), MessageConstant.GetFilterText(captionTranslationKey), buttons, icon, defaultButton, options);
        }
        public static DialogResult ShowFilterMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
        {
            return MessageBox.Show(MessageConstant.GetFilterText(messageTranslationKey), MessageConstant.GetFilterText(captionTranslationKey), buttons, icon, defaultButton, options, displayHelpButton);
        }
        public static DialogResult ShowFilterMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
        {
            return MessageBox.Show(MessageConstant.GetFilterText(messageTranslationKey), MessageConstant.GetFilterText(captionTranslationKey), buttons, icon, defaultButton, options, helpFilePath);
        }
        public static DialogResult ShowFilterMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            return MessageBox.Show(MessageConstant.GetFilterText(messageTranslationKey), MessageConstant.GetFilterText(captionTranslationKey), buttons, icon, defaultButton, options, helpFilePath, navigator);
        }
        public static DialogResult ShowFilterMessage(string messageTranslationKey, string captionTranslationKey, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
        {
            return MessageBox.Show(MessageConstant.GetFilterText(messageTranslationKey), MessageConstant.GetFilterText(captionTranslationKey), buttons, icon, defaultButton, options, helpFilePath, keyword);
        }
    }
}
