using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Aldwych.TimelineEditor
{
    public class TrackPanel : StackPanel, IVirtualizingPanel
    {
        private Size _availableSpace;
        private double _takenSpace;
        private int _canBeRemoved;
        private double _averageItemSize;
        private int _averageCount;
        private double _pixelOffset;
        private double _crossAxisOffset;
        private bool _forceRemeasure;

        public TrackPanel()
        {
            ZoomLevel = 0.5;
        }

        bool IVirtualizingPanel.IsFull
        {
            get
            {
                return Orientation == Orientation.Horizontal ?
                    _takenSpace >= _availableSpace.Width :
                    _takenSpace >= _availableSpace.Height;
            }
        }

        IVirtualizingController IVirtualizingPanel.Controller { get; set; }
        int IVirtualizingPanel.OverflowCount => _canBeRemoved;

        Orientation IVirtualizingPanel.ScrollDirection => Orientation;

        double IVirtualizingPanel.AverageItemSize => _averageItemSize;

        double IVirtualizingPanel.PixelOverflow
        {
            get
            {
                var bounds = Orientation == Orientation.Horizontal ?
                    _availableSpace.Width : _availableSpace.Height;
                return Math.Max(0, _takenSpace - bounds);
            }
        }

        private double _zoomLevel;
        public double ZoomLevel
        {
            get { return _zoomLevel; } 

            set
            {
                if (_zoomLevel != value)
                {
                    _zoomLevel = value;
                    InvalidateArrange();
                }
            }
        }

        double IVirtualizingPanel.PixelOffset
        {
            get { return _pixelOffset; }

            set
            {
                if (_pixelOffset != value)
                {
                    _pixelOffset = value;
                    InvalidateArrange();
                }
            }
        }

        double IVirtualizingPanel.CrossAxisOffset
        {
            get { return _crossAxisOffset; }

            set
            {
                if (_crossAxisOffset != value)
                {
                    _crossAxisOffset = value;
                    InvalidateArrange();
                }
            }
        }

        private IVirtualizingController Controller => ((IVirtualizingPanel)this).Controller;

        void IVirtualizingPanel.ForceInvalidateMeasure()
        {
            InvalidateMeasure();
            _forceRemeasure = true;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_forceRemeasure || availableSize != ((ILayoutable)this).PreviousMeasure)
            {
                _forceRemeasure = false;
                _availableSpace = availableSize;
                Controller?.UpdateControls();
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _availableSpace = finalSize;
            _canBeRemoved = 0;
            _takenSpace = 0;
            _averageItemSize = 0;
            _averageCount = 0;
            var result = ArrangeOverride_Base(finalSize);
            _takenSpace += _pixelOffset;
            Controller?.UpdateControls();
            return result;
        }

        Size ArrangeOverride_Base(Size finalSize)
        {
            var children = Children;
            bool fHorizontal = (Orientation == Orientation.Horizontal);
            Rect rcChild = new Rect(finalSize);
            double previousChildSize = 0.0;
            var spacing = Spacing;

            //
            // Arrange and Position Children.
            //
            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];

                if (child == null || !child.IsVisible)
                { continue; }

                if (child is TrackItem ti)
                {
                    var adjustedZoom = TimeSpan.FromSeconds(1).Ticks * ZoomLevel;

                    var startTicks = ti.Start.Ticks / adjustedZoom;
                    var endTicks = ti.End.Ticks / adjustedZoom;
                    var durationTicks = (endTicks - startTicks);


                    rcChild = rcChild.WithX(startTicks);
                    rcChild = rcChild.WithWidth(durationTicks);
                    rcChild = rcChild.WithHeight(Math.Max(finalSize.Height, child.DesiredSize.Height));
                }
                else if (fHorizontal)
                {
                    rcChild = rcChild.WithX(rcChild.X + previousChildSize);
                    previousChildSize = child.DesiredSize.Width;
                    rcChild = rcChild.WithWidth(previousChildSize);
                    rcChild = rcChild.WithHeight(Math.Max(finalSize.Height, child.DesiredSize.Height));
                    previousChildSize += spacing;
                }
                else
                {
                    rcChild = rcChild.WithY(rcChild.Y + previousChildSize);
                    previousChildSize = child.DesiredSize.Height;
                    rcChild = rcChild.WithHeight(previousChildSize);
                    rcChild = rcChild.WithWidth(Math.Max(finalSize.Width, child.DesiredSize.Width));
                    previousChildSize += spacing;
                }
                child.Arrange(rcChild);
            }

            return finalSize;
        }


        protected override void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ChildrenChanged(sender, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IControl control in e.NewItems)
                    {
                        UpdateAdd(control);
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (IControl control in e.OldItems)
                    {
                        UpdateRemove(control);
                    }

                    break;
            }
        }

        protected override IInputElement GetControlInDirection(NavigationDirection direction, IControl from)
        {
            if (from == null)
                return null;

            var logicalScrollable = Parent as ILogicalScrollable;

            if (logicalScrollable?.IsLogicalScrollEnabled == true)
            {
                return logicalScrollable.GetControlInDirection(direction, from);
            }
            else
            {
                return base.GetControlInDirection(direction, from);
            }
        }



        private void UpdateAdd(IControl child)
        {
            var bounds = Bounds;
            var spacing = Spacing;

            child.Measure(_availableSpace);
            ++_averageCount;

            if (Orientation == Orientation.Vertical)
            {
                var height = child.DesiredSize.Height;
                _takenSpace += height + spacing;
                AddToAverageItemSize(height);
            }
            else
            {
                var width = child.DesiredSize.Width;
                _takenSpace += width + spacing;
                AddToAverageItemSize(width);
            }
        }

        private void UpdateRemove(IControl child)
        {
            var bounds = Bounds;
            var spacing = Spacing;

            if (Orientation == Orientation.Vertical)
            {
                var height = child.DesiredSize.Height;
                _takenSpace -= height + spacing;
                RemoveFromAverageItemSize(height);
            }
            else
            {
                var width = child.DesiredSize.Width;
                _takenSpace -= width + spacing;
                RemoveFromAverageItemSize(width);
            }

            if (_canBeRemoved > 0)
            {
                --_canBeRemoved;
            }
        }

        private void AddToAverageItemSize(double value)
        {
            ++_averageCount;
            _averageItemSize += (value - _averageItemSize) / _averageCount;
        }

        private void RemoveFromAverageItemSize(double value)
        {
            _averageItemSize = ((_averageItemSize * _averageCount) - value) / (_averageCount - 1);
            --_averageCount;
        }


    }
}
