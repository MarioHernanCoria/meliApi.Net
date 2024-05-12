﻿using meliApi.Data.Repositories;
using meliApi.Data;
using meliApi.Services.Interfaces;

namespace meliApi.Services.Implementacion
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UsuarioRepository UsuarioRepository { get; set; }


        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            UsuarioRepository = new UsuarioRepository(_context);
        }

        public Task<int> Complete()
        {
            return _context.SaveChangesAsync();
        }
    }
}