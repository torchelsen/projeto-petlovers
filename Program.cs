using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PetLovers
{
    class Cliente
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
    }
    class PetShop
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
    }
    class Entregador
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
    }

    class BasePetLovers : DbContext
	{
		public BasePetLovers(DbContextOptions options) : base(options)
		{
		}
		
		public DbSet<Cliente> Clientes { get; set; } = null!;
		public DbSet<PetShop> PetShops { get; set; } = null!;
		public DbSet<Entregador> Entregadores { get; set; } = null!;

	}

    class Sistema
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			
			var connectionString = builder.Configuration.GetConnectionString("BasePetLovers") ?? "Data Source=BasePetLovers.db";
			builder.Services.AddSqlite<BasePetLovers>(connectionString);
			
			var app = builder.Build();

			// CRUD CLIENTE
            // listar todos os clientes
			app.MapGet("/cliente", (BasePetLovers basePetLovers) => {
				return basePetLovers.Clientes.ToList();
			});
			
			// listar cliente especifico (por id)
			app.MapGet("/cliente/{id}", (BasePetLovers basePetLovers, string id) => {
				return basePetLovers.Clientes.Find(id);
			});

            // cadastrar cliente
			app.MapPost("/cliente", (BasePetLovers basePetLovers, Cliente cliente) =>
			{
				basePetLovers.Clientes.Add(cliente);
				basePetLovers.SaveChanges();
				return "cliente adicionado";
			});
			
			// atualizar cliente
			app.MapPut("/cliente/{id}", (BasePetLovers basePetLovers, Cliente clienteAtualizado, int id) =>
			{
				var cliente = basePetLovers.Clientes.Find(id);
				cliente.nome = clienteAtualizado.nome;
				cliente.email = clienteAtualizado.email;
				basePetLovers.SaveChanges();
				return "cliente atualizado";
			});
						
			// deletar cliente
			app.MapDelete("/cliente/{id}", (BasePetLovers basePetLovers, int id) =>
			{
				var cliente = basePetLovers.Clientes.Find(id);
				basePetLovers.Remove(cliente);
				basePetLovers.SaveChanges();
				return "cliente atualizado";
			});


			// CRUD PETSHOP
            // listar todos os petshops
			app.MapGet("/petshop", (BasePetLovers basePetLovers) => {
				return basePetLovers.PetShops.ToList();
			});
			
			// listar petshop especifico (por id)
			app.MapGet("/petshop/{id}", (BasePetLovers basePetLovers, string id) => {
				return basePetLovers.PetShops.Find(id);
			});

            // cadastrar pet shop
			app.MapPost("/petshop", (BasePetLovers basePetLovers, PetShop petShop) =>
			{
				basePetLovers.PetShops.Add(petShop);
				basePetLovers.SaveChanges();
				return "pet shop adicionado";
			});

			// atualizar pet shop
			app.MapPut("/petshop/{id}", (BasePetLovers basePetLovers, PetShop petShopAtualizado, int id) =>
			{
				var petShop = basePetLovers.PetShops.Find(id);
				petShop.nome = petShopAtualizado.nome;
				petShop.email = petShopAtualizado.email;
				basePetLovers.SaveChanges();
				return "pet shop atualizado";
			});
						
			// deletar pet shop
			app.MapDelete("/petshop/{id}", (BasePetLovers basePetLovers, int id) =>
			{
				var petShop = basePetLovers.PetShops.Find(id);
				basePetLovers.Remove(petShop);
				basePetLovers.SaveChanges();
				return "pet shop atualizado";
			});

            // CRUD ENTREGADOR
            // listar todos os entregadores
			app.MapGet("/entregador", (BasePetLovers basePetLovers) => {
				return basePetLovers.Entregadores.ToList();
			});
			
			// listar entregador especifico (por id)
			app.MapGet("/entregador/{id}", (BasePetLovers basePetLovers, string id) => {
				return basePetLovers.Entregadores.Find(id);
			});

            // cadastrar entregador
			app.MapPost("/entregador", (BasePetLovers basePetLovers, Entregador entregador) =>
			{
				basePetLovers.Entregadores.Add(entregador);
				basePetLovers.SaveChanges();
				return "entregador adicionado";
			});
			
			// atualizar entregador
			app.MapPut("/entregador/{id}", (BasePetLovers basePetLovers, Entregador entregadorAtualizado, int id) =>
			{
				var entregador = basePetLovers.Entregadores.Find(id);
				entregador.nome = entregadorAtualizado.nome;
				entregador.email = entregadorAtualizado.email;
				basePetLovers.SaveChanges();
				return "entregador atualizado";
			});
						
			// deletar entregador
			app.MapDelete("/entregador/{id}", (BasePetLovers basePetLovers, int id) =>
			{
				var entregador = basePetLovers.Entregadores.Find(id);
				basePetLovers.Remove(entregador);
				basePetLovers.SaveChanges();
				return "entregador atualizado";
			});

            app.Run("http://localhost:3000/");
        }
    }

}