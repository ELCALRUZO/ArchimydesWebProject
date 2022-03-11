using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ArchimydesWeb.Models;
using ArchimydesWeb.Repositories;
using Microsoft.AspNetCore.Identity;
using ArchimydesWeb.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ArchimydesWeb.Helpers;

namespace ArchimydesWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
      
        #region repositories
        private IUnitOfWork _unitOfWork;
        #endregion

        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager, 
               SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _logger = logger; 
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork; 
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(SysRole model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                ViewBag.Message = "Please fill in the required fields.";
                return View();
            }
             var newRole = new SysRole
            { 
                RoleName = model.RoleName,
                DateCreated = DateTime.Now,
                IsActive=true
                
            };

            await _unitOfWork.Role.AddSaveAsync(newRole);
           // _unitOfWork.Complete();

            var role = new ApplicationRole
            {
                Name = model.RoleName 
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                ModelState.Clear();
                ViewBag.Message = "Role has been created successfully";
                return View();
            }
            ModelState.Clear();
            ViewBag.Message = "Role creation failed";
            return View();
        }


        //Display all Roles
        [HttpGet] 
        public async Task<IActionResult> Roles(int? pageIndex, string searchText)
        { 
            try
            {
                List<DisplayRoleViewModel> model = new List<DisplayRoleViewModel>();
                var roles = _unitOfWork.Role.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    roles = roles.Where(role => role.RoleName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var role in roles)
                {
                    model.Add(new DisplayRoleViewModel
                    {
                        RoleID = role.RoleID,
                        RoleName = role.RoleName
                    });
                }
               
                var modelR = new HoldDisplayRoleViewModel
                {
                    HoldAllRoles = model
                };

                return View(modelR);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

         

        public async Task<IActionResult> AddUser()
        {
            ViewBag.ListofRole = await RoleList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(CreateUserVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Please fill in the required fields.");
                }

                var systemUserViewModel = new CreateUserVM();
                var role = _unitOfWork.Role.Get(model.RoleID);

                var APIServiceConfig = Startup.StaticConfig.GetSection("APIService").Get<APIService>();
                string Baseurl = APIServiceConfig.Url+ "/api/User/user/Create";
                 
                var aregResponse = new APIServiceReponse();
                 
                //passing model to postdat
                var postdata = new CreateUserVM
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    CompanyName = model.CompanyName,
                    DateCreated = DateTime.Now,
                    RoleID = model.RoleID 
                };

                var inputs = JsonConvert.SerializeObject(postdata);
                var request = new HttpRequestMessage(HttpMethod.Post, "");

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(inputs, Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                 
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(Baseurl);

                    var responses = await httpClient.SendAsync(request);
                    var content = await responses.Content.ReadAsStringAsync();
                    //responses.EnsureSuccessStatusCode();

                    if (!String.IsNullOrEmpty(content))
                    {
                        aregResponse = JsonConvert.DeserializeObject<APIServiceReponse>(content);

                        if (Convert.ToInt32(aregResponse.Status) >= 1)
                        {
                            ViewBag.ListofRole = await RoleList();
                            ViewBag.ErrorMessage = "Response " + aregResponse.Message;
                            return View(model);
                        }
                    }

                    if (responses.IsSuccessStatusCode == true)
                    {
                        ViewBag.ListofRole = await RoleList();
                        ViewBag.SuccessMessage = "Response " + aregResponse.Message;
                        ModelState.Clear();
                        return View();
                    }
                    else if (responses.IsSuccessStatusCode != true)
                    {
                        ViewBag.ListofRole = await RoleList();
                        ModelState.AddModelError("", responses.ReasonPhrase);
                        ViewBag.ErrorMessage = responses.ReasonPhrase;
                        return View(model);
                    }
                    ViewBag.ListofRole = await RoleList(); 
                    return View(model);
                }  
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }

            ViewBag.Message = "System User was not created successfully";
            ViewBag.ListofRole = await RoleList();
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> UpdateSystemUser(int id)
        {
            try
            {
                var APIServiceConfig = Startup.StaticConfig.GetSection("APIService").Get<APIService>();
                string Baseurl = APIServiceConfig.Url;

                var aregResponse = new UpdateUserViewModel();
                var gResponse = new UserResponse();
                using (var client = new HttpClient())
                {
                     if (id != 0)
                    { 
                        string IResponse = General.MakeRequest(Baseurl + "/api/User/UpdateUser/" + id , "", "GET");
                         
                        if (!String.IsNullOrEmpty(IResponse))
                        { 
                            gResponse = JsonConvert.DeserializeObject<UserResponse>(IResponse);

                            if (Convert.ToInt32(gResponse.Status) >= 1)
                            {
                                ViewBag.ErrorMessage = "Response " + gResponse.Message;

                                return View();
                            }
                        }

                        gResponse = JsonConvert.DeserializeObject<UserResponse>(IResponse);
                        if (Convert.ToInt32(gResponse.Status) >= 00)
                        {
                            var rmodel = new UpdateUserViewModel
                            {
                                UserID = gResponse.UserID,
                                FirstName = gResponse.FirstName,
                                MiddleName = gResponse.MiddleName,
                                LastName = gResponse.LastName,
                                Email = gResponse.Email,
                                PhoneNumber = gResponse.PhoneNumber,
                                CompanyName = gResponse.CompanyName,
                                DateCreated = gResponse.DateCreated.ToString(),
                            };
                            return View(rmodel);
                        } 
                        return View();
                    }
                } 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Updating User POST method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSystemUser(UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListofRole = await RoleList();
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var APIServiceConfig = Startup.StaticConfig.GetSection("APIService").Get<APIService>();
            string Baseurl = APIServiceConfig.Url + "/api/User/user/update";
             
            var aregResponse = new APIServiceReponse();

            //passing model to postdat
            var postdata = new UpdateUserViewModel
            {
                UserID=model.UserID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CompanyName = model.CompanyName,
                //DateCreated = DateTime.Now,
            };

            var inputs = JsonConvert.SerializeObject(postdata);
            var request = new HttpRequestMessage(HttpMethod.Post, "");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(inputs, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);

                var responses = await httpClient.SendAsync(request);
                var content = await responses.Content.ReadAsStringAsync();
                //responses.EnsureSuccessStatusCode();

                if (!String.IsNullOrEmpty(content))
                {
                    aregResponse = JsonConvert.DeserializeObject<APIServiceReponse>(content);

                    if (Convert.ToInt32(aregResponse.Status) >= 1)
                    {
                        ViewBag.ListofRole = await RoleList();
                        ViewBag.ErrorMessage = "Response " + aregResponse.Message;
                        return View(model);
                    }
                }

                if (responses.IsSuccessStatusCode == true)
                {
                    ViewBag.ListofRole = await RoleList();
                    ViewBag.SuccessMessage = "Response " + aregResponse.Message;
                    ModelState.Clear();
                    return View();
                }
                else if (responses.IsSuccessStatusCode != true)
                {
                    ViewBag.ListofRole = await RoleList();
                    ModelState.AddModelError("", responses.ReasonPhrase);
                    ViewBag.ErrorMessage = responses.ReasonPhrase;
                    return View(model);
                }
                ViewBag.ListofRole = await RoleList();
                return View(model);
            } 
        }


        [HttpGet]
        public async Task<IActionResult> SystemUsers(int? pageIndex, string searchText)
        {
            try
            {
                List<SystemUserViewModel> model = new List<SystemUserViewModel>();
                var activeSystemUsers = await _unitOfWork.User.GetAllActiveUsers();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    activeSystemUsers = activeSystemUsers
                        .Where(u => (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                                    (!string.IsNullOrEmpty(u.PhoneNumber) && u.PhoneNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase))).ToList();
                }

                foreach (var activesystemUser in activeSystemUsers)
                {
                    model.Add(new SystemUserViewModel
                    {
                        UserID = activesystemUser.UserID,
                        FirstName = activesystemUser?.FirstName + " " + activesystemUser?.LastName ?? null,
                        LastName = activesystemUser?.LastName ?? null,
                        MiddleName = activesystemUser?.MiddleName ?? null,
                        PhoneNumber = activesystemUser?.PhoneNumber ?? null,
                        Email = activesystemUser?.Email ?? null,
                        CompanyName = activesystemUser?.CompanyName ?? null

                    });
                }

                var modelR = new HoldSystemUserViewModel
                {
                    HoldAllSystemUsers = model
                };
                return View(modelR);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }



        public async Task<IActionResult> Delete(int id)
        {
            var APIServiceConfig = Startup.StaticConfig.GetSection("APIService").Get<APIService>();
            string Baseurl = APIServiceConfig.Url ;
            var gResponse = new APIServiceReponse();

            using (var client = new HttpClient())
            {
                if (id != 0)
                { 
                    string IResponse = General.MakeRequest(Baseurl + "/api/User/DeleteUser/" + id, "", "POST");

                    if (!String.IsNullOrEmpty(IResponse))
                    {
                        gResponse = JsonConvert.DeserializeObject<APIServiceReponse>(IResponse);

                        if (Convert.ToInt32(gResponse.Status) >= 1)
                        {
                            ViewBag.ErrorMessage = "Response " + gResponse.Message;

                            return View();
                        }
                    }

                    gResponse = JsonConvert.DeserializeObject<APIServiceReponse>(IResponse);
                    if (Convert.ToInt32(gResponse.Status) >= 00)
                    {
                        var rmodel = new APIServiceReponse
                        {
                            Status= gResponse.Status,
                            Message= gResponse.Message
                        };
                        //return View(rmodel);
                        ViewBag.Message = gResponse.Message;
                        return RedirectToAction("SystemUsers", "Home");
                    }
                }
            }
            return RedirectToAction("SystemUsers", "Home");
        }












        public async Task<List<SysRole>> RoleList()
        {
            List<SysRole> roleList = new List<SysRole>();

            roleList = await _unitOfWork.Role.GetAllRoles();

            roleList.Insert(0, new SysRole { RoleID = 0, RoleName = "--Select Role--" });

            return roleList;
        }












        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
