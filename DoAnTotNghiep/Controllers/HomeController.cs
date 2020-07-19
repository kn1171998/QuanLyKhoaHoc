using AutoMapper;
using DoAnTotNghiep.Common;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICourseCategoryService _courseCategoryService;
        private readonly ICourseService _courseService;
        private readonly IChapterService _chapterService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IDiscountService _discountService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IUserService userService
            , IMapper mapper
            , ICourseCategoryService courseCategoryService
            , ICourseService courseService
            , IChapterService chapterService
            , IOrderService orderService
            , IDiscountService discountService
            , IOrderDetailService orderDetailService
            , IHostingEnvironment hostingEnvironment)
        {
            _userService = userService;
            _mapper = mapper;
            _courseCategoryService = courseCategoryService;
            _courseService = courseService;
            _chapterService = chapterService;
            _orderService = orderService;
            _discountService = discountService;
            _orderDetailService = orderDetailService;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var context = _courseService.GetContext();
            ViewBag.listPromotion = await (from c in context.Courses
                                           join u in context.Users
                                           on c.UserId equals u.Id
                                           where c.Status == true
                                           orderby (c.PromotionPrice ?? 0 - c.Price) descending
                                           select new CourseVM
                                           {
                                               Id = c.Id,
                                               Name = c.Name,
                                               Image = c.Image,
                                               FullName = u.FullName,
                                               PromotionPrice = c.PromotionPrice,
                                               Price = c.Price
                                           }).Take(6).ToListAsync();
            var lstUser = context.Users.Where(m => m.Status == true);
            ViewBag.studyCount = lstUser.Where(m => m.TypeUser == TypeUser.User).Count();
            ViewBag.teacherCount = lstUser.Where(m => m.TypeUser == TypeUser.Teacher).Count();
            ViewBag.courseCount = _courseService.CountCondition(m => m.Status == true);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error404()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region ModelFacebook

        public class FacebookAccount
        {
            public string id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string age_range { get; set; }
            public string birthday { get; set; }
            public string gender { get; set; }
            public string locale { get; set; }
            public Picture picture { get; set; }
        }

        public class Picture
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public int height { get; set; }
            public bool is_silhouette { get; set; }
            public string url { get; set; }
            public int width { get; set; }
        }

        #endregion ModelFacebook

        #region Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FacebookLogin(string Token)
        {
            if (string.IsNullOrEmpty(Token))
            {
                return Json(new { status = false });
            }
            var httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v2.9/") };
            var response = await httpClient.GetAsync($"me?access_token={Token}&fields=id,name,email,first_name,last_name,age_range,birthday,gender,locale,picture");
            if (!response.IsSuccessStatusCode) return Json(new { status = false });
            var result = await response.Content.ReadAsStringAsync();
            var facebookAccount = JsonConvert.DeserializeObject<FacebookAccount>(result);
            var facebookUser = _userService.GetCondition(m => m.FacebookId.ToString() == facebookAccount.id).FirstOrDefault();
            long idfb = long.Parse(facebookAccount.id);
            if (facebookUser == null)
            {
                try
                {
                    var user = new Users
                    {
                        TypeUser = TypeUser.User,
                        FullName = facebookAccount.name,
                        Email = facebookAccount.email,
                        FacebookId = idfb,
                        ImageUrl = facebookAccount.picture.data.url != string.Empty ? facebookAccount.picture.data.url : TypeUser.AvatarDefault,
                        Status = true
                    };
                    await _userService.CreateAsync(user);
                    var getIdUs = _userService.GetCondition(m => m.FacebookId == idfb && m.Status == true).FirstOrDefault();
                    if (getIdUs != null)
                    {
                        var identity = new ClaimsIdentity(new[] {
                                                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                                                new Claim(ClaimTypes.Name, user.FullName),
                                                                new Claim(ClaimTypes.Email, user.Email),
                                                                new Claim(ClaimTypes.Role, user.TypeUser),
                                                                new Claim(ClaimTypes.CookiePath, user.ImageUrl)
                                                            }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        return Json(new { status = true, message = "Đăng nhập thành công", redirect = "/Home/Index" });
                    }
                }
                catch (Exception)
                {
                    return Json(new { status = false });
                }
            }
            else if (facebookUser.FacebookId == idfb)
            {
                var identity = new ClaimsIdentity(new[] {
                                                          new Claim(ClaimTypes.NameIdentifier, facebookUser.Id.ToString()),
                                                          new Claim(ClaimTypes.Name, facebookUser.FullName),
                                                           new Claim(ClaimTypes.Email, facebookUser.Email),
                                                          new Claim(ClaimTypes.Role, facebookUser.TypeUser),
                                                           new Claim(ClaimTypes.CookiePath, facebookUser.ImageUrl)
                                                        }, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Json(new { status = true, message = "Đăng nhập thành công", redirect = "/Home/Index" });
            }
            return Json(new { status = false });
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _userService.GetCondition(m => m.Email == email && m.Password == password).FirstOrDefault();
            if (user != null)
            {
                if (user.Status == false)
                {
                    return Json(new { status = false, message = "Tài khoản đã bị khoá" });
                }
                var identity = new ClaimsIdentity(new[] {
                                                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                                                new Claim(ClaimTypes.Name, user.FullName),
                                                                new Claim(ClaimTypes.Email, user.Email),
                                                                new Claim(ClaimTypes.Role, user.TypeUser),
                                                                 new Claim(ClaimTypes.CookiePath, user.ImageUrl)
                                                            }, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                if (user.TypeUser == TypeUser.User)
                {
                    return Json(new { status = true, message = "Đăng nhập thành công", redirect = "/Home/Index" });
                }
                else if (user.TypeUser == TypeUser.Teacher)
                {
                    return Json(new { status = true, message = "Đăng nhập thành công", redirect = "/Dashboard/Index" });
                }
                else
                {
                    return Json(new { status = true, message = "Đăng nhập thành công", redirect = "/Dashboard/Index" });
                }
            }
            else
            {
                return Json(new { status = false, message = "Sai tên tài khoản hoặc mật khẩu" });
            }
        }

        #endregion Login

        public JsonResult CheckUserEmail(string email)
        {
            var checkUserExist = _userService.CountCondition(m => m.Email == email);
            return Json(checkUserExist > 0 ? false : true);
        }
        [HttpPost]
        public JsonResult CheckPassword(string oldpassword)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
            var User = _userService.GetById(currentUserId);
            return Json(User.Password == oldpassword ? true : false);
        }
        public async Task<IActionResult> ChangePassword()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                try
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserId = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var User = _userService.GetById(currentUserId);
                    User.Password = password;
                    await _userService.UpdateAsync(User);
                    return Json(true);
                }
                catch (Exception)
                {
                    return Json(false);
                }
            }
            return Json(false);
        }
        public IActionResult RegisterUser()
        {
            UserVM vm = new UserVM();
            return PartialView(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    vm.TypeUser = TypeUser.User;
                    vm.CreatedDate = DateTime.Now;
                    var model = _mapper.Map<Users>(vm);
                    model.Status = true;
                    model.ImageUrl = TypeUser.AvatarDefault;
                    model.Birthday = DateTime.Now;
                    model.Sex = true;
                    await _userService.CreateAsync(model);
                    return Json(new { Message = "Đăng kí thành công", IsSuccess = true });
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "Đăng kí thất bại", IsSuccess = false });
                }
            }
            return Json(new { Message = "Đăng kí thất bại", IsSuccess = false });
        }

        public IActionResult ListCategory()
        {
            var category = _courseCategoryService.GetCondition(m => m.Status == true && m.ParentId == 0).Select(m => new
            {
                m.Id,
                m.Name,
                m.SortOrder
            }).OrderBy(m => m.SortOrder);
            Dictionary<int, List<CourseCategories>> listCategoryChild = new Dictionary<int, List<CourseCategories>>();
            foreach (var item in category)
            {
                var categoryChild = _courseCategoryService.GetChildCategory(item.Id).Select(m => new CourseCategories
                {
                    Id = m.Id,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    SortOrder = m.SortOrder
                }).OrderBy(m => m.SortOrder).ToList();
                if (categoryChild.Count > 0)
                {
                    listCategoryChild.Add(item.Id, categoryChild);
                }
                else
                {
                    listCategoryChild.Add(item.Id, new List<CourseCategories>());
                }
            }
            bool status = false;
            if (listCategoryChild.Count > 0)
            {
                status = true;
            }
            return Json(new { status = status, parentCategory = category, listChild = listCategoryChild });
        }

        public async Task<IActionResult> ListAllCourseTop(int ID)
        {
            var categoryChild = _courseCategoryService.GetCondition(m => m.ParentId == ID && m.Status == true)
                                                        .Select(m => new
                                                        {
                                                            m.Id,
                                                            m.Name,
                                                            m.SortOrder
                                                        }).ToList();
            List<object> course = new List<object>();
            foreach (var item in categoryChild)
            {
                if (course.Count >= 5)
                {
                    return Json(new { status = true, topCourse = course.Take(5) });
                }
                var listCourse = _courseService.GetCondition(m => m.CategoryId == item.Id && m.Status == true)
                                                .Select(m => new
                                                {
                                                    m.Id,
                                                    m.UserId,
                                                    m.Name,
                                                    m.Image,
                                                    m.PromotionPrice,
                                                    m.Price,
                                                    parentId = ID
                                                }).ToList().Take(5);
                if (listCourse.Count() > 0)
                    course.AddRange(listCourse);
            }
            return Json(new { status = true, topCourse = course.Take(5) });
        }

        public async Task<IActionResult> Detail(int ID)
        {
            DetailHomeVM vm = new DetailHomeVM();
            var context = _courseService.GetContext();
            var model = from c in context.Courses
                        join u in context.Users
                        on c.UserId equals u.Id
                        join cate in context.CourseCategories
                        on c.CategoryId equals cate.Id
                        where c.Id == ID && c.Status == true
                        select new DetailHomeVM
                        {
                            IdCourse = c.Id,
                            lstChapter = (context.Chapter.Where(m => m.CourseId == ID).ToList()),
                            lstCourseLesson = (context.Chapter
                                                      .Join(context.CourseLessons,
                                                            chap => chap.Id,
                                                            cl => cl.ChapterId,
                                                            (chap, cl) => new
                                                            {
                                                                Chapter = chap,
                                                                CourseLessons = cl
                                                            })
                                                      .Where(m => m.Chapter.CourseId == ID)
                                                      .Select(m => m.CourseLessons).ToList()),
                            Name = c.Name,
                            NameCategory = cate.Name,
                            FullName = u.FullName,
                            ImageTeacher = u.ImageUrl,
                            UserId = u.Id,
                            PromotionPrice = c.PromotionPrice ?? 0,
                            Price = c.Price,
                            Content = c.Content,
                            Image = c.Image,
                            Description = c.Description,
                            IsFree = c.IsFree ?? false,
                            NumberStudent = (from od in context.OrderDetails
                                             join co in context.Courses
                                             on od.CourseId equals co.Id
                                             join o in context.Orders
                                             on od.OrderId equals o.Id
                                             where co.UserId == c.UserId //id teacher
                                             select o.UserId).Distinct().Count()//id hoc vien
                        };
            vm = model.FirstOrDefault();
            if (!vm.IsFree)
            {
                if (User.Identity.IsAuthenticated)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserName = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var _context = _courseService.GetContext();
                    var checkHasBuy = (
                                        from o in _context.Orders
                                        join od in _context.OrderDetails
                                        on o.Id equals od.OrderId
                                        join u in _context.Users
                                        on o.UserId equals u.Id
                                        where u.Id == currentUserName
                                              && od.CourseId == vm.IdCourse
                                              && o.Status == OrderStatus.Paid
                                        select od
                                     ).Count();
                    vm.HasBuy = checkHasBuy > 0 ? true : false;
                }
                else
                {
                    vm.HasBuy = false;
                }
            }
            return View(vm);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        public IActionResult View(int ID)
        {
            DetailHomeVM vm = new DetailHomeVM();
            var context = _courseService.GetContext();
            var model = from c in context.Courses
                        join u in context.Users
                        on c.UserId equals u.Id
                        join cate in context.CourseCategories
                        on c.CategoryId equals cate.Id
                        where c.Id == ID && c.Status == true
                        select new DetailHomeVM
                        {
                            IdCourse = c.Id,
                            lstChapter = (context.Chapter.Where(m => m.CourseId == ID).ToList()),
                            lstCourseLesson = (context.Chapter
                                                      .Join(context.CourseLessons,
                                                            chap => chap.Id,
                                                            cl => cl.ChapterId,
                                                            (chap, cl) => new
                                                            {
                                                                Chapter = chap,
                                                                CourseLessons = cl
                                                            })
                                                       .Where(m => m.Chapter.CourseId == ID)
                                                       .Select(m => m.CourseLessons).ToList()),
                            Name = c.Name,
                            NameCategory = cate.Name,
                            FullName = u.FullName,
                            UserId = u.Id,
                            PromotionPrice = c.PromotionPrice ?? 0,
                            Price = c.Price,
                            Content = c.Content,
                            Image = c.Image,
                            Description = c.Description
                        };
            vm = model.FirstOrDefault();
            var lst = context.LessonComments.Where(m => m.LessonId == vm.lstCourseLesson[0].Id).Select(m => m).ToList();
            vm.lstComment = (from cm in lst
                             join u in context.Users
                             on cm.UserId equals u.Id
                             select new LessonCommentVM
                             {
                                 FullName = u.FullName,
                                 Content = cm.Content,
                                 ImageUrl = u.ImageUrl
                             }).ToList();
            return View(vm);
        }

        [Authorize(Roles = "User")]
        public ActionResult GetMedia(string path)
        {
            string fn = _hostingEnvironment.WebRootPath + path;
            var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(fn));
            var result = File(fileStream: memoryStream,
                              contentType: new MediaTypeHeaderValue("video/mp4").MediaType,
                              enableRangeProcessing: true);
            return result;
        }

        public PaymentVM ComputePayment(string listIdCourse)
        {
            var ls = listIdCourse.Split(';');
            PaymentVM vm = new PaymentVM();
            foreach (var item in ls)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var id = int.Parse(item.ToString());
                    var infoCourse = _courseService.GetById(id);
                    vm.TotalMoney += infoCourse.Price;
                    vm.lstCourse.Add(infoCourse);
                }
            }
            vm.SumMoney = vm.TotalMoney;
            vm.DiscountMoney = 0;
            return vm;
        }

        [HttpGet]
        public IActionResult BuyNow(string listIdCourse)
        {
            PaymentVM vm = ComputePayment(listIdCourse);
            return View("Payment", vm);
        }

        public IActionResult Payment(PaymentVM vm)
        {
            return View(vm);
        }

        public string SignatureMomo(string requestId, string amount, string orderId, string orderInfo, string extraData = "")
        {
            string rawHash = string.Format(
                                                     "partnerCode={0}" +
                                                     "&accessKey={1}" +
                                                     "&requestId={2}" +
                                                     "&amount={3}" +
                                                     "&orderId={4}" +
                                                     "&orderInfo={5}" +
                                                     "&returnUrl={6}" +
                                                     "&notifyUrl={7}" +
                                                     "&extraData={8}",
                                                     DefinePaymentMomo.PartnerCode,//0 partnerCode
                                                     DefinePaymentMomo.AccessKey,//1 accessKey
                                                     requestId,//2 requestId
                                                     amount,//3 amount
                                                     orderId,//4 orderId
                                                     orderInfo,//5 orderInfo
                                                     DefinePaymentMomo.ReturnUrl,//6 returnUrl
                                                     DefinePaymentMomo.NotifyUrl,//7 notifyUrl
                                                     extraData//8 extraData
                                                   );
            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, DefinePaymentMomo.SecretKey);
            return signature;
        }

        public string SignatureCheckResponseMomo(string requestId, string orderId, string message, string localMessage, string payUrl, string errorCode, string requestType)
        {
            string rawHash = string.Format(
                                               "requestId={0}" +
                                               "&orderId={1}" +
                                               "&message={2}" +
                                               "&localMessage={3}" +
                                               "&payUrl={4}" +
                                               "&errorCode={5}" +
                                               "&requestType={6}",
                                               requestId,//2 requestId
                                               orderId,//3 amount
                                               message,//4 orderId
                                               localMessage,//5 orderlocalMessageInfo
                                               payUrl,//6 payUrl
                                               errorCode,//7 errorCode
                                               requestType//8 requestType
                                             );
            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, DefinePaymentMomo.SecretKey);
            return signature;
        }

        public async Task<string> CallMomo(string signature, string requestId, string amount, string orderId, string orderInfo, string extraData = "")
        {
            string responseFromMomo = string.Empty;
            try
            {
                JObject message = new JObject
                                                    {
                                                        { "partnerCode",  DefinePaymentMomo.PartnerCode },
                                                        { "accessKey", DefinePaymentMomo.AccessKey },
                                                        { "requestId", requestId },
                                                        { "amount", amount },
                                                        { "orderId", orderId },
                                                        { "orderInfo", orderInfo },
                                                        { "returnUrl", DefinePaymentMomo.ReturnUrl },
                                                        { "notifyUrl", DefinePaymentMomo.NotifyUrl },
                                                        { "extraData", extraData },
                                                        { "requestType", "captureMoMoWallet" },
                                                        { "signature", signature }
                                                    };
                responseFromMomo = await PaymentRequest.sendPaymentRequest(DefinePaymentMomo.MomoTest, message.ToString());
                return responseFromMomo;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        internal class ReposonsePayMomo
        {
            public string requestId { get; set; }
            public string errorCode { get; set; }
            public string message { get; set; }
            public string localMessage { get; set; }
            public string requestType { get; set; }
            public string payUrl { get; set; }
            public string qrCodeUrl { get; set; }
            public string deeplink { get; set; }
            public string deeplinkWebInApp { get; set; }
            public string signature { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentComplete(string listIdCourse)
        {
            PaymentVM vm = ComputePayment(listIdCourse);
            long totalamount = 0;
            if (User.Identity.IsAuthenticated)
            {
                if (vm.lstCourse != null && vm.lstCourse.Count > 0)
                {
                    try
                    {
                        ClaimsPrincipal currentUser = this.User;
                        var currentUserName = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
                        quanlykhoahocContext context = _courseService.GetContext();
                        using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                string orderId = Guid.NewGuid().ToString();
                                int i = 0;
                                var countlstCourse = listOrderDetail.Count;
                                foreach (var item in vm.lstCourse)
                                {
                                    var course = _courseService.GetById(item.Id);
                                    if (course != null)
                                    {
                                        OrderDetails orderDetails = new OrderDetails();
                                        orderDetails.OrderId = orderId;
                                        orderDetails.CourseId = course.Id;
                                        if (countlstCourse > 0)
                                        {
                                            totalamount += course.Price - (listOrderDetail[i].Quantity ?? 0);
                                            orderDetails.DiscountId = listOrderDetail[i].DiscountId;
                                            orderDetails.Quantity = listOrderDetail[i].Quantity;
                                            orderDetails.Amount = course.Price - (listOrderDetail[i].Quantity ?? 0);
                                        }
                                        else
                                        {
                                            totalamount += course.Price;
                                            orderDetails.Amount = course.Price;
                                        }
                                        await _orderDetailService.CreateAsync(orderDetails);
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        return Json(new { status = false, message = "Xảy ra lỗi trong quá trình thanh toán" });
                                    }
                                }
                                string convertTotal = totalamount.ToString();
                                long amount = Convert.ToInt64(convertTotal);
                                string endpoint = DefinePaymentMomo.MomoTest;
                                string requestId = Guid.NewGuid().ToString();
                                string orderInfo = "PaymentCourse";
                                string extraData = "";
                                Orders orderVM = new Orders();
                                orderVM.Id = orderId;
                                orderVM.OrderDate = DateTime.Now;
                                orderVM.PayMethod = PaymentMethod.MomoPay;
                                //trang thai chua thanh cong
                                orderVM.Status = OrderStatus.Unpaid;
                                orderVM.TotalAmount = totalamount;
                                //tao chu ky dien tu
                                string signature = string.Empty;
                                orderVM.Signature = signature = SignatureMomo(requestId, amount.ToString(), orderId, orderInfo, extraData);
                                orderVM.RequestId = requestId;
                                orderVM.UserId = currentUserName;
                                await _orderService.CreateAsync(orderVM);
                                string result = await CallMomo(signature, requestId, amount.ToString(), orderId, orderInfo, extraData); //yeu cau cung cap url thanh toan
                                ReposonsePayMomo reposonsePayMomo = JsonConvert.DeserializeObject<ReposonsePayMomo>(result);
                                if (reposonsePayMomo.errorCode == "0")
                                {
                                    string signatureReponse = SignatureCheckResponseMomo(requestId, orderId, reposonsePayMomo.message, reposonsePayMomo.localMessage, reposonsePayMomo.payUrl, reposonsePayMomo.errorCode, reposonsePayMomo.requestType);
                                    if (signatureReponse == reposonsePayMomo.signature)
                                    {
                                        transaction.Commit();
                                        return Json(new { status = true, loca = reposonsePayMomo.payUrl });
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        return Json(new { status = false, message = "Xảy ra lỗi trong quá trình thanh toán" });
                                    }
                                }
                            }
                            catch
                            {
                                transaction.Rollback();
                                return Json(new { status = false, message = "Xảy ra lỗi trong quá trình thanh toán" });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { status = false, message = "Vui lòng đăng nhập để thanh toán" });
                    }
                }
            }
            return Json(new { status = false, message = "Vui lòng đăng nhập để thanh toán" });
        }

        #region Discount

        public static List<OrderDetails> listOrderDetail { set; get; } = new List<OrderDetails>();

        public long ComputePaymentDiscount(string listIdCourse, string codeDiscount, IEnumerable<Discount> listDiscount)
        {
            var context = _courseService.GetContext();
            var ls = listIdCourse.Split(';');
            long totalAmount = 0;
            foreach (var item in ls)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    OrderDetails orderDetails = new OrderDetails();
                    var discountIdCourse = (from ld in listDiscount
                                            join dc in context.DiscountCourse
                                            on ld.Id equals dc.Iddiscount
                                            join c in context.Courses
                                            on dc.Idcourse equals c.Id
                                            where item == c.Id.ToString()
                                            select ld).FirstOrDefault();
                    if (discountIdCourse.DiscountAmount != null && discountIdCourse.DiscountPercent != 0)
                    {
                        totalAmount += discountIdCourse.DiscountAmount ?? 0;
                    }
                    if (discountIdCourse.DiscountPercent != null && discountIdCourse.DiscountPercent != 0)
                    {
                        int idcourse = int.Parse(item);
                        var course = _courseService.GetById(idcourse);
                        long temp = (course.Price * discountIdCourse.DiscountPercent ?? 0) / 100;
                        totalAmount += temp;
                    }
                    orderDetails.Quantity = totalAmount;
                    orderDetails.DiscountId = discountIdCourse.Id;
                    listOrderDetail.Add(orderDetails);
                }
            }
            return totalAmount;
        }

        public IEnumerable<Discount> listDiscountCourse(string codeDiscount)
        {
            var context = _courseService.GetContext();
            var currentCodeDiscount = from d in context.Discount
                                      where d.CodeDiscount == codeDiscount
                                      select d;
            return currentCodeDiscount;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckDiscount(string listCourse, string codeDiscount)
        {
            if (listOrderDetail != null || listOrderDetail.Count > 0)
            {
                listOrderDetail.Clear();
            }
            var currentCodeDiscount = listDiscountCourse(codeDiscount);
            var checkExist = currentCodeDiscount.Count();
            if (checkExist == 0)
            {
                return Json(new { status = false, message = "Mã giảm giá không tồn tại!" });
            }
            var nowDate = DateTime.Now;
            var checkExpried = currentCodeDiscount.Where(m => nowDate >= m.FromDate && nowDate <= m.ToDate).Count();
            if (checkExpried == 0)
            {
                return Json(new { status = false, message = "Mã giảm giá đã hết hạn!" });
            }
            long discountmoney = ComputePaymentDiscount(listCourse, codeDiscount, currentCodeDiscount);
            return Json(new { status = true, message = "Mã giảm giá đã được áp dụng", discountmoney = discountmoney });
        }

        #endregion Discount
    }
}