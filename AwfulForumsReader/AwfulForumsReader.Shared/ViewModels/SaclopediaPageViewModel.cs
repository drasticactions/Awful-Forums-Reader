﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Core.Tools;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class SaclopediaPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private List<SaclopediaNavigationEntity> _saclopediaNavigationEntities; 
        public SaclopediaManager SaclopediaManager = new SaclopediaManager();
        public NavigateToSaclopediaTopicsList NavigateToSaclopediaTopicsList { get; set; } = new NavigateToSaclopediaTopicsList();
        public NavigateToSaclopediaTopic NavigateToSaclopediaTopic { get; set; } = new NavigateToSaclopediaTopic();
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
                OnPropertyChanged();
            }
        }

        private string _body;
        public string Body
        {
            get { return _body; }
            set
            {
                SetProperty(ref _body, value);
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }
        public List<SaclopediaNavigationEntity> SaclopediaNavigationEntities
        {
            get { return _saclopediaNavigationEntities; }
            set
            {
                SetProperty(ref _saclopediaNavigationEntities, value);
                OnPropertyChanged();
            }
        }

        private List<SaclopediaNavigationTopicEntity> _saclopediaNavigationTopicEntities;
        public List<SaclopediaNavigationTopicEntity> SaclopediaNavigationTopicEntities
        {
            get { return _saclopediaNavigationTopicEntities; }
            set
            {
                SetProperty(ref _saclopediaNavigationTopicEntities, value);
                OnPropertyChanged();
            }
        }


        public async Task Initialize()
        {
            IsLoading = true;
            try
            {
                SaclopediaNavigationEntities = await SaclopediaManager.GetSaclopediaNavigationBar();
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Failed to set up SAclopedia", ex);
            }
            IsLoading = false;
        }
    }
}