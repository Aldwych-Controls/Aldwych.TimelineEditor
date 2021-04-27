using Aldwych.TimelineEditor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ReactiveUI;
using System.Windows.Input;
using System.Linq;

namespace Sample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public List<TrackItem> SelectedItems { get; set; } = new List<TrackItem>();


        public TimeSpan Duration => TimeSpan.FromHours(5);

        public ICommand AddNewItem { get; private set; }

        public ICommand Remove10Seconds { get; private set; }
        public ICommand Add10Seconds { get; private set; }


        public ObservableCollection<TrackItem> Clips { get; private set; }

        private double zoomLevel;
        public double ZoomLevel
        {
            get => zoomLevel;
            set => this.RaiseAndSetIfChanged(ref zoomLevel, value);
        }

        public MainWindowViewModel()
        {
            ZoomLevel = 1.0;
            Clips = new ObservableCollection<TrackItem>();

            Clips.Add(new TrackItem() { Title = "Test 1", Start = TimeSpan.FromSeconds(10), End = TimeSpan.FromSeconds(20), IsSelected = true });
            Clips.Add(new TrackItem() { Title = "Test 2", Start = TimeSpan.FromSeconds(30), End = TimeSpan.FromMinutes(1) });
            Clips.Add(new TrackItem() { Title = "Test 3", Start = TimeSpan.FromMinutes(1.5), End = TimeSpan.FromMinutes(2) });
            Clips.Add(new TrackItem() { Title = "Test 3.5", Start = TimeSpan.FromMinutes(2), End = TimeSpan.FromMinutes(4) });

            Clips.Add(new TrackItem() { Title = "Test 4", Start = TimeSpan.FromMinutes(5), End = TimeSpan.FromMinutes(15) });

            AddNewItem = ReactiveCommand.Create(CreateNewTimeLineEvent);
            Remove10Seconds = ReactiveCommand.Create(Remove10Secs);
            Add10Seconds = ReactiveCommand.Create(Add10Secs);

        }

        private void Add10Secs()
        {
            foreach (var ti in SelectedItems)
            {
                ti.End = ti.End.Add(TimeSpan.FromSeconds(10));
            }
            this.RaisePropertyChanged();
        }

        private void Remove10Secs()
        {
            foreach (var ti in SelectedItems)
            {
                ti.End = ti.End.Subtract(TimeSpan.FromSeconds(10));
            }
            this.RaisePropertyChanged();
        }


        private void CreateNewTimeLineEvent()
        {
            var count = 20;
            for (int i = 0; i < count; i++)
            {
                var lastClip = Clips.LastOrDefault();

                var clip = new TrackItem() { Title = $"Test {Clips.Count + 1}", Start = lastClip.End.Add(TimeSpan.FromSeconds(10)), End = lastClip.End.Add(TimeSpan.FromSeconds(20)) };
                Clips.Add(clip);
            }
          
        }


    }

    public class Clip
    {
        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public string Title { get; set; }
    }
}
