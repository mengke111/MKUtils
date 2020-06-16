using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MK
{
    public class MKOS
    {

        public static SysInfoBean SystemInfo(out string jsonStr)
        {
            string result = string.Empty;
            SysInfoBean sib = new SysInfoBean();

            sib.OsName = OSName();
            sib.OsVer = OSVer();
            sib.CpuInfo = CPUInfo();
            sib.MemorySize = MemorySize();
            sib.BiosVer = BIOSVer();
            sib.EcVer = ECVer();
            sib.SerialNumer = SerialNumber();
            sib.SecureBootStatus = "N/A";
            sib.PlatformRole = "N/A";
            sib.ProductName = ProductName();
            string hdd0 = string.Empty;
            string hdd1 = string.Empty;
            StorageInfo(out hdd0, out hdd1);
            sib.Hdd0 = hdd0;
            sib.Hdd1 = hdd1;
            sib.LocalIp = GetLocalIp();
            sib.Uuid = uuId();

            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(sib.GetType());

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, sib);
                    result = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            jsonStr = result;

            return sib;
        }

        public static SysInfoBean GetSysElement()
        {
            SysInfoBean sib = new SysInfoBean();

            sib.OsName = OSName();
            sib.OsVer = OSVer();
            sib.CpuInfo = CPUInfo();
            sib.MemorySize = MemorySize();
            sib.BiosVer = BIOSVer();
            sib.EcVer = ECVer();
            sib.SerialNumer = SerialNumber();
            sib.SecureBootStatus = "N/A";
            sib.PlatformRole = "N/A";
            sib.ProductName = ProductName();
            string hdd0 = string.Empty;
            string hdd1 = string.Empty;
            StorageInfo(out hdd0, out hdd1);
            sib.Hdd0 = hdd0;
            sib.Hdd1 = hdd1;
            sib.LocalIp = GetLocalIp();
            sib.Uuid = uuId();

            return sib;
        }

        
            public static string GetLocalIp()
            {
                UnicastIPAddressInformation addrInfo = null;
                NetworkInterface[] nis = null;

                try
                {
                    nis = NetworkInterface.GetAllNetworkInterfaces();
                }
                catch (NetworkInformationException)
                {
                }

                foreach (NetworkInterface ni in nis)
                {
                    if ((ni.OperationalStatus != OperationalStatus.Up) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                        continue;

                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            addrInfo = ip;
                        }
                    }
                }
                if (addrInfo != null)
                {
                    return addrInfo.Address.ToString();
                }
                else
                {
                    return "";
                }
            }
        

        public class SysInfoBean
        {
            private string testId;
            private string osName;
            private string osVer;
            private string cpuInfo;
            private string hdd0;
            private string hdd1;
            private string memorySize;
            private string biosVer;
            private string ecVer;
            private string secureBootStatus;
            private string platformRole;
            private string serialNumer;
            private string productName;
            private string localIp;
            private string uuid;
            //public string ItemUUID;

            public string TestId
            {
                get
                {
                    return testId;
                }

                set
                {
                    testId = value;
                }
            }

            public string OsName
            {
                get
                {
                    return osName;
                }

                set
                {
                    osName = value;
                }
            }

            public string OsVer
            {
                get
                {
                    return osVer;
                }

                set
                {
                    osVer = value;
                }
            }

            public string CpuInfo
            {
                get
                {
                    return cpuInfo;
                }

                set
                {
                    cpuInfo = value;
                }
            }

            public string Hdd0
            {
                get
                {
                    return hdd0;
                }

                set
                {
                    hdd0 = value;
                }
            }

            public string Hdd1
            {
                get
                {
                    return hdd1;
                }

                set
                {
                    hdd1 = value;
                }
            }

            public string MemorySize
            {
                get
                {
                    return memorySize;
                }

                set
                {
                    memorySize = value;
                }
            }

            public string BiosVer
            {
                get
                {
                    return biosVer;
                }

                set
                {
                    biosVer = value;
                }
            }

            public string EcVer
            {
                get
                {
                    return ecVer;
                }

                set
                {
                    ecVer = value;
                }
            }

            public string SecureBootStatus
            {
                get
                {
                    return secureBootStatus;
                }

                set
                {
                    secureBootStatus = value;
                }
            }

            public string PlatformRole
            {
                get
                {
                    return platformRole;
                }

                set
                {
                    platformRole = value;
                }
            }

            public string SerialNumer
            {
                get
                {
                    return serialNumer;
                }

                set
                {
                    serialNumer = value;
                }
            }

            public string ProductName
            {
                get
                {
                    return productName;
                }

                set
                {
                    productName = value;
                }
            }

            public string LocalIp
            {
                get
                {
                    return localIp;
                }

                set
                {
                    localIp = value;
                }
            }

            public string Uuid
            {
                get
                {
                    return uuid;
                }

                set
                {
                    uuid = value;
                }
            }
        }
        public static string uuId()
        {
            return identifier("Win32_ComputerSystemProduct", "UUID");
        }

        private static string BIOSVer()
        {
            string strDate = identifier("Win32_BIOS", "ReleaseDate"); //yyyy - MM - dd

            strDate = strDate.Substring(0, 8);
            //return identifier("Win32_BIOS", "Manufacturer") + " "
            //       + identifier("Win32_BIOS", "SMBIOSBIOSVersion") + " "
            //       + strDate;
            return identifier("Win32_BIOS", "SMBIOSBIOSVersion") + " " + strDate;        //CDCN25WW
        }
        private static string ECVer()
        {
            //return "EC Version: " + identifier("Win32_BIOS", "EmbeddedControllerMajorVersion") + "."
            //       + identifier("Win32_BIOS", "EmbeddedControllerMinorVersion");

            string result = "N/A";
            string biosVer = identifier("Win32_BIOS", "SMBIOSBIOSVersion");
            string ecMinorVer = identifier("Win32_BIOS", "EmbeddedControllerMinorVersion");
            if (!ecMinorVer.Equals(""))
            {
                result = biosVer.Substring(0, 2) + "EC" + ecMinorVer + biosVer.Substring(6, biosVer.Length - 6);
            }

            return result;
        }
        public static string OSName()
        {
            return identifier("Win32_OperatingSystem", "Caption");  ////Microsoft Windows 7 专业版
        }

        private static string OSVer()
        {
            return identifier("Win32_OperatingSystem", "Version") + " "             ////6.1.7601
                + identifier("Win32_OperatingSystem", "CSDVersion") + " Build "     ////Service Pack 1
                + identifier("Win32_OperatingSystem", "BuildNumber");               ////7601
           
        }

        private static string CPUInfo()
        {
            return identifier("Win32_Processor", "Name");    //Intel(R) Core(TM) i7-6700HQ CPU @ 2.60GHz

            //return identifier("Win32_Processor", "Name") + ", "
            //    + identifier("Win32_Processor", "MaxClockSpeed") + " Mhz"
            //    + identifier("Win32_Processor", "NumberOfCores") + ", Core(s), "
            //    + identifier("Win32_Processor", "NumberOfLogicalProcessors") + " Logical Processor(s)";
        }
        private static string SecureBootStatus()
        {
            return identifier("Win32_ComputerSystemProduct", "Version");
        }
        private static string ProductName()
        {
            return identifier("Win32_ComputerSystemProduct", "Version");
        }
        private static string SerialNumber()
        {
            return identifier("Win32_BIOS", "SerialNumber");
        }

        private static void StorageInfo(out string hdd0, out string hdd1)
        {
            string result = string.Empty;
            string storageInfo = ident("Win32_DiskDrive", "Caption");
            try
            {
                if (storageInfo.IndexOf("#") > -1)
                {
                    hdd0 = storageInfo.Substring(0, storageInfo.IndexOf("#"));
                    hdd1 = storageInfo.Substring(storageInfo.IndexOf("#") + 1);
                    if (hdd0.IndexOf("SCSI") > -1)
                    {
                        hdd0 = hdd0.Substring(0, hdd0.IndexOf("SCSI")).Trim();
                    }
                    if (hdd1.IndexOf("SCSI") > -1)
                    {
                        hdd1 = hdd1.Substring(0, hdd1.IndexOf("SCSI")).Trim();
                    }
                }
                else
                {
                    if (storageInfo.IndexOf("SCSI") > -1)
                    {
                        hdd0 = storageInfo.Substring(0, storageInfo.IndexOf("SCSI")).Trim();
                    }
                    else
                    {
                        hdd0 = storageInfo;
                    }
                    hdd1 = "N/A";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                hdd0 = "N/A";
                hdd1 = "N/A";
            }
        }

        private static string MemorySize()
        {
            string result = string.Empty;
            try
            {
                //long memorySize = Convert.ToInt64(identifier("Win32_PhysicalMemory", "Capacity")) / 1024 / 1024 / 1024;
                //result = "Memory Size: " + memorySize + "GB" + "</br>"
                //    + "Memory Info: " + identifier("Win32_PhysicalMemory", "Manufacturer");

                long memorySize = Convert.ToInt64(identifier("Win32_PhysicalMemory", "Capacity")) / 1024 / 1024 / 1024;
                result = memorySize + "GB";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            ManagementClass mc = new ManagementClass(wmiClass);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() == "True")
                {
                    //Only get the first one
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return result;
        }

        private static string identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            try
            {
                ManagementClass mc = new ManagementClass(wmiClass);
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    //Only get the first one
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("identifier: WMI Exception when getting " + wmiClass + ":" + wmiProperty);
            }
            return result;
        }

        private static string ident(string wmiClass, string wmiProperty)
        {
            string result = string.Empty;
            try
            {
                ManagementClass mc = new ManagementClass(wmiClass);
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    result += mo[wmiProperty].ToString() + "#";
                }
                result = result.TrimEnd('#');
            }
            catch (Exception)
            {
                Console.WriteLine("identifier: WMI Exception when getting " + wmiClass + ":" + wmiProperty);
            }
            return result;
        }
    }
}
