using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrankAWSWebApp.Models;
using Microsoft.AspNetCore.Identity;
using PrankAWSWebApp.Areas.Admin.Data;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using PrankAWSWebApp.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrankAWSWebApp.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using PrankAWSWebApp.ApiSettings;
using Prank.Model;
namespace PrankAWSWebApp.Areas.Admin.Controllers
{
    // [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class EmployeeController : Controller
    {
        IConfiguration _iconfiguration;

        private readonly UserManager<PrankIdentityUser> userManager;
        private readonly SignInManager<PrankIdentityUser> loginManager;
        private readonly RoleManager<PrankIdentityRole> roleManager;
        private IPasswordHasher<PrankIdentityUser> passwordHasher;
        private Task<PrankIdentityUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        public string EmployeeId
        {
            get
            {
                //var usr = await GetCurrentUserAsync();
                var task = Task.Run(async () => await GetCurrentUserAsync());
                return task.Result.Id;
                //return usr?.Id;
            }
        }

        private readonly EmailSettings _emailSettings;
        public EmployeeController(EmailSettings emailSettings, UserManager<PrankIdentityUser> userManager, SignInManager<PrankIdentityUser> loginManager, RoleManager<PrankIdentityRole> roleManager, IConfiguration iconfiguration, IPasswordHasher<PrankIdentityUser> passwordHasher)
        {
            this.userManager = userManager;
            this.loginManager = loginManager;
            this.roleManager = roleManager;
            _iconfiguration = iconfiguration;
            this.passwordHasher = passwordHasher;
            _emailSettings = emailSettings;

        }

        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            string emailTemplateFile = await this.RenderViewToStringAsync("ConfimationEmail", "");
            string[] roleNames = { "SuperAdmin", "Admin", "Manager", "Member" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {

                    //create the roles and seed them to the database: Question 1
                    roleResult = await roleManager.CreateAsync(new PrankIdentityRole(roleName, roleName));
                }
            }


            var employee = userManager.Users.Select(s => new EmployeModelViewModel()
            {
                Id = s.Id,
                //EmpId = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                Phone = s.PhoneNumber,

            }).ToList();
            return View(employee);
        }

        public async Task<IActionResult> PartialCreate()
        {
            var model = new EmployeModelViewModel();
            model.Password = "Password@123";
            model.ConfirmPassword = "Password@123";
            var roles = roleManager.Roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Name }).ToList();
            model.RoleList = roles;
            string viewFromCurrentController = await this.RenderViewToStringAsync("PartialCreate", model);
            return Json(
               new
               {
                   html = viewFromCurrentController
               });
        }

        public async Task<IActionResult> PartialGetEmployees()
        {
            var employee = userManager.Users.Select(s => new EmployeModelViewModel()
            {
                Id = s.Id,
                //EmpId = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                Phone = s.PhoneNumber,

            }).ToList();

            string viewFromCurrentController = await this.RenderViewToStringAsync("PartialGetEmployees", employee);
            var roleList = roleManager.Roles;
            ViewBag.RolesList = roleList;
            return Json(
               new
               {
                   html = viewFromCurrentController
               });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveUpdate(EmployeModelViewModel employeModelViewModel)
        {
            var data = new PackagesDataModel();
            var objResponse = new SaveResponse();
            string viewFromCurrentController = string.Empty;
            IdentityResult result;
            employeModelViewModel.ConfirmPassword = employeModelViewModel.Password;
            if (!string.IsNullOrEmpty(employeModelViewModel.Id))
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }
            if (ModelState.IsValid)
            {
                try
                {

                    if (string.IsNullOrEmpty(employeModelViewModel.Id))
                    {
                        var appUser = new PrankIdentityUser()
                        {
                            Email = employeModelViewModel.Email,
                            UserName = employeModelViewModel.Email,
                            FirstName = employeModelViewModel.FirstName,
                            IsAdmin = true,
                            LastName = employeModelViewModel.LastName,
                            PhoneNumber = employeModelViewModel.Phone,

                        };
                        result = await userManager.CreateAsync(appUser, employeModelViewModel.Password);
                        if (result.Succeeded)
                        {
                            var result1 = await userManager.AddToRoleAsync(appUser, employeModelViewModel.RoleId);
                        }
                    }
                    else
                    {
                        var appUser = await userManager.FindByIdAsync(employeModelViewModel.Id);
                        appUser.Email = employeModelViewModel.Email;
                        appUser.UserName = employeModelViewModel.Email;
                        appUser.FirstName = employeModelViewModel.FirstName;
                        appUser.IsAdmin = true;
                        appUser.LastName = employeModelViewModel.LastName;
                        appUser.PhoneNumber = employeModelViewModel.Phone;
                        //appUser.PasswordHash = passwordHasher.HashPassword(appUser, employeModelViewModel.Password);
                        result = await userManager.UpdateAsync(appUser);
                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrEmpty(employeModelViewModel.RoleId))
                            {
                                var userroles = await userManager.GetRolesAsync(appUser);
                                if (userroles.Count > 0)
                                {
                                    foreach (var role in userroles)
                                    {
                                        var isExists = await userManager.IsInRoleAsync(appUser, role);
                                        if (isExists)
                                        {
                                            await userManager.RemoveFromRoleAsync(appUser, employeModelViewModel.RoleId);
                                        }
                                    }
                                }
                                var result1 = await userManager.AddToRoleAsync(appUser, employeModelViewModel.RoleId);
                            }

                        }
                    }
                    if (result.Succeeded)
                    {
                        var employee = userManager.Users.Select(s => new EmployeModelViewModel()
                        {
                            Id = s.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Email = s.Email,
                            Phone = s.PhoneNumber,

                        }).ToList();

                        viewFromCurrentController = await this.RenderViewToStringAsync("PartialGetEmployees", employee);
                        objResponse.StatusCode = 200;
                        objResponse.Html = viewFromCurrentController;
                        objResponse.Message = "Record saved successfully";


                        string token = string.Empty;
                        var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                        var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                        if (userTokenClaim != null)
                        {
                            token = userTokenClaim.Value;
                        }
                        await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName,employeModelViewModel.FirstName+"  "+employeModelViewModel.LastName,
                          0, !string.IsNullOrEmpty(employeModelViewModel.Id) ? "Update" : "Save");

                        if (string.IsNullOrEmpty(employeModelViewModel.Id))
                        {
                            #region Email Send 

                            string emailTemplateFile = await this.RenderViewToStringAsync("ConfimationEmail", "");

                            if (string.IsNullOrEmpty(emailTemplateFile))
                            {
                                throw new Exception("No email template found");
                            }

                            var mailBccEmail = new List<string>();
                            string bccEmail = _emailSettings.EmailsInTestMode ? _emailSettings.EmailToTest : _emailSettings.EmailBcc;

                            if (!string.IsNullOrEmpty(bccEmail))
                            {
                                mailBccEmail.AddRange(bccEmail.Split(';'));
                            }

                            var message = new EmailMessage
                            {
                                IsHtml = true,
                                Subject = "Dagga Email Confirmation",
                                To = _emailSettings.EmailsInTestMode ? _emailSettings.EmailToTest : employeModelViewModel.Email,
                                BCC = mailBccEmail,
                            };



                            message.Body = emailTemplateFile.Replace("###Name###", employeModelViewModel.FirstName + " " + employeModelViewModel.LastName)
                                               .Replace("###Email###", employeModelViewModel.Email).Replace("###Password###", employeModelViewModel.Password);

                            await Helpers.EmailHelpers.SendAsync(message.Subject, message.Body, message.To, _emailSettings.SmtpServer,
                                                                _emailSettings.SmtpPort, _emailSettings.SmtpUserName, _emailSettings.SmtpPassword,
                                                                message.From ?? _emailSettings.SmtpFrom, message.IsHtml, message.CC,
                                                                message.BCC);

                            #endregion

                        }
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                            objResponse.Message += error.Description + ", ";
                        }

                        objResponse.StatusCode = 400;
                        objResponse.Html = viewFromCurrentController;
                    }
                }
                catch (Exception ex)
                {
                    string exep = ex.ToString();
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                objResponse.StatusCode = 400;
                objResponse.Html = viewFromCurrentController;
                objResponse.Message = "Record not saved successfully";
            }
            return new JsonResult(new
            {
                objResponse
            });
        }

        public async Task<IActionResult> PartialEdit(string id)
        {
            string viewFromCurrentController = "";
            var user = await userManager.FindByIdAsync(id);
            try
            {
                if (user != null)
                {
                    var model = new EmployeModelViewModel()
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        Phone = user.PhoneNumber,
                        LastName = user.LastName,
                        Id = user.Id
                    };
                    var roles = roleManager.Roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Name }).ToList();
                    model.RoleList = roles;
                    var userroles = await userManager.GetRolesAsync(user);
                    model.RoleName = userroles.FirstOrDefault();
                    model.RoleId = userroles.FirstOrDefault();
                    viewFromCurrentController = await this.RenderViewToStringAsync("PartialEdit", model);
                }
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
            }
            return Json(
               new
               {

                   html = viewFromCurrentController
               });
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(string id)
        {
            string viewFromCurrentController = string.Empty;
            var objResponse = new SaveResponse();
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    var employee = userManager.Users.Select(s => new EmployeModelViewModel()
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        Phone = s.PhoneNumber,

                    }).ToList();
                    viewFromCurrentController = await this.RenderViewToStringAsync("PartialGetEmployees", employee);
                    objResponse.StatusCode = 200;
                    objResponse.Html = viewFromCurrentController;
                    objResponse.Message = "Record deleted successfully";

                    string token = string.Empty;
                    var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                    var userTokenClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Token");
                    if (userTokenClaim != null)
                    {
                        token = userTokenClaim.Value;
                    }
                    await TrackingInfo.TrackInfo(token, EmployeeId, ControllerContext.ActionDescriptor.ControllerName, id.ToString(), 0, "Delete");
                }
                else
                {
                    objResponse.StatusCode = 400;
                    objResponse.Message = "Record not deleted successfully";
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
                objResponse.Message = "User Not Found";
            }

            return new JsonResult(new
            {
                objResponse
            });


        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(string id)
        {
            var objResponse = new SaveResponse();
            if (string.IsNullOrEmpty(id))
            {
                objResponse.Message = "User not found";
            }
            else
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    objResponse.Message = "User does not exist";
                }
                else
                {
                    string code = await userManager.GeneratePasswordResetTokenAsync(user);
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await userManager.ResetPasswordAsync(user, token, "Password@123");
                    if (result.Succeeded)
                    {
                        objResponse.Message = "Your password reset successfully";
                        objResponse.StatusCode = 200;
                    }
                    else
                    {
                        objResponse.Message = "Your password not reset successfully";
                    }

                }
            }

            return new JsonResult(new
            {
                objResponse
            });
        }
        public IActionResult ConfimationEmail() {

            return View();
        } 
    }
}
