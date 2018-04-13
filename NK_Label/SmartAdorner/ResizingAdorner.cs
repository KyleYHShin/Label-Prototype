using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace SmartAdorner
{
    /// <summary>
    /// A control that adds resize handlers to a control.
    /// </summary>
    [TemplatePart(Name = "PART_THUMB_MOVE", Type = typeof(Thumb))]
    [TemplatePart(Name="PART_THUMB_NW", Type=typeof(Thumb))]
    [TemplatePart(Name = "PART_THUMB_NE", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_THUMB_SW", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_THUMB_SE", Type = typeof(Thumb))]
    public class ResizingAdorner : ContentControl
    {
        public ResizingAdorner()
        {
            AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(OnThumbDragDeltaEvent));
        }

        static ResizingAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizingAdorner), new FrameworkPropertyMetadata(typeof(ResizingAdorner)));
            WidthProperty.OverrideMetadata(typeof(ResizingAdorner), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal));
            HeightProperty.OverrideMetadata(typeof(ResizingAdorner), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal));
        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(ResizingAdorner), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.Journal | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(ResizingAdorner), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.Journal | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double HandleSize
        {
            get { return (double)GetValue(HandleSizeProperty); }
            set { SetValue(HandleSizeProperty, value); }
        }

        public static readonly DependencyProperty HandleSizeProperty =
            DependencyProperty.Register("HandleSize", typeof(double), typeof(ResizingAdorner), new PropertyMetadata(12d));

        private void OnThumbDragDeltaEvent(object sender, DragDeltaEventArgs e)
        {
            string thumbName = ((Thumb)e.Source).Name;
            if (thumbName == "PART_THUMB_MOVE")
            {
                X += e.HorizontalChange;
                Y += e.VerticalChange;
            }
            if( thumbName == "PART_THUMB_NW" || thumbName == "PART_THUMB_SW" ) // Adjust left
            {
                double oldWidth = Width;
                Width = Math.Min(Math.Max(Width - e.HorizontalChange, MinWidth), MaxWidth);
                X += oldWidth - Width;
            }
            if (thumbName == "PART_THUMB_NW" || thumbName == "PART_THUMB_NE") // Adjust top
            {
                double oldHeight = Height;
                Height = Math.Min(Math.Max(Height - e.VerticalChange, MinHeight), MaxHeight);
                Y += oldHeight-Height;
            }
            if (thumbName == "PART_THUMB_NE" || thumbName == "PART_THUMB_SE") // Adjust right
            {
                Width = Math.Min(Math.Max(Width+e.HorizontalChange,MinWidth), MaxWidth);
            }
            if (thumbName == "PART_THUMB_SW" || thumbName == "PART_THUMB_SE") // Adjust bottom
            {
                Height = Math.Min(Math.Max(Height + e.VerticalChange, MinHeight), MaxHeight);
            }
        }
    }

    class NegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return -(double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return -(double)value; 
        }
    }
}
