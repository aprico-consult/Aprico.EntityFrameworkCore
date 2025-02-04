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
using System.Linq;
using System.Threading.Tasks;
using Aprico.AutoFixture.Xunit2;
using Aprico.EntityFrameworkCore.Storage.ValueConversion.Dummies;
using Aprico.Xunit;
using MicroElements.AutoFixture.NodaTime;
using NodaTime;

namespace Aprico.EntityFrameworkCore.Storage.ValueConversion;

[Collection("Shared MsSql Test Container")]
public class InstantToLongConverterFixture(MsSqlDbContextFixture<LongInstantDbContext> sqlDbContextFixture) : IClassFixture<MsSqlDbContextFixture<LongInstantDbContext>>
{
	[Theory]
	[AutoData<NodaTimeCustomization>]
	public void ShouldConvertFromLongOrDbNull(long id, Instant now)
	{
		var dbContext = sqlDbContextFixture.DbContext;
		dbContext.InsertRow(id, now.ToUnixTimeTicks());

		var entity = dbContext.Entities.Single(l => l.Id == id);

		entity.CreationInstant.Should()
			.Be(now);
		entity.ModificationInstant.Should()
			.BeNull();
	}

	[Theory]
	[AutoData<NodaTimeCustomization>]
	public async Task ShouldConvertToLongOrDbNull(long id, Instant now)
	{
		var dbContext = sqlDbContextFixture.DbContext;
		await dbContext.Entities.AddAsync(
			new InstantEntity {
				Id = id,
				CreationInstant = now
			});

		await dbContext.SaveChangesAsync();

		var row = dbContext.SingleRow(id);
		row[nameof(InstantEntity.CreationInstant)]
			.Should()
			.Be(now.ToUnixTimeTicks());
		row[nameof(InstantEntity.ModificationInstant)]
			.Should()
			.Be(DBNull.Value);
	}
}
