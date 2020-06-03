using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using System.Net;
using System.IO;
/*--------------------------------------------------------------------
 * Wits_lzss
 * 软件作者:lzss
 * 作者QQ:1324500251
 * 作者博客:http://lzonel.cn/
 * 软件GitHub:https://github.com/lzonel/Wits_lzss
 * 免责声明:本人(站)发布的系统与软件仅为个人学习测试使用，
 * 请在下载后24小时内彻底删除，不得用于任何商业用途，否则后果自负，
 * 如侵犯到您的权益，请及时通知我们，我们会及时处理。
--------------------------------------------------------------------*/
/**
* _ooOoo_
* o8888888o
* 88" . "88
* (| -_- |)
*  O\ = /O
* ___/`---'\____
* .   ' \\| |// `.
* / \\||| : |||// \
* / _||||| -:- |||||- \
* | | \\\ - /// | |
* | \_| ''\---/'' | |
* \ .-\__ `-` ___/-. /
* ___`. .' /--.--\ `. . __
* ."" '< `.___\_<|>_/___.' >'"".
* | | : `- \`.;`\ _ /`;.`/ - ` : | |
* \ \ `-. \_ __\ /__ _/ .-` / /
* ======`-.____`-.___\_____/___.-`____.-'======
* `=---='
*          .............................................
*           佛曰：bug泛滥，我已瘫痪！
*/
namespace Wits_lzss
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string yh = @"""";
        string All_Cookies,CASTGC,CASLOGC,SESSION, uuid,Document_src,Login_type,This_ver, realName;
        private void Button_Login_Click(object sender, EventArgs e)
        {
            if (RadioButton1.Checked==true)
            {
                Get_Session(Get_Ticket(Get_LT()));  //手机号登录
            }
            else if(RadioButton2.Checked==true)
            {
                Get_Session(Get_This_Ticket(Get_This_pwd()));       //学号登录
            }
            Get_User_Info();      //必须项,向服务器激活SESSION!!!,不然SESSION无法正常使用
            if (uuid != "")
            {
                if (CheckBox1.Checked)
                {
                    //文件路径
                    string filePath = "account.ini";
                    //检测文件夹是否存在，不存在则创建

                    //定义编码方式，text1.Text为文本框控件中的内容
                    if (RadioButton1.Checked==true)
                    {
                        byte[] mybyte = Encoding.UTF8.GetBytes("账号：" + TextBox_Phone.Text + "|密码：" + TextBox_Pwd.Text + "|" + "|登录类型：Phone|");
                        string mystr1 = Encoding.UTF8.GetString(mybyte);
                        //写入文件
                        //File.WriteAllBytes(filePath,mybyte);//写入新文件
                        File.WriteAllText(filePath, mystr1);//写入新文件
                        //File.AppendAllText(filePath, mystr1);//添加至文件

                    }
                    else if (RadioButton2.Checked==true)
                    {
                        byte[] mybyte = Encoding.UTF8.GetBytes("账号：" + TextBox_Phone.Text + "|密码：" + TextBox_Pwd.Text + "|" + "|登录类型：School|" + "|学校代码：" + TextBox_Schoolcode.Text + "|");
                        string mystr1 = Encoding.UTF8.GetString(mybyte);
                        //写入文件
                        //File.WriteAllBytes(filePath,mybyte);//写入新文件
                        File.WriteAllText(filePath, mystr1);//写入新文件
                        //File.AppendAllText(filePath, mystr1);//添加至文件
                    }
                }
                else
                {
                    //文件路径
                    string filePath = "account.ini";
                    //检测文件夹是否存在，不存在则创建
                    byte[] mybyte = Encoding.UTF8.GetBytes("http://lzonel.cn");
                    string mystr1 = Encoding.UTF8.GetString(mybyte);
                    //写入文件
                    //File.WriteAllBytes(filePath,mybyte);//写入新文件
                    File.WriteAllText(filePath, mystr1);//写入新文件
                   //File.AppendAllText(filePath, mystr1);//添加至文件
                }
                Form2 form2 = new Form2();
                form2.SESSION = SESSION;    //通用性,且只能使用一次,次数性限制!!
                form2.uuid = uuid;
                form2.Text_Ver_Name = This_ver + "     当前用户: " + realName;
                form2.CASLOGC = CASLOGC;
                form2.CASTGC = CASTGC;
                form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("uuid获取失败!(请检查账号密码是否正确)");
            }
        }
        ///<summary>
        ///取出文本中间内容
        ///<summary>
        ///<return>完事返回成功文本|没有找到返回空</return>
        public static string TextGainCenter(string left, string right, string text)
        {
            //判断是否为null或者是empty
            if (string.IsNullOrEmpty(left))
                return "";
            if (string.IsNullOrEmpty(right))
                return "";
            if (string.IsNullOrEmpty(text))
                return "";
            //判断是否为null或者是empty

            int Lindex = text.IndexOf(left); //搜索left的位置

            if (Lindex == -1)
            { //判断是否找到left
                return "";
            }

            Lindex = Lindex + left.Length; //取出left右边文本起始位置

            int Rindex = text.IndexOf(right, Lindex);//从left的右边开始寻找right

            if (Rindex == -1)
            {//判断是否找到right
                return "";
            }

            return text.Substring(Lindex, Rindex - Lindex);//返回查找到的文本
        }
        public string Get_LT()
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://passport.zhihuishu.com/login?service=https://onlineservice.zhihuishu.com/login/gologin ",//URL     必需项    
                Method = "Get",
                Host= "passport.zhihuishu.com",
                Referer= "https://www.zhihuishu.com/",
               // Cookie= "Hm_lvt_0a1b7151d8c580761c3aef32a3d501c6=1589187834; Hm_lpvt_0a1b7151d8c580761c3aef32a3d501c6=1589187834"
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            string acw_tc = "acw_tc=" + TextGainCenter("acw_tc=",";",cookie)+";";
            string route = "route=" + TextGainCenter("route=", ";", cookie) + ";";
            string source = "source=" + TextGainCenter("source=", ",", cookie) + ";";
            string JSESSIONID = "JSESSIONID=" + TextGainCenter("JSESSIONID=", ";", cookie) + ";";
            string SERVERID = "SERVERID=" + TextGainCenter("SERVERID=", ";", cookie) + ";";
            All_Cookies = acw_tc+ route+ JSESSIONID+ source + SERVERID;
            string LT = TextGainCenter("name=" + yh + "lt" + yh + " value=" + yh,yh,html);
            return LT;
        }



        public string Get_Ticket(string LT)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://passport.zhihuishu.com/login ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host= "passport.zhihuishu.com",
                Referer= "https://passport.zhihuishu.com/login?service=https://onlineservice.zhihuishu.com/login/gologin",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Cookie= All_Cookies,//+ "_uab_collina=158918880366643198618811; u_asec=099%23KAFEwEEKEjMEhGTLEEEEEpEQz0yFD603ScLYQ6AwDuyqW6NHSri4A6PFZ67TEEiStEE7sYFET%2FyZTEw2gHGTEELStEwHsOB8WuGTE1LSt3llsyaSt3iSh3nP%2F3kYt37MlXZddqLStTLtsyaGQ3iSh3nP%2F3wYAYFE9EE%2FbL%2BdCwUQrjDt92iHry4QPSs3uUESPwxWwIWdCjXAPs0tay%2FoPwX9r7PcUbrAbzQSadu4DAZBNsf6aynibO8Cc5dqPtIB3ixSaEutaLxWHsh6w9rBPscS6nZRrtXnbs7naquYSR4fNVX3LWc63ixS6EdqzduWHshnwBnUE7TxYh3PD5A4D4Acma2GkLF6mQN05c7nBEP0D4Gd%2BwDlkf0qiNyV0HzqiNf3fyT2q7DB1DN7VwjoiDvkiwpIK8ikiIDBKsT2qMDBiVtrLLFSmGP0D4MTEEySl3llsyauE7EFNIaHFmQTEE7EERpC"
                Postdata = "lt="+LT+"&execution=e1s1&_eventId=submit&username="+TextBox_Phone.Text+"&password="+TextBox_Pwd.Text+"&clCode=&clPassword=&tlCode=&tlPassword=&remember=on",//Post要发送的数据
            };
            item.Header.Add("Origin", "https://passport.zhihuishu.com");
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string header = result.Header.ToString();
            string Ticket = TextGainCenter("ticket=", ".com",header)+".com";
            //获取请求的Cookie
            string cookie = result.Cookie;
             CASTGC= "CASTGC=" + TextGainCenter("CASTGC=", ";", cookie) + ";";
             CASLOGC = "CASLOGC=" + TextGainCenter("CASLOGC=", ";", cookie) + ";";
            return Ticket;
        }
        public string Get_Session(string Ticket)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://onlineservice.zhihuishu.com/login/gologin?ticket=" + Ticket,//URL     必需项    
                Method = "Get",
                Host = "onlineservice.zhihuishu.com",
                Referer = "https://passport.zhihuishu.com/login?service=https://onlineservice.zhihuishu.com/login/gologin",
                Cookie = CASTGC+ CASLOGC
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            string Session = "SESSION="+TextGainCenter("SESSION=", ";", cookie);
            SESSION = Session;
            // Console.WriteLine(html);
            if (html == "")
            {
                MessageBox.Show("登录成功!","Tips:",MessageBoxButtons.OK);
            }
            return Session;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/lzonel/Wits_lzss");
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton2.Checked==true)
            {
                label3.Visible = true;
                TextBox_Schoolcode.Visible = true;
                label1.Text = "学 号:";
                TextBox_Phone.Text = "";
                TextBox_Pwd.Text = "";
                TextBox_Schoolcode.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            string Ver = "1.0.0";       //当前版本号
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://update.lzonel.cn/Wits_lzss.txt",//URL     必需项    
                Method = "Get",
                //Encoding = Encoding.UTF8    //解决了返回值为乱码
            };
            //请求的返回值对象

            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            string Version = TextGainCenter("<version>", "</version>", html);
            string lzonel = TextGainCenter("<lzonel>", "</lzonel>", html);
            string lzonel_1 = TextGainCenter("<lzonel_1>", "</lzonel_1>", html);
            string Download_Url = TextGainCenter("<Download_Url>", "</Download_Url>", html);
            string View_Url = TextGainCenter("<View_Url>", "</View_Url>", html);
            this.Text = this.Text + "  v" + Ver;
            This_ver = "  v" + Ver;
            if (Version=="")
            {
                linkLabel1.Visible = true;
                MessageBox.Show("服务器链接失败!");
            }
            if (Version != Ver)
            {
                DialogResult dialogResult = MessageBox.Show(lzonel_1, lzonel, MessageBoxButtons.YesNo);
                try
                {
                    if (dialogResult == DialogResult.Yes)
                    {
                        //自动下载
                        ProgressBar1.Visible = true;
                        label4.Visible = true;
                        DownloadFile(Download_Url, "Download_Music" + Version + ".zip", ProgressBar1, label4, View_Url);
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(View_Url);
                    }
                }
                catch (Exception)
                {
                    linkLabel1.Visible = true;
                    MessageBox.Show("服务器链接失败!");
                }
               
                this.Text = this.Text + "   最新版本：" + Version;
                This_ver = "  v" + Ver + "   最新版本：" + Version;
            }
        }
        /// <summary>        
        /// c#,.net 下载文件        
        /// </summary>        
        /// <param name="URL">下载文件地址</param>       
        /// <param name="Filename">下载后的存放地址</param>        
        /// <param name="Prog">用于显示的进度条</param> 
        public void DownloadFile(string URL, string filename, ProgressBar prog, Label label4, string View_Url)
        {

            float percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    label4.Text = percent.ToString() + "%";
                    System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                }
                so.Close();
                st.Close();
                if (label4.Text == "100%")
                {
                    MessageBox.Show("下载最新版成功，请手动解压当前目录的(zip)压缩包!!!", "提示：", MessageBoxButtons.OK);
                    Close();
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("自动下载失败，请去官网手动下载", "提示：", MessageBoxButtons.OK);
                System.Diagnostics.Process.Start(View_Url);
                //throw;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            //文件路径
            string filePath = "account.ini";
            try
            {
                if (File.Exists(filePath))
                {
                    Document_src = File.ReadAllText(filePath);
                    byte[] mybyte = Encoding.UTF8.GetBytes(Document_src);
                    Document_src = Encoding.UTF8.GetString(mybyte);
                    Login_type = TextGainCenter("登录类型：", "|", Document_src);
                    if (Login_type =="School")
                    {
                        RadioButton2.Checked = true;
                        TextBox_Schoolcode.Text = TextGainCenter("学校代码：", "|", Document_src);
                    }
                    else
                    {
                        RadioButton1.Checked = true;
                    }
                    TextBox_Phone.Text = TextGainCenter("账号：", "|", Document_src);
                    TextBox_Pwd.Text = TextGainCenter("密码：", "|", Document_src);
                    CheckBox1.Checked = true;
                }
                else
                {
                    //文件不存在
                    RadioButton1.Checked = true;
                    File.Create("account.ini").Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked==true)
            {
                label3.Visible = false;
                TextBox_Schoolcode.Visible = false;
                label1.Text = "手机号:";
                TextBox_Phone.Text = "";
                TextBox_Pwd.Text = "";
                TextBox_Schoolcode.Text = "";
            }
        }

        public void Get_User_Info()
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/login/getLoginUserInfo",//URL     必需项    
                Method = "Get",
                Host = "onlineservice.zhihuishu.com",
                Referer = " https://onlineh5.zhihuishu.com/onlineWeb.html",
                Cookie = SESSION
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            //secret = TextGainCenter("secret"+yh+":"+yh,yh,html);
            uuid = TextGainCenter("uuid" + yh + ":" + yh, yh, html);
            realName = TextGainCenter("realName" + yh + ":" + yh, yh, html);
            //Console.WriteLine(html);
        }

        //-----------学号登录
        public string Get_This_pwd()
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://passport.zhihuishu.com/user/validateCodeAndPassword ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host = "passport.zhihuishu.com",
                Referer = " https://passport.zhihuishu.com/login?service=https%3A%2F%2Fonlineservice.zhihuishu.com%2Flogin%2Fgologin%3Ffromurl%3Dhttps%253A%252F%252Fonlineh5.zhihuishu.com%252FonlineWeb.html%2523%252FstudentIndex",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Postdata = "code="+TextBox_Phone.Text+"&password="+TextBox_Pwd.Text+"&schoolId="+TextBox_Schoolcode.Text+"&captcha=",//Post要发送的数据
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            string pwd = TextGainCenter("pwd"+yh+":"+yh, yh, html);
            string acw_tc = "acw_tc=" + TextGainCenter("acw_tc=", ";", cookie) + ";";
            string route = "route=" + TextGainCenter("route=", ";", cookie) + ";";
            string JSESSIONID = "JSESSIONID=" + TextGainCenter("JSESSIONID=", ";", cookie) + ";";
            string SERVERID = "SERVERID=" + TextGainCenter("SERVERID=", ";", cookie) + ";";
            All_Cookies = acw_tc + route + JSESSIONID+ "source=-1;" + SERVERID;
            return pwd; //临时的 动态变化的,用来获取 CASTGC+ CASLOGC
        }
        public string Get_This_Ticket(string pwd)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://passport.zhihuishu.com/login?pwd="+pwd+"&service=https://onlineservice.zhihuishu.com/login/gologin ",//URL     必需项    
                Method = "Get",//URL     可选项 默认为Get   
                Host = "passport.zhihuishu.com",
                Referer = "https://passport.zhihuishu.com/login?service=https://onlineservice.zhihuishu.com/login/gologin",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Cookie = All_Cookies
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string header = result.Header.ToString();
            string Ticket = TextGainCenter("ticket=", ".com", header) + ".com";
            //获取请求的Cookie
            string cookie = result.Cookie;
            CASTGC = "CASTGC=" + TextGainCenter("CASTGC=", ";", cookie) + ";";
            CASLOGC = "CASLOGC=" + TextGainCenter("CASLOGC=", ";", cookie) + ";";
            return Ticket;
        }

    }
}
