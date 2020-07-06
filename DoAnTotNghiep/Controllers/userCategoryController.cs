using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class UserCategoryController : Controller
    {
        private readonly IUserService _iUserService;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        public UserCategoryController(IUserService iUserService,
                  IHostingEnvironment hostingEnvironment,
            IMapper mapper
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _iUserService = iUserService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {

            var User = new UserVM();
            return View(User);
        }
        [HttpGet]
        public IActionResult _Index(string searchName, int page, int pageSize = 3)
        {
            var model = new Object();
            int totalRow = 0;
            if (string.IsNullOrEmpty(searchName))
            {
                model = _iUserService.GetPaging(null, out totalRow, page, pageSize, x => x.Id).
                 Select(m => new
                 {
                     m.Id,
                     m.FullName,
                     m.Birthday,
                     m.Sex,
                     m.Introduction,
                     m.Status
                 }).ToList();
            }
            else
            {
                model = _iUserService.GetPaging(m => m.FullName.Contains(searchName), out totalRow, page, pageSize, x => x.Id).
                  Select(m => new
                  {
                      m.Id,
                      m.FullName,
                      m.Birthday,
                      m.Sex,
                      m.Introduction,
                      m.Status
                  }).ToList();
            }
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            });
        }

        public IActionResult Create(int ID)
        {
            var vm = new UserVM();
            if (ID == 0)
            {
                vm.listUsers = _iUserService.GetAll();
            }
            else
            {
                var modelcourse = _iUserService.GetById(ID);
                if (modelcourse == null)
                {
                    return NotFound();
                }
                vm = _mapper.Map<UserVM>(modelcourse);
                vm.listUsers = _iUserService.GetAll();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create
                    if (vm.Id == 0)
                    {
                        var model = _mapper.Map<Users>(vm);
                        await _iUserService.CreateAsync(model);
                        ViewBag.IsSuccess = true;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var model = _mapper.Map<Users>(vm);
                        await _iUserService.UpdateAsync(model);
                        ViewBag.IsSuccess = true;
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IsSuccess = false;
                    return View(vm);
                }
            }
            return View(vm);
        }


        public async Task<IActionResult> Delete(int ID)
        {
            bool result = true;
            try
            {
                await _iUserService.Delete(ID);
                return Json(result);
            }
            catch (Exception)
            {
                result = false;
                return Json(result);
            }
        }
    
    }
}