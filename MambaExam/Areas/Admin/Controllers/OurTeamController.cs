using Core3.Entities;
using DLA.Interfaces;
using MambaExam.Areas.Admin.Models;
using MambaExam.Utilities;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MambaExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OurTeamController : Controller
    {
        private readonly IOurTeamRepository _ourTeam;
        private readonly IWebHostEnvironment _env;


        public OurTeamController(IOurTeamRepository ourTeam, IWebHostEnvironment env)
        {
            _ourTeam = ourTeam;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var ourteam = await _ourTeam.GetAllAsync();
            return View(ourteam);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateOurTeamModel model)
        { if(!ModelState.IsValid) return View(model);
            if (!model.img.CheckSizeFile(200))
            {
                ModelState.AddModelError("img", "need to be below 200kb");
                return View(model);
            }
            //if (!model.img.CheckFormat("image/"))
            //{
            //    ModelState.AddModelError("Img", "Img yukle");
            //    return View(model);
            //}
            if (!model.img.CheckSizeFile(200))
            {
                ModelState.AddModelError("img", "need to be below 200kb");
                return View(model);
            }
            var filename = string.Empty;
            var wwwroot = _env.WebRootPath;
            try
            {
                filename = await model.img.CopyFileAsync(wwwroot, "assets", "img", "team");
            }
            catch (Exception)
            {

                throw;
            }


            OurTeam team = new OurTeam()
            {
                Name = model.Name,
                Role = model.Role,
                Img = filename,
                MediaId = 1
             };
            await _ourTeam.Create(team);
            await _ourTeam.SavechangeAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult>Detail(int id)
        {
            
            var team = await _ourTeam.GetByIdAsync(id);
            if (team == null) { return NotFound(); }
            return View(team);
        }

        public async Task<IActionResult>  Delete(int id)
        {
            var ourteam= await _ourTeam.GetByIdAsync(id);
            if(ourteam == null) { return NotFound(); }
            _ourTeam.Delete(ourteam);
            await _ourTeam.SavechangeAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var ourteam = await _ourTeam.GetByIdAsync(id);
            if(ourteam==null) { return NotFound(); }
            UpdateOurTeamModel teammodel = new()
            {
                Id=ourteam.Id,
                name=ourteam.Name,
                Role = ourteam.Role,
                MediaId=ourteam.MediaId,
            };
            return View(teammodel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateOurTeamModel teammodel)
        {
            if (!ModelState.IsValid) return View(teammodel);
            //if (!teammodel.img.CheckFormat("/image"))
            //{
            //    ModelState.AddModelError("", "Img yukle");
            //    return View(teammodel);
            //}
            if (!teammodel.img.CheckSizeFile(200))
            {
                ModelState.AddModelError("img", "need to be below 200kb");
                return View(teammodel);
            }
            var filename = string.Empty;
            var wwwroot = _env.WebRootPath;
            try
            {
                filename = await teammodel.img.CopyFileAsync(wwwroot, "assets", "img", "team");
            }
            catch (Exception)
            {

                throw;
            }

            OurTeam team = new OurTeam()
            {
                Id = teammodel.Id,
                Name = teammodel.name,
                Role = teammodel.Role,
                Img = filename,
                MediaId = 1
            };
             _ourTeam.Update(team);
            await _ourTeam.SavechangeAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
