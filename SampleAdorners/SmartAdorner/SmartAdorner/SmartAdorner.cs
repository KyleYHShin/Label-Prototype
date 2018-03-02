using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace SmartAdorner
{
    public class SmartAdorner : Adorner
    {


        private static SmartAdorner GetAdorner(DependencyObject obj)
        {
            return (SmartAdorner)obj.GetValue(AdornerProperty);
        }

        private static void SetAdorner(DependencyObject obj, SmartAdorner value)
        {
            obj.SetValue(AdornerProperty, value);
        }

        private static readonly DependencyProperty AdornerProperty =
            DependencyProperty.RegisterAttached("Adorner", typeof(SmartAdorner), typeof(SmartAdorner), new PropertyMetadata(null));



        public static DataTemplate GetTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(TemplateProperty);
        }

        public static void SetTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(TemplateProperty, value);
        }

        public static readonly DependencyProperty TemplateProperty =
            DependencyProperty.RegisterAttached("Template", typeof(DataTemplate), typeof(SmartAdorner), new PropertyMetadata(null, OnTemplateChanged));

        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SmartAdorner adorner = GetAdorner(d);
            if (adorner != null)
            {
                adorner.Template = (DataTemplate)e.NewValue;
            }
        }



        public static DataTemplateSelector GetTemplateSelector(DependencyObject obj)
        {
            return (DataTemplateSelector)obj.GetValue(TemplateSelectorProperty);
        }

        public static void SetTemplateSelector(DependencyObject obj, DataTemplateSelector value)
        {
            obj.SetValue(TemplateSelectorProperty, value);
        }

        // Using a DependencyProperty as the backing store for TemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplateSelectorProperty =
            DependencyProperty.RegisterAttached("TemplateSelector", typeof(DataTemplateSelector), typeof(SmartAdorner), new PropertyMetadata(null, OnTemplateSelectorChanged));

        private static void OnTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SmartAdorner adorner = GetAdorner(d);
            if (adorner != null)
            {
                adorner.TemplateSelector = (DataTemplateSelector)e.NewValue;
            }
        }

        public static bool GetVisible(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsVisibleProperty);
        }

        public static void SetVisible(DependencyObject obj, bool value)
        {
            obj.SetValue(VisibleProperty, value);
        }

        public static readonly DependencyProperty VisibleProperty =
            DependencyProperty.RegisterAttached("Visible", typeof(bool), typeof(SmartAdorner), new PropertyMetadata(false, OnVisibleChanged));

        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement adornedElement = d as FrameworkElement;
            if (adornedElement == null) throw new InvalidOperationException("Adorners can only be applied to elements deriving from FrameworkElement");
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(adornedElement);
            if (layer == null) throw new InvalidOperationException("Cannot show adorner since no adorner layer was found in the visual tree");
            
            SmartAdorner adorner = GetAdorner(adornedElement);
            
            bool isVisible = (bool)e.NewValue;

            if (isVisible && adorner == null)
            {
                adorner = new SmartAdorner(adornedElement);
                
                SetAdorner(adornedElement, adorner);
                layer.Add(adorner);
            }
            else if( !isVisible && adorner != null )
            {
                layer.Remove(adorner);
                SetAdorner(d, null);
            }
        }


        private ContentPresenter presenter;

        public SmartAdorner(FrameworkElement adornedElement)
            : base(adornedElement)
        {
            presenter = new ContentPresenter();
            Binding dataContextBinding = new Binding("DataContext");
            dataContextBinding.Source = adornedElement;
            BindingOperations.SetBinding(presenter, ContentPresenter.ContentProperty, dataContextBinding);
            Template = GetTemplate(adornedElement);
            TemplateSelector = GetTemplateSelector(adornedElement);
            AddVisualChild(presenter);
            AddLogicalChild(presenter);
        }
        
        public DataTemplate Template
        {
            get { return presenter.ContentTemplate; }
            set { presenter.ContentTemplate = value; }
        }

        public DataTemplateSelector TemplateSelector
        {
            get { return presenter.ContentTemplateSelector; }
            set { presenter.ContentTemplateSelector = value; }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override System.Windows.Media.Visual GetVisualChild(int index)
        {
            if (index == 0) return presenter;
            throw new ArgumentOutOfRangeException("index");
        }

        protected override Size MeasureOverride(Size constraint)
        {
            presenter.Measure(constraint);
            return presenter.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            presenter.Arrange(new Rect(new Point(0,0), finalSize));
            return finalSize;
        }
    }
}
