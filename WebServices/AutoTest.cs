//using AutoItX3Lib;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebData.Implementation;
using WebData.Models;

namespace WebServices
{
    public class AutoTest
    {
        private IWebDriver webDriver;

        public void Init_Browser()
        {
            webDriver = new ChromeDriver();
            webDriver.Manage().Window.Maximize();
        }

        public string Title
        {
            get { return webDriver.Title; }
        }

        public void Goto(string url)
        {
            webDriver.Url = url;
        }

        public void Close()
        {
            webDriver.Quit();
        }

        public IWebDriver getDriver
        {
            get { return webDriver; }
        }
    }

    public class Common
    {
        public const string Url = "http://localhost:5000/Course";
        private readonly ICourseService _courseService;
        private readonly IChapterService _chapterService;
        quanlykhoahocContext ql = new quanlykhoahocContext();
        public Common()
        {
            _courseService = new CourseService(ql);
            _chapterService = new ChapterService(ql);
        }
        public List<Courses> GetListCourse()
        {

            return _courseService.GetAll().ToList();
        }
        public List<Chapter> GetListChapter()
        {
            return _chapterService.GetAll().ToList();
        }
    }

    internal class FunctionEvent
    {
        private AutoTest brow = new AutoTest();
        private IWebDriver driver;

        [SetUp]
        public void start_Browser()
        {
            if (driver == null)
            {
                brow.Init_Browser();
            }
        }

        [Test]
        public void CInsertEvent()
        {
            Common common = new Common();
            //thêm giá trị
            // tìm đối tượng theo ID
            int count = 1;
            foreach (var item in common.GetListCourse())
            {
                for (int j = 0; j < 5; j++)
                {
                    IWebElement clickCourse = driver.FindElement(By.XPath("//*[@id='detail" + item.Id + "']"));
                    clickCourse.Clear();
                    clickCourse.Click();


                    IWebElement clickChapter = driver.FindElement(By.XPath("//*[@id='addChapter']"));
                    clickChapter.Clear();
                    clickChapter.Click();


                    IWebElement nameChapter = driver.FindElement(By.XPath("//*[@id='NameChapter']"));
                    System.Threading.Thread.Sleep(500);
                    nameChapter.SendKeys("Tổng quan " + count);


                    IWebElement clickAddChapter = driver.FindElement(By.XPath("//*[@id='btnSubmit']"));
                    clickAddChapter.Clear();
                    clickAddChapter.Click();
                    System.Threading.Thread.Sleep(700);

                    
                    var idchapter = common.GetListChapter().Max(m => m.Id);
                    var countLesson = 1;
                    for (int i = 0; i < 5; i++)
                    {
                        IWebElement clickAddLesson = driver.FindElement(By.XPath("//*[@id='chapter" + idchapter + "']"));
                        clickCourse.Clear();
                        clickCourse.Click();


                        IWebElement nameCourseLesson = driver.FindElement(By.XPath("//*[@id='Name']"));
                        System.Threading.Thread.Sleep(500);
                        nameChapter.SendKeys("Khái niệm " + countLesson);

                        //// khởi tạo đối tượng autoIT để dùng cho C# -> nhờ nó send key click chuột dùm cái ở ngoài web browser
                        //AutoItX3 autoIT = new AutoItX3();
                        //// đưa title của cửa sổ File upload vào chuỗi. 
                        //// Cửa sổ hiện ra có thể có tiêu đề là File Upload hoặc Tải lên một tập tin
                        //// lấy ra cửa sổ active có tiêu đề như dưới
                        //autoIT.WinActivate("Open");
                        // file data nằm trong thư mục debug
                        // gửi link vào ô đường dẫn
                        //autoIT.Send(AppDomain.CurrentDomain.BaseDirectory + "logo2.jpg");
                        //Thread.Sleep(TimeSpan.FromSeconds(1));
                        // gửi phím enter sau khi truyền link vào
                        //autoIT.Send("{ENTER}");
                        System.Threading.Thread.Sleep(2000);

                        // đổi frame
                        driver.SwitchTo().Frame(0);

                        //// khởi tạo đối tượng autoIT để dùng cho C# -> nhờ nó send key click chuột dùm cái ở ngoài web browser
                        //AutoItX3 autoITSlide = new AutoItX3();
                        //// đưa title của cửa sổ File upload vào chuỗi. 
                        //// Cửa sổ hiện ra có thể có tiêu đề là File Upload hoặc Tải lên một tập tin
                        //// lấy ra cửa sổ active có tiêu đề như dưới
                        //autoITSlide.WinActivate("Open");
                        // file data nằm trong thư mục debug
                        // gửi link vào ô đường dẫn
                        //autoITSlide.Send(AppDomain.CurrentDomain.BaseDirectory + "logo2.jpg");
                        //Thread.Sleep(TimeSpan.FromSeconds(1));
                        // gửi phím enter sau khi truyền link vào
                        //autoITSlide.Send("{ENTER}");
                        System.Threading.Thread.Sleep(2000);

                        // đổi frame
                        driver.SwitchTo().Frame(0);

                        IWebElement clickSaveLesson = driver.FindElement(By.XPath("//*[@id='btnSubmit']"));
                        clickSaveLesson.Clear();
                        clickSaveLesson.Click();
                        countLesson++;
                    }
                    count++;
                }                               
            }            
        }
    }
}