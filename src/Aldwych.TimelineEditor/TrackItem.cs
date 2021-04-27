using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using System;

namespace Aldwych.TimelineEditor
{
    [PseudoClasses(":pressed", ":selected")]
    public class TrackItem : ContentControl, ISelectable 
    {
        /// <summary>
        /// Defines the <see cref="IsSelected"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsSelectedProperty = AvaloniaProperty.Register<TrackItem, bool>(nameof(IsSelected));

        public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<TrackItem, string>(nameof(Title));

        public static readonly StyledProperty<TimeSpan> StartProperty = AvaloniaProperty.Register<TrackItem, TimeSpan>(nameof(Start));

        public static readonly StyledProperty<TimeSpan> EndProperty = AvaloniaProperty.Register<TrackItem, TimeSpan>(nameof(End));

        /// <summary>
        /// Initializes static members of the <see cref="ListBoxItem"/> class.
        /// </summary>
        static TrackItem()
        {
            SelectableMixin.Attach<TrackItem>(IsSelectedProperty);
            PressedMixin.Attach<TrackItem>();
            FocusableProperty.OverrideDefaultValue<TrackItem>(true);

            AffectsMeasure<TrackItem>(StartProperty);
            AffectsMeasure<TrackItem>(EndProperty);

            AffectsRender<TrackItem>(StartProperty);
            AffectsRender<TrackItem>(EndProperty);

        }

        /// <summary>
        /// Gets or sets the selection state of the item.
        /// </summary>
        public bool IsSelected
        {
            get { return GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public string Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public TimeSpan Start
        {
            get { return GetValue(StartProperty); }
            set 
            { 
                SetValue(StartProperty, value);
                InvalidateMeasure();
            }
        }

        public TimeSpan End
        {
            get { return GetValue(EndProperty); }
            set 
            { 
                SetValue(EndProperty, value);
                InvalidateMeasure();
            }
        }
    
        
    }
}
