using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using CV_WPF.Infrastructure.Commands;
using CV_WPF.Models;
using CV_WPF.ViewModels.Base;

namespace CV_WPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            #region Commands

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecute, CanCloseApplicationCommandExecute);

            #endregion

            var data_points = new List<DataPoint>((int)(360 / 0.1));
            const double to_rad = Math.PI / 180;
            for (var x = 0d; x < 360; x += 0.1)
            {
                var y = Math.Sin(x * to_rad);
                data_points.Add(new DataPoint(x, y));
            }
            TestDataPoint = data_points;
        }

        #region TestDataPoint : IEnumerable<DataPoint> - Test data for graph visualization
        
        private IEnumerable<DataPoint> _testDataPoint;

        /// <summary>Test data for graph visualization</summary>
        public IEnumerable<DataPoint> TestDataPoint
        {
            get => _testDataPoint;
            set => Set(ref _testDataPoint, value);
        }

        #endregion

        #region Window Title
        private string _title = "Statistic Analyses CV";

        /// <summary>Window Title</summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        #endregion

        #region Status : string - Status of program
        private string _status = "Ready!";

        /// <summary>Status of program</summary>
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
        #endregion

        #region Commands

        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }
        public bool CanCloseApplicationCommandExecute(object p) => true;
        public void OnCloseApplicationCommandExecute(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion
        
        #endregion
    }
}
