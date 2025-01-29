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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aprico.EntityFrameworkCore.Storage.ValueConversion.Dummies;

public sealed class DateTimeInstantConfiguration : IEntityTypeConfiguration<InstantEntity>
{
	#region IEntityTypeConfiguration<InstantEntity> Members

	public void Configure(EntityTypeBuilder<InstantEntity> builder)
	{
		((EntityTypeBuilder) builder).ToTable($"{nameof(InstantEntity)}s");
		builder.HasKey(static c => c.Id);
		builder.Property(static c => c.Id)
			.ValueGeneratedNever();
		builder.Property(static c => c.CreationInstant)
			.HasConversion(new InstantToDateTimeConverter());
		builder.Property(static c => c.ModificationInstant)
			.HasConversion(new InstantToDateTimeConverter());
	}

	#endregion
}
