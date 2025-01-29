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
using MassTransit;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Aprico.EntityFrameworkCore.ValueGeneration;

/// <summary>A <see cref="ValueGenerator{T}"/> for Entity Framework that uses the <see cref="NewId"/> to generate sequential GUIDs.</summary>
/// <remarks>This class is based on the <see cref="NewIdValueGenerator"/> from the <see cref="MassTransit"/> project.</remarks>
/// <seealso cref="ValueGenerator{T}"/>
/// <seealso cref="NewId"/>
public class NewIdValueGenerator : ValueGenerator<Guid>
{
	#region Base Class Member Overrides

	/// <summary>Generates the next sequential GUID value.</summary>
	/// <param name="entry">The Entity Framework entity entry.</param>
	/// <returns>A new sequential GUID.</returns>
	public override Guid Next(EntityEntry entry)
	{
		return NewId.NextGuid();
	}

	#endregion

	#region Base Class Member Overrides

	/// <summary>
	/// Gets a value indicating whether the values generated are temporary or permanent. This implementation always returns
	/// false, indicating permanent values.
	/// </summary>
	public override bool GeneratesTemporaryValues => false;

	#endregion
}
