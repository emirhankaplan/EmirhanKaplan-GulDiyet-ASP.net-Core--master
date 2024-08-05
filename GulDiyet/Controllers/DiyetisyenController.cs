using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Diyetisyens;
using GulDiyet.Core.Application.ViewModels.Users;
using GulDiyet.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using GulDiyet.Middlewares;
using GulDiyet.Core.Application.Enums;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;

namespace GulDiyet.Controllers
{
    public class DiyetisyenController : Controller
    {
        private readonly IDiyetisyenService _DiyetisyenService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;

        public DiyetisyenController(IDiyetisyenService DiyetisyenService, ValidateUserSession validateUserSession,
            IHttpContextAccessor httpContextAccessor)
        {
            _DiyetisyenService = DiyetisyenService;
            _validateUserSession = validateUserSession;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            var list = await _DiyetisyenService.GetAllViewModel();
            return View(list);
        }

        public IActionResult Create()
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            SaveDiyetisyenViewModel vm = new();
            return View("SaveDiyetisyen", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveDiyetisyenViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            if (!ModelState.IsValid)
            {
                return View("SaveDiyetisyen", vm);
            }

            SaveDiyetisyenViewModel DiyetisyenVm = await _DiyetisyenService.Add(vm);

            if (DiyetisyenVm.Id != 0 && DiyetisyenVm != null)
            {
                DiyetisyenVm.ImageUrl = UploadFile(vm.File, DiyetisyenVm.Id);
                await _DiyetisyenService.Update(DiyetisyenVm);
            }

            return RedirectToRoute(new { controller = "Diyetisyen", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            var Diyetisyen = await _DiyetisyenService.GetByIdSaveViewModel(id);
            return View("SaveDiyetisyen", Diyetisyen);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveDiyetisyenViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            if (!ModelState.IsValid)
            {
                return View("SaveDiyetisyen", vm);
            }

            SaveDiyetisyenViewModel DiyetisyenVm = await _DiyetisyenService.GetByIdSaveViewModel(vm.Id);
            vm.ImageUrl = UploadFile(vm.File, vm.Id, true, DiyetisyenVm.ImageUrl);

            await _DiyetisyenService.Update(vm);
            return RedirectToRoute(new { controller = "Diyetisyen", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            var Diyetisyen = await _DiyetisyenService.GetByIdSaveViewModel(id);
            return View("Delete", Diyetisyen);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDiyetisyen(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            await _DiyetisyenService.Delete(id);

            //get directory path
            string basePath = $"/Images/Diyetisyens/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo folder in directoryInfo.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }

            return RedirectToRoute(new { Controller = "Diyetisyen", action = "Index" });
        }

        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode && file == null)
            {
                return imagePath;
            }

            //get directory path
            string basePath = $"/Images/Diyetisyens/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if no exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file path
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string filename = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, filename);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImageName = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImageName);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }

            return $"{basePath}/{filename}";
        }
    }
}
