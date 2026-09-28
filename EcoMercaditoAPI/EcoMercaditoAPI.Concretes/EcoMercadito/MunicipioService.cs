using EcoMercaditoAPI.Concretes.Context;
using EcoMercaditoAPI.Helpers;
using EcoMercaditoAPI.Interfaces.EcoMercadito;
using EcoMercaditoAPI.Models.EcoMercadito;
using EcoMercaditoAPI.ViewModels.EcoMercadito.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Concretes.EcoMercadito
{
    public class MunicipioService : IMunicipioService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MunicipioService> _logger;

        public MunicipioService(AppDbContext context, ILogger<MunicipioService> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<PagedResponse<MunicipioResponse>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Municipios
                .Include(m => m.Departamento)
                .OrderBy(m => m.Nombre)
                .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            // Aplicar paginación
            var municipios = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var response = municipios.Select(m => new MunicipioResponse
            {
                MunicipioId = m.MunicipioId,
                Nombre = m.Nombre,
                DepartamentoId = m.DepartamentoId,
                DepartamentoNombre = m.Departamento?.Nombre
            }).ToList();

            return PagedResponse<MunicipioResponse>.Create(response, pageNumber, pageSize, totalCount);
        }

        public async Task<PagedResponse<MunicipioResponse>> GetByDepartamentoIdPagedAsync(int departamentoId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Municipios
                .Include(m => m.Departamento)
                .Where(m => m.DepartamentoId == departamentoId)
                .OrderBy(m => m.Nombre)
                .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var municipios = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var response = municipios.Select(m => new MunicipioResponse
            {
                MunicipioId = m.MunicipioId,
                Nombre = m.Nombre,
                DepartamentoId = m.DepartamentoId,
                DepartamentoNombre = m.Departamento?.Nombre
            }).ToList();

            return PagedResponse<MunicipioResponse>.Create(response, pageNumber, pageSize, totalCount);
        }

        public async Task<MunicipioDetalleResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var municipio = await _context.Municipios
            .Include(m => m.Departamento)
            .FirstOrDefaultAsync(m => m.MunicipioId == id, cancellationToken);

            if (municipio == null)
            {
                _logger.LogWarning("Municipio con ID {MunicipioId} no encontrado", id);
                return null;
            }

            return new MunicipioDetalleResponse
            {
                MunicipioId = municipio.MunicipioId,
                Nombre = municipio.Nombre,
                DepartamentoId = municipio.DepartamentoId,
                Departamento = municipio.Departamento != null ? new DepartamentoSimpleResponse
                {
                    DepartamentoId = municipio.Departamento.DepartamentoId,
                    Nombre = municipio.Departamento.Nombre
                } : null
            };
        }

        public async Task<PagedResponse<MunicipioResponse>> SearchByNamePagedAsync(string nombre, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Municipios
                .Include(m => m.Departamento)
                .Where(m => m.Nombre.Contains(nombre))
                .OrderBy(m => m.Nombre)
                .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var municipios = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var response = municipios.Select(m => new MunicipioResponse
            {
                MunicipioId = m.MunicipioId,
                Nombre = m.Nombre,
                DepartamentoId = m.DepartamentoId,
                DepartamentoNombre = m.Departamento?.Nombre
            }).ToList();

            return PagedResponse<MunicipioResponse>.Create(response, pageNumber, pageSize, totalCount);
        }
    }
}
