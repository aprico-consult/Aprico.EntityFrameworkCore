#region region Copyright & License

// Copyright © 2024 - 2025 Aprico Consultants
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Aprico.EntityFrameworkCore.Storage.ValueConversion.Dummies;

public abstract class SingleEntityDbContext<TEntity, TConfiguration>(string connectionString) : DbContext
	where TConfiguration : class, IEntityTypeConfiguration<TEntity>, new()
	where TEntity : class
{
	#region Base Class Member Overrides

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer(connectionString);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new TConfiguration());
	}

	#endregion

	internal DbSet<TEntity> Entities => Set<TEntity>();

	protected void ExecuteNonQuery(Action<SqlCommand> commandBuilder)
	{
		using var connection = new SqlConnection(connectionString);
		using var command = new SqlCommand();
		commandBuilder(command);
		connection.Open();
		command.Connection = connection;
		command.ExecuteNonQuery();
	}

	protected IEnumerable<DataRow> SelectRows(Action<SqlCommand> commandBuilder)
	{
		ArgumentNullException.ThrowIfNull(commandBuilder);
		using var connection = new SqlConnection(connectionString);
		using var command = new SqlCommand();
		commandBuilder(command);
		connection.Open();
		command.Connection = connection;
		using var table = new DataTable();
		using var reader = command.ExecuteReader();
		table.Load(reader);
		return table.Rows.Cast<DataRow>();
	}
}
