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

        public async Task<EventoDto> AddEventos(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _geralPersistence.Insert<Evento>(evento);
                if (await _geralPersistence.SaveChangesAsync())
                {
                    var eventoResult = await _eventoPersistence.GetEventoByIdAsync(evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoResult);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao adicionar evento: " + ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = _eventoPersistence.GetEventoByIdAsync(eventoId, false);
                if (evento == null) return null;

                model.Id = eventoId;

                var resultEvento = _mapper.Map<Evento>(model);
                _geralPersistence.Update<Evento>(resultEvento);

                if (await _geralPersistence.SaveChangesAsync())
                {                    
                    var result = await _eventoPersistence.GetEventoByIdAsync(resultEvento.Id, false);
                    return _mapper.Map<EventoDto>(result);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar eventos -> " + ex.Message);
            }

        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoPersistence.GetEventoByIdAsync(eventoId, false);
                if (evento == null) throw new Exception("Evento não foi encontrado para ser deletado.");

                _geralPersistence.Delete(evento);
                return await _geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao atualizar evento -> " + ex.Message);
            }

        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersistence.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                var result = _mapper.Map<EventoDto[]>(eventos);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao buscar todos os eventos -> " + ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersistence.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                var result = _mapper.Map<EventoDto[]>(eventos);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao buscar todos os eventos -> " + ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersistence.GetEventoByIdAsync(eventoId, includePalestrantes);
                if (evento == null) return null;

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