using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wastelands.Service.Domain.Models.Dto
{
	public class CharacterDto : BaseDto
	{
		public long UserId { get; set; }

		public string Name { get; set; }
	}
}
