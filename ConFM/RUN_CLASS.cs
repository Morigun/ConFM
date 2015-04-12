using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConFM
{
    class RUN_CLASS
    {
        public enum eError
        {
            OK,
            ArgEx,
            NSEx,
            NfoundEx,
            AccessEx,
            Win32Ex,
            Other
        }

        public static eError RunOutApp(string sPathApp)
        {
            try
            {
                Process.Start(sPathApp);
                return eError.OK;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                return eError.Win32Ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return eError.Other;
            }
        }
    }
}
