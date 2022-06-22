using PCInfo.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace PCInfo.Forms
{
    public partial class MainForm : Form
    {

        //SystemManager é a Classe que roda os comandos e pega as informações do computador remoto
        private readonly SystemManager manager;
        
        //Instancia para método estático
        private static MainForm instance;

        //Inicialização do Formulário
        public MainForm()
        {
            InitializeComponent();
            manager = new SystemManager();
            instance = this;
        }

        //Método Assíncrono da Ação de Clique do Botão "Consultar"
        private async void SearchButtonClick(object sender, EventArgs e)
        {
            try
            {
                if(InputIsValid())
                {
                    //Só irá consultar quando não houver consultas em andamento.
                    if (manager.IsRunning)
                        Program.WarningDialog("Por favor espere a consulta anterior finalizar!");
                    else
                        await manager.LoadComputerInfo(inputBox.Text);
                }   
            }
            catch (Exception error)
            {
                Program.ErrorDialog(error.Message);
            }
        }

        //Método para botão do clique de abertura de chamado
        private async void NewTicketButtonClick(object sender, EventArgs e)
        {
            await manager.OpenNewTicket();
        }

        //Método do clique para realizar a conexão remota
        private async void RemoteConnectionClick(object sender, EventArgs e)
        {
            try
            {
                if (InputIsValid())
                    await manager.RemoteConnection(inputBox.Text);

            }
            catch(Exception exc)
            {
                Program.ErrorDialog(exc.Message);
            }
        }

        private async void RemoteRestartClick(object sender, EventArgs e)
        {
            try
            {
                if (InputIsValid())
                    await manager.RemoteRestart(inputBox.Text, timeValue.Value);

            }
            catch (Exception exc)
            {
                Program.ErrorDialog(exc.Message);
            }
        }

        //Métodos utilitários para organização e melhorias do Form
        private void ClearBox(object sender, EventArgs e)
        {
            if(inputBox.Text == "Informe o nome ou o IP do Computador")
                inputBox.Clear();
        }

        private void ResetInputBox(object sender, EventArgs e)
        {
            if (inputBox.TextLength <= 0)
                inputBox.Text = "Informe o nome ou o IP do Computador";
        }

        //Realiza o Clique do Botão através da tecla "Enter"
        private void InputBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter) && !manager.IsRunning)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                searchButton.PerformClick();
            }
        }

        //Método para verificar se a entrada foi modificada
        private bool InputIsValid()
        {
            var defaultText = "Informe o nome ou o IP do Computador";

            if (inputBox.Text == defaultText || inputBox.TextLength <= 0)
            {
                Program.WarningDialog("Por favor informe um nome ou IP de um computador!");
                return false;
            }    
            else
                return true;
        }

        //Métodos com polimorfismo para formatação do texto
        private string FormatText(string text, Color color)
        {
            var textIndex = outputBox.Text.IndexOf(text);
            var textLenght = text.Length;

            outputBox.Select(textIndex, textLenght);
            outputBox.SelectionColor = color;

            return text;
        }

        private string FormatText(string text, FontStyle style)
        {
            var textIndex = outputBox.Text.IndexOf(text);
            var textLenght = text.Length;

            var fontName = outputBox.Font.Name;
            var fontHeight = outputBox.Font.Size;
            var newFont = new Font(fontName, fontHeight, style);

            outputBox.Select(textIndex, textLenght);
            outputBox.SelectionFont = newFont;

            return text;
        }

        private string FormatText(string text, FontStyle style, Color color)
        {
            var textIndex = outputBox.Text.IndexOf(text);
            var textLenght = text.Length;

            var fontName = outputBox.Font.Name;
            var fontHeight = outputBox.Font.Size;
            var newFont = new Font(fontName, fontHeight, style);

            outputBox.Select(textIndex, textLenght);
            outputBox.SelectionFont = newFont;
            outputBox.SelectionColor = color;

            return text;
        }

        //Método para Saída dos dados e formatação geral da mesma
        private void OutputInfo()
        {
            try
            {
                ClearOutput();

                if (manager.CurrentTarget != null)
                {
                    OutputConnectionInfo();

                    if (manager.CurrentTarget.IsConnected)
                    {
                        OutputTimeStamp();
                        OutputUserAndBootTime();
                        OutputResourcesInfo();
                        //OutputUsersInfo();
                        OutputNetworkInfo();
                        OutputComputerInfo();
                        OutputOSInfo();
                    }
                }
                else
                    OutputLoadingStatus();
            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
            }
        }

        private void OutputConnectionInfo()
        {
            //Dados de conexão ===========================================

            if (manager.CurrentTarget.IsConnected)
            {

                var computerName = manager.CurrentTarget.Name;
                var connectionStatus = $"{computerName} CONECTADO!";

                outputBox.AppendText(connectionStatus + Environment.NewLine);

                FormatText(computerName, FontStyle.Bold);
                FormatText(connectionStatus, Color.Green);

                outputBox.AppendText(Environment.NewLine);
            }
            else
            {
                var computerName = inputBox.Text.ToUpper();
                var connectionStatus = $"{computerName} DESCONECTADO!";

                outputBox.AppendText(connectionStatus + Environment.NewLine);

                FormatText(computerName, FontStyle.Bold);
                FormatText(connectionStatus, Color.Red);
            }
        }

        private void OutputUserAndBootTime()
        {
            //Dados do Tempo de boot ===========================================

            string outTimeSpan = "Carregando...";
            string outBootTime = "Carregando...";
            string outLoggedUser = "Carregando...";

            if (manager.IsThisTaskFinished(0))
                outLoggedUser = manager.CurrentTarget.LoggedUser;

            if (manager.IsThisTaskFinished(3))
            {
                outTimeSpan = Util.ToFormatedTimeSpan(manager.CurrentTarget.OSUptime);
                outBootTime = manager.CurrentTarget.OSBootTime.ToString();
            }

            outputBox.AppendText($"Usuário Logado: {outLoggedUser}" + Environment.NewLine);
            outputBox.AppendText(Environment.NewLine);
            outputBox.AppendText($"Ligado em: {outBootTime}" + Environment.NewLine);
            outputBox.AppendText($"Total de: {outTimeSpan}" + Environment.NewLine);

            if (manager.IsThisTaskFinished(3))
                if (manager.CurrentTarget.OSUptime.Days >= Util.MAX_DAYS) //Regras estão no UTIL.
                {
                    FormatText(outBootTime, FontStyle.Bold, Color.Red);
                    FormatText(outTimeSpan, FontStyle.Bold, Color.Red);
                }

            outputBox.AppendText(Environment.NewLine);  
        }

        private void OutputNetworkInfo()
        {
            //Dados de Rede ===========================================
            var netWorkTitle = "Informações de Rede";
            outputBox.AppendText($"=> {netWorkTitle} <=" + Environment.NewLine);

            FormatText(netWorkTitle, FontStyle.Bold, outputBox.ForeColor);

            string outIpAddress = "Carregando...";
            string outMacAddress = "Carregando...";

            if (manager.IsThisTaskFinished(6))
            {
                outIpAddress = manager.CurrentTarget.IpAddress;
                outMacAddress = manager.CurrentTarget.MacAddress;
            }

            outputBox.AppendText($"Endereço IP: {outIpAddress}" + Environment.NewLine);
            outputBox.AppendText($"Endereço MAC: {outMacAddress}" + Environment.NewLine);

            outputBox.AppendText(Environment.NewLine);
        }

        //private void OutputUsersInfo()
        //{
        //    //Dados de usuários ===========================================
        //    var usersTitle = "Dados de Usuários";
        //    outputBox.AppendText($"=> {usersTitle} <=" + Environment.NewLine);

        //    FormatText(usersTitle, FontStyle.Bold, outputBox.ForeColor);



        //    outputBox.AppendText(Environment.NewLine);
        //}

        private void OutputComputerInfo()
        {
            //Dados do computador ===========================================
            var computerTitle = "Dados do Computador";
            outputBox.AppendText($"=> {computerTitle} <=" + Environment.NewLine);

            FormatText(computerTitle, FontStyle.Bold, outputBox.ForeColor);

            string outManufacturer = "Carregando...";
            string outModel = "Carregando...";
            string outServiceTag = "Carregando...";
            string outCPUName = "Carregando...";
            string outCPUClock = "Carregando...";

            if (manager.IsThisTaskFinished(2))
            {
                outManufacturer = manager.CurrentTarget.Manufacturer;
                outModel = manager.CurrentTarget.Model;
                outServiceTag = manager.CurrentTarget.ServiceTag;
                
            }

            if(manager.IsThisTaskFinished(4))
            {
                outCPUName = manager.CurrentTarget.CPUName;
                outCPUClock = $"{manager.CurrentTarget.CPUClock:F2}";
            }
               
            outputBox.AppendText($"Fabricante: {outManufacturer}" + Environment.NewLine);
            outputBox.AppendText($"Modelo: {outModel}" + Environment.NewLine);
            outputBox.AppendText($"Service Tag: {outServiceTag}" + Environment.NewLine);
            outputBox.AppendText($"CPU: {outCPUName}" + Environment.NewLine);
            outputBox.AppendText($"Clock da CPU: {outCPUClock} Ghz" + Environment.NewLine);

            outputBox.AppendText(Environment.NewLine);
        }

        private void OutputOSInfo()
        {
            //Dados do Sistema Operacional ===========================================
            var osTitle = "Dados do Sistema Operacional";
            outputBox.AppendText($"=> {osTitle} <=" + Environment.NewLine);

            FormatText(osTitle, FontStyle.Bold, outputBox.ForeColor);

            string outOSName = "Carregando...";
            string outOSVersion = "Carregando...";
            string outOSInstallDate = "Carregando...";

            if(manager.IsThisTaskFinished(3))
            {
                outOSName = manager.CurrentTarget.OSName;
                outOSVersion = manager.CurrentTarget.OSVersion;
                outOSInstallDate = manager.CurrentTarget.OSInstallDate.ToString();
            }

            outputBox.AppendText($"Sistema Operacional: {outOSName}" + Environment.NewLine);
            outputBox.AppendText($"Versão do Sistema: {outOSVersion}" + Environment.NewLine);
            outputBox.AppendText($"Data de Instalação: {outOSInstallDate}" + Environment.NewLine);
            outputBox.AppendText($"Usuários Locais: ");

            if (manager.IsThisTaskFinished(1))
            {
                outputBox.AppendText(Environment.NewLine);

                foreach (string user in manager.CurrentTarget.LocalUsers)
                    outputBox.AppendText($"- {user}" + Environment.NewLine);
            }
            else
                outputBox.AppendText("Carregando..." + Environment.NewLine);

            outputBox.AppendText(Environment.NewLine);
        }

        private void OutputResourcesInfo()
        {
            //Dados do Uso de Recursos ===========================================
            var resourceTitle = "Uso de Recursos";
            outputBox.AppendText($"=> {resourceTitle} <=" + Environment.NewLine);

            FormatText(resourceTitle, FontStyle.Bold, outputBox.ForeColor);

            string outCpuUsage = "Carregando...";
            string outFreeMemory = "Carregando...";
            string outUsedMemory = "Carregando...";
            string outTotalMemory = "Carregando...";
            string outFreeDisk = "Carregando...";
            string outUsedDisk = "Carregando...";
            string outTotalDisk = "Carregando...";
            string outNetworkUsage = "Carregando...";

            byte cpuUsage = 0;
            double freeMemory = 0;
            double freeDisk = 0;
            double networkUsage = 0;

            if (manager.IsThisTaskFinished(3))
            {
                freeMemory = manager.CurrentTarget.FreeMemory;          
                outUsedMemory = $"{manager.CurrentTarget.UsedMemory:F2}";
                outTotalMemory = $"{manager.CurrentTarget.TotalMemory:F2} GB";
                outFreeMemory = $"{freeMemory:F2} GB";  
            }

            if(manager.IsThisTaskFinished(4))
            {
                cpuUsage = manager.CurrentTarget.CPUPercent;
                outCpuUsage = $"{cpuUsage}%";
            }

            if(manager.IsThisTaskFinished(5))
            {
                freeDisk = manager.CurrentTarget.DiskFreeSpace;
                outUsedDisk = $"{manager.CurrentTarget.DiskUsedSpace:F2}";
                outTotalDisk = $"{manager.CurrentTarget.DiskSize:F2} GB";
                outFreeDisk = $"{freeDisk:F2} GB";
            }

            if (manager.IsThisTaskFinished(7))
            {
                networkUsage = manager.CurrentTarget.NetworkUsage;
                outNetworkUsage = $"{networkUsage:F2} Mbps";
            }

            outputBox.AppendText($"Uso da CPU: {outCpuUsage}" + Environment.NewLine);
            outputBox.AppendText($"Uso da Memória: {outUsedMemory} / {outTotalMemory} | Memória Livre: {outFreeMemory}" + Environment.NewLine);
            outputBox.AppendText($"Uso do Disco Local: {outUsedDisk} / {outTotalDisk} | Disco Livre: {outFreeDisk}" + Environment.NewLine);
            outputBox.AppendText($"Uso da Rede: {outNetworkUsage}" + Environment.NewLine);

            outputBox.AppendText(Environment.NewLine);

            if (manager.IsThisTaskFinished(3))
                if (freeMemory < Util.MIN_MEMORY)
                    FormatText(outFreeMemory, Color.Red);

            if (manager.IsThisTaskFinished(4))
                if (cpuUsage > Util.MAX_CPU)
                    FormatText(outCpuUsage, Color.Red);

            if (manager.IsThisTaskFinished(5))
                if (freeDisk < Util.MIN_DISK)
                    FormatText(outFreeDisk, Color.Red);

            if (manager.IsThisTaskFinished(7))
                if (networkUsage > Util.MAX_NETWORK)
                    FormatText(outNetworkUsage, Color.Red);
        }

        private void OutputTimeStamp()
        {
            //Dados da Consulta ===========================================
            outputBox.AppendText($"Horário desta Consulta: {manager.TimeStamp}" + Environment.NewLine);

            outputBox.AppendText(Environment.NewLine);
        }

        private void OutputLoadingStatus()
        {
            var loadingMsg = "Carregando...";
            outputBox.Text = loadingMsg;

            FormatText(loadingMsg, FontStyle.Italic, Color.DarkGray);
        }

        private void ClearOutput()
        {
            outputBox.Clear();
        }

        public static void UpdateOutput()
        {
            instance.OutputInfo();
        }
    }
}
