using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace RoboTwin.UI.Core.Controls
{
    /// <summary>
    /// A high-performance 2D gauge for visualizing real-time telemetry 
    /// (e.g., joint torque, velocity, LiDAR distance).
    /// Uses low-level DrawingContext to avoid WPF/Avalonia visual tree bloat.
    /// </summary>
    public class TelemetryDial : Control
    {
        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<TelemetryDial, double>(nameof(Value), defaultValue: 0.0);

        public double Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        static TelemetryDial()
        {
            AffectsRender<TelemetryDial>(ValueProperty);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            
            var bounds = Bounds;
            var center = new Point(bounds.Width / 2, bounds.Height / 2);
            var radius = System.Math.Min(bounds.Width, bounds.Height) / 2.0 - 5;

            // Draw Background Dial
            context.DrawEllipse(Brushes.DarkGray, new Pen(Brushes.Black, 2), center, radius, radius);

            // Draw Value Indicator (Simple arc or line)
            double angle = (Value / 100.0) * System.Math.PI - (System.Math.PI / 2);
            var endPoint = new Point(
                center.X + System.Math.Cos(angle) * radius * 0.8,
                center.Y + System.Math.Sin(angle) * radius * 0.8);

            context.DrawLine(new Pen(Brushes.Red, 4), center, endPoint);
        }
    }
}
