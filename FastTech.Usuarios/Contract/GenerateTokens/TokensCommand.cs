﻿using FastTech.Usuarios.Domain.Enums;

namespace FastTech.Usuarios.Contract.GenerateTokens;

public class TokensCommand
{
    /// <summary>
    ///     Identificador do cliente, que pode ser um CPF ou um e-mail, conforme o tipo especificado.
    ///     <para>Exemplo (CPF): <c>82909117006</c></para>
    ///     <para>Exemplo (E-mail): <c>admin@admin.com</c></para>
    /// </summary>
    public string User { get; set; }
    
    /// <summary>
    ///     Senha codificada em Base64 correspondente ao usuário.
    ///     <para>Exemplo (Base64 de <c>admin123</c>): <c>YWRtaW4xMjM=</c></para>
    /// </summary>
    public string PasswordBase64 { get; set; }
}