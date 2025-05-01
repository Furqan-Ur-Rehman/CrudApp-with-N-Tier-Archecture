using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace N_Tier_Architecture.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("admin/[controller]/[action]")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // Action to list all users and their roles
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Roles = string.Join(", ", roles)
                });
            }
            return View(userRoles);
        }
        // GET: Edit User Roles
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new EditUserRoleViewModel
            {
                UserId = user.Id,
                Email = user.Email!,
                Roles = allRoles.Select(role => new UserRoleAssignment
                {
                    RoleName = role!,
                    IsAssigned = userRoles.Contains(role!)
                }).ToList()
            };

            return View(model);
        }

        // POST: Edit User Roles
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId!);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.Roles!.Where(r => r.IsAssigned && !currentRoles.Contains(r.RoleName!)).Select(r => r.RoleName);
            var rolesToRemove = currentRoles.Where(r => !model.Roles!.Any(er => er.IsAssigned && er.RoleName == r));

            await _userManager.AddToRolesAsync(user, rolesToAdd!);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            return RedirectToAction(nameof(Index));
        }

        // GET: Delete Confirmation
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(new UserRoleViewModel { Id = user.Id, Email = user.Email });
        }

        // POST: Confirm Delete
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }

    // ViewModel to hold user data along with roles
    public class UserRoleViewModel
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Roles { get; set; }
    }

    // ViewModel for Editing User Roles
    public class EditUserRoleViewModel
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public List<UserRoleAssignment>? Roles { get; set; }
    }

    public class UserRoleAssignment
    {
        public string? RoleName { get; set; }
        public bool IsAssigned { get; set; }
    }
}
