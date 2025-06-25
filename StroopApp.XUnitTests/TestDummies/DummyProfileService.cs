﻿using System.Collections.ObjectModel;

using StroopApp.Models;
using StroopApp.Services;

namespace StroopApp.XUnitTests.TestDummies
{
	public class DummyProfileService : IProfileService
	{
		public ObservableCollection<ExperimentProfile> LoadProfiles() => new ObservableCollection<ExperimentProfile>();
		public void SaveProfiles(ObservableCollection<ExperimentProfile> profiles)
		{
		}
		public void AddProfile(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles)
		{
		}
		public void DeleteProfile(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles)
		{
		}
		public Guid? LoadLastSelectedProfile() => null;
		public void SaveLastSelectedProfile(ExperimentProfile profile)
		{
		}
		public ObservableCollection<ExperimentProfile> UpsertProfile(ExperimentProfile profile)
		{
			return new ObservableCollection<ExperimentProfile> { profile };
		}
	}
}
