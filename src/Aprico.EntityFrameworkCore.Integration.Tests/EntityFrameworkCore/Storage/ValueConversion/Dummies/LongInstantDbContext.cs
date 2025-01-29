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
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Aprico.Xunit;
using Microsoft.Data.SqlClient;

namespace Aprico.EntityFrameworkCore.Storage.ValueConversion.Dummies;

[SuppressMessage("ReSharper", "UseRawString")]
public sealed class LongInstantDbContext(string connectionString) : SingleEntityDbContext<InstantEntity, LongInstantConfiguration>(connectionString), ICreateDbContext<LongInstantDbContext>
{
	public static LongInstantDbContext Create(string connectionString)
	{
		var builder = new SqlConnectionStringBuilder(connectionString) {
			InitialCatalog = nameof(LongInstantDbContext)
		};
		return new LongInstantDbContext(builder.ToString());
	}

	internal void InsertRow(long id, long creationInstant, long? modificationInstant = null)
	{
		ExecuteNonQuery(
			command => {
				command.CommandText = @$"
  INSERT INTO {nameof(InstantEntity)}s ({nameof(InstantEntity.Id)}, {nameof(InstantEntity.CreationInstant)}, {nameof(InstantEntity.ModificationInstant)})
  VALUES (@{nameof(id)}, @{nameof(creationInstant)}, @{nameof(modificationInstant)})";
				command.Parameters.AddWithValue(nameof(id), id);
				command.Parameters.AddWithValue(nameof(creationInstant), creationInstant);
				command.Parameters.AddWithValue(nameof(modificationInstant), modificationInstant ?? (object) DBNull.Value);
			});
	}

	internal DataRow SingleRow(long id)
	{
		return SelectRows(
				command => {
					command.CommandText = @$"
  SELECT [{nameof(InstantEntity.Id)}], [{nameof(InstantEntity.CreationInstant)}], [{nameof(InstantEntity.ModificationInstant)}]
  FROM {nameof(InstantEntity)}s
  WHERE [{nameof(InstantEntity.Id)}] = @id";
					command.Parameters.AddWithValue(nameof(id), id);
				})
			.Single();
	}
}
