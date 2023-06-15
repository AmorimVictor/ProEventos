using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Contratos
{
    public interface IPalestrantesService
    {
        Task<Palestrante> AddPalestrantes(Evento model);
        Task<Palestrante> UpdatePalestrante(int  palestranteId, Evento model);
        Task<bool> DeletePalestrante(int palestranteId);

        Task<Palestrante[]> GetAllPalestrantesByNameAsync(string nome, bool includeEventos = false);
        Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false);
        Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false);
    }
}