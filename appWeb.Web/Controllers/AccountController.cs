using appWeb.Common.Entities;
using appWeb.Common.Response;
using appWeb.Web.Chat;
using appWeb.Web.Data;
using appWeb.Web.Data.Entities;
using appWeb.Web.Helpers;
using appWeb.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appWeb.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly UserManager<User> _userManager;

        public AccountController(IUserHelper userHelper, DataContext context, IMailHelper mailHelper, UserManager<User> userManager)
        {
            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Chat()
        {
            return View();
        }

        public async Task<IActionResult> CreateMessage( Message message)
        {
            if (ModelState.IsValid)
            {
                message.UserName = User.Identity.Name;
                var sender = await _userManager.GetUserAsync(User);
                message.UserId = sender.Id;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email or password incorrect.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                //if (model.ImageFile != null)
                //{
                //    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                //}

                User user = await _userHelper.AddUserAsync(model, imageId, UserType.User);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(model);
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.UserName,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:<p></br></br><a href = \"{tokenLink}\">Confirm Email</a><p>");
                if (response.IsSuccess)
                {
                    ViewBag.Message = "Please check your email adress to comfirm.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            return View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel
            {
                FirstName = user.Name,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                //ImageId = user.ImageId,
                Id = user.Id,
                Document = user.Document
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Guid imageId = model.ImageId;

                //if (model.ImageFile != null)
                //{
                //    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                //}

                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.Name = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                //user.ImageId = imageId;
                user.Document = model.Document;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail(model.Email, "Password Reset", $"<h1>Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
                ViewBag.Message = "The instructions to recover your password has been sent to email.";
                return View();

            }

            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset successful.";
                    return View();
                }

                ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            ViewBag.Message = "User not found.";
            return View(model);
        }

        public IActionResult Set_Publications()
        {
            return RedirectToAction("Set_Publications", "Products");
        }

        public async Task<IActionResult> Friends()
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name );
            var friends = await _context.Friends
                .Where(f => f.FirstPersonId == user.Id).ToListAsync();
            List<User> list = new List<User>();
            foreach(var item in friends)
            {
                var usr = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == item.FirstPersonId);
                list.Add(usr);
            }
            return View(list);
        }

        public async Task<IActionResult> Acept(string nameid, string name)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == nameid);
            var from = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == name);

            Response response = _mailHelper.SendMail(from.UserName, user.UserName + " Has aceptedd your invitation! ", user.UserName + " Has aceptedd your invitation! ");

            var uno = await _context.Friends
                .FirstOrDefaultAsync(f => f.FirstPersonId == user.Id);
            var dos = await _context.Friends
                .FirstOrDefaultAsync(f => f.FirstPersonId == from.Id);
            uno.Relation = true;
            dos.Relation = true;
            _context.Update(uno);
            await _context.SaveChangesAsync();
            _context.Update(dos);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

        public ActionResult Add_Friend()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Add_Friend([Bind("NameToFind")]AddFriendViewModel addFriendViewModel)
        //{
        //    return null;    
        //}

        [HttpPost]
        public async Task<IActionResult> Add_Friend(User user)
        {
            return RedirectToAction("Search", "Account", new { UserName = user.UserName });           
        }

        //[HttpPost]
        public async Task<IActionResult> Search(string UserName)
        {
            var users = await _context.Users.Where(u => u.UserName.Contains(UserName)).ToListAsync();
            if (User == null)
            {
                return NotFound();
            }
            else
            {
                List<AddFriendViewModel> result = new List<AddFriendViewModel>();
                foreach (var i in users)
                {
                    result.Add(new AddFriendViewModel
                    {
                        ToId = i.Id,
                        NameToFind = i.UserName
                    });
                }
                return View(await _context.Users.Where(u => u.UserName.Contains(UserName)).ToListAsync());
            }
        }

        public async Task<IActionResult> Send(string id, string fromid)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            var from = await _context.Users
                .FirstOrDefaultAsync(m => m.UserName == fromid);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "This user does not exist.");
                return RedirectToAction("Add_Friend", "Account");
            }

            string Link = Url.Action("Acept", "Account", new
            {
                nameid = id,
                name = from.Id
            }, protocol: HttpContext.Request.Scheme);

            Response response = _mailHelper.SendMail(user.UserName, "You have a new friend Invitation!", $"<h1>You have a new friend Invitation!</h1>" +
                $"To acept the friend: " + from.UserName + $"</br></br>" +
                $"plase click in this link:<p></br></br><a href = \"{Link}\">Acept Friend</a><p>");
            if (response.IsSuccess)
            {
                Friend data_1 = new Friend
                {
                    FirstPersonId = from.Id,
                    SecondPersonId = user.Id
                };
                Friend data_2 = new Friend
                {
                    FirstPersonId = user.Id,
                    SecondPersonId = from.Id
                };
                _context.Add(data_1);
                _context.Add(data_2);
                await _context.SaveChangesAsync();
                ViewBag.Message = "The invitation has been sended.";
                return RedirectToAction("Add_Friend", "Account");             
            }
            return RedirectToAction("Add_Friend", "Account");
        }

    }
}
