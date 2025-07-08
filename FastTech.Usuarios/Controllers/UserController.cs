using System.Security.Claims;
using FastTech.Usuarios.Application.Interfaces;
using FastTech.Usuarios.Contract.CreateClient;
using FastTech.Usuarios.Contract.CreateEmployee;
using FastTech.Usuarios.Contract.DeleteUser;
using FastTech.Usuarios.Contract.GetUserById;
using FastTech.Usuarios.Contract.UpdateUser;
using FastTech.Usuarios.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastTech.Usuarios.Controllers;

[ApiController]
[Route("api/v1.0/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IValidator<CreateClientCommand> _validatorCreateClientCommand;
    private readonly IValidator<CreateEmployeeCommand> _validatorCreateEmployeeCommand;
    private readonly IValidator<DeleteUserCommand> _validatorDeleteUserCommand;
    private readonly IValidator<GetUserByIdQuery> _validatorGetUserByIdQuery;
    private readonly IValidator<UpdateUserCommand> _validatorUpdateUserCommand;

    public UserController(ILogger<UserController> logger, IUserService userService, IValidator<CreateClientCommand> validatorCreateClientCommand, IValidator<CreateEmployeeCommand> validatorCreateEmployeeCommand, IValidator<DeleteUserCommand> validatorDeleteUserCommand,
        IValidator<GetUserByIdQuery> validatorGetUserByIdQuery, IValidator<UpdateUserCommand> validatorUpdateUserCommand)
    {
        _logger = logger;
        _userService = userService;
        _validatorCreateClientCommand = validatorCreateClientCommand;
        _validatorCreateEmployeeCommand = validatorCreateEmployeeCommand;
        _validatorDeleteUserCommand = validatorDeleteUserCommand;
        _validatorGetUserByIdQuery = validatorGetUserByIdQuery;
        _validatorUpdateUserCommand = validatorUpdateUserCommand;
    }

    /// <summary>
    ///     Cadastra um novo cliente no sistema.
    /// </summary>
    [HttpPost("clientes")]
    [ProducesResponseType(typeof(CreateClientCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand payload)
    {
        try
        {
            await _validatorCreateClientCommand.ValidateAndThrowAsync(payload);
            var result = await _userService.RegisterUserAsync(payload.Name, payload.Cpf, payload.Email, payload.PasswordBase64, UserRole.Customer);

            var commandResult = new CreateClientCommandResult
            {
                Id = result.Id,
                Name = result.Name,
                Cpf = result.Cpf,
                Email = result.Email,
                Role = result.Role
            };
            return new OkObjectResult(commandResult);
        }
        catch (Exception ex)
        {
            var message = "Failed to create client. Error: {ex.Message}";
            _logger.LogError(message);
            return new BadRequestObjectResult(new
            {
                error = message,
                details = ex.Message
            });
        }
    }

    /// <summary>
    ///     Cadastra um novo funcionário (ex: atendente, gerente).
    /// </summary>
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost("funcionarios")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(CreateEmployeeCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand payload)
    {
        try
        {
            await _validatorCreateEmployeeCommand.ValidateAndThrowAsync(payload);
            var result = await _userService.RegisterUserAsync(payload.Name, payload.Cpf, payload.Email, payload.PasswordBase64, payload.Role);

            var commandResult = new CreateEmployeeCommandResult
            {
                Id = result.Id,
                Name = result.Name,
                Cpf = result.Cpf,
                Email = result.Email,
                Role = result.Role
            };

            return new OkObjectResult(commandResult);
        }
        catch (Exception ex)
        {
            var message = "Failed to create employee. Error: {ex.Message}";
            _logger.LogError(message);
            return new BadRequestObjectResult(new
            {
                error = message,
                details = ex.Message
            });
        }
    }

    /// <summary>
    ///     Obtém os dados de um usuário específico.
    /// </summary>
    /// <param name="id">ID do usuário</param>
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(GetUserByIdResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var payload = new GetUserByIdQuery { TargetUserId = id };
            payload.RequestingUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            payload.RequestingUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            await _validatorGetUserByIdQuery.ValidateAndThrowAsync(payload);

            var usuario = await _userService.GetUserByIdAsync(payload.TargetUserId, payload.RequestingUserId, payload.RequestingUserRole);

            if (usuario is null) return NotFound();
            var result = new GetUserByIdResult
            {
                Id = usuario.Id,
                Name = usuario.Name,
                Cpf = usuario.Cpf,
                Email = usuario.Email,
                Role = usuario.Role,
                IsAvailable = usuario.IsAvailable
            };
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            var message = "Failed to retrieve user with ID {id}. Error: {ex.Message}";
            _logger.LogError(message);
            return new BadRequestObjectResult(new
            {
                error = message,
                details = ex.Message
            });
        }
    }

    /// <summary>
    ///     Atualiza os dados de um usuário existente.
    /// </summary>
    /// <param name="id">ID do usuário</param>
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(UpdateUserCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand payload)
    {
        try
        {
            payload.Id = id;
            await _validatorUpdateUserCommand.ValidateAndThrowAsync(payload);
            var requestingUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var requestingUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var usuario = await _userService.UpdateUserAsync(payload.Id, payload.Name, payload.Cpf, payload.Email, payload.PasswordBase64, requestingUserId, requestingUserRole);
            
            return new OkObjectResult(true);
        }
        catch (Exception ex)
        {
            var message = "Failed to update user with ID {id}. Error: {ex.Message}";
            _logger.LogError(message);
            return new BadRequestObjectResult(new
            {
                error = message,
                details = ex.Message
            });
        }
    }

    /// <summary>
    ///     Exclui um usuário (por gerente ou o próprio usuário, se for cliente).
    /// </summary>
    /// <param name="id">ID do usuário</param>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(DeleteUserCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var payload = new DeleteUserCommand { Id = id };
            await _validatorDeleteUserCommand.ValidateAndThrowAsync(payload);
            
            await _userService.DeleteUserAsync(payload.Id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value, User.FindFirst(ClaimTypes.Role)?.Value);
            
            return new OkObjectResult(new DeleteUserCommandResult
            {
                Success = true
            });
        }
        catch (Exception ex)
        {
            var message = $"Failed to delete user with ID {id}. Error: {ex.Message}";
            _logger.LogError(message);
            return new BadRequestObjectResult(new
            {
                error = message,
                details = ex.Message
            });
        }
    }
}