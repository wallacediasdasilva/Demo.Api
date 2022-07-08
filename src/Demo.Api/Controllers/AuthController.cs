using AutoMapper;
using Demo.Api.Services.Intefaces;
using Demo.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;

[Route("api")]
public class AuthController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(SignInManager<IdentityUser> singInManager,
                          UserManager<IdentityUser> userManager,
                          INotificador notificador,
                          IMapper mapper) : base(mapper, notificador)
    {
        _signInManager = singInManager;
        _userManager = userManager;
    }

    [HttpPost("nova-conta")]
    public async Task<IActionResult> Registrar(RegisterUserViewModel registerUser)
    {
        if(ModelState.IsValid is false) return CustomResponse(ModelState);

        var user = new IdentityUser
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return CustomResponse(registerUser);
        }

        foreach (var error in result.Errors)
            NotificarErro(error.Description);

        return CustomResponse(registerUser);
    }

    [HttpPost("entrar")]
    public async Task<IActionResult> Login(LoginUserViewModel loginUser)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

        if (result.Succeeded)
        {
            return CustomResponse(loginUser);
        }
        if (result.IsLockedOut)
        {
            NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
            return CustomResponse(loginUser);
        }

        NotificarErro("Usuário ou Senha incorretos");
        return CustomResponse(loginUser);
    }
}
