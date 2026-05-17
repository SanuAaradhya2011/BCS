/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework.Utility;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class ChangePassword : MdiChildForm 
    {
		private UserInformationBLL userInformationBLL;
		UserInformationEntity userInformationEntity;
        
        public ChangePassword()
        {
			userInformationBLL = new UserInformationBLL();
            InitializeComponent();
        }

        // check for the validation on the change password
		private bool ValidateData(UserInformationEntity entity)
        {
			bool Flag = false;
			if (string.IsNullOrEmpty(entity.User_Password))
			{
				this.StatusMessage = MessageConstant.GetText("L000011") +" " +  MessageConstant.GetText("M000001");
				Application.DoEvents();
				txtNewPassword.Focus();
				return Flag;
			}
			else if (!ValidationProvider.ValidateData(entity.User_Password, ValidationConstant.consumerID))
			{
				this.StatusMessage = MessageConstant.GetText("L000011") + " " + MessageConstant.GetText("M000012");
				txtNewPassword.Focus();
				return Flag;
			}
			else if (string.IsNullOrEmpty(entity.User_Confirm_Password))
			{
				this.StatusMessage = MessageConstant.GetText("L000007") + " " + MessageConstant.GetText("M000001");
				Application.DoEvents();
				txtConfirmPassword.Focus();
				return Flag;
			}
			else if (!ValidationProvider.ValidateData(entity.User_Confirm_Password, ValidationConstant.consumerID))
			{
				this.StatusMessage = MessageConstant.GetText("L000007") + " " + MessageConstant.GetText("M000012");
				Application.DoEvents();
				txtConfirmPassword.Focus();
				return Flag;
			}
			else if (entity.User_Password != entity.User_Confirm_Password)
			{
				this.StatusMessage = MessageConstant.GetText("M000005");
				Application.DoEvents();
				txtConfirmPassword.Focus();
				return Flag;
			}

			return Flag = true;
        }
        private void ChangePassword_Load(object sender, EventArgs e)
        {
            InitializeAnimations();InitializeAnimations();
			userInformationEntity = new UserInformationEntity();
			userInformationEntity = (UserInformationEntity)userInformationBLL.GetDetailData(ConfigInfo.UserInformationID);
			txtCurrentPassword.Text = userInformationEntity.User_Password;
        }

		private void btnSave_Click(object sender, EventArgs e)
		{
			userInformationEntity = new UserInformationEntity();
			userInformationEntity.UserInformation_ID = ConfigInfo.UserInformationID;
			userInformationEntity.User_Password = this.txtNewPassword.Text.Trim();
			userInformationEntity.User_Confirm_Password = this.txtConfirmPassword.Text.Trim();
			if (ValidateData(userInformationEntity))
			{
				userInformationBLL.UpdatePassword(userInformationEntity);
                this.StatusMessage = MessageConstant.GetText("M000008");
			}
		} 
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (System.Drawing.SolidBrush shadowBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(20, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(
                    shadowBrush,
                    panelCard.Left + 6,
                    panelCard.Top + 6,
                    panelCard.Width,
                    panelCard.Height);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
		{
            this.StatusMessage = string.Empty;
			this.Close();
		}
        private void ChangePassword_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        // --- ANIMATION UI LOGIC (Added 2026) ---
        private Timer animTimer;
        private int slideOffset = 30;
        private int currentSlide = 30;
        
        private int curFocusWidth = 0;
        private int newFocusWidth = 0;
        private int confFocusWidth = 0;
        private TextBox activeBox = null;

        private int strengthWidth = 0;
        private int targetStrengthWidth = 0;
        private System.Drawing.Color strengthColor = System.Drawing.Color.Red;

        private void InitializeAnimations()
        {
            animTimer = new Timer();
            animTimer.Interval = 16;
            animTimer.Tick += AnimTimer_Tick;
            
            this.panelCard.Top += slideOffset;
            this.panelCard.Visible = false;

            this.txtCurrentPassword.GotFocus += (s, ev) => activeBox = txtCurrentPassword;
            this.txtCurrentPassword.LostFocus += (s, ev) => { if(activeBox == txtCurrentPassword) activeBox = null; };
            
            this.txtNewPassword.GotFocus += (s, ev) => activeBox = txtNewPassword;
            this.txtNewPassword.LostFocus += (s, ev) => { if(activeBox == txtNewPassword) activeBox = null; };
            this.txtNewPassword.TextChanged += TxtNewPassword_TextChanged;

            this.txtConfirmPassword.GotFocus += (s, ev) => activeBox = txtConfirmPassword;
            this.txtConfirmPassword.LostFocus += (s, ev) => { if(activeBox == txtConfirmPassword) activeBox = null; };

            this.btnSave.MouseEnter += (s, ev) => this.btnSave.BackColor = System.Drawing.Color.FromArgb(0, 86, 179);
            this.btnSave.MouseLeave += (s, ev) => this.btnSave.BackColor = System.Drawing.Color.FromArgb(26, 115, 232);
            this.btnCancel.MouseEnter += (s, ev) => this.btnCancel.BackColor = System.Drawing.Color.FromArgb(220, 224, 230);
            this.btnCancel.MouseLeave += (s, ev) => this.btnCancel.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);

            this.groupBox1.Paint += GroupBox1_Paint;
            animTimer.Start();
        }

        private void TxtNewPassword_TextChanged(object sender, EventArgs e)
        {
            int len = txtNewPassword.Text.Length;
            if (len == 0) { targetStrengthWidth = 0; }
            else if (len < 5) { targetStrengthWidth = 70; strengthColor = System.Drawing.Color.Red; }
            else if (len < 8) { targetStrengthWidth = 140; strengthColor = System.Drawing.Color.Orange; }
            else { targetStrengthWidth = 214; strengthColor = System.Drawing.Color.MediumSeaGreen; }
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            bool needsRepaint = false;
            if (currentSlide > 0)
            {
                this.panelCard.Visible = true;
                int step = Math.Max(1, currentSlide / 4);
                this.panelCard.Top -= step;
                currentSlide -= step;
            }

            curFocusWidth = Interpolate(curFocusWidth, activeBox == txtCurrentPassword ? 214 : 0, ref needsRepaint);
            newFocusWidth = Interpolate(newFocusWidth, activeBox == txtNewPassword ? 214 : 0, ref needsRepaint);
            confFocusWidth = Interpolate(confFocusWidth, activeBox == txtConfirmPassword ? 214 : 0, ref needsRepaint);
            strengthWidth = Interpolate(strengthWidth, targetStrengthWidth, ref needsRepaint);

            if (needsRepaint) this.groupBox1.Invalidate();
        }

        private int Interpolate(int current, int target, ref bool needsRepaint)
        {
            if (current == target) return current;
            needsRepaint = true;
            int diff = target - current;
            int step = diff / 4;
            if (step == 0) step = Math.Sign(diff);
            return current + step;
        }

        private void GroupBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawFocusLine(e.Graphics, txtCurrentPassword, curFocusWidth);
            DrawFocusLine(e.Graphics, txtNewPassword, newFocusWidth);
            DrawFocusLine(e.Graphics, txtConfirmPassword, confFocusWidth);

            if (strengthWidth > 0)
            {
                int startX = txtNewPassword.Left;
                int startY = txtNewPassword.Bottom + 8;
                e.Graphics.FillRectangle(new System.Drawing.SolidBrush(strengthColor), startX, startY, strengthWidth, 4);
            }
        }

        private void DrawFocusLine(System.Drawing.Graphics g, TextBox tb, int width)
        {
            g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.LightGray), tb.Left, tb.Bottom + 2, tb.Width, 2);
            if (width > 0)
            {
                int startX = tb.Left + (tb.Width / 2) - (width / 2);
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(0, 120, 215)), startX, tb.Bottom + 2, width, 2);
            }
        }
    }
}
