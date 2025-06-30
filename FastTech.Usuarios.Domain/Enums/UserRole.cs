using System.ComponentModel.DataAnnotations;

namespace FastTech.Usuarios.Domain.Enums;

public enum UserRole
{
    /// <summary>
    ///     Usuário com permissões administrativas completas no sistema.
    /// </summary>
    [Display(Name = "Admin")] Admin = 0,

    /// <summary>
    ///     Usuário com permissões de gerenciamento, como supervisão de funcionários ou projetos.
    /// </summary>
    [Display(Name = "Manager")] Manager = 1,

    /// <summary>
    ///     Usuário com perfil de colaborador, com acesso às funcionalidades operacionais.
    /// </summary>
    [Display(Name = "Employee")] Employee = 2,
    

    /// <summary>
    ///     Usuário final do sistema, que consome os serviços ou produtos oferecidos.
    /// </summary>
    [Display(Name = "Customer")] Customer = 3,
    
    /// <summary>
    /// Funcionário responsável pelas atividades na cozinha, como preparação e finalização dos pedidos.
    /// </summary>
    [Display(Name = "Kitchen Staff")]
    KitchenStaff = 4
}