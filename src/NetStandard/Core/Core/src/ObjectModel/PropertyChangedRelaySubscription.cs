using System.Collections.Generic;
using System.ComponentModel;
using Teronis.Reflection.Caching;

namespace Teronis.ObjectModel
{
    public readonly struct PropertyChangedRelaySubscription
    {
        private readonly PropertyChangedRelay relay;
        private readonly SingleTypePropertyCache<INotifyPropertyChanged> cache;

        internal PropertyChangedRelaySubscription(PropertyChangedRelay relay, SingleTypePropertyCache<INotifyPropertyChanged> cache)
        {
            this.relay = relay;
            this.cache = cache;
        }

        public void Unsubscribe()
        {
            if (relay == null || cache == null) {
                return;
            }

            foreach (var notifierFromCache in cache.CachedPropertyValues.Values) {
                foreach (var notifierFromRelay in relay.PropertyChangedNotifiers) {
                    if (cache.PropertyValueEqualityComparer.Equals(notifierFromCache, notifierFromRelay)) {
                        relay.UnsubscribePropertyChangedNotifier(notifierFromCache);
                    }
                }
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is PropertyChangedRelaySubscription subscription &&
                EqualityComparer<PropertyChangedRelay>.Default.Equals(relay, subscription.relay) &&
                EqualityComparer<SingleTypePropertyCache<INotifyPropertyChanged>>.Default.Equals(cache, subscription.cache);
        }

        public override int GetHashCode()
        {
            var hashCode = -674897417;
            hashCode = hashCode * -1521134295 + EqualityComparer<PropertyChangedRelay>.Default.GetHashCode(relay);
            hashCode = hashCode * -1521134295 + EqualityComparer<SingleTypePropertyCache<INotifyPropertyChanged>>.Default.GetHashCode(cache);
            return hashCode;
        }
    }
}
