using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace PetLovers
{
	
    class Cliente
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string password { get; set; }

		// Construtor do cliente
		public Cliente(int id, string nome, string email, string password)
		{
			this.id = id;
			this.nome = nome;
			this.email = email;
			this.password = GerarHash(password);
		}

		// metodo para gerar a hash do password
		public static string GerarHash(string password)
		{
			byte[] passwordEmBytes = System.Text.Encoding.UTF8.GetBytes(password);
			byte[] hashEmBytes = SHA256.Create().ComputeHash(passwordEmBytes);
			string hashEmString = BitConverter.ToString(hashEmBytes);
			hashEmString = hashEmString.Replace("-", String.Empty);
			return hashEmString;
		}

		public string GetEmail()
		{
			return email;
		}

		public bool Authenticate(string email, string providedPassword)
        {
            // Verify the provided password by hashing and comparing it with the stored hash
            string providedPasswordHash = GerarHash(providedPassword);
            return e == email && p == providedPasswordHash;
        }
		
		
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

	class Authenticator
    {
        private readonly BasePetLovers dbContext;

        public Authenticator(BasePetLovers dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool AuthenticateCliente(string email, string password)
        {
            var cliente = dbContext.Clientes.FirstOrDefault(c => c.email == email);

            if (cliente != null)
            {
                // Compare the provided password with the stored hashed password
                return cliente.Authenticate(email, password);
            }

            return false;
        }

        public bool AuthenticatePetShop(string email, string password)
        {
            var petShop = dbContext.PetShops.FirstOrDefault(p => p.email == email);

            if (petShop != null)
            {
                // Compare the provided password with the stored hashed password
                return petShop.Authenticate(email, password);
            }

            return false;
        }

        public bool AuthenticateEntregador(string email, string password)
        {
            var entregador = dbContext.Entregadores.FirstOrDefault(e => e.email == email);

            if (entregador != null)
            {
                // Compare the provided password with the stored hashed password
                return entregador.Authenticate(email, password);
            }

            return false;
        }
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
			app.MapGet("/cliente/{id}", (BasePetLovers basePetLovers, int id) => {
				return basePetLovers.Clientes.Find(id);
			});

            // cadastrar cliente
			app.MapPost("/cliente", (BasePetLovers basePetLovers, Cliente novoCliente) =>
			{
				foreach(var cliente in basePetLovers.Clientes)
				{
					//lanca erro caso email ja esteja em uso
					if(cliente.GetEmail() == novoCliente.GetEmail())
					{
						throw new Exception($"Email '{novoCliente.GetEmail()}' já está em uso.");
					}
				}
				basePetLovers.Clientes.Add(novoCliente);
				basePetLovers.SaveChanges();
				return "cliente adicionado";
			});
			
			// atualizar cliente
			app.MapPut("/cliente/{id}", (BasePetLovers basePetLovers, Cliente clienteAtualizado, int id) =>
			{
				var cliente = basePetLovers.Clientes.Find(id);
				// foreach(var cliente in basePetLovers.Clientes)
				// {
				// 	//lanca erro caso email ja esteja em uso
				// 	if(cliente.GetEmail() == clienteAtualizado.GetEmail())
				// 	{
				// 		throw new Exception($"Email '{clienteAtualizado.GetEmail()}' já está em uso.");
				// 	}
				// }
				cliente.nome = clienteAtualizado.nome;
				cliente.email = clienteAtualizado.email;
				cliente.password = clienteAtualizado.password;
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
			app.MapGet("/petshop/{id}", (BasePetLovers basePetLovers, int id) => {
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
			app.MapGet("/entregador/{id}", (BasePetLovers basePetLovers, int id) => {
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

            app.Run("http://localhost:3000/"); // aplicação fica ouvindo a porta 3000
        }
    }

}