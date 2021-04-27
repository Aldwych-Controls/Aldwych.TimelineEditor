﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Selection;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;
using System.Collections;

namespace Aldwych.TimelineEditor
{
    public class Track : SelectingItemsControl, ISelectable
    {
        public static readonly StyledProperty<bool> IsSelectedProperty = AvaloniaProperty.Register<Track, bool>(nameof(IsSelected));


        /// <summary>
        /// The default value for the <see cref="ItemsControl.ItemsPanel"/> property.
        /// </summary>
        private static readonly FuncTemplate<IPanel> DefaultPanel = new FuncTemplate<IPanel>(() => new TrackPanel() { Orientation = Avalonia.Layout.Orientation.Horizontal});

        /// <summary>
        /// Defines the <see cref="Scroll"/> property.
        /// </summary>
        public static readonly DirectProperty<Track, IScrollable> ScrollProperty = AvaloniaProperty.RegisterDirect<Track, IScrollable>(nameof(Scroll), o => o.Scroll);

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

        /// <summary>
        /// Defines the <see cref="VirtualizationMode"/> property.
        /// </summary>
        public static readonly StyledProperty<ItemVirtualizationMode> VirtualizationModeProperty = ItemsPresenter.VirtualizationModeProperty.AddOwner<TimelinePanel>();

        private IScrollable _scroll;

        /// <summary>
        /// Initializes static members of the <see cref="ItemsControl"/> class.
        /// </summary>
        static Track()
        {
            ItemsPanelProperty.OverrideDefaultValue<Track>(DefaultPanel);
            VirtualizationModeProperty.OverrideDefaultValue<Track>(ItemVirtualizationMode.Simple);
            SelectableMixin.Attach<Track>(IsSelectedProperty);
        }

        /// <summary>
        /// Gets the scroll information for the <see cref="ListBox"/>.
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


        /// <summary>
        /// Gets or sets the virtualization mode for the items.
        /// </summary>
        public ItemVirtualizationMode VirtualizationMode
        {
            get { return GetValue(VirtualizationModeProperty); }
            set { SetValue(VirtualizationModeProperty, value); }
        }

        /// <summary>
        /// Selects all items in the <see cref="ListBox"/>.
        /// </summary>
        public void SelectAll() => Selection.SelectAll();

        /// <summary>
        /// Deselects all items in the <see cref="ListBox"/>.
        /// </summary>
        public void UnselectAll() => Selection.Clear();

        /// <inheritdoc/>
        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator<TrackItem>(
                this,
                TrackItem.ContentProperty,
                TrackItem.ContentTemplateProperty);
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);

            if (e.NavigationMethod == NavigationMethod.Directional)
            {
                e.Handled = UpdateSelectionFromEventSource(
                    e.Source,
                    true,
                    e.KeyModifiers.HasFlagCustom(KeyModifiers.Shift),
                    e.KeyModifiers.HasFlagCustom(KeyModifiers.Control));
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.Source is IVisual source)
            {
                var point = e.GetCurrentPoint(source);

                if (point.Properties.IsLeftButtonPressed || point.Properties.IsRightButtonPressed)
                {
                    e.Handled = UpdateSelectionFromEventSource(
                        e.Source,
                        true,
                        e.KeyModifiers.HasFlagCustom(KeyModifiers.Shift),
                        e.KeyModifiers.HasFlagCustom(KeyModifiers.Control),
                        point.Properties.IsRightButtonPressed);
                }
            }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            Scroll = e.NameScope.Find<IScrollable>("PART_ScrollViewer");
        }

        public bool IsSelected
        {
            get { return GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

    }
}
