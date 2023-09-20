using System.Security.Claims;

namespace ProEventos.API.Extensions
{
    // Aqui começa a definição de um namespace chamado ProEventos.API.Extensions.
    
    public static class ClaimsPrincipalExtensions
    {
        // Aqui começa a definição de uma classe estática chamada ClaimsPrincipalExtensions.
        
        public static string GetUserName(this ClaimsPrincipal user)
        {
            // Este método de extensão chamado GetUserName estende a classe ClaimsPrincipal.
            // Ele recebe um objeto ClaimsPrincipal chamado 'user' como parâmetro.
            
            return user.FindFirst(ClaimTypes.Name)?.Value;
            // Este código procura a primeira claim com o tipo ClaimTypes.Name no objeto 'user'
            // e retorna o valor (nome de usuário) dessa claim. O operador '?.' é usado para
            // evitar erros se a claim não for encontrada.
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            // Este método de extensão chamado GetUserId estende a classe ClaimsPrincipal.
            // Ele também recebe um objeto ClaimsPrincipal chamado 'user' como parâmetro.
            
            return  int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // Este código procura a primeira claim com o tipo ClaimTypes.NameIdentifier no objeto 'user'
            // e tenta convertê-la para um valor inteiro (ID de usuário). O operador '?.' é usado para
            // evitar erros se a claim não for encontrada ou se não puder ser convertida para um inteiro.
        }
    }
}
