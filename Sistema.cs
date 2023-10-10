using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace PetLovers
{
    class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    // Constructor for the Cliente class
		public Cliente(int id, string nome, string email, string password)
		{
			Id = id;
			Nome = nome;
			Email = email;
			Password = GerarHashPassword(password); // Change method name here
		}

		// Method to generate the password hash
		public static string GerarHashPassword(string password) // Change method name here
		{
			byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
			byte[] hashBytes = SHA256.Create().ComputeHash(passwordBytes);
			string hashString = BitConverter.ToString(hashBytes);
			hashString = hashString.Replace("-", String.Empty);
			return hashString;
		}

		public string GetEmail()
		{
			return Email;
		}
	}

    class PetShop
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    class Entregador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    class Consulta
	{
		public int Id { get; set; }
		public string NomeCliente { get; set; }
		public string NomePetShop { get; set; }

		public Consulta(int id, string nomeCliente, string nomePetShop)
		{
			Id = id;
			NomeCliente = nomeCliente;
			NomePetShop = nomePetShop;
		}
	}

    class Entrega
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public string NomeEntregador { get; set; }
        public string NomePetShop { get; set; }

        public Entrega(int id, string nomeCliente, string nomeEntregador, string nomePetShop)
        {
            Id = id;
            NomeCliente = nomeCliente;
            NomeEntregador = nomeEntregador;
            NomePetShop = nomePetShop;
        }
    }

    class BasePetLovers : DbContext
    {
        public BasePetLovers(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<PetShop> PetShops { get; set; } = null!;
        public DbSet<Entregador> Entregadores { get; set; } = null!;
        public DbSet<Consulta> Consultas { get; set; } = null!;
        public DbSet<Entrega> Entregas { get; set; } = null!;
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
                cliente.Nome = clienteAtualizado.Nome;
                cliente.Email = clienteAtualizado.Email;
                cliente.Password = clienteAtualizado.Password;
                basePetLovers.SaveChanges();
                return "cliente atualizado";
            });

            // deletar cliente
            app.MapDelete("/cliente/{id}", (BasePetLovers basePetLovers, int id) =>
            {
                var cliente = basePetLovers.Clientes.Find(id);
                basePetLovers.Remove(cliente);
                basePetLovers.SaveChanges();
                return "cliente removido";
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
                petShop.Nome = petShopAtualizado.Nome;
                petShop.Email = petShopAtualizado.Email;
                basePetLovers.SaveChanges();
                return "pet shop atualizado";
            });

            // deletar pet shop
            app.MapDelete("/petshop/{id}", (BasePetLovers basePetLovers, int id) =>
            {
                var petShop = basePetLovers.PetShops.Find(id);
                basePetLovers.Remove(petShop);
                basePetLovers.SaveChanges();
                return "pet shop removido";
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
				entregador.Nome = entregadorAtualizado.Nome;
				entregador.Email = entregadorAtualizado.Email;
				basePetLovers.SaveChanges();
				return "entregador atualizado";
			});

			// deletar entregador
			app.MapDelete("/entregador/{id}", (BasePetLovers basePetLovers, int id) =>
			{
				var entregador = basePetLovers.Entregadores.Find(id);
				basePetLovers.Remove(entregador);
				basePetLovers.SaveChanges();
				return "entregador removido";
			});

			// CRUD CONSULTA
			// Listar todas as consultas
			app.MapGet("/consulta", (BasePetLovers basePetLovers) =>
			{
				return basePetLovers.Consultas.ToList();
			});

			// Listar consulta específica (por ID)
			app.MapGet("/consulta/{id}", (BasePetLovers basePetLovers, int id) =>
			{
				return basePetLovers.Consultas.Find(id);
			});

			// Cadastrar consulta
			app.MapPost("/consulta", (BasePetLovers basePetLovers, Consulta novaConsulta) =>
			{
				basePetLovers.Consultas.Add(novaConsulta);
				basePetLovers.SaveChanges();
				return "Consulta adicionada";
			});

			// Atualizar consulta
			app.MapPut("/consulta/{id}", (BasePetLovers basePetLovers, Consulta consultaAtualizada, int id) =>
			{
				var consulta = basePetLovers.Consultas.Find(id);
				if (consulta != null)
				{
					consulta.NomeCliente = consultaAtualizada.NomeCliente;
					consulta.NomePetShop = consultaAtualizada.NomePetShop;
					basePetLovers.SaveChanges();
					return "Consulta atualizada";
				}
				return "Consulta não encontrada";
			});

			// Deletar consulta
			app.MapDelete("/consulta/{id}", (BasePetLovers basePetLovers, int id) =>
			{
				var consulta = basePetLovers.Consultas.Find(id);
				if (consulta != null)
				{
					basePetLovers.Remove(consulta);
					basePetLovers.SaveChanges();
					return "Consulta excluída";
				}
				return "Consulta não encontrada";
			});

			// CRUD ENTREGA
			// Listar todas as entregas
			app.MapGet("/entrega", (BasePetLovers basePetLovers) =>
			{
				return basePetLovers.Entregas.ToList();
			});

			// Listar entrega específica (por ID)
			app.MapGet("/entrega/{id}", (BasePetLovers basePetLovers, int id) =>
			{
				return basePetLovers.Entregas.Find(id);
			});

			// Cadastrar entrega
			app.MapPost("/entrega", (BasePetLovers basePetLovers, Entrega novaEntrega) =>
			{
				basePetLovers.Entregas.Add(novaEntrega);
				basePetLovers.SaveChanges();
				return "Entrega adicionada";
			});

			// Atualizar entrega
			app.MapPut("/entrega/{id}", (BasePetLovers basePetLovers, Entrega entregaAtualizada, int id) =>
			{
				var entrega = basePetLovers.Entregas.Find(id);
				if (entrega != null)
				{
					entrega.NomeCliente = entregaAtualizada.NomeCliente;
					entrega.NomeEntregador = entregaAtualizada.NomeEntregador;
					entrega.NomePetShop = entregaAtualizada.NomePetShop;
					basePetLovers.SaveChanges();
					return "Entrega atualizada";
				}
				return "Entrega não encontrada";
			});

			// Deletar entrega
			app.MapDelete("/entrega/{id}", (BasePetLovers basePetLovers, int id) =>
			{
				var entrega = basePetLovers.Entregas.Find(id);
				if (entrega != null)
				{
					basePetLovers.Remove(entrega);
					basePetLovers.SaveChanges();
					return "Entrega excluída";
				}
				return "Entrega não encontrada";
			});


            app.Run("http://localhost:3000/");
        }
    }
}
