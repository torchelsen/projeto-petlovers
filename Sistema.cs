using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;


namespace PetLovers
{
    public class Cliente
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

    public class PetShop
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

		public string GetEmail()
		{
			return Email;
		}

    }

    public class Entregador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
		public string GetEmail()
		{
			return Email;
		}

    }

    public class Consulta
	{
		public int Id { get; set; }
		public int ClienteId { get; set; } // Change the property name to indicate it's an Id
		public int PetShopId { get; set; } // Change the property name to indicate it's an Id
		public DateTime HorarioConsulta { get; set; }

		public Consulta(int id, int clienteId, int petShopId, DateTime horarioConsulta)
		{
			Id = id;
			ClienteId = clienteId;
			PetShopId = petShopId;
			HorarioConsulta = horarioConsulta;
		}
		public Consulta() { }
	}

    public class Entrega
	{
		public int Id { get; set; }
		public DateTime HorarioEntrega { get; set; }

		public int ClienteId { get; set; } // Change the property name to indicate it's an Id
		public int EntregadorId { get; set; } // Change the property name to indicate it's an Id
		public int PetShopId { get; set; } // Change the property name to indicate it's an Id

		public Entrega(int id, DateTime horarioEntrega, int clienteId, int entregadorId, int petShopId)
		{
			Id = id;
			HorarioEntrega = horarioEntrega;
			ClienteId = clienteId;
			EntregadorId = entregadorId;
			PetShopId = petShopId;
		}
		public Entrega() { }
	}
	
	public class BasePetLovers : DbContext
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
			builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
			var connectionString = builder.Configuration.GetConnectionString("BasePetLovers") ?? "Data Source=BasePetLovers.db";
			builder.Services.AddSqlite<BasePetLovers>(connectionString);
			var app = builder.Build();
			app.UseCors();


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

				if (cliente != null)
				{
					// Anexa a entidade ao contexto antes de modificar suas propriedades
					basePetLovers.Attach(cliente);

					cliente.Nome = clienteAtualizado.Nome;
					cliente.Email = clienteAtualizado.Email;

					basePetLovers.SaveChanges();
					
					return "cliente atualizado";
				}
				else
				{
					return "cliente não encontrado";
				}
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
            app.MapPost("/petshop", (BasePetLovers basePetLovers, PetShop novoPetshop) =>
            {
				foreach(var petshop in basePetLovers.PetShops)
                {
                    //lanca erro caso email ja esteja em uso
                    if(petshop.GetEmail() == novoPetshop.GetEmail())
                    {
                        throw new Exception($"Email '{novoPetshop.GetEmail()}' já está em uso.");
                    }
                }
                basePetLovers.PetShops.Add(novoPetshop);
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
			app.MapPost("/entregador", (BasePetLovers basePetLovers, Entregador novoEntregador) =>
			{
				foreach(var entregador in basePetLovers.Entregadores)
                {
                    //lanca erro caso email ja esteja em uso
                    if(entregador.GetEmail() == novoEntregador.GetEmail())
                    {
                        throw new Exception($"Email '{novoEntregador.GetEmail()}' já está em uso.");
                    }
                }
				basePetLovers.Entregadores.Add(novoEntregador);
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
			
			// cadastra consulta
			app.MapPost("/consulta", (BasePetLovers basePetLovers, Consulta novaConsulta) =>
			{
				// Verificar a disponibilidade para PetShop
				var petShop = basePetLovers.PetShops.SingleOrDefault(ps => ps.Id == novaConsulta.PetShopId);

				if (petShop != null)
				{
					// Verificar se o horário já está marcado para consultas no PetShop
					var horarioExistente = basePetLovers.Consultas
						.Any(c => c.PetShopId == petShop.Id && c.HorarioConsulta == novaConsulta.HorarioConsulta);

					if (!horarioExistente)
					{
						// Verificar se o ClienteId existe
						var clienteExistente = basePetLovers.Clientes.Any(c => c.Id == novaConsulta.ClienteId);

						if (clienteExistente)
						{
							// Adicionar a nova consulta
							basePetLovers.Consultas.Add(novaConsulta);

							// Salvar as alterações
							basePetLovers.SaveChanges();

							return Results.Created("Consulta adicionada", novaConsulta); // Código 201 CREATED
						}
						else
						{
							return Results.BadRequest("Cliente não encontrado."); // Código 400 BAD REQUEST
						}
					}
					else
					{
						return Results.Conflict("Horário já está marcado para consulta no PetShop."); // Código 409 CONFLICT
					}
				}
				else
				{
					return Results.NotFound("PetShop não encontrado."); // Código 404 NOT FOUND
				}
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



			// // CRUD ENTREGA
			// // Listar todas as entregas
			// app.MapGet("/entrega", (BasePetLovers basePetLovers) =>
			// {
			// 	return basePetLovers.Entregas.ToList();
			// });

			// // Listar entrega específica (por ID)
			// app.MapGet("/entrega/{id}", (BasePetLovers basePetLovers, int id) =>
			// {
			// 	return basePetLovers.Entregas.Find(id);
			// });

			// // Cadastrar entrega
			// app.MapPost("/entrega", (BasePetLovers basePetLovers, Entrega novaEntrega) =>
			// {
			// 	basePetLovers.Entregas.Add(novaEntrega);
			// 	basePetLovers.SaveChanges();
			// 	return "Entrega adicionada";
			// });

			// // Atualizar entrega
			// app.MapPut("/entrega/{id}", (BasePetLovers basePetLovers, Entrega entregaAtualizada, int id) =>
			// {
			// 	var entrega = basePetLovers.Entregas.Find(id);
			// 	if (entrega != null)
			// 	{
			// 		entrega.NomeCliente = entregaAtualizada.NomeCliente;
			// 		entrega.NomeEntregador = entregaAtualizada.NomeEntregador;
			// 		entrega.NomePetShop = entregaAtualizada.NomePetShop;
			// 		basePetLovers.SaveChanges();
			// 		return "Entrega atualizada";
			// 	}
			// 	return "Entrega não encontrada";
			// });

			// // Deletar entrega
			// app.MapDelete("/entrega/{id}", (BasePetLovers basePetLovers, int id) =>
			// {
			// 	var entrega = basePetLovers.Entregas.Find(id);
			// 	if (entrega != null)
			// 	{
			// 		basePetLovers.Remove(entrega);
			// 		basePetLovers.SaveChanges();
			// 		return "Entrega excluída";
			// 	}
			// 	return "Entrega não encontrada";
			// });


            app.Run("http://localhost:3000/");
        }
    }
}
