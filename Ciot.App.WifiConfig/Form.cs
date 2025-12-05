using Ciot.Protos.V2;
using Ciot.Sdk.Infra.Interfaces;
using System.Diagnostics;
using System.IO.Ports;

namespace Ciot.App.WifiConfig
{
    public partial class Form : System.Windows.Forms.Form
    {
        WifiConfig _wifiConfig;
        Uart _uart;

        public Form()
        {
            InitializeComponent();
            _uart = new Uart(0);
            
            _wifiConfig = new WifiConfig(_uart, 2);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // Carregar as portas seriais disponíveis
            LoadSerialPorts();
        }

        private void LoadSerialPorts()
        {
            comboBoxSerialPort.Items.Clear();
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length > 0)
            {
                comboBoxSerialPort.Items.AddRange(ports);
                comboBoxSerialPort.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Nenhuma porta serial encontrada.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            UartStop();

            // Validar campos
            if (comboBoxSerialPort.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione uma porta serial.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxSsid.Text))
            {
                MessageBox.Show("Por favor, digite o nome do WiFi (SSID).", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSsid.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                MessageBox.Show("Por favor, digite a senha do WiFi.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPassword.Focus();
                return;
            }

            // Obter os dados
            string serialPort = comboBoxSerialPort.SelectedItem.ToString() ?? "";
            string ssid = textBoxSsid.Text;
            string password = textBoxPassword.Text;

            if (UartStart(serialPort) == false)
            {
                return;
            }

            try
            {
                await _wifiConfig.SendConfig(ssid, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao enviar a configuração WiFi: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MessageBox.Show($"Configuração WiFi salva:\n\nPorta Serial: {serialPort}\nSSID: {ssid}\nSenha: {new string('*', password.Length)}",
                "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            UartStop();
        }

        private bool UartStart(string port)
        {
            uint portNum = uint.Parse(port.Replace("COM", ""));

            try
            {
                var err = _uart.Start(new UartCfg
                {
                    Num = portNum,
                    BaudRate = 115200,
                    ReadTimeout = 1000,
                    WriteTimeout = 1000,
                });
                return err == Err.Ok;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao iniciar a porta serial: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void UartStop()
        {
            Debug.WriteLine("UART Stop");
            _uart.Stop();
        }

        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            string serialPort = comboBoxSerialPort?.SelectedItem?.ToString() ?? "";

            if (UartStart(serialPort))
            {
                var config = await _wifiConfig.GetConfig();
                if (config == null)
                {
                    MessageBox.Show("Falha ao obter a configuração WiFi.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UartStop();
                    return;
                }
                textBoxSsid.Text = config.Ssid.Length == 0 ? "<Vazio>" : config.Ssid;
                textBoxPassword.Text = config.Password;
            }
            else
            {
                MessageBox.Show("Falha ao abrir a porta de comunicação");
            }

            UartStop();
        }
    }
}
