using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Selection;
using Avalonia.Controls.Templates;
using System;
using System.Collections;

namespace Aldwych.TimelineEditor
{
    public class TimelinePanel : SelectingItemsControl
    {

        private Size _extent = new Size();
        private Size _viewport = new Size();
        private double _zoomX = 1.0;
        private double _zoomY = 1.0;
        private bool _canHorizontallyScroll = false;
        private bool _canVerticallyScroll = false;
        private EventHandler? _scrollInvalidated;



        public static readonly StyledProperty<double> ZoomLevelProperty = AvaloniaProperty.Register<TimelinePanel, double>(nameof(ZoomLevel));

        private static readonly FuncTemplate<IPanel> DefaultPanel = new FuncTemplate<IPanel>(() => new StackPanel() { Orientation = Avalonia.Layout.Orientation.Vertical });

        public static readonly StyledProperty<ItemVirtualizationMode> VirtualizationModeProperty = ItemsPresenter.VirtualizationModeProperty.AddOwner<TimelinePanel>();

        public static readonly StyledProperty<TimeSpan> DurationProperty = AvaloniaProperty.Register<TimelinePanel, TimeSpan>(nameof(Duration));


        public TimeSpan Duration
        {
            get => GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }


        /// <summary>
        /// Identifies the <seealso cref="ZoomX"/> avalonia property.
        /// </summary>
        public static readonly DirectProperty<TimelinePanel, double> ZoomXProperty = AvaloniaProperty.RegisterDirect<TimelinePanel, double>(nameof(ZoomX), o => o.ZoomX, null, 1.0);

        /// <summary>
        /// Identifies the <seealso cref="ZoomY"/> avalonia property.
        /// </summary>
        public static readonly DirectProperty<TimelinePanel, double> ZoomYProperty = AvaloniaProperty.RegisterDirect<TimelinePanel, double>(nameof(ZoomY), o => o.ZoomY, null, 1.0);



        /// <summary>
        /// Gets the zoom ratio for x axis.
        /// </summary>
        public double ZoomX => _zoomX;

        /// <summary>
        /// Gets the zoom ratio for y axis.
        /// </summary>
        public double ZoomY => _zoomY;

        public TimelinePanel()
        {
            ZoomLevel = 0.5;
        }

        /// <summary>
        /// Defines the <see cref="Scroll"/> property.
        /// </summary>
        public static readonly DirectProperty<TimelinePanel, IScrollable> ScrollProperty = AvaloniaProperty.RegisterDirect<TimelinePanel, IScrollable>(nameof(Scroll), o => o.Scroll);

        /// <summary>
        /// Defines the <see cref="SelectedItems"/> property.
        /// </summary>
        public static readonly new DirectProperty<SelectingItemsControl, IList> SelectedItemsProperty = SelectingItemsControl.SelectedItemsProperty;

        /// <summary>
        /// Defines the <see cref="Selection"/> property.
        /// </summary>
        public static readonly new DirectProperty<SelectingItemsControl, ISelectionModel> SelectionProperty = SelectingItemsControl.SelectionProperty;

        /// <summary>
        /// Defines the <see cref="SelectionMode"/> property.
        /// </summary>
        public static readonly new StyledProperty<SelectionMode> SelectionModeProperty = SelectingItemsControl.SelectionModeProperty;


        private IScrollable _scroll;

        static TimelinePanel()
        {
            ItemsPanelProperty.OverrideDefaultValue<TimelinePanel>(DefaultPanel);
            VirtualizationModeProperty.OverrideDefaultValue<TimelinePanel>(ItemVirtualizationMode.Simple);
        }

        /// <summary>
        /// Gets the scroll information for the <see cref="TimelinePanel"/>.
        /// </summary>
        public IScrollable Scroll
        {
            get { return _scroll; }
            private set { SetAndRaise(ScrollProperty, ref _scroll, value); }
        }

        /// <inheritdoc/>
        public new IList SelectedItems
        {
            get => base.SelectedItems;
            set => base.SelectedItems = value;
        }

        /// <inheritdoc/>
        public new ISelectionModel Selection
        {
            get => base.Selection;
            set => base.Selection = value;
        }

        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        /// <remarks>
        /// Note that the selection mode only applies to selections made via user interaction.
        /// Multiple selections can be made programatically regardless of the value of this property.
        /// </remarks>
        public new SelectionMode SelectionMode
        {
            get { return base.SelectionMode; }
            set { base.SelectionMode = value; }
        }


        protected override Size MeasureCore(Size availableSize)
        {
            var defaultSize = base.MeasureCore(availableSize);
            var adjustedZoom = TimeSpan.FromSeconds(1).Ticks * ZoomLevel;

            var adjustedSize = new Size(Duration.Ticks / adjustedZoom, defaultSize.Height);
            return adjustedSize;
        }

        /// <summary>
        /// Gets or sets the virtualization mode for the items.
        /// </summary>
        public ItemVirtualizationMode VirtualizationMode
        {
            get { return GetValue(VirtualizationModeProperty); }
            set { SetValue(VirtualizationModeProperty, value); }
        }

        public double ZoomLevel
        {
            get => GetValue(ZoomLevelProperty);
            set
            {
                SetValue(ZoomLevelProperty, value);
                foreach (var child in this.VisualChildren)
                {
                    if (child is Track trk)
                    {
                        var trkPanel = trk.ItemsPanel as TrackPanel;
                        trkPanel.ZoomLevel = ZoomLevel;
                    }
                }
            }
        }

    }
}
