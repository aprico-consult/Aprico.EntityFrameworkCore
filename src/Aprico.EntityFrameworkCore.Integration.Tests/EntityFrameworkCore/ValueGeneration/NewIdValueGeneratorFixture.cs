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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Aprico.EntityFrameworkCore.ValueGeneration.Dummies;
using Aprico.Xunit;
using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;

namespace Aprico.EntityFrameworkCore.ValueGeneration;

[Collection("Shared MsSql Test Container")]
public class NewIdValueGeneratorFixture(MsSqlDbContextFixture<NewIdEntityDbContext> sqlDbContextFixture) : IClassFixture<MsSqlDbContextFixture<NewIdEntityDbContext>>
{
	[Theory]
	[AutoData]
	[SuppressMessage("ReSharper", "UseCollectionExpression", Justification = "Fails FluentAssertions")]
	[SuppressMessage("Style", "IDE0300:Simplify collection initialization", Justification = "Fails FluentAssertions")]
	public async Task CanInsertAndFetchEntities(Guid name1, Guid name2)
	{
		var e1 = new NewIdEntity {
			Name = name1.ToString("D")
		};
		var e2 = new NewIdEntity {
			Name = name2.ToString("D")
		};
		string connectionString;
		await using (var dbContext = sqlDbContextFixture.DbContext)
		{
			connectionString = dbContext.Database.GetConnectionString() ?? throw new InvalidOperationException();
			await dbContext.Entities.AddAsync(e1);
			await dbContext.Entities.AddAsync(e2);
			await dbContext.SaveChangesAsync();
		}

		await using (var dbContext = new NewIdEntityDbContext(connectionString))
		{
			dbContext.Entities.ToArray()
				.Should()
				// @formatter:wrap_array_initializer_style wrap_if_long
				.BeEquivalentTo(new[] { e1, e2 })
				// @formatter:wrap_array_initializer_style restore
				;
		}
	}
}
