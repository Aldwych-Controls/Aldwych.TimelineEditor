using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aldwych.TimelineEditor
{
    public class TimeRuler : Control
    {
        public static readonly StyledProperty<IBrush> BackgroundProperty = AvaloniaProperty.Register<TimeRuler, IBrush>(nameof(Background), Brushes.Gray);
        public static readonly StyledProperty<IBrush> SplitBrushProperty = AvaloniaProperty.Register<TimeRuler, IBrush>(nameof(SplitBrush), Brushes.Gray);
        public static readonly StyledProperty<IBrush> ForegroundProperty = AvaloniaProperty.Register<TimeRuler, IBrush>(nameof(Foreground), Brushes.Gray);
        public static readonly StyledProperty<IBrush> VerticalLineBrushProperty = AvaloniaProperty.Register<TimelineGrid, IBrush>(nameof(VerticalLineBrush), Brushes.Gray);
        public static readonly StyledProperty<double> VerticalLineThicknessProperty = AvaloniaProperty.Register<TimeRuler, double>(nameof(VerticalLineThickness), 1d);

        public IBrush Background
        {
            get { return this.GetValue(TimeRuler.BackgroundProperty); }
            set
            {
                this.SetValue(TimeRuler.BackgroundProperty, value);
                InvalidateVisual();
            }
        }

        public IBrush SplitBrush
        {
            get { return this.GetValue(TimeRuler.SplitBrushProperty); }
            set
            {
                this.SetValue(TimeRuler.SplitBrushProperty, value);
                InvalidateVisual();
            }
        }

        public IBrush Foreground
        {
            get { return this.GetValue(TimeRuler.ForegroundProperty); }
            set
            {
                this.SetValue(TimeRuler.ForegroundProperty, value);
                InvalidateVisual();
            }
        }


        public override void Render(DrawingContext context)
        {
            base.Render(context);
            DrawMinuteMarkers(context);
            Draw20SecondMarkers(context);
            DrawHDivider(context);
        }

        private void DrawMinuteMarkers(DrawingContext context)
        {
            //Minute Markers 
            double firstTickPosition = 0;
            var h = this.Bounds.Height * 0.1;

            var majorPen = new Pen(VerticalLineBrush, VerticalLineThickness);
            if (majorPen != null)
            {
                for (double i = 0; i < this.markerCount * this.TicksPerInterval; i++)
                {
                    var x = firstTickPosition + i * MarkerSpacing;

                    var firstPoint = new Point(x, h);
                    var secondPoint = new Point(x, Bounds.Height);

                    context.DrawLine(majorPen, firstPoint, secondPoint);

                    var minuteNumber = (i);
                    var textPosition = new Point(x + 4, 2);
                
                    context.DrawText(Foreground, textPosition, new FormattedText(minuteNumber.ToString(), Typeface.Default, 14, TextAlignment.Left, TextWrapping.NoWrap, new Size(100, Bounds.Height / 2)));

                }
            }
        }

        private void Draw20SecondMarkers(DrawingContext context)
        {
            //Minute Markers 
            double firstTickPosition = 0;
            var h = this.Bounds.Height / 1.7;

            var majorPen = new Pen(VerticalLineBrush, VerticalLineThickness);
            if (majorPen != null)
            {
                for (double i = 0; i < this.markerCount * this.TicksPerInterval; i++)
                {
                    var x = firstTickPosition + i * MarkerSpacing / 6;

                    var firstPoint = new Point(x, Bounds.Height);
                    var secondPoint = new Point(x, h);

                    context.DrawLine(majorPen, firstPoint, secondPoint);
                }
            }
        }


        private void DrawHDivider(DrawingContext context)
        {
            var h = this.Bounds.Height / 2;
            var majorPen = new Pen(SplitBrush, VerticalLineThickness);

            var firstPoint = new Point(0, h);
            var secondPoint = new Point(Bounds.Width, h);
            context.DrawLine(majorPen, firstPoint, secondPoint);
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
