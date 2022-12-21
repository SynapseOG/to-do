using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol;
using System.Security.Claims;
using to_do.Data;
using to_do.Models;

namespace to_do.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public ToDoController(ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager
            )
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
         
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var user = await GetCurrentUser();

            var listOfTasks = await _dbContext.ToDos
                .Where(x=>x.UserId == user.Id)
                .ToListAsync();
                
            return View(listOfTasks);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ToDoCreate todo)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUser();

                ToDo toDo = new();

                toDo.Name = todo.Name;
                toDo.Date = todo.Date;
                toDo.Notes = todo.Notes;
                toDo.UserId = user.Id;
            
                _dbContext.Add(toDo);
                _dbContext.SaveChanges();


            }
                return RedirectToAction("GetAll");
          
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await GetCurrentUser();

            var todo = await _dbContext.ToDos
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

            if (todo == null)
            {
                return Unauthorized();
            }

            ToDoUpdate update = new ToDoUpdate();

            update.Id = todo.Id;
            update.Name = todo.Name; 
            update.Date = todo.Date;
            update.Notes = todo.Notes;

            return View(update);

        }
        [HttpPost]
        public async Task<IActionResult> EditPost(ToDoUpdate update)
        {
            var task = await _dbContext.ToDos.FirstOrDefaultAsync(x => x.Id == update.Id);

            task.Name = update.Name;
            task.Notes = update.Notes;  
            task.Date= update.Date;

            _dbContext.ToDos.Update(task);
            _dbContext.SaveChanges();

            return RedirectToAction("GetAll");
        }
        public async Task<IActionResult> Details(int id)
        {
            var user = await GetCurrentUser();

            var todo = await _dbContext
                .ToDos
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id); 
            //userId musi się zgadzać zeby inne zalogowane osoby nie miały dostępu do szczegółów

            if(todo == null)
            {
                return Unauthorized();
            }

            return View(todo);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await GetCurrentUser();

            var todo = await _dbContext
                .ToDos
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

            if (todo == null)
            {
                return Unauthorized();
            }

            return View(todo);

        }
        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var toDelete = _dbContext.ToDos.FirstOrDefault(x => x.Id == id);

            if (toDelete != null)
            {
                 _dbContext.Remove(toDelete);
                _dbContext.SaveChanges();
            }
            else
            {
                return NotFound();
            }


            return RedirectToAction("GetAll");
        }

        private async Task<IdentityUser> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return user;
        }


    }
}
