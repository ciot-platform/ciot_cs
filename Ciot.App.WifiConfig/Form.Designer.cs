namespace Ciot.App.WifiConfig
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelSerialPort = new Label();
            comboBoxSerialPort = new ComboBox();
            labelSsid = new Label();
            textBoxSsid = new TextBox();
            labelPassword = new Label();
            textBoxPassword = new TextBox();
            buttonSend = new Button();
            buttonConnect = new Button();
            SuspendLayout();
            // 
            // labelSerialPort
            // 
            labelSerialPort.AutoSize = true;
            labelSerialPort.Location = new Point(20, 20);
            labelSerialPort.Name = "labelSerialPort";
            labelSerialPort.Size = new Size(69, 15);
            labelSerialPort.TabIndex = 0;
            labelSerialPort.Text = "Porta Serial:";
            // 
            // comboBoxSerialPort
            // 
            comboBoxSerialPort.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSerialPort.FormattingEnabled = true;
            comboBoxSerialPort.Location = new Point(20, 40);
            comboBoxSerialPort.Name = "comboBoxSerialPort";
            comboBoxSerialPort.Size = new Size(175, 23);
            comboBoxSerialPort.TabIndex = 1;
            // 
            // labelSsid
            // 
            labelSsid.AutoSize = true;
            labelSsid.Location = new Point(20, 75);
            labelSsid.Name = "labelSsid";
            labelSsid.Size = new Size(120, 15);
            labelSsid.TabIndex = 2;
            labelSsid.Text = "Nome do WiFi (SSID):";
            // 
            // textBoxSsid
            // 
            textBoxSsid.Location = new Point(20, 95);
            textBoxSsid.Name = "textBoxSsid";
            textBoxSsid.Size = new Size(258, 23);
            textBoxSsid.TabIndex = 3;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(20, 130);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(42, 15);
            labelPassword.TabIndex = 4;
            labelPassword.Text = "Senha:";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(20, 150);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.Size = new Size(258, 23);
            textBoxPassword.TabIndex = 5;
            // 
            // buttonSend
            // 
            buttonSend.Location = new Point(20, 190);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(258, 35);
            buttonSend.TabIndex = 6;
            buttonSend.Text = "Enviar Configuração";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // buttonConnect
            // 
            buttonConnect.Location = new Point(201, 40);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new Size(77, 23);
            buttonConnect.TabIndex = 7;
            buttonConnect.Text = "Conectar";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(298, 249);
            Controls.Add(buttonConnect);
            Controls.Add(buttonSend);
            Controls.Add(textBoxPassword);
            Controls.Add(labelPassword);
            Controls.Add(textBoxSsid);
            Controls.Add(labelSsid);
            Controls.Add(comboBoxSerialPort);
            Controls.Add(labelSerialPort);
            Name = "Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "WifiConfig";
            Load += Form_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelSerialPort;
        private ComboBox comboBoxSerialPort;
        private Label labelSsid;
        private TextBox textBoxSsid;
        private Label labelPassword;
        private TextBox textBoxPassword;
        private Button buttonSend;
        private Button buttonConnect;
    }
}
