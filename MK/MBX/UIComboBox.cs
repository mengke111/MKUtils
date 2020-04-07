using MK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MBX
{
   public class UIComboBox
    {
      static  public void cbaddstr(ComboBox cB, string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                cB.Items.Add(str);
            }
        }

        static public void cbaddnull(ComboBox cB)
        {
            cB.Items.Add("");
        }

        static public void InitComboBoxWithNull(ComboBox cB, List<string> StrList,string str1,string str2)
        {
            cB.Items.Clear();
            int i = 0;
            try
            {
                foreach (string  item in StrList)
                {
                    cB.Items.Add(item);
                    LogHelper.Log(str1 + i + " : " + item);
                    i++;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                LogHelper.Log(str2 + " Failed " + ee.ToString());
            }
            LogHelper.Log(str2 + " Success");
        }
    }
}
