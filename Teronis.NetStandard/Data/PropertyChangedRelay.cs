using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Teronis.Reflection;

namespace Teronis.Data
{
    public class PropertyChangedRelay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IList<INotifyPropertyChanged> PropertyChangedNotifiers { get; private set; }
        public Type CommonPropertiesContainerType { get; private set; }
        public object AlternativePropertyChangedSender { get; private set; }

        public PropertyChangedRelay(Type commonPropertiesContainerType, params INotifyPropertyChanged[] propertyChangedNotifiers)
        {
            PropertyChangedNotifiers = propertyChangedNotifiers ?? throw new ArgumentNullException(nameof(propertyChangedNotifiers));

            foreach (var propertyChangedNotifier in PropertyChangedNotifiers)
                propertyChangedNotifier.PropertyChanged += PropertyChangedNotifier_PropertyChanged;

            CommonPropertiesContainerType = commonPropertiesContainerType;
            //AlternativePropertyChangedSender = alternativePropertyChangedSender;
        }

        //public PropertyChangedRelay(Type commonPropertiesContainerType, params INotifyPropertyChanged[] propertyChangedNotifiers)
        //    : this(commonPropertiesContainerType, default(object), propertyChangedNotifiers) { }

        //public PropertyChangedRelay(object alternativePropertyChangedSender, params INotifyPropertyChanged[] propertyChangedNotifiers)
        //    : this(default, alternativePropertyChangedSender, propertyChangedNotifiers) { }

        //public PropertyChangedRelay(object alternativePropertyChangedSender, bool alternativePropertyChangedSenderTypeIsCommonPropertyContainerType, params INotifyPropertyChanged[] propertyChangedNotifiers)
        //    : this(alternativePropertyChangedSender, propertyChangedNotifiers)
        //{
        //    if (alternativePropertyChangedSender != null && alternativePropertyChangedSenderTypeIsCommonPropertyContainerType)
        //        CommonPropertiesContainerType = alternativePropertyChangedSender.GetType();
        //}

        private void PropertyChangedNotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var readonlyPropertyBindingAttributes = VariableInfoSettings.DefaultFlags | System.Reflection.BindingFlags.GetProperty;
            var shouldNotNotifyUncommonProperty = CommonPropertiesContainerType != null && CommonPropertiesContainerType.GetProperty(e.PropertyName, readonlyPropertyBindingAttributes) == null;

            if (shouldNotNotifyUncommonProperty)
                return;

            if (AlternativePropertyChangedSender != null)
                sender = AlternativePropertyChangedSender;

            PropertyChanged?.Invoke(sender, e);
        }
    }
}
