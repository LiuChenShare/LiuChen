using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace Chenyuan.Lottery.Web.Timedtask
{
    public class CensusdemoTask
    {
        System.Threading.Timer timer;
        private static int count = 1;

        public CensusdemoTask()
        {
            timer = new System.Threading.Timer(SetCensusURL, null, 0, 1000 * 1);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetCensusURL(object obj)
        {
            string txt = string.Format("写入时间:{0},次数{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), count);
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                string path = "D:\\1.txt";//文件的路径，保证文件存在。  
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.WriteLine(txt);
                count++;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sw.Dispose();
                sw.Close();
                fs.Dispose();
                fs.Close();
            }
        }
    }
}