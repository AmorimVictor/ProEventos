using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application.Services
{


    public class EventoService : IEventoService
    {

        private readonly IGeralPersitence _geralPersistence;
        private readonly IEventoPersistence _eventoPersistence;
        private readonly IMapper _mapper;
        public EventoService(IGeralPersitence geralPersistence, IEventoPersistence eventoPersistence, IMapper mapper)
        {
            _geralPersistence = geralPersistence;
            _eventoPersistence = eventoPersistence;
            _mapper = mapper;
        }

        public async Task<EventoDto> AddEventos(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _geralPersistence.Insert<Evento>(evento);
                if (await _geralPersistence.SaveChangesAsync())
                {
                    var eventoResult = await _eventoPersistence.GetEventoByIdAsync(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoResult);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao adicionar evento: " + ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersistence.GetEventoByIdAsync(userId, eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;
                model.UserId = userId;

                _mapper.Map(model, evento);               

                _geralPersistence.Update<Evento>(evento);

                if (await _geralPersistence.SaveChangesAsync())
                {                    
                    var result = await _eventoPersistence.GetEventoByIdAsync(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(result);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar eventos -> " + ex.Message);
            }

        }

        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
            try
            {
                var evento = await _eventoPersistence.GetEventoByIdAsync(userId, eventoId, false);
                if (evento == null) throw new Exception("Evento não foi encontrado para ser deletado.");
                
                evento.UserId = userId;
                _geralPersistence.Delete(evento);
                return await _geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar evento -> " + ex.Message);
            }

        }

        public async Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersistence.GetAllEventosAsync(userId, includePalestrantes);
                if (eventos == null) return null;

                var result = _mapper.Map<EventoDto[]>(eventos);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao buscar todos os eventos -> " + ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersistence.GetAllEventosByTemaAsync(userId, tema, includePalestrantes);
                if (eventos == null) return null;

                var result = _mapper.Map<EventoDto[]>(eventos);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao buscar todos os eventos -> " + ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersistence.GetEventoByIdAsync(userId, eventoId, includePalestrantes);
                if (evento == null) return null;

                evento.UserId = userId;
                var result = _mapper.Map<EventoDto>(evento);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao buscar todos os eventos -> " + ex.Message);
            }
        }
    }
}