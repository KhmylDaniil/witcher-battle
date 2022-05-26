using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Деятельность
	/// </summary>
	public class Activity : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_interaction"/>
		/// </summary>
		public const string InteractionField = nameof(_interaction);

		private Interaction _interaction;

		/// <summary>
		/// Поле для <see cref="_imgFile"/>
		/// </summary>
		public const string ImgFileField = nameof(_imgFile);

		private ImgFile _imgFile;

		/// <summary>
		/// Поле для <see cref="_applicationArea"/>
		/// </summary>
		public const string ApplicationAreaField = nameof(_applicationArea);

		private ApplicationArea _applicationArea;

		/// <summary>
		/// Айди взаимодействия для деятельности
		/// </summary>
		public Guid InteractionId { get; protected set; }

		/// <summary>
		/// Айди графического файла для дейтельности
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди области применения для деятельности
		/// </summary>
		public Guid ApplicationAreaId { get; protected set; }

		/// <summary>
		/// Название деятельности
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание деятельности
		/// </summary>
		public string Description { get; set; }

		#region navigation properties

		/// <summary>
		/// Взаимодействие для деятельности
		/// </summary>
		public Interaction Interaction
		{
			get => _interaction;
			set
			{
				_interaction = value ?? throw new ApplicationException("Необходимо передать взаимодействие");
				InteractionId = value.Id;
			}
		}

		/// <summary>
		/// Графический файл для деятельности
		/// </summary>
		public ImgFile ImgFile
		{
			get => _imgFile;
			set
			{
				_imgFile = value;
				ImgFileId = value.Id;
			}
		}

		/// <summary>
		/// Область применения для деятельности
		/// </summary>
		public ApplicationArea ApplicationArea
		{
			get => _applicationArea;
			set
			{
				_applicationArea = value ?? throw new ApplicationException("Необходимо передать область применения");
				ApplicationAreaId = value.Id;
			}
		}

		/// <summary>
		/// Деятельности роли
		/// </summary>
		public List<InteractionsRoleActivity> InteractionsRoleActivities { get; set; }

		/// <summary>
		/// Предметы во взаимодействии
		/// </summary>
		public List<InteractionItem> InteractionItems { get; set; }

		/// <summary>
		/// Действия деятельности
		/// </summary>
		public List<ActivityAction> ActivityActions { get; set; }
		#endregion navigation properties
	}
}
