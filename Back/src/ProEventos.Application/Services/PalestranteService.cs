using System;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Domain;

namespace ProEventos.Application.Services
{
    public class PalestranteService : IPalestrantesService
    {
        public Task<Palestrante> AddPalestrantes(Evento model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePalestrante(int palestranteId)
        {
            throw new NotImplementedException();
        }

        public Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            throw new NotImplementedException();
        }

        public Task<Palestrante[]> GetAllPalestrantesByNameAsync(string nome, bool includeEventos = false)
        {
            throw new NotImplementedException();
        }

        public Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false)
        {
            throw new NotImplementedException();
        }

        public Task<Palestrante> UpdatePalestrante(int palestranteId, Evento model)
        {
            throw new NotImplementedException();
        }
    }
}