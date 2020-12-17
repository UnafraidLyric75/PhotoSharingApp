using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotoSharingApp.Data;
using PhotoSharingApp.Models;

namespace PhotoSharingApp.Controllers
{
    public class UserAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserAccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                // Check if username/email is in use
                bool isEmailTaken = await (from account in _context.UserAccount
                                           where account.Email == reg.Email
                                           select account).AnyAsync();

                // if so, add custom error and send back to view
                if (isEmailTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), "That emai is already in use");
                }

                bool isUsernameTaken = await (from account in _context.UserAccount
                                              where account.Username == reg.Username
                                              select account).AnyAsync();

                if (isUsernameTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Username), "That username is taken");
                }

                if (isEmailTaken || isUsernameTaken)
                {
                    return View(reg);
                }

                // Map data to user account instance
                UserAccount acc = new UserAccount()
                {
                    DateOfBirth = reg.DateOfBirth,
                    Email = reg.Email,
                    Password = reg.Password,
                    Username = reg.Username
                };

                // add to database
                _context.UserAccount.Add(acc);
                await _context.SaveChangesAsync();

                LogUserIn(acc.UserId);

                // redirect to home
                return RedirectToAction("Index", "Home");

            }

            return View(reg);
        }

        public IActionResult Login()
        {
            // Check if user already logged in
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserAccount account =
                       await (from u in _context.UserAccount
                              where (u.Username == model.UsernameOrEmail
                                 || u.Email == model.UsernameOrEmail)
                                 && u.Password == model.Password
                              select u).SingleOrDefaultAsync();

            if (account == null)
            {
                // credential did not match

                // Custom error msg
                ModelState.AddModelError(string.Empty, "Credential were not found");

                return View(model);
            }

            LogUserIn(account.UserId);

            return RedirectToAction("Index", "Home");
        }

        private void LogUserIn(int accountId)
        {
            // Log user into website
            HttpContext.Session.SetInt32("UserId", accountId);
        }

        public IActionResult Logout()
        {
            // removes all current session data
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
