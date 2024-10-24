using System;
using System.Diagnostics;
using System.Management;

namespace MonitorSistema
{
    public class LectorRecursosSistema
    {
        PerformanceCounter memoria = new PerformanceCounter();
        PerformanceCounter cpu = new PerformanceCounter();
        PerformanceCounter discoLectura = new PerformanceCounter();
        PerformanceCounter discoEscritura = new PerformanceCounter();

        string[] nombresInstancia;
        PerformanceCounter[] redPaquetesEnviados;
        PerformanceCounter[] redPaquetesRecibidos;

        bool compacto = false;

        public LectorRecursosSistema()
        {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Network Interface");
            nombresInstancia = cat.GetInstanceNames();

            redPaquetesRecibidos = new PerformanceCounter[nombresInstancia.Length];
            for (int i = 0; i < nombresInstancia.Length; i++)
                redPaquetesRecibidos[i] = new PerformanceCounter();

            redPaquetesEnviados = new PerformanceCounter[nombresInstancia.Length];
            for (int i = 0; i < nombresInstancia.Length; i++)
                redPaquetesEnviados[i] = new PerformanceCounter();
        }



        public double getCPU()
        {
            return GetCounterValue(cpu, "Processor", "% Processor Time", "_Total");
        }

        public double getMemoriaVirtual()
        {
            string str;
            double d = GetCounterValue(memoria, "Memory", "% Committed Bytes In Use", null);
            str = d.ToString("F") + "% (";

            d = GetCounterValue(memoria, "Memory", "Committed Bytes", null);
            str += FormatBytes(d) + " / ";

            return d = GetCounterValue(memoria, "Memory", "Commit Limit", null);
           // return str + FormatBytes(d) + ") ";
        }

        internal string getUsuario()
        {
            return QueryComputerSystem("username");
        }

        internal string getOrdenadorModelo()
        {
            return QueryComputerSystem("model"); 
        }

        internal string getOrdenadorFabricante()
        {
            return QueryComputerSystem("manufacturer");
        }

        internal string [] getProcesadores()
        {
            ManagementObjectSearcher pos = new ManagementObjectSearcher("select * from Win32_Processor");

            string[] result = new string[pos.Get().Count];
            int i = 0;
            foreach (ManagementObject obj in pos.Get())
            {
                result[i++] = obj["Name"].ToString();
            }
            return result;
        }

        public double getMemoriaFisica()
        {
            string s = QueryComputerSystem("totalphysicalmemory");
            double totalphysicalmemory = System.Convert.ToDouble(s);

            double d = GetCounterValue(memoria, "Memory", "Available Bytes", null);
            d = totalphysicalmemory - d;

            s = compacto ? "%" : "% (" + FormatBytes(d) + " / " + FormatBytes(totalphysicalmemory) + ")";
            d /= totalphysicalmemory;
            return d *= 100;
            //return compacto ? (int)d + s : d.ToString("F") + s;
        }

        public enum DiskData { ReadAndWrite, Read, Write };

        public double getDatosDisco(DiskData dd)
        {
            return dd == DiskData.Read ?
                        GetCounterValue(discoLectura, "PhysicalDisk", "Disk Read Bytes/sec", "_Total") :
                    dd == DiskData.Write ?
                        GetCounterValue(discoEscritura, "PhysicalDisk", "Disk Write Bytes/sec", "_Total") :
                    dd == DiskData.ReadAndWrite ?
                        GetCounterValue(discoLectura, "PhysicalDisk", "Disk Read Bytes/sec", "_Total") +
                        GetCounterValue(discoEscritura, "PhysicalDisk", "Disk Write Bytes/sec", "_Total") :
                    0;
        }

        public enum NetData { ReceivedAndSent, Received, Sent };

        public double getDatosRed(NetData nd)
        {
            if (nombresInstancia.Length == 0)
                return 0;

            double d = 0;
            for (int i = 0; i < nombresInstancia.Length; i++)
            {
                d += nd == NetData.Received ?
                        GetCounterValue(redPaquetesRecibidos[i], "Network Interface", "Bytes Received/sec", nombresInstancia[i]) :
                    nd == NetData.Sent ?
                        GetCounterValue(redPaquetesEnviados[i], "Network Interface", "Bytes Sent/sec", nombresInstancia[i]) :
                    nd == NetData.ReceivedAndSent ?
                        GetCounterValue(redPaquetesRecibidos[i], "Network Interface", "Bytes Received/sec", nombresInstancia[i]) +
                        GetCounterValue(redPaquetesEnviados[i], "Network Interface", "Bytes Sent/sec", nombresInstancia[i]) :
                    0;
            }

            return d;
        }

        enum Unit { B, KB, MB, GB, ER }
        public string FormatBytes(double bytes)
        {
            int unit = 0;
            while (bytes > 1024)
            {
                bytes /= 1024;
                ++unit;
            }

            string s = compacto ? ((int)bytes).ToString() : bytes.ToString("F") + " ";
            return s + ((Unit)unit).ToString();
        }

        public string QueryComputerSystem(string type)
        {
            string str = null;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject objMgmt in objCS.Get())
            {
                try
                {
                    str = objMgmt[type].ToString();
                }
                catch (Exception)
                {
                    return "QueryComputerSystem: " + type + " not found"; //TODO
                }
            }
            return str;
        }

        public string QueryEnvironment(string type)
        {
            return Environment.ExpandEnvironmentVariables(type);
        }

        public string getDiscosLogicos()
        {
            string diskSpace = string.Empty;
            object device, space;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            foreach (ManagementObject objMgmt in objCS.Get())
            {
                device = objMgmt["DeviceID"];       // C:
                if (null != device)
                {
                    space = objMgmt["FreeSpace"];   // C:10.32 GB, D:5.87GB
                    if (null != space)
                        diskSpace += device.ToString() + FormatBytes(double.Parse(space.ToString())) + ", ";
                }
            }

            diskSpace = diskSpace.Substring(0, diskSpace.Length - 2);
            return diskSpace;
        }


        double GetCounterValue(PerformanceCounter pc, string categoryName, string counterName, string instanceName)
        {
            pc.CategoryName = categoryName;
            pc.CounterName = counterName;
            pc.InstanceName = instanceName;
            return pc.NextValue();
        }
    }
}