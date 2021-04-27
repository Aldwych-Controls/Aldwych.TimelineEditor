using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
namespace Aldwych.TimelineEditor
{
    public class TimelineGrid : Control
    {
        public static readonly StyledProperty<IBrush> BackgroundProperty = AvaloniaProperty.Register<TimelineGrid, IBrush>(nameof(Background), Brushes.Gray);
        public static readonly StyledProperty<IBrush> VerticalLineBrushProperty = AvaloniaProperty.Register<TimelineGrid, IBrush>(nameof(VerticalLineBrush), Brushes.Gray);
        public static readonly StyledProperty<IBrush> HorizontalLineBrushProperty = AvaloniaProperty.Register<TimelineGrid, IBrush>(nameof(HorizontalLineBrush), Brushes.Gray);

        public static readonly StyledProperty<double> TrackHeightProperty = AvaloniaProperty.Register<TimelineGrid, double>(nameof(TrackHeight), 25d);
        public static readonly StyledProperty<double> HorizontalLineThicknessProperty = AvaloniaProperty.Register<TimelineGrid, double>(nameof(HorizontalLineThickness), 2d);
        public static readonly StyledProperty<double> VerticalLineThicknessProperty = AvaloniaProperty.Register<TimelineGrid, double>(nameof(VerticalLineThickness), 1d);



        public override void Render(DrawingContext context)
        {
            base.Render(context);
            context.FillRectangle(new SolidColorBrush(Color.FromRgb(24, 24, 24)), this.Bounds);

            DrawMinorLines(context);
            DrawMajorLines(context);
            DrawRowLines(context);         
        }

        protected virtual void DrawMinorLines(DrawingContext context)
        {
            double firstTickPosition = 0;
            var h = this.Bounds.Height;

            var minorPen = new Pen(VerticalLineBrush, VerticalLineThickness / 2, DashStyle.Dash);
            if (minorPen != null)
            {

                for (double i = 0; i < this.markerCount * this.TicksPerInterval; i++)
                {
                    var x = firstTickPosition + i * this.TickSpacing;

                    var firstPoint = new Point(x, 0);
                    var secondPoint = new Point(x, h);

                    context.DrawLine(minorPen, firstPoint, secondPoint);
                }
            }
        }

        protected virtual void DrawMajorLines(DrawingContext context)
        {
            double firstTickPosition = 0;
            var h = this.Bounds.Height;

            var majorPen = new Pen(VerticalLineBrush, VerticalLineThickness);
            if (majorPen != null)
            {

                for (double i = 0; i < this.markerCount * this.TicksPerInterval; i++)
                {
                    var x = firstTickPosition + i * MarkerSpacing;

                    var firstPoint = new Point(x, 0);
                    var secondPoint = new Point(x, h);

                    context.DrawLine(majorPen, firstPoint, secondPoint);
                }
            }
        }

        protected virtual void DrawRowLines(DrawingContext context)
        {
            var rowCount = this.Bounds.Height / TrackHeight;

            var majorPen = new Pen(HorizontalLineBrush, HorizontalLineThickness);
            for (int i = 0; i < rowCount; i++)
            {
                var y = TrackHeight * i;
                var p1 = new Point(0, y);
                var p2 = new Point(Bounds.Width, y);
                context.DrawLine(majorPen, p1, p2);
            }
        }


        double markerSpacing;
        public double MarkerSpacing
        {
            get => markerSpacing;
            set
            {
                markerSpacing = value;
                InvalidateVisual();
            }
        }



        int markerCount;
        public int MarkerCount
        {
            get => markerCount;
            set
            {
                markerCount = value;
                InvalidateVisual();
            }
        }

        double ticksPerInterval;
        public double TicksPerInterval
        {
            get => ticksPerInterval;
            set
            {
                ticksPerInterval = value;
                InvalidateVisual();
            }
        }

        public IBrush Background
        {
            get { return this.GetValue(TimelineGrid.BackgroundProperty); }
            set
            {
                this.SetValue(TimelineGrid.BackgroundProperty, value);
                InvalidateVisual();
            }
        }


        public IBrush VerticalLineBrush
        {
            get { return this.GetValue(TimelineGrid.VerticalLineBrushProperty); }
            set
            {
                this.SetValue(TimelineGrid.VerticalLineBrushProperty, value);
                InvalidateVisual();
            }
        }


        public double VerticalLineThickness
        {
            get { return this.GetValue(TimelineGrid.VerticalLineThicknessProperty); }
            set
            {
                this.SetValue(TimelineGrid.VerticalLineThicknessProperty, value);
                InvalidateVisual();
            }
        }


        public IBrush HorizontalLineBrush
        {
            get { return this.GetValue(TimelineGrid.HorizontalLineBrushProperty); }
            set
            {
                this.SetValue(TimelineGrid.HorizontalLineBrushProperty, value);
                InvalidateVisual();
            }
        }

        public double HorizontalLineThickness
        {
            get { return this.GetValue(TimelineGrid.HorizontalLineThicknessProperty); }
            set
            {
                this.SetValue(TimelineGrid.HorizontalLineThicknessProperty, value);
                InvalidateVisual();
            }
        }


        public double TrackHeight
        {
            get { return this.GetValue(TimelineGrid.TrackHeightProperty); }
            set
            {
                this.SetValue(TimelineGrid.TrackHeightProperty, value);
                InvalidateVisual();
            }
        }


        double tickSpacing;
        public double TickSpacing
        {
            get => tickSpacing;
            set
            {
                tickSpacing = value;
                InvalidateVisual();
            }
        }
    }
}
