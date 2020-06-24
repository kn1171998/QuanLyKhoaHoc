using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class UserController : Controller
    {
    
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly IUserService _IUserServed;


        public UserController(
            IHostingEnvironment hostingEnvironment,
            IMapper mapper,
            IUserService usersv)
        {
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _IUserServed = usersv;

        }

        public IActionResult Index(int id)
        {
            var model = _IUserServed.GetById(id);
            var userVM = _mapper.Map<UserVM>(model);            
            return View(userVM);
        }
        public IActionResult Teacher(int id)
        {
           
            var model = _IUserServed.GetById(id);
            var userVM = _mapper.Map<UserVM>(model);
            
         return View(userVM);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UserVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create
                        var model = _mapper.Map<Users>(vm);
                        await _IUserServed.UpdateAsync(model);
                        ViewBag.IsSuccess = true;
                        return RedirectToAction(nameof(Index));
                  
                }
                catch (Exception ex)
                {
                    ViewBag.IsSuccess = false;
                    return View(vm);
                }
            }
            return View(vm);
        }

      
      
    }
}