using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace 自动压缩视频
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = true;
            


            // 读取配置文件
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;

            // 获取配置值
            var mySettingPath = settings["path"];
            if (mySettingPath != null)
            {
                // 解密字符串
                string decrypted = jiamijiemi.DecryptStr(mySettingPath.Value);
                richTextBox1.Text = decrypted;
                qianpath = richTextBox1.Text;
            }
            else {
                // 添加一个新的键值对
                settings.Add("path", "myValue");

                // 保存配置文件
                config.Save(ConfigurationSaveMode.Modified);
            }
            // 获取配置值
            var mySettingConn = settings["conn"];
            if (mySettingConn != null)
            {
                string decrypted = jiamijiemi.DecryptStr(mySettingConn.Value);
                richTextBox2.Text = decrypted;
                // 在此处使用配置值
            }
            else
            {
                // 添加一个新的键值对
                settings.Add("conn", "myValue");

                // 保存配置文件
                config.Save(ConfigurationSaveMode.Modified);
            }
        }



        private bool isRunning = false;

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            cls_database.conn =richTextBox2.Text;
            if (isRunning)
            {
                return;
            }

            isRunning = true;
            button3.Enabled = true;
            await Task.Run(() => ListenForData());
            //cls_database.ExecuteSQL();


           
        }

        private void ListenForData()
        {
            while (isRunning)
            {
                // 获取需要处理的数据
                DataTable data = cls_database.GetDataTable("select * from pgm_service_files where is_yasuo=1");
                if (data.Rows.Count > 0) {
                    CompressVideo(data.Rows[0]);
                }

                System.Threading.Thread.Sleep(5000); // wait for 5 seconds before querying again
            }
        }
        string qianpath = "";
        private void CompressVideo(DataRow path)
        {
            
            if (!File.Exists(qianpath + path["path"])) {
                MessageBox.Show(qianpath + path["path"] + "\r\n\r\n" + "视频文件不存在！");
            }
            string arguments = "-i " + qianpath+path["path"] + " -vcodec h264 -vf \"scale='max(480,iw*0.1)':-2\" -r 15 -crf 28 -preset veryfast -acodec libmp3lame -ac 2 -ar 22050 -f mp4 -y " + qianpath+path["path"]+".mp4" ;


             ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "ffmpeg.exe";
            startInfo.Arguments = arguments;
           // startInfo.UseShellExecute = false;
            //startInfo.RedirectStandardOutput = true;
           // startInfo.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();
            if (File.Exists(qianpath + path["path"] + ".mp4")) {
                File.Delete(qianpath + path["path"]);
                File.Copy(qianpath + path["path"] + ".mp4", qianpath + path["path"]);
                File.Delete(qianpath + path["path"] + ".mp4");
            }
            
            cls_database.ExecuteSQL("update pgm_service_files set is_yasuo=0 where md5='" + path["md5"] +"'");

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 在程序关闭时保存配置文件
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;

            // 加密字符串
            string encrypted1 = jiamijiemi.EncryptStr(richTextBox1.Text);
            settings["path"].Value = encrypted1;

            // 加密字符串
            string encrypted2 = jiamijiemi.EncryptStr(richTextBox2.Text);
            settings["conn"].Value = encrypted2;

            config.Save(ConfigurationSaveMode.Modified);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "隐藏私密信息")
            {
                richTextBox2.Visible = false;
                button2.Text = "显示私密信息";
            }
            else {
                richTextBox2.Visible = true;
                button2.Text = "隐藏私密信息";
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
                isRunning = false;
            button3.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            qianpath = richTextBox1.Text;
        }
    }
}