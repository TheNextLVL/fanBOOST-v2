using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLibSys;
using System.Threading;

namespace fanBOOST___METRO_UI
{
    class HardwareAccess{
        protected readonly static Ols Ols;
        
        static HardwareAccess()
        {
            Ols = new Ols();
        }
    }

    class FanController : HardwareAccess
    {
        public enum SPEED
        {
            STOP = 0x0000,
            SLOW = 0x0001,
            AUTO = 0x0002,
            AUTO_1 = 0x003,
            BOOST_1 = 0x0004,
            BOOST_2 = 0x0005,
            BOOST_3 = 0x0006,
        }

        private enum REG
        {
            fanSpeed = 0x0000,
            cpuTemp = 0x0000,
            FAN = 0xA0,
            EC_SC = 0x66,
            EC_DATA = 0x62,
        }

        private string vendor = "TOSHIBA";
        private string name = "SATELLITE L855";

        public FanController(string vendor, string name)
        {
            if (this.vendor != vendor || this.name != name)
                throw new System.SystemException("LAPTOP NOT SUPPORTED");
        }

        private byte readEC(byte address)
        {
            /*
            Read Algorithm: (Thanks to Read & Write Utility)
            1.Address = [EC_SC]
            2.Wait EC free
            3.Write 0x80 to[EC_SC]
            4.Wait IBF free
            5.Write Address to[EC_DATA]
            6.Wait IBF free
            7.Wait OBF full
            8.Read Data from[EC_DATA]
            */
            Ols.WriteIoPortByte((ushort)REG.EC_SC, 0x80);
            System.Threading.Thread.Sleep(100);
            Ols.WriteIoPortByte((ushort)REG.EC_DATA, address);
            System.Threading.Thread.Sleep(100);
            return Ols.ReadIoPortByte((ushort)REG.EC_DATA);
        }

        private void writeEC(byte address,byte value)
        {
            /*
            1. Address = [EC_SC]
            2. Wait EC free
            3. Write 0x81 to [EC_SC]
            4. Wait IBF free
            5. Write Address to [EC_DATA]
            6. Wait IBF free
            7. Write Data to [EC_DATA]
            8. Wait IBF free
            */
            Ols.WriteIoPortByte((ushort)REG.EC_SC, 0x81);
            System.Threading.Thread.Sleep(150);
            Ols.WriteIoPortByte((ushort)REG.EC_DATA, address);
            System.Threading.Thread.Sleep(300);
            Ols.WriteIoPortByte((ushort)REG.EC_DATA, value);
        }

        public string getStatus()
        {
            string status="OK";

            // Check support library sutatus
            switch (Ols.GetStatus())
            {
                case (uint)Ols.Status.NO_ERROR:
                    break;
                case (uint)Ols.Status.DLL_NOT_FOUND:
                    status = "Status Error!! DLL_NOT_FOUND";
                    break;
                case (uint)Ols.Status.DLL_INCORRECT_VERSION:
                    status = "Status Error!! DLL_INCORRECT_VERSION";
                    break;
                case (uint)Ols.Status.DLL_INITIALIZE_ERROR:
                    status = "Status Error!! DLL_INITIALIZE_ERROR";
                    break;
            }

            // Check WinRing0 status
            switch (Ols.GetDllStatus())
            {
                case (uint)Ols.OlsDllStatus.OLS_DLL_NO_ERROR:
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_LOADED:
                    status = "DLL Status Error!! OLS_DRIVER_NOT_LOADED";
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_UNSUPPORTED_PLATFORM:
                    status = "DLL Status Error!! OLS_UNSUPPORTED_PLATFORM";
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_FOUND:
                    status = "DLL Status Error!! OLS_DLL_DRIVER_NOT_FOUND";
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_UNLOADED:
                    status = "DLL Status Error!! OLS_DLL_DRIVER_UNLOADED";
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_LOADED_ON_NETWORK:
                    status = "DLL Status Error!! DRIVER_NOT_LOADED_ON_NETWORK";
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_UNKNOWN_ERROR:
                    status = "DLL Status Error!! OLS_DLL_UNKNOWN_ERROR";
                    break;
            }
            return status;
        }

        public int getCpuTemp()
        {
            return 0;
        }

        public SPEED getFanSpeed()
        {
            return (SPEED)readEC((byte)REG.FAN);
        }

        public void setFanSpeed(SPEED speed)
        {
                writeEC((byte)REG.FAN, (byte)speed);
        }

    }
}
