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
        

        public static Program.eError RunOutApp(string sPathApp)
        {
            try
            {
                Process.Start(sPathApp);
                return Program.eError.OK;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                return Program.eError.Win32Ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Program.eError.Other;
            }
        }
    }
}
