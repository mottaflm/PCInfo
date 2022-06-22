using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.NetworkInformation;
using PCInfo.Forms;

namespace PCInfo.Classes
{
    class SystemManager
    {
        //Propriedades públicas para uso de outras classes
        public DateTime TimeStamp { get; private set; } //Horário da Consulta
        public Computer CurrentTarget { get; private set; } //Computador que está sendo consultado
        public bool IsRunning { get; private set; } //Flag para verificar se o programa está processando dados ou não

        //Atributos privados para uso interno desta classe

        private readonly List<ObjectQuery> wmiQueryList; //Lista de Queries para consulta
        private readonly List<Action<ObjectQuery>> queryActions; //Lista de ações (métodos) que devem ser executados
        private readonly List<Task> tasks; //Lista de tarefas para rodar de forma assíncrona
        private ManagementScope targetScope; //Atributo que conecta na máquina remota

        public SystemManager()
        {
            tasks = new List<Task>();

            //Aqui ficam as queries que consultam o WMIObject do computador remoto
            //OBS: Pode fazer um dicionário também, mas dá mto trabalho
            wmiQueryList = new List<ObjectQuery>()
                    {
                        new ObjectQuery("SELECT * FROM Win32_ComputerSystem"),
                        new ObjectQuery("SELECT * FROM Win32_UserAccount WHERE LocalAccount = 1"),
                        new ObjectQuery("SELECT * FROM Win32_ComputerSystemProduct"),
                        new ObjectQuery("SELECT * FROM Win32_OperatingSystem"),
                        new ObjectQuery("SELECT * FROM Win32_Processor"),
                        new ObjectQuery("SELECT * FROM Win32_LogicalDisk"),
                        new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration"),
                        new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface WHERE PacketsPerSec > 0"),
                    };

            //Lista das ações, cada método vai para uma query
            queryActions = new List<Action<ObjectQuery>>()
                    {
                        new Action<ObjectQuery>(LoadUserInfo),
                        new Action<ObjectQuery>(LoadLocalUsersInfo),
                        new Action<ObjectQuery>(LoadBiosInfo),
                        new Action<ObjectQuery>(LoadOSInfo),
                        new Action<ObjectQuery>(LoadCPUInfo),
                        new Action<ObjectQuery>(LoadDiskInfo),
                        new Action<ObjectQuery>(LoadNetworkInfo),
                        new Action<ObjectQuery>(LoadNetworkUsage),
                    };
        }

        //Método para abertura de um novo chamado
        public async Task OpenNewTicket()
        {
            try
            {
                await Task.Run(() => Process.Start("https://www.google.com/search?q=Tickets+Systems"));
            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
            }
        }

        //Método para acessar o computador remotamente
        public async Task RemoteConnection(string hostname)
        {
            try
            {
                var isPinging = await Task.Run(() => PingResponse(hostname));

                if(isPinging)
                    await Task.Run(() => StartRemoteAssistance(hostname));

            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
            }
        }

        //Método para reiniciar o computador remotamente
        public async Task RemoteRestart(string hostname, decimal time)
        {
            try
            {
                var isPinging = await Task.Run(() => PingResponse(hostname));

                if (isPinging)
                {
                    Task.Run(() => PingTarget(hostname));
                    await Task.Run(() => StartRemoteShutdown(hostname, time));
                }
            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
            }
        }
       
        //Método principal de consulta
        public async Task LoadComputerInfo(string hostname)
        {
            IsRunning = true;
            TimeStamp = DateTime.Now;
            CurrentTarget = null;

            MainForm.UpdateOutput();

            try
            {
                await Task.Run(() => ConnectToTarget(hostname));
                
                if(CurrentTarget != null)
                {
                    if (CurrentTarget.IsConnected)
                    {
                        //Cada ação será acionada com sua Query respectiva.
                        foreach (var action in queryActions)
                        {
                            var currentIndex = queryActions.IndexOf(action);
                            tasks.Add(Task.Run(() => action.Invoke(wmiQueryList.ElementAt(currentIndex))));
                        }

                        //Roda Update para cada Task que finalizar
                        foreach (Task task in tasks)
                        {
                            await task;
                            MainForm.UpdateOutput();
                        }

                        //Aciona a Flag quando todas as tasks finalizarem
                        var allTasks = Task.WhenAll(tasks);
                        await allTasks;
                        IsRunning = !allTasks.IsCompleted;
                        tasks.Clear();
                    }
                    else
                        MainForm.UpdateOutput();
                }
            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
            }
        }

        public bool IsThisTaskFinished(int taskIndex)
        {
            return tasks[taskIndex].IsCompleted;
        }

        //Método de conexão ao computador.
        private void ConnectToTarget(string hostname)
        {
            try
            {
                CurrentTarget = new Computer(hostname);

                var isPinging = PingResponse(hostname);
                if (isPinging)
                {
                    //Padrão de conexão ao utilizar o targetScope
                    targetScope = new ManagementScope($"\\\\{CurrentTarget.Name}\\root\\cimv2");
                    targetScope.Connect();
                }

                CurrentTarget.IsConnected = isPinging;
            }
            catch (Exception e)
            {
                IsRunning = false;
                Program.WarningDialog($"Erro ao tentar acessar o computador: {e.Message}");
            } 
        }

        //Método Ping para validar conexões
        private bool PingResponse(string hostname)
        {
            try
            {
                //Objeto para teste de ping
                Ping testConnection = new Ping();
                var connectionStatus = testConnection.Send(hostname, 1000).Status;

                //Status 0 significa que a conexão deu certo
                if (connectionStatus == 0)
                    return true;
                else
                {
                    IsRunning = false;
                    Program.WarningDialog($"Falha de conexão!\nErro: {connectionStatus}");
                    return false;
                }
            }
            catch
            {
                IsRunning = false;
                Program.ErrorDialog($"Falha de conexão!\nNão houve resposta do Computador {hostname.ToUpper()}");
                return false;
            }
        }

        //Métodos utilizado nas ações
        //É passado a Query para a ação, cada ação tem o seu Searcher e o seu Storage.
        //O Searcher realiza a consulta baseada na query e no computador alvo
        //O Storage recebe os resultados em uma coleção
        //Para cada propriedade dentro da Storage ele vai procurar a "coluna" desejada
        //O resultado é armazenado nas propriedades da classe Computer
        //OBS: Há uma classe Util para conversões de valores.

        private void LoadUserInfo(ObjectQuery currentQuery)
        {
            try
            {
                ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher(targetScope, currentQuery);
                ManagementObjectCollection wmiStorage = wmiSearcher.Get();

                object queryResult = null;

                foreach (ManagementObject wmiProperty in wmiStorage)
                    queryResult = wmiProperty["UserName"];

                //Existe outra forma de pegar o usuário que é rodando o comando do QueryUser
                if (queryResult == null)
                    CurrentTarget.LoggedUser = GetUserByQuser();
                else
                {
                    //O resultado vem como UNIMEDCG\Login, no caso só quero o Login, logo removo o restante
                    var login = queryResult.ToString();
                    CurrentTarget.LoggedUser = login.Substring(login.IndexOf('\\') + 1);
                }
            }
            catch (Exception e)
            {
                Program.WarningDialog(e.Message);
            }
        }

        private void LoadLocalUsersInfo(ObjectQuery currentQuery)
        {
            try
            {
                ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher(targetScope, currentQuery);
                ManagementObjectCollection wmiStorage = wmiSearcher.Get();

                //Correção de um Bug onde o LocalUsers vinha com informações do anterior quando não era preenchido (referencia da memória preenchida)
                if (CurrentTarget.LocalUsers.Count > 0)
                    CurrentTarget.LocalUsers.Clear();
                else
                    foreach (ManagementObject wmiProperty in wmiStorage)
                        CurrentTarget.LocalUsers.Add(wmiProperty["Name"].ToString());
            }
            catch (Exception e)
            {
                Program.WarningDialog(e.Message);
            }
        }

        private void LoadBiosInfo(ObjectQuery currentQuery)
        {
            try
            {
                ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher(targetScope, currentQuery);
                ManagementObjectCollection wmiStorage = wmiSearcher.Get();

                foreach (ManagementObject wmiProperty in wmiStorage)
                {
                    CurrentTarget.Manufacturer = wmiProperty["Vendor"].ToString();
                    CurrentTarget.Model = wmiProperty["Name"].ToString();
                    CurrentTarget.ServiceTag = wmiProperty["IdentifyingNumber"].ToString();
                }
            }
            catch (Exception e)
            {
                Program.WarningDialog(e.Message);
            }
        }

        private void LoadOSInfo(ObjectQuery currentQuery)
        {
            try
            {
                ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher(targetScope, currentQuery);
                ManagementObjectCollection wmiStorage = wmiSearcher.Get();

                foreach (ManagementObject wmiProperty in wmiStorage)
                {
                    CurrentTarget.Name = wmiProperty["CSName"].ToString();
                    CurrentTarget.OSName = wmiProperty["Caption"].ToString();
                    CurrentTarget.OSVersion = Util.FormatOSVersion(wmiProperty["Version"].ToString());
                    CurrentTarget.OSInstallDate = Util.ToDateTime(wmiProperty["InstallDate"].ToString());

                    var bootTime = Util.ToDateTime(wmiProperty["LastBootUptime"].ToString());
                    CurrentTarget.OSBootTime = bootTime;
                    CurrentTarget.OSUptime = TimeStamp - bootTime;

                    var totalMemory = Util.ToDouble(wmiProperty["TotalVisibleMemorySize"].ToString());
                    totalMemory = Util.ConvertToLowerDouble(totalMemory, ConvertType.Memory);
                    CurrentTarget.TotalMemory = totalMemory;

                    var freeMemory = Util.ToDouble(wmiProperty["FreePhysicalMemory"].ToString());
                    freeMemory = Util.ConvertToLowerDouble(freeMemory, ConvertType.Memory);
                    CurrentTarget.FreeMemory = freeMemory;

                    CurrentTarget.UsedMemory = totalMemory - freeMemory;
                }
            }
            catch (Exception e)
            {
                Program.WarningDialog(e.Message);
            }
        }

        private void LoadCPUInfo(ObjectQuery currentQuery)
        {
            try
            {
                ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher(targetScope, currentQuery);
                ManagementObjectCollection wmiStorage = wmiSearcher.Get();

                foreach (ManagementObject wmiProperty in wmiStorage)
                {
                    CurrentTarget.CPUName = wmiProperty["Name"].ToString();
                    CurrentTarget.CPUClock = Util.ConvertToLowerDouble(Util.ToDouble(wmiProperty["MaxClockSpeed"].ToString()), ConvertType.ClockSpeed);
                    CurrentTarget.CPUPercent = Util.ToByte(wmiProperty["LoadPercentage"].ToString());
                }
            }
            catch (Exception e)
            {
                Program.WarningDialog(e.Message);
            }
        }

        private void LoadDiskInfo(ObjectQuery currentQuery)
        {
            try
            {
                ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher(targetScope, currentQuery);
                ManagementObjectCollection wmiStorage = wmiSearcher.Get();

                foreach (ManagementObject wmiProperty in wmiStorage)
                {
                    var property = wmiProperty["Name"].ToString();

                    if (property.Equals("C:"))
                    {
                        var diskSize = wmiProperty["Size"];
                        var diskFree = wmiProperty["FreeSpace"];

                        if (diskSize != null)
                            CurrentTarget.DiskSize = Util.ConvertToLowerDouble(Util.ToDouble(diskSize.ToString()), ConvertType.DiskSpace);

                        if (diskFree != null)
                            CurrentTarget.DiskFreeSpace = Util.ConvertToLowerDouble(Util.ToDouble(diskFree.ToString()), ConvertType.DiskSpace);

                        if (diskSize != null && diskFree != null)
                            CurrentTarget.DiskUsedSpace = CurrentTarget.DiskSize - CurrentTarget.DiskFreeSpace;
                    }
                }
            }
            catch (Exception e)
            {
                Program.WarningDialog(e.Message);
            }
        }

        private void LoadNetworkUsage(ObjectQuery currentQuery)
        {
            try
            {
                ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher(targetScope, currentQuery);
                ManagementObjectCollection wmiStorage = wmiSearcher.Get();

                foreach (ManagementObject wmiProperty in wmiStorage)
                {
                    var netUsage = Util.ConvertToLowerDouble(Util.ToDouble(wmiProperty["BytesTotalPerSec"].ToString()), ConvertType.Network);
                    CurrentTarget.NetworkUsage = netUsage;
                }
            }
            catch (Exception e)
            {
                Program.WarningDialog(e.Message);
            }
        }

        private void LoadNetworkInfo(ObjectQuery currentQuery)
        {
            try
            {
                //Outro Searcher e Storage para pegar outra informação de Rede, no caso, IP e MAC.
                ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher(targetScope, currentQuery);
                ManagementObjectCollection wmiStorage = wmiSearcher.Get();

                foreach (ManagementObject wmiProperty in wmiStorage)
                {
                    var property = wmiProperty["IPAddress"];

                    if (property != null)
                    {
                        List<string> ipsList = new List<string>(property as string[]);

                        //Verifica e puxa o IP que faz parte da rede Unimed
                        var ipFound = ipsList.Find(
                           i =>
                           i.ToString().Contains("192.168") ||
                           i.ToString().Contains("10.200") ||
                           i.ToString().Contains("10.201") ||
                           i.ToString().Contains("10.199")
                           );

                        if (ipFound != null)
                        {
                            CurrentTarget.IpAddress = ipFound;
                            CurrentTarget.MacAddress = wmiProperty["MACAddress"].ToString();
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Program.WarningDialog(e.Message);
            }
        }
        //Método para conexão via MSRA
        private void StartRemoteAssistance(string hostname)
        {
            try
            {
                //Inicia o processo do CMD
                ProcessStartInfo startInfo = new ProcessStartInfo(@"msra.exe", $"/offerra {hostname}")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };

                //Inicia o processo e pega o código de erro.
                Process process = Process.Start(startInfo);
                process.WaitForExit();
                var exitCode = process.ExitCode;

                if (exitCode != 0)
                    Program.WarningDialog("Falha ao Inicializar a Conexão Remota!");
            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
            }
        }

        private void StartRemoteShutdown(string hostname, decimal time)
        {
            try
            {
                var message = "A TI iniciou uma reinicialização programada do seu computador.";

                //Inicia o processo do CMD para reiniciar
                ProcessStartInfo startInfo = new ProcessStartInfo(@"shutdown.exe", $"/r /m \\\\{hostname} /t {time} /c \"{message}\" /f")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };

                //Inicia o processo e pega o código de erro.
                Process process = Process.Start(startInfo);
                process.WaitForExit();
                var exitCode = process.ExitCode;

                if (exitCode != 0)
                    Program.WarningDialog("Falha ao Reiniciar!");
            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
            }
        }

        private void PingTarget(string hostname)
        {
            try
            {
                //Inicia o processo do CMD para o ping
                ProcessStartInfo startInfo = new ProcessStartInfo(@"ping.exe", $"{hostname} -t")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = false,
                };

                //Inicia o processo e pega o código de erro.
                Process process = Process.Start(startInfo);

            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
            }
        }

        //Metódo para rodar o Quser via CMD.exe
        private string GetUserByQuser()
        {
            string userName = "Não encontrado!";

            try
            {
                //Utilizado para Bug do SysWoW64
                IntPtr val = IntPtr.Zero;
                Wow64DisableWow64FsRedirection(ref val);

                //Inicia o processo do CMD
                ProcessStartInfo startInfo = new ProcessStartInfo(@"cmd.exe", $"/C quser /server:{CurrentTarget.Name}")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };

                //Inicia o processo e pega o código de erro.
                Process process = Process.Start(startInfo);
                process.WaitForExit();
                var exitCode = process.ExitCode;

                //Caso o código seja 0, significa que deu certo.
                if (exitCode == 0)
                {
                    //Conversão do Formato padrão do Quser para Lista separado por vírgula
                    string tempText = process.StandardOutput.ReadToEnd();
                    Regex regex = new Regex("[ ]{1,}", RegexOptions.None);
                    tempText = regex.Replace(tempText, ",");

                    //Este é o elemento que está o login do usuário
                    userName = tempText.Split(',').ToList().ElementAt(9);
                }

                Wow64EnableWow64FsRedirection(ref val);
            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
            }

            return userName;
        }



        //Métodos Utilitários para converter a saída da pasta SysWOW64 para System32
        //Caso não possua, irá ocorrer um erro na hora de executar o QueryUser via CommandPrompt
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int Wow64EnableWow64FsRedirection(ref IntPtr ptr);
    }
}
