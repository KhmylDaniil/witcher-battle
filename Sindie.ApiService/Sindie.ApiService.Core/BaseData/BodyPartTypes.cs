﻿using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.BaseData
{
	/// <summary>
	/// Типы частей тела
	/// </summary>
	public static class BodyPartTypes
	{
		/// <summary>
		/// Айди типа части тела - голова
		/// </summary>
		public static readonly Guid HeadId = new("8894e0d0-3147-4791-9153-9667cbe127d7");

		/// <summary>
		/// Название типа части тела - голова
		/// </summary>
		public static readonly string HeadName = "Head";

		/// <summary>
		/// Айди типа части тела - торс
		/// </summary>
		public static readonly Guid TorsoId = new("8894e0d0-3147-4791-9353-9667cbe127d7");

		/// <summary>
		/// Название типа части тела - торс
		/// </summary>
		public static readonly string TorsoName = "Torso";

		/// <summary>
		/// Айди типа части тела - рука 
		/// </summary>
		public static readonly Guid ArmId = new("8894e0d0-3147-4791-9553-9667cbe127d7");

		/// <summary>
		/// Название типа части тела -  рука
		/// </summary>
		public static readonly string ArmName = "Arm";

		/// <summary>
		/// Айди типа части тела - нога
		/// </summary>
		public static readonly Guid LegId = new("8894e0d0-3147-4791-9753-9667cbe127d7");

		/// <summary>
		/// Название типа части тела - нога
		/// </summary>
		public static readonly string LegName = "Leg";

		/// <summary>
		/// Айди типа части тела - крыло
		/// </summary>
		public static readonly Guid WingId = new("8894e0d0-3147-4791-9953-9667cbe127d7");

		/// <summary>
		/// Название типа части тела - крыло
		/// </summary>
		public static readonly string WingName = "Wing";

		/// <summary>
		/// Айди типа части тела - хвост
		/// </summary>
		public static readonly Guid TailId = new("8894e0d0-3147-4791-1153-9667cbe127d7");

		/// <summary>
		/// Название типа части тела - хвост
		/// </summary>
		public static readonly string TailName = "Tail";

		/// <summary>
		/// Айди типа части тела - нет анатомии
		/// </summary>
		public static readonly Guid VoidId = new("8894e0d0-3147-4791-1353-9667cbe127d7");

		/// <summary>
		/// Название типа части тела - нет анатомии
		/// </summary>
		public static readonly string VoidName = "Void";

		/// <summary>
		/// Список типов частей тела
		/// </summary>
		public static readonly List<Guid> BodyPartTypesList = new List<Guid>() { HeadId, TorsoId, ArmId, LegId, WingId, TailId, VoidId };
	}
}