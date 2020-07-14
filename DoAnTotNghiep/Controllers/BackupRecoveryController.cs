using AutoMapper;
using DoAnTotNghiep.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep.Controllers
{
    public class BackupRecoveryController : Controller
    {
        public BackupRecoveryController()
        {
        }
        public IActionResult ConfigBackup()
        {
            return View();
        }
    }
}