using CsharpHttpHelper;
using JinYiHelp.JsHelp;
using JinYiHelp.StringHelp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
/*--------------------------------------------------------------------
 * Wits_lzss
 * 软件作者:lzss
 * 作者QQ:1324500251
 * 作者博客:http://lzonel.cn/
 * 软件开源GitHub:https://github.com/lzonel/Wits_lzss
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public string SESSION, uuid, CASTGC, CASLOGC,Text_Ver_Name;
        string JMK, WK, WK_loadVideoPointerInfo, acw_tc, route, SESSION_Temp, SERVERID, WK_recruitId, WK_JSON, watchPointPost, saveCacheIntervalTime, saveDatabaseIntervalTime, learningTokenId;
        string ccCourseId, chapterId, recruitId, videoId, ID, videoSec, Staus, Study_Times, lessonVideoId, SM_lessonId;
        int ALL_Nums,Checkets_Nums, totalStudyTime,Timer1000,WK_m;
        string yh = @"""";
        bool KC = true;
        int[] Timer_Listviews =new int[30];
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
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
        public void Get_getImportantNoticeList()            //获取见面课
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://onlineservice.zhihuishu.com/student/message/message/getImportantNoticeList ",//URL     必需项    
                Method = "Get",
                Host = "onlineservice.zhihuishu.com",
                Referer = "https://onlineh5.zhihuishu.com/onlineWeb.html",
                Cookie= SESSION
            };
            item.Header.Add("Origin", "https://onlineh5.zhihuishu.com");
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            JMK = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            Root_getImportantNoticeList root_GetImportantNoticeList = JsonConvert.DeserializeObject<Root_getImportantNoticeList>(JMK);
            // Console.WriteLine(root_GetImportantNoticeList.result.Count);  //总数量
            if (root_GetImportantNoticeList.result.Count != 0)
            {
                ComboBox1.Items.Add("(见面课)" + root_GetImportantNoticeList.result[0].courseName);
                if (root_GetImportantNoticeList.result.Count != 1)
                {
                    for (int i = 0; i < root_GetImportantNoticeList.result.Count - 2; i++)      //因为目前发现见面课只有 两种类型 所以这里面都是2  2020-05-25 11:29
                    {
                        if (root_GetImportantNoticeList.result[i].courseId != root_GetImportantNoticeList.result[i + 1].courseId)
                        {
                            ComboBox1.Items.Add("(见面课)" + root_GetImportantNoticeList.result[i + 1].courseName);    //2020-05-27 再次修改
                        }
                       /*
                        if (root_GetImportantNoticeList.result[i+1].courseId != root_GetImportantNoticeList.result[i + 2].courseId)  // 2020-05-25 11:29    王书阳
                        {
                            ComboBox1.Items.Add("(见面课)" + root_GetImportantNoticeList.result[i + 2].courseName);
                        }
                        */
                    }
                }
               // ComboBox1.SelectedIndex = 0;
            }
        }
        public void Get_queryShareCourseInfo()      //获取你所选的课程
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://onlineservice.zhihuishu.com/student/course/share/queryShareCourseInfo ",//URL     必需项    
                Method = "Post",//URL     可选项 默认为Get   
                Host = "onlineservice.zhihuishu.com",       //HOST 出错返回为空!!!
                Referer = "https://onlineh5.zhihuishu.com/onlineWeb.html",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Cookie = SESSION,
                Postdata = "status=0&pageNo=1&pageSize=5&uuid="+ uuid 
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            WK = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            // Console.WriteLine(html);
            Root_queryShareCourseInfo root_QueryShareCourseInfo = JsonConvert.DeserializeObject<Root_queryShareCourseInfo>(WK);
            // Console.WriteLine(root_QueryShareCourseInfo.result.courseOpenDtos.Count);
            try
            {
                if (root_QueryShareCourseInfo.result.courseOpenDtos.Count != 0)
                {
                    ComboBox1.Items.Add(root_QueryShareCourseInfo.result.courseOpenDtos[0].courseName);
                    if (root_QueryShareCourseInfo.result.courseOpenDtos.Count != 1)
                    {
                        for (int i = 0; i < root_QueryShareCourseInfo.result.courseOpenDtos.Count - 1; i++)
                        {
                            if (root_QueryShareCourseInfo.result.courseOpenDtos[i].courseId != root_QueryShareCourseInfo.result.courseOpenDtos[i + 1].courseId)
                            {
                                ComboBox1.Items.Add(root_QueryShareCourseInfo.result.courseOpenDtos[i + 1].courseName);
                            }
                        }
                    }
                    // ComboBox1.SelectedIndex = 0;
                }
                ComboBox1.SelectedIndex = 0;
            }
            catch (Exception)
            {

                MessageBox.Show("密码错误!!!");
            }
            
        }
        private void ComboBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Root_getImportantNoticeList root_GetImportantNoticeList = JsonConvert.DeserializeObject<Root_getImportantNoticeList>(JMK);

            Root_queryShareCourseInfo root_QueryShareCourseInfo = JsonConvert.DeserializeObject<Root_queryShareCourseInfo>(WK);
            // root_GetImportantNoticeList.result.Count      root_QueryShareCourseInfo.result.courseOpenDtos.Count;
            ALL_Nums = root_GetImportantNoticeList.result.Count;
            int mm = 0;
            for (int i = 0; i < root_GetImportantNoticeList.result.Count; i++)      //见面课遍历
            {
                if (ComboBox1.SelectedItem.ToString() == "(见面课)" + root_GetImportantNoticeList.result[i].courseName)    // 1  i==1 可以改为 i */*/*/*/*/*/
                {
                   // Console.WriteLine("(见面课)" + root_GetImportantNoticeList.result[i].courseName);
                    //执行搜索并解析它的各种属性
                    /*
                     listView1.Items.Add("123456");
                    listView1.Items[i].Checked = true;
                    listView1.Items[i++].SubItems.Add("222");
                    listView1.Items[j++].SubItems.Add("333");
                     */
                    KC = true;
                    label3.Visible = false;
                    TextBox_atime.Visible = false;
                    label2.Visible = true;
                    TextBox_Time.Visible = true;
                    RadioButton1.Visible = false;
                    RadioButton2.Visible = false;
                    RadioButton3.Visible = false;
                    label2.Text = "每节课提交速率(ms):";
                    TextBox_Time.Text = "3000";
                    try
                    {
                        listView1.Items.Add(root_GetImportantNoticeList.result[i].taskName);
                        if (root_GetImportantNoticeList.result[i].endTime > ConvertToTimeStamp(DateTime.Now))
                        {
                            listView1.Items[mm].Checked = false;
                            listView1.Items[mm].SubItems.Add("未开始");
                            listView1.Items[mm].BackColor = Color.Red;
                        }
                        else
                        {
                            listView1.Items[mm].Checked = true;
                            listView1.Items[mm].SubItems.Add(" ");
                        }
                        mm++;
                        //   listView1.Items[i].SubItems.Add(root_GetImportantNoticeList.result[i].taskId.ToString());
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("获取失败!,请查看任务是否开始");
                    }
                }
            }
            for (int i = 0; i < root_QueryShareCourseInfo.result.courseOpenDtos.Count; i++)     //网课遍历
            {
                if (ComboBox1.SelectedItem.ToString() == root_QueryShareCourseInfo.result.courseOpenDtos[i].courseName)
                {
                    KC = false;
                    label3.Visible = true;
                    TextBox_atime.Visible = true;
                    RadioButton1.Visible = true;
                    RadioButton2.Visible = true;
                    RadioButton3.Visible = true;
                    label1.Text = root_QueryShareCourseInfo.result.courseOpenDtos[i].progress;
                    Get_WK_Videolist(root_QueryShareCourseInfo.result.courseOpenDtos[i].secret);
                    //执行搜索并解析它的各种属性
                }
            }
        }
        /// <summary>
        /// 时间转为13位时间戳
        /// </summary>
        /// <param name="timeStamp">
        /// <returns></returns>
        public static long ConvertToTimeStamp(DateTime time)
        {

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));

            long t = (time.Ticks - startTime.Ticks) / 10000;

            return t;
        }
        public void Get_WK_Videolist(string recruitAndCourseId)     //需要请求两遍,一遍获取SESSION一遍获取Videolist
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/learning/videolist ",//URL     必需项    
                Method = "Post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",       //HOST 出错返回为空!!!
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Cookie = CASTGC+ CASLOGC,
                Postdata = "recruitAndCourseId="+ recruitAndCourseId + "&uuid="+uuid
            };
            item.Header.Add("Origin", "https://studyh5.zhihuishu.com");
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
             acw_tc = "acw_tc=" + TextGainCenter("acw_tc=", ";", cookie) + ";";
             route = "route=" + TextGainCenter("route=", ";", cookie) + ";";
            SESSION_Temp = "SESSION=" + TextGainCenter("SESSION=", ";", cookie);
            SERVERID = "SERVERID=" + TextGainCenter("SERVERID=", ";", cookie);
            //Console.WriteLine(html);
            Get_WK_SERVERID_Temp(recruitAndCourseId);//获取SERVERID_Temp
        }
        public void Get_WK_SERVERID_Temp(string recruitAndCourseId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "http://passport.zhihuishu.com/?service=http://studyservice.zhihuishu.com/login/gologin?fromurl=https%3A%2F%2Fstudyh5.zhihuishu.com%2FvideoStudy.html%23%2FstudyVideo%3FrecruitAndCourseId%3D"+ recruitAndCourseId,//URL     必需项    
                Method = "Get",//URL     可选项 默认为Get   
                Host = "passport.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Cookie = acw_tc + route  + CASTGC + CASLOGC+ SERVERID
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string header = result.Header.ToString();
            //获取请求的Cookie
            string cookie = result.Cookie;
            string SERVERID1 = TextGainCenter("SERVERID=", ";", cookie);
            Get_WK_Ticket( SERVERID1, recruitAndCourseId);
        }
        public void Get_WK_Ticket(string SERVERID1,string recruitAndCourseId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://passport.zhihuishu.com/login?service=http%3A%2F%2Fstudyservice.zhihuishu.com%2Flogin%2Fgologin%3Ffromurl%3Dhttps%253A%252F%252Fstudyh5.zhihuishu.com%252FvideoStudy.html%2523%252FstudyVideo%253FrecruitAndCourseId%253D"+ recruitAndCourseId,//URL     必需项    
                Method = "Get",//URL     可选项 默认为Get   
                Host = "passport.zhihuishu.com",
                Cookie = CASTGC + CASLOGC+ SERVERID1
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string header = result.Header.ToString();
            string Ticket = TextGainCenter("ticket=", ".com", header) + ".com";
            //获取请求的Cookie
            string cookie = result.Cookie;
            Get_WK_JH_SESSION( Ticket, recruitAndCourseId);
        }
        public void Get_WK_JH_SESSION(string Ticket,string recruitAndCourseId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "http://studyservice.zhihuishu.com/login/gologin?fromurl=https%3A%2F%2Fstudyh5.zhihuishu.com%2FvideoStudy.html%23%2FstudyVideo%3FrecruitAndCourseId%3D"+ recruitAndCourseId + "&ticket="+ Ticket,//URL     必需项    
                Method = "Get",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",    //又是HOST,太对了哥太对
                Cookie = CASTGC + CASLOGC + SESSION_Temp
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html  = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            //Console.WriteLine(html);
            Get_WK_Videolist2(recruitAndCourseId);      //Oh,原来它的 SESSION 只能用一次!!!
        }
        public void Get_WK_Videolist2(string recruitAndCourseId)     //需要请求两遍,一遍获取SESSION一遍获取Videolist
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/learning/videolist ",//URL     必需项    
                Method = "Post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",       //HOST 出错返回为空!!!
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
               // Encoding = Encoding.UTF8,    //解决了返回值为乱码
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Cookie = SESSION_Temp,
                Postdata = "recruitAndCourseId=" + recruitAndCourseId + "&uuid=" + uuid
            };
            item.Header.Add("Origin", "https://studyh5.zhihuishu.com");
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
             WK_JSON = html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            Root_WK root_WK = JsonConvert.DeserializeObject<Root_WK>(html);
            int m=0;
            WK_recruitId = root_WK.data.recruitId.ToString();
            for (int i = 0; i < root_WK.data.videoChapterDtos.Count; i++)       //网课遍历
            {
                listView1.Items.Add("章节: "+root_WK.data.videoChapterDtos[i].name);
                listView1.Items[m].BackColor = Color.Lime;
                Timer_Listviews[i] = m;
                // listView1.Items[m].SubItems.Add(root_WK.data.videoChapterDtos[i].id.ToString());
                // listView1.Items[m].SubItems.Add(root_WK.data.videoChapterDtos[i].learningLessonStatus.ToString());
                ++m;
                for (int j = 0; j < root_WK.data.videoChapterDtos[i].videoLessons.Count; j++)
                {
                    if (root_WK.data.videoChapterDtos[i].videoLessons[j].videoId==0)
                    {
                        listView1.Items.Add(" 小组: "+root_WK.data.videoChapterDtos[i].videoLessons[j].name);
                        listView1.Items[m].BackColor = Color.Turquoise;
                        ++m;
                        for (int k = 0; k < root_WK.data.videoChapterDtos[i].videoLessons[j].videoSmallLessons.Count; k++)
                        {
                            listView1.Items.Add("  "+root_WK.data.videoChapterDtos[i].videoLessons[j].videoSmallLessons[k].name);
                            listView1.Items[m].Checked = true;
                            ++m;
                        }
                    }
                    else
                    {
                        listView1.Items.Add(" "+root_WK.data.videoChapterDtos[i].videoLessons[j].name);
                        listView1.Items[m].Checked = true;
                        ++m;
                    }
                    //   listView1.Items[m].SubItems.Add(root_WK.data.videoChapterDtos[i].videoLessons[j].id.ToString());
                    //   listView1.Items[m].SubItems.Add(root_WK.data.videoChapterDtos[i].videoLessons[j].videoId.ToString());

                }
            }
            Checkets_Nums = listView1.Items.Count;
            //Console.WriteLine(html);
        }
        // 获取见面课 JSON 实体类
        public class Result_getImportantNoticeList
        {
            /// <summary>
            /// C君晋级篇
            /// </summary>
            public string taskName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int courseId { get; set; }
            /// <summary>
            /// C君带你玩编程
            /// </summary>
            public string courseName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int taskId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string videoId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int liveCourseId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long startTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long endTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int recruitId { get; set; }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (RadioButton1.Checked == true)
            {
                label2.Visible = false;
                TextBox_Time.Visible = false;
            }
            Get_getImportantNoticeList();
            Get_queryShareCourseInfo();
            this.Text = this.Text + Text_Ver_Name;
        }

        public class Root_getImportantNoticeList
        {
            /// <summary>
            /// 
            /// </summary>
            public List<Result_getImportantNoticeList> result { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string message { get; set; }
        }
        //将秒数转化为时分秒
        private string sec_to_hms(long duration)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
            string str = "";
            if (ts.Hours > 0)
            {
                str = String.Format("{0:00}", ts.Hours) + ":" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = "00:" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:00:" + String.Format("{0:00}", ts.Seconds);
            }
            return str;
        }
        /// <summary>
        /// 判断文本是为数字
        /// </summary>
        /// <param name="str">
        /// <returns></returns>
        public static bool IsInt(string str)
        {

            Regex reg = new Regex(@"^[0-9]+$");
            if (reg.IsMatch(str))
            {

                return true;
            }
            else
            {
                return false;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (KC==true)   //见面课为True    
            {
                string recruitId, liveCourseId, userId, videoId, paramSecret, Staus;
                int qr=0;
                userId = TextGainCenter("%22userId%22%3A", "%2", CASLOGC);
                Root_getImportantNoticeList root_GetImportantNoticeList = JsonConvert.DeserializeObject<Root_getImportantNoticeList>(JMK);
                for (int i = 0; i < root_GetImportantNoticeList.result.Count; i++)
                {
                    try
                    {
                        if (listView1.Items[i].Checked == true)
                        {
                            if (listView1.Items[i].SubItems[1].Text != "未开始")
                            {
                                for (int q = 0; q < root_GetImportantNoticeList.result.Count; q++)
                                {
                                    if (listView1.Items[i].Text == root_GetImportantNoticeList.result[q].taskName)
                                    {
                                        qr = q;
                                        break;
                                    }
                                }
                                recruitId = root_GetImportantNoticeList.result[qr].recruitId.ToString();
                                liveCourseId = root_GetImportantNoticeList.result[qr].liveCourseId.ToString();
                                videoId = "1324500251";
                                try
                                {
                                    if (IsInt(root_GetImportantNoticeList.result[qr].videoId.ToString()))
                                    {
                                        videoId = root_GetImportantNoticeList.result[qr].videoId.ToString();
                                    }
                                    else
                                    {
                                        videoId = root_GetImportantNoticeList.result[qr].videoId.ToString().GetLeft(",");

                                    }
                                }
                                catch (Exception)
                                {

                                    MessageBox.Show("该见面课还未公开(或已经结束,结束的可以刷!)");
                                    break;
                                }
                                for (int k = 0; k < 101; k++)
                                {
                                    paramSecret = V8Test_JMK("[" + yh + recruitId + yh + ", " + yh + liveCourseId + yh + ", " + userId + ", " + yh + k.ToString() + yh + ", " + yh + "2" + yh + ", " + videoId + "]");
                                    Staus = Post_JMK_Progress(paramSecret, liveCourseId, root_GetImportantNoticeList.result[i].courseId.ToString(), recruitId);
                                    listView1.Items[i].SubItems[1].Text = Staus + "  " + k.ToString() + " %";
                                    // Console.WriteLine(Staus + "  " + k.ToString() + " %");
                                    Delay(int.Parse(TextBox_Time.Text));    //延时3000ms
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("没有了...");
                    }
                    
                }
                MessageBox.Show("提交完成,请返回客户端查看!", "Tips:", MessageBoxButtons.OK);
            }
            else
            {
                Random random = new Random();

                Root_WK root_WK = JsonConvert.DeserializeObject<Root_WK>(WK_JSON);
                int m = 0;
                WK_recruitId = root_WK.data.recruitId.ToString();
                for (int i = 0; i < root_WK.data.videoChapterDtos.Count; i++)
                {
                    //  listView1.Items.Add(root_WK.data.videoChapterDtos[i].name);
                    //  listView1.Items[m].SubItems.Add(root_WK.data.videoChapterDtos[i].id.ToString());
                    //  listView1.Items[m].SubItems.Add(root_WK.data.videoChapterDtos[i].learningLessonStatus.ToString());
                    ++m;
                    for (int j = 0; j < root_WK.data.videoChapterDtos[i].videoLessons.Count; j++)
                    {

                        if (root_WK.data.videoChapterDtos[i].videoLessons[j].videoId == 0)
                        {
                            ++m;
                            for (int k = 0; k < root_WK.data.videoChapterDtos[i].videoLessons[j].videoSmallLessons.Count; k++)      // Small
                            {
                                ID = root_WK.data.videoChapterDtos[i].videoLessons[j].id.ToString();    //或者 ID = root_WK.data.videoChapterDtos[i].videoLessons[j].videoSmallLessons[k].lessonId.ToString();
                                lessonVideoId = root_WK.data.videoChapterDtos[i].videoLessons[j].videoSmallLessons[k].id.ToString();
                                videoId = root_WK.data.videoChapterDtos[i].videoLessons[j].videoSmallLessons[k].videoId.ToString();
                                SM_lessonId = root_WK.data.videoChapterDtos[i].videoLessons[j].videoSmallLessons[k].lessonId.ToString();
                                videoSec = root_WK.data.videoChapterDtos[i].videoLessons[j].videoSmallLessons[k].videoSec.ToString();
                                if (listView1.Items[m].Checked == true)
                                {
                                    ccCourseId = root_WK.data.courseId.ToString();
                                    recruitId = root_WK.data.recruitId.ToString();
                                    chapterId = root_WK.data.videoChapterDtos[i].videoLessons[j].chapterId.ToString();
                                    learningTokenId = Post_WK_learningTokenId_2(ccCourseId, chapterId, ID, lessonVideoId, recruitId, videoId);
                                    Study_Times = sec_to_hms(long.Parse(videoSec) - random.Next(1, 15));
                                    Writes_item_task_Small(ID, recruitId, ccCourseId, lessonVideoId);
                                    Root_loadVideoPointerInfo root_loadVideoPointerInfo = JsonConvert.DeserializeObject<Root_loadVideoPointerInfo>(WK_loadVideoPointerInfo);
                                    try
                                    {
                                        for (int ik = root_loadVideoPointerInfo.data.questionPoint.Count - 1; ik >= 0; ik--)
                                        {
                                           // Post_WK_Progress(V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + root_loadVideoPointerInfo.data.questionPoint[ik].timeSec + ", " + videoSec + ", " + yh + root_loadVideoPointerInfo.data.questionPoint[ik].timers + yh + "]"), learningTokenId);
                                            lessonPopupExam_Small(ID, root_loadVideoPointerInfo.data.questionPoint[ik].timers, ccCourseId, recruitId, lessonVideoId);
                                          //  Delay(int.Parse(TextBox_atime.Text));    //延时1000ms
                                        }
                                        //Console.WriteLine(Study_Times);
                                        listView1.Items[m].SubItems.Add("初始化ing...");
                                        listView1.Items[m].SubItems.Add("初始化ing...");
                                        Timer1000 = 0;
                                        totalStudyTime = 500;
                                        watchPointPost = "0,1";
                                        if (RadioButton2.Checked==true) //极速模式
                                        {
                                            Timer_totalStudyTime.Interval = 4990 / int.Parse(TextBox_Time.Text);
                                            Timer_watchPointPost.Interval = 1990 / int.Parse(TextBox_Time.Text);
                                            Timer2.Interval = 1000 / int.Parse(TextBox_Time.Text);
                                        }
                                        else if(RadioButton1.Checked==true)
                                        {
                                            Timer_totalStudyTime.Interval = 4990;
                                            Timer_watchPointPost.Interval = 1990;
                                            Timer2.Interval =1000;
                                            Timer2.Start();
                                        }
                                        Timer_totalStudyTime.Start();
                                        Timer_watchPointPost.Start();
                                       // Timer2.Start();
                                        WK_m = m;
                                        if (RadioButton2.Checked == true) //极速模式
                                        {
                                            for (int iwk = 1; iwk <= int.Parse(videoSec); iwk++)
                                            {
                                                listView1.Items[WK_m].SubItems[2].Text = Timer1000.ToString() + "/" + videoSec;
                                                Timer1000++;
                                                if (Timer1000 % 180 == 0) //是180的整数倍就可以了
                                                {
                                                    saveCacheIntervalTime = V8Test("[" + recruitId + ", " + chapterId + ", " + ccCourseId + ", " + SM_lessonId + ", " + yh + sec_to_hms(Timer1000) + yh + ", " + totalStudyTime + ", " + videoId + ", " + lessonVideoId + ", " + Timer1000 + "]");
                                                    //saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + root_loadVideoPointerInfo.data.questionPoint[ik].timeSec + ", " + videoSec + ", " + yh + root_loadVideoPointerInfo.data.questionPoint[ik].timers + yh + "]");
                                                    Staus = Post_WK_saveCacheIntervalTime(saveCacheIntervalTime, learningTokenId);
                                                    listView1.Items[WK_m].SubItems[1].Text = Staus + ": " + Timer1000.ToString();
                                                    watchPointPost = "0,1," + (totalStudyTime / 5 + 2).ToString();
                                                }
                                                else if (Timer1000 % 300 == 0)
                                                {
                                                    saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + Timer1000 + ", " + videoSec + ", " + yh + sec_to_hms(Timer1000) + yh + "]");
                                                    Staus = Post_WK_Progress(saveDatabaseIntervalTime, learningTokenId);
                                                    listView1.Items[WK_m].SubItems[1].Text = Staus + ": " + Timer1000.ToString();
                                                }
                                                else if (Timer1000 == int.Parse(videoSec)) //最终提交100%
                                                {
                                                    saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + Timer1000 + ", " + videoSec + ", " + yh + Study_Times + yh + "]");
                                                    Staus = Post_WK_Progress(saveDatabaseIntervalTime, learningTokenId);
                                                    listView1.Items[WK_m].SubItems[1].Text = Staus + ": Success!";
                                                    listView1.Items[WK_m].SubItems[2].Text = Timer1000.ToString() + "/" + videoSec;
                                                    Timer1000 = 0;
                                                }
                                                Delay(1000 / int.Parse(TextBox_Time.Text));
                                            }
                                        }
                                        else if (RadioButton3.Checked==true)
                                        {
                                            try
                                            {
                                                for (int ik = root_loadVideoPointerInfo.data.questionPoint.Count - 1; ik >= 0; ik--)
                                                {
                                                    Post_WK_Progress(V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + root_loadVideoPointerInfo.data.questionPoint[ik].timeSec + ", " + videoSec + ", " + yh + root_loadVideoPointerInfo.data.questionPoint[ik].timers + yh + "]"), learningTokenId);
                                                    lessonPopupExam_Small(ID, root_loadVideoPointerInfo.data.questionPoint[ik].timers, ccCourseId, recruitId, lessonVideoId);
                                                    Delay(int.Parse(TextBox_atime.Text));    //延时1000ms
                                                }
                                                //Console.WriteLine(Study_Times);
                                                Staus = Post_WK_Progress(V8Test("[" + WK_recruitId + ", " + ID + ", " + lessonVideoId + ", " + videoId + ", 1, " + yh + "0" + yh + ", " + videoSec + ", " + videoSec + ", " + yh + Study_Times + yh + "]"), learningTokenId);
                                                listView1.Items[m].SubItems.Add(Staus);
                                                Delay(int.Parse(TextBox_Time.Text));    //延时3000ms
                                            }
                                            catch (Exception)
                                            {

                                                Console.WriteLine("此节课没有题...");
                                            }
                                        }
                                        else
                                        {
                                            Delay(int.Parse(videoSec + "000") + 1000);
                                        }
                                        //延时该节课的时长 让Timer1执行完毕
                                        //Console.WriteLine(Study_Times);
                                        // Staus = Post_WK_Progress(V8Test("[" + WK_recruitId + ", " + ID + ", " + lessonVideoId + ", " + videoId + ", 1, " + yh + "0" + yh + ", " + videoSec + ", " + videoSec + ", " + yh + Study_Times + yh + "]"), learningTokenId);
                                        //listView1.Items[m].SubItems.Add("初始化ing...");
                                        // Delay(int.Parse(TextBox_Time.Text));    //延时3000ms
                                    }
                                    catch (Exception)
                                    {

                                        Console.WriteLine("此节课没有题...");
                                    }
                                    Timer_totalStudyTime.Stop();
                                    Timer_watchPointPost.Stop();
                                    if (RadioButton1.Checked==true)
                                    {
                                        Timer2.Stop();
                                    }
                                   // Timer2.Stop();
                                }
                                ++m;
                            }
                        }
                        else
                        {
                            ID = root_WK.data.videoChapterDtos[i].videoLessons[j].id.ToString();
                            videoId = root_WK.data.videoChapterDtos[i].videoLessons[j].videoId.ToString();
                            videoSec = root_WK.data.videoChapterDtos[i].videoLessons[j].videoSec.ToString();
                            if (listView1.Items[m].Checked == true)
                            {
                                ccCourseId = root_WK.data.courseId.ToString();
                                recruitId = root_WK.data.recruitId.ToString();
                                chapterId = root_WK.data.videoChapterDtos[i].videoLessons[j].chapterId.ToString();
                                learningTokenId = Post_WK_learningTokenId(ccCourseId, chapterId, ID, recruitId, videoId);
                                Study_Times = sec_to_hms(long.Parse(videoSec) - random.Next(1, 15));
                                Writes_item_task(ID, recruitId, ccCourseId);
                                Root_loadVideoPointerInfo root_loadVideoPointerInfo = JsonConvert.DeserializeObject<Root_loadVideoPointerInfo>(WK_loadVideoPointerInfo);
                                try
                                {
                                    for (int ik = root_loadVideoPointerInfo.data.questionPoint.Count - 1; ik >= 0; ik--)
                                    {
                                        lessonPopupExam(ID, root_loadVideoPointerInfo.data.questionPoint[ik].timers, ccCourseId, recruitId);
                                    }
                                    //Console.WriteLine(Study_Times);
                                    listView1.Items[m].SubItems.Add("初始化ing...");
                                    listView1.Items[m].SubItems.Add("初始化ing...");
                                    Timer1000 = 0;
                                    totalStudyTime = 500;
                                    watchPointPost = "0,1";
                                    if (RadioButton2.Checked == true)   //极速模式
                                    {
                                        Timer_totalStudyTime.Interval = 4990 / int.Parse(TextBox_Time.Text);
                                        Timer_watchPointPost.Interval = 1990 / int.Parse(TextBox_Time.Text);
                                        Timer1.Interval = 1000 / int.Parse(TextBox_Time.Text);
                                    }
                                    else if(RadioButton1.Checked==true)
                                    {
                                        Timer_totalStudyTime.Interval = 4990;
                                        Timer_watchPointPost.Interval = 1990;
                                        Timer1.Interval = 1000;
                                        Timer1.Start();
                                    }
                                    Timer_totalStudyTime.Start();
                                    Timer_watchPointPost.Start();
                                    //Timer1.Start();
                                    WK_m = m;
                                    if (RadioButton2.Checked == true) //极速模式
                                    {
                                        for (int iwk = 1; iwk <= int.Parse(videoSec); iwk++)
                                        {
                                            listView1.Items[WK_m].SubItems[2].Text = Timer1000.ToString() + "/" + videoSec;
                                            Timer1000++;
                                            if (Timer1000 % 180 == 0) //是180的整数倍就可以了
                                            {
                                                saveCacheIntervalTime = V8Test("[" + recruitId + ", " + chapterId + ", " + ccCourseId + ", " + ID + ", " + yh + sec_to_hms(Timer1000) + yh + ", " + totalStudyTime + ", " + videoId + ", 0, " + Timer1000 + "]");
                                                //saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + root_loadVideoPointerInfo.data.questionPoint[ik].timeSec + ", " + videoSec + ", " + yh + root_loadVideoPointerInfo.data.questionPoint[ik].timers + yh + "]");
                                                Staus = Post_WK_saveCacheIntervalTime(saveCacheIntervalTime, learningTokenId);
                                                listView1.Items[WK_m].SubItems[1].Text = Staus + ": " + Timer1000.ToString();
                                                watchPointPost = "0,1," + (totalStudyTime / 5 + 2).ToString();
                                            }
                                            else if (Timer1000 % 300 == 0)
                                            {
                                                saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + Timer1000 + ", " + videoSec + ", " + yh + sec_to_hms(Timer1000) + yh + "]");
                                                Staus = Post_WK_Progress(saveDatabaseIntervalTime, learningTokenId);
                                                listView1.Items[WK_m].SubItems[1].Text = Staus + ": " + Timer1000.ToString();
                                            }
                                            else if (Timer1000 == int.Parse(videoSec)) //最终提交100%
                                            {
                                                saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + Timer1000 + ", " + videoSec + ", " + yh + Study_Times + yh + "]");
                                                Staus = Post_WK_Progress(saveDatabaseIntervalTime, learningTokenId);
                                                listView1.Items[WK_m].SubItems[1].Text = Staus + ": Success!";
                                                listView1.Items[WK_m].SubItems[2].Text = Timer1000.ToString() + "/" + videoSec;
                                                Timer1000 = 0;
                                            }
                                            Delay(1000 / int.Parse(TextBox_Time.Text));
                                        }
                                       // Delay(int.Parse(videoSec + "000") / int.Parse(TextBox_Time.Text)+1000);
                                    }
                                    else if (RadioButton3.Checked == true)
                                    {
                                        try
                                        {
                                            for (int ik = root_loadVideoPointerInfo.data.questionPoint.Count - 1; ik >= 0; ik--)
                                            {
                                                Post_WK_Progress(V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + root_loadVideoPointerInfo.data.questionPoint[ik].timeSec + ", " + videoSec + ", " + yh + root_loadVideoPointerInfo.data.questionPoint[ik].timers + yh + "]"), learningTokenId);
                                                lessonPopupExam(ID, root_loadVideoPointerInfo.data.questionPoint[ik].timers, ccCourseId, recruitId);
                                                Delay(int.Parse(TextBox_atime.Text));    //延时1000ms
                                            }
                                            //Console.WriteLine(Study_Times);
                                            Staus = Post_WK_Progress(V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + videoSec + ", " + videoSec + ", " + yh + Study_Times + yh + "]"), learningTokenId);
                                            listView1.Items[m].SubItems.Add(Staus);
                                            Delay(int.Parse(TextBox_Time.Text));    //延时3000ms
                                        }
                                        catch (Exception)
                                        {

                                            Console.WriteLine("此节课没有题...");
                                        }
                                    }
                                    else
                                    {
                                        Delay(int.Parse(videoSec + "000") + 1000);  //延时该节课的时长 让Timer1执行完毕
                                    }
                                   // Staus = Post_WK_Progress(V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + videoSec + ", " + videoSec + ", " + yh + Study_Times + yh + "]"), learningTokenId);
                                   // listView1.Items[m].SubItems.Add("初始化ing...");
                                   // Delay(int.Parse(TextBox_Time.Text));    //延时3000ms
                                }
                                catch (Exception)
                                {

                                    Console.WriteLine("此节课没有题...");
                                }
                                Timer_totalStudyTime.Stop();
                                Timer_watchPointPost.Stop();
                                if (RadioButton1.Checked==true)
                                {
                                    Timer1.Stop();
                                }
                               // Timer1.Stop();
                            }
                            ++m;
                        }
                    }
                }
                MessageBox.Show("提交完成,请返回客户端查看!", "Tips:", MessageBoxButtons.OK);
            }
        }
        public void Writes_item_task(string lessonId, string recruitId, string courseId)
        {
            loadVideoPointerInfo( lessonId, recruitId, courseId);
        }
        public void loadVideoPointerInfo(string lessonId,string recruitId,string courseId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/popupAnswer/loadVideoPointerInfo ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
                Cookie = SESSION_Temp,
                Postdata = "lessonId="+ lessonId + "&recruitId="+ recruitId + "&courseId="+ courseId + "&uuid="+uuid//Post要发送的数据
            };
            item.Header.Add("Origin", "https://studyh5.zhihuishu.com");
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            WK_loadVideoPointerInfo = html;
            // Root_loadVideoPointerInfo root_loadVideoPointerInfo = JsonConvert.DeserializeObject<Root_loadVideoPointerInfo>(WK_loadVideoPointerInfo);
        }
        public void lessonPopupExam(string lessonId, string task_time,string ccCourseId, string recruitId)  //task_time需要进行URL编码
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/popupAnswer/lessonPopupExam ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
                Cookie = SESSION_Temp,
                Postdata = "lessonId="+ lessonId + "&time="+ System.Web.HttpUtility.UrlEncode(task_time) + "&uuid="+uuid//Post要发送的数据
            };
            item.Header.Add("Origin", "https://studyh5.zhihuishu.com");
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            Root_lessonPopupExam root_lessonPopupExam = JsonConvert.DeserializeObject<Root_lessonPopupExam>(html);
            if (root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionType.inputType== "radio")  //判断是不是单选模式
            {
                string ananswer = "";
                for (int i = 0; i < root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions.Count; i++)
                {
                    if (root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].result=="1")
                    {
                        //Console.WriteLine(root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].id);  //输出正确答案的ID
                        ananswer = root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].id.ToString();
                    }
                }
               // Console.WriteLine(root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionId + ananswer);  //问题的ID+输出正确答案的ID
                saveLessonPopupExamSaveAnswer( ccCourseId,  recruitId, root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionId.ToString(), ananswer,  lessonId);
            }
            else if (root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionType.inputType == "checkbox")
            {
                string ananswer="";
                for (int i = 0; i < root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions.Count; i++)
                {
                    if (root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].result == "1")
                    {
                        if (ananswer == "")
                        {
                            ananswer = root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].id.ToString();
                        }
                        else
                        {
                            ananswer = ananswer + "," + root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].id.ToString();
                        }
                    }
                }
               // Console.WriteLine(ananswer);  //输出正确答案的ID
                saveLessonPopupExamSaveAnswer(ccCourseId, recruitId, root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionId.ToString(), ananswer, lessonId);
            }
            else
            {
                MessageBox.Show("此项不是选择模式-未知错误"+ lessonId);
            }
        }
        public void saveLessonPopupExamSaveAnswer(string ccCourseId, string recruitId,string testQuestionId, string answer, string lessonId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/popupAnswer/saveLessonPopupExamSaveAnswer ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
                Cookie = SESSION_Temp,
                Postdata = "courseId="+ ccCourseId + "&recruitId="+ recruitId + "&testQuestionId="+ testQuestionId + "&isCurrent=1&lessonId="+ lessonId + "&answer="+ System.Web.HttpUtility.UrlEncode(answer) + "&testType=0&uuid="+uuid//Post要发送的数据
            };
            item.Header.Add("Origin", "https://studyh5.zhihuishu.com");
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            string Staus = TextGainCenter(yh + "message" + yh + ":" + yh, yh, html);
            Console.WriteLine(Staus);
        }
        public void Writes_item_task_Small(string lessonId, string recruitId, string courseId,string lessonVideoId)
        {
            loadVideoPointerInfo_Small(lessonId, recruitId, courseId, lessonVideoId);
        }
        public void loadVideoPointerInfo_Small(string lessonId, string recruitId, string courseId,string lessonVideoId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/popupAnswer/loadVideoPointerInfo ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
                Cookie = SESSION_Temp,
                Postdata = "lessonId=" + lessonId + "&lessonVideoId="+ lessonVideoId + "&recruitId=" + recruitId + "&courseId=" + courseId + "&uuid=" + uuid//Post要发送的数据
            };
            item.Header.Add("Origin", "https://studyh5.zhihuishu.com");
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            WK_loadVideoPointerInfo = html;
            // Root_loadVideoPointerInfo root_loadVideoPointerInfo = JsonConvert.DeserializeObject<Root_loadVideoPointerInfo>(WK_loadVideoPointerInfo);
        }
        public void lessonPopupExam_Small(string lessonId, string task_time, string ccCourseId, string recruitId,string lessonVideoId)  //task_time需要进行URL编码
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/popupAnswer/lessonPopupExam ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
                Cookie = SESSION_Temp,
                Postdata = "lessonId=" + lessonId + "&lessonVideoId=" + lessonVideoId + "&time=" + System.Web.HttpUtility.UrlEncode(task_time) + "&uuid=" + uuid//Post要发送的数据
            };
            item.Header.Add("Origin", "https://studyh5.zhihuishu.com");
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            Root_lessonPopupExam root_lessonPopupExam = JsonConvert.DeserializeObject<Root_lessonPopupExam>(html);
            if (root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionType.inputType == "radio")  //判断是不是单选模式
            {
                string ananswer = "";
                for (int i = 0; i < root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions.Count; i++)
                {
                    if (root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].result == "1")
                    {
                        //Console.WriteLine(root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].id);  //输出正确答案的ID
                        ananswer = root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].id.ToString();
                    }
                }
                //Console.WriteLine(root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionId + ananswer);  //问题的ID+输出正确答案的ID
                saveLessonPopupExamSaveAnswer_Small(ccCourseId, recruitId, root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionId.ToString(), ananswer, lessonId, lessonVideoId);
            }
            else if (root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionType.inputType == "checkbox")
            {
                string ananswer = "";
                for (int i = 0; i < root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions.Count; i++)
                {
                    if (root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].result == "1")
                    {
                        if (ananswer == "")
                        {
                            ananswer = root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].id.ToString();
                        }
                        else
                        {
                            ananswer = ananswer + "," + root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionOptions[i].id.ToString();
                        }
                    }
                }
                //Console.WriteLine(ananswer);  //输出正确答案的ID
                saveLessonPopupExamSaveAnswer_Small(ccCourseId, recruitId, root_lessonPopupExam.data.lessonTestQuestionUseInterfaceDtos[0].testQuestion.questionId.ToString(), ananswer, lessonId, lessonVideoId);
            }
            else
            {
                MessageBox.Show("此项不是选择模式-未知错误" + lessonId);
            }
        }
        public void saveLessonPopupExamSaveAnswer_Small(string ccCourseId, string recruitId, string testQuestionId, string answer, string lessonId,string lessonVideoId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/popupAnswer/saveLessonPopupExamSaveAnswer ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
                Cookie = SESSION_Temp,
                Postdata = "courseId=" + ccCourseId + "&recruitId=" + recruitId + "&testQuestionId=" + testQuestionId + "&isCurrent=1&lessonId=" + lessonId + "&lessonVideoId=" + lessonVideoId + "&answer=" + System.Web.HttpUtility.UrlEncode(answer) + "&testType=0&uuid=" + uuid//Post要发送的数据
            };
            item.Header.Add("Origin", "https://studyh5.zhihuishu.com");
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            string Staus = TextGainCenter(yh + "message" + yh + ":" + yh, yh, html);
            Console.WriteLine(Staus);
        }
        public string Post_WK_Progress(string ev,string learningTokenId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/learning/saveDatabaseIntervalTime ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host= "studyservice.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer= "https://studyh5.zhihuishu.com/videoStudy.html",
                Cookie= SESSION_Temp,
                Postdata = "ev="+ev+"&learningTokenId="+ learningTokenId + "&uuid="+uuid,//Post要发送的数据
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            string Staus = TextGainCenter(yh + "message" + yh + ":" + yh, yh, html);
            return Staus;
        }
        public string Post_WK_learningTokenId(string ccCourseId,string chapterId,string lessonId,string recruitId,string videoId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/learning/prelearningNote ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
                Cookie = SESSION_Temp,
                Postdata = "ccCourseId="+ ccCourseId + "&chapterId="+ chapterId + "&isApply=1&lessonId="+ lessonId + "&recruitId="+ recruitId + "&videoId="+ videoId,//Post要发送的数据
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            string learningTokenId = TextGainCenter(yh+"id"+yh+":",",",html);
            byte[] b = System.Text.Encoding.Default.GetBytes(learningTokenId);
            //转成 Base64 形式的 System.String  
            learningTokenId = Convert.ToBase64String(b);
           // Console.WriteLine(learningTokenId);
            return learningTokenId;
        }
        public string Post_WK_learningTokenId_2(string ccCourseId, string chapterId, string lessonId,string lessonVideoId, string recruitId, string videoId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/learning/prelearningNote ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
                Cookie = SESSION_Temp,
                Postdata = "ccCourseId=" + ccCourseId + "&chapterId=" + chapterId + "&isApply=1&lessonId=" + lessonId + "&lessonVideoId="+ lessonVideoId + "&recruitId=" + recruitId + "&videoId=" + videoId,//Post要发送的数据
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            string learningTokenId = TextGainCenter(yh + "id" + yh + ":", ",", html);
            byte[] b = System.Text.Encoding.Default.GetBytes(learningTokenId);
            //转成 Base64 形式的 System.String  
            learningTokenId = Convert.ToBase64String(b);
            // Console.WriteLine(learningTokenId);
            return learningTokenId;
        }
        public string Post_JMK_Progress(string paramSecret,string liveId,string courseId,string recruitId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://im.zhihuishu.com//live/setWatchHistorySecret?callback=jsonpCallBack&paramSecret="+ paramSecret + "&jsonpCallBack=jsonpCallBack",//URL     必需项    
                Method = "Get",
                Host = "im.zhihuishu.com",
                Referer = " https://lc.zhihuishu.com/live/vod_room.html?liveId="+ liveId + "&courseId="+ courseId + "&recruitId="+ recruitId,
                Cookie = SESSION
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            string Staues = TextGainCenter("jsonpCallBack(",")",html);
            return Staues;
        }
        public string Post_WK_saveCacheIntervalTime(string ev, string learningTokenId)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://studyservice.zhihuishu.com/learning/saveCacheIntervalTime ",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Host = "studyservice.zhihuishu.com",
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer = "https://studyh5.zhihuishu.com/videoStudy.html",
                Cookie = SESSION_Temp,
                Postdata = "watchPoint=" + watchPointPost + "&ev=" + ev + "&learningTokenId=" + learningTokenId + "&uuid=" + uuid,    //也不知道watchPointPost 需不需要编码
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            string Staus = TextGainCenter(yh + "message" + yh + ":" + yh, yh, html);
            Console.WriteLine(watchPointPost);
            Console.WriteLine(totalStudyTime);
            return Staus;
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton2.Checked==true)
            {
                label2.Visible = true;
                TextBox_Time.Visible = true;
                label2.Text = "请填写提交进度的倍速:";
                TextBox_Time.Text = "10";
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked == true)
            {
                label2.Visible = false;
                TextBox_Time.Visible = false;
            }
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton3.Checked == true)
            {
                label2.Visible = true;
                TextBox_Time.Visible = true;
                label2.Text = "每节课提交速率(ms):";
                TextBox_Time.Text = "3000";
            }
        }

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本程序仅供学习交流,严禁用于商业用途\r\n软件作者:lzss");
        }

        private void 软件主页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://lzonel.cn");
        }

        private void 软件主页GitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/lzonel/Wits_lzss");
        }

        public string V8Test(string para)
        {
            string js_str = TextBox1.Text;
            string result = JsHelper.V8Method(js_str, "Z", para);
            return result;
        }
        public string V8Test_JMK(string paramSecret)
        {
            string js_str = TextBox2.Text;
            string result = JsHelper.V8Method(js_str, "Z", paramSecret);
            return result;
        }
        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {
                Application.DoEvents();//可执行某无聊的操作
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked==true)
            {
                for (int i = 0; i < Checkets_Nums; i++)
                {
                    listView1.Items[i].Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < Checkets_Nums; i++)
                {
                    listView1.Items[i].Checked = false;
                }
            }
        }

        //
        //                       _oo0oo_
        //                      o8888888o
        //                      88" . "88
        //                      (| -_- |)
        //                      0\  =  /0
        //                    ___/`---'\___
        //                  .' \\|     |// '.
        //                 / \\|||  :  |||// \
        //                / _||||| -:- |||||- \
        //               |   | \\\  -  /// |   |
        //               | \_|  ''\---/''  |_/ |
        //               \  .-\__  '-'  ___/-. /
        //             ___'. .'  /--.--\  `. .'___
        //          ."" '<  `.___\_<|>_/___.' >' "".
        //         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
        //         \  \ `_.   \_ __\ /__ _/   .-` /  /
        //     =====`-.____`.___ \_____/___.-`___.-'=====
        //                       `=---='
        //
        //
        //  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //
        //               佛祖保佑         永无BUG
        //        佛曰:  
        //              写字楼里写字间，写字间里程序员；  
        //              程序人员写程序，又拿程序换酒钱。  
        //              酒醒只在网上坐，酒醉还来网下眠；  
        //              酒醉酒醒日复日，网上网下年复年。  
        //              但愿老死电脑间，不愿鞠躬老板前；  
        //              奔驰宝马贵者趣，公交自行程序员。  
        //              别人笑我忒疯癫，我笑自己命太贱；  
        //              不见满街漂亮妹，哪个归得程序员？
        //-----------------------------------------------------------------------------------------------------------------------

        //课程 JSON 实体类
        public class CourseOpenDtos_queryShareCourseInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public int courseId { get; set; }
            /// <summary>
            /// C君带你玩编程
            /// </summary>
            public string courseName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string courseImg { get; set; }
            /// <summary>
            /// 方娇莉
            /// </summary>
            public string teacherName { get; set; }
            /// <summary>
            /// 二级等考公共基础知识辅导及考点解析
            /// </summary>
            public string lessonName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string lessonNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string progress { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int recruitId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string lessonId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string courseCount { get; set; }
            /// <summary>
            /// 昆明理工大学
            /// </summary>
            public string schoolName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string secret { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long courseStartTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long courseEndTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int courseStatus { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int courseType { get; set; }
        }

        public class Result_queryShareCourseInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public int totalCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<CourseOpenDtos_queryShareCourseInfo> courseOpenDtos { get; set; }
        }

        public class Root_queryShareCourseInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public Result_queryShareCourseInfo result { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string message { get; set; }
        }

        //-----------------------------------------------------------------------------------------------------------------------
        //课程列表 JSON 实体类
        public class VideoSmallLessons
        {
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 公共关系学学科属性
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int orderNumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int lessonId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int videoId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int videoSec { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isStudiedLesson { get; set; }
        }
        public class VideoLessons_WK
        {
            /// <summary>
            /// 
            /// </summary>
            public List<VideoSmallLessons> videoSmallLessons { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// C语言的历史与特点
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int orderNumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int chapterId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int videoId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int videoSec { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isDeteled { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int ishaveChildrenLesson { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isStudiedLesson { get; set; }
        }

        public class ExamPaper_WK
        {
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
        }

        public class Exam_WK
        {
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isRecruit { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string limitTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string parentId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long startDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public ExamPaper_WK examPaper { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reviewEndTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string lateSetting { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string lateScore { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string totalScore { get; set; }
        }

        private void Timer_watchPointPost_Tick(object sender, EventArgs e)
        {
            int t = totalStudyTime / 5 + 2;
            watchPointPost += "," + t.ToString();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            listView1.Items[WK_m].SubItems[2].Text = Timer1000.ToString() + "/" + videoSec;
            Timer1000++;
            if (Timer1000 % 180==0) //是180的整数倍就可以了
            {
                saveCacheIntervalTime = V8Test("[" +recruitId +", " +chapterId+ ", " + ccCourseId + ", " + ID+ ", "+yh + sec_to_hms(Timer1000) +yh+", "+ totalStudyTime + ", " + videoId + ", 0, "+ Timer1000+ "]");
                //saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + root_loadVideoPointerInfo.data.questionPoint[ik].timeSec + ", " + videoSec + ", " + yh + root_loadVideoPointerInfo.data.questionPoint[ik].timers + yh + "]");
                Staus = Post_WK_saveCacheIntervalTime(saveCacheIntervalTime, learningTokenId);
                listView1.Items[WK_m].SubItems[1].Text = Staus + ": " + Timer1000.ToString();
                watchPointPost = "0,1,"+(totalStudyTime / 5 + 2).ToString();
            }
            else if (Timer1000 % 300==0)
            {
                saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + Timer1000 + ", " + videoSec + ", " + yh + sec_to_hms(Timer1000) + yh + "]");
                Staus = Post_WK_Progress(saveDatabaseIntervalTime, learningTokenId);
                listView1.Items[WK_m].SubItems[1].Text = Staus + ": " + Timer1000.ToString();
            }
            else if(Timer1000 == int.Parse(videoSec) ) //最终提交100%
            {
                saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + Timer1000 + ", " + videoSec + ", " + yh + Study_Times + yh + "]");
                Staus = Post_WK_Progress(saveDatabaseIntervalTime, learningTokenId);
                listView1.Items[WK_m].SubItems[1].Text = Staus + ": Success!";
                Timer1000 = 0;
            }
        }
        private void Timer2_Tick(object sender, EventArgs e)
        {
            listView1.Items[WK_m].SubItems[2].Text = Timer1000.ToString() + "/" + videoSec;
            Timer1000++;
            if (Timer1000 % 180 == 0) //是180的整数倍就可以了
            {
                saveCacheIntervalTime = V8Test("[" + recruitId + ", " + chapterId + ", " + ccCourseId + ", " + SM_lessonId + ", " + yh + sec_to_hms(Timer1000) + yh + ", " + totalStudyTime + ", " + videoId + ", "+ lessonVideoId + ", " + Timer1000 + "]");
                //saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + root_loadVideoPointerInfo.data.questionPoint[ik].timeSec + ", " + videoSec + ", " + yh + root_loadVideoPointerInfo.data.questionPoint[ik].timers + yh + "]");
                Staus = Post_WK_saveCacheIntervalTime(saveCacheIntervalTime, learningTokenId);
                listView1.Items[WK_m].SubItems[1].Text = Staus + ": " + Timer1000.ToString();
                watchPointPost = "0,1," + (totalStudyTime / 5 + 2).ToString();
            }
            else if (Timer1000 % 300 == 0)
            {
                saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + Timer1000 + ", " + videoSec + ", " + yh + sec_to_hms(Timer1000) + yh + "]");
                Staus = Post_WK_Progress(saveDatabaseIntervalTime, learningTokenId);
                listView1.Items[WK_m].SubItems[1].Text = Staus + ": " + Timer1000.ToString();
            }
            else if (Timer1000 == int.Parse(videoSec)) //最终提交100%
            {
                saveDatabaseIntervalTime = V8Test("[" + WK_recruitId + ", " + ID + ", 0, " + videoId + ", 1, " + yh + "0" + yh + ", " + Timer1000 + ", " + videoSec + ", " + yh + Study_Times + yh + "]");
                Staus = Post_WK_Progress(saveDatabaseIntervalTime, learningTokenId);
                listView1.Items[WK_m].SubItems[1].Text = Staus + ": Success!";
                Timer1000 = 0;
            }
        }
        public class StudentExamDto_WK
        {
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string correctProgress { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string createTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string updateTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string courseId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string classId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string userId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isDelete { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string oneHundredPont { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string gradePont { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string levelPont { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string passPont { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string isUpdate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string remainingTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int state { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string achieveCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string achieve { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string courseSourceType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Exam_WK exam { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string remark { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string publishDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string endDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string score { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string examId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string examStartFlag { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string parentId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string examUrl { get; set; }
        }

        private void Timer_totalStudyTime_Tick(object sender, EventArgs e)
        {
            totalStudyTime += 5 * 1;  //1是代表当前播放速率
        }

        public class VideoChapterDtos_WK
        {
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 认识C语言
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int orderNumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int learningDay { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int learningLessonStatus { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isPassBadge { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int limitChecked { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<VideoLessons_WK> videoLessons { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isPass { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public StudentExamDto_WK studentExamDto { get; set; }
        }

        public class Data_WK
        {
            /// <summary>
            /// 
            /// </summary>
            public int recruitId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int courseId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<VideoChapterDtos_WK> videoChapterDtos { get; set; }
        }

        public class Root_WK
        {
            /// <summary>
            /// 
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 请求成功
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Data_WK data { get; set; }
        }

        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //获取章节视频播放中的题
        public class QuestionPoint_loadVideoPointerInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public string timers { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int timeSec { get; set; }
        }

        public class Data_loadVideoPointerInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public string videoThemeDtos { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> popupPictureDtos { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<QuestionPoint_loadVideoPointerInfo> questionPoint { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string knowledgeCardDtos { get; set; }
        }

        public class Root_loadVideoPointerInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 请求成功
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Data_loadVideoPointerInfo data { get; set; }
        }
        //-----------------------------------------------------------------------------------------------------------------------
        //获取章节播放中的题
        public class QuestionOptions_lessonPopupExam
        {
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 当循环条件不成立的时候，循环会结束
            /// </summary>
            public string content { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int sort { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string result { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string acount { get; set; }
        }

        public class QuestionType_lessonPopupExam
        {
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 单选题
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string inputType { get; set; }
        }

        public class TestQuestion_lessonPopupExam
        {
            /// <summary>
            /// 
            /// </summary>
            public int inputType { get; set; }
            /// <summary>
            /// 在C语言中，以下有关循环的说法哪个是错误的？
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int questionId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<QuestionOptions_lessonPopupExam> questionOptions { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public QuestionType_lessonPopupExam questionType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> videoInfo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> imgUrls { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string txtInfo { get; set; }
        }

        public class ZAnswer_lessonPopupExam
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string answer { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isDelete { get; set; }
        }

        public class LessonTestQuestionUseInterfaceDtos_lessonPopupExam
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public TestQuestion_lessonPopupExam testQuestion { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string analysis { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string datas { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string data { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public ZAnswer_lessonPopupExam zAnswer { get; set; }
        }

        public class Data
        {
            /// <summary>
            /// 
            /// </summary>
            public List<LessonTestQuestionUseInterfaceDtos_lessonPopupExam> lessonTestQuestionUseInterfaceDtos { get; set; }
        }

        public class Root_lessonPopupExam
        {
            /// <summary>
            /// 
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 请求成功
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Data data { get; set; }
        }


        /**
*　　　　　　　　┏┓　　　┏┓+ +
*　　　　　　　┏┛┻━━━┛┻┓ + +
*　　　　　　　┃　　　　　　　┃ 　
*　　　　　　　┃　　　━　　　┃ ++ + + +
*　　　　　　 ████━████ ┃+
*　　　　　　　┃　　　　　　　┃ +
*　　　　　　　┃　　　┻　　　┃
*　　　　　　　┃　　　　　　　┃ + +
*　　　　　　　┗━┓　　　┏━┛
*　　　　　　　　　┃　　　┃　　　　　　　　　　　
*　　　　　　　　　┃　　　┃ + + + +
*　　　　　　　　　┃　　　┃　　　　Code is far away from bug with the animal protecting　　　　　　　
*　　　　　　　　　┃　　　┃ + 　　　　神兽保佑,代码无bug　　
*　　　　　　　　　┃　　　┃
*　　　　　　　　　┃　　　┃　　+　　　　　　　　　
*　　　　　　　　　┃　 　　┗━━━┓ + +
*　　　　　　　　　┃ 　　　　　　　┣┓
*　　　　　　　　　┃ 　　　　　　　┏┛
*　　　　　　　　　┗┓┓┏━┳┓┏┛ + + + +
*　　　　　　　　　　┃┫┫　┃┫┫
*　　　　　　　　　　┗┻┛　┗┻┛+ + + +
*/
    }
}
