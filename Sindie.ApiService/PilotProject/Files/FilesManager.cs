using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PilotProject.Files
{
	internal class FilesManager
	{
		internal static void SaveCT(GetCreatureTemplateByIdResponse input, IAppDbContext appDbContext)
		{
			CreateCreatureTemplateCommand command = new(
				gameId: input.GameId,
				imgFileId: null,
				bodyTemplateId: input.BodyTemplateId,
				creatureTypeId: input.CreatureTypeId,
				name: input.Name,
				description: input.Description,
				hp: input.HP,
				sta: input.Sta,
				@int: input.Int,
				@ref: input.Ref,
				dex: input.Dex,
				body: input.Body,
				emp: input.Emp,
				cra: input.Cra,
				will: input.Will,
				speed: input.Speed,
				luck: input.Luck,
				armorList: input.CreatureTemplateParts.Select(ctp => new CreateCreatureTemplateRequestArmorList
				{
					Armor = ctp.Armor,
					BodyTemplatePartId = appDbContext.BodyTemplateParts.FirstOrDefault(btp => btp.Name.Equals(ctp.Name)).Id
				}).ToList(),
				abilities: input.Abilities.Select(x => x.Id).ToList(),
				creatureTemplateSkills: input.CreatureTemplateSkills.Select(x => new CreateCreatureTemplateRequestSkill
				{
					SkillId = x.SkillId,
					Value = x.SkillValue
				}).ToList());

			using (FileStream fs = new FileStream(Constants.SavedCTPath + "CT" + command.Name + ".json", FileMode.OpenOrCreate))
			{
				JsonSerializer.Serialize<CreateCreatureTemplateCommand>(fs, command);
				Console.WriteLine("Data has been saved to file");
			}
		}

		internal static List<CreateCreatureTemplateCommand> LoadCT()
		{
			string[] documents = Directory.GetFiles(Constants.SavedCTPath);

			List<CreateCreatureTemplateCommand> result = new();

			var aaa = new List<string>();

			foreach (string document in documents)
				result.Add(JsonSerializer.Deserialize<CreateCreatureTemplateCommand>(File.ReadAllText(document)));

			//using (StreamReader fs = new StreamReader(document))
			//{
			//	aaa.Add(fs.ReadToEnd());
			//}


			//var files = File.ReadAllText(documents[0]);

			//var vvv = JsonSerializer.Deserialize<CreateCreatureTemplateCommand>(files);


			return result;
		}
	}
}
