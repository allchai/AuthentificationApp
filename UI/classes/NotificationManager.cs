using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace UI.classes
{
    internal class NotificationManager
    {
        private List<Notification> _notifications;

        public NotificationManager()
            => _notifications = new List<Notification>();

        public void Add(Notification message)
            => _notifications.Add(message);

        public void Clear()
            => _notifications.Clear();

        public List<StackPanel> Tick()
        {
            foreach (Notification message in _notifications)
                message.TimeRemaining -= 1;

            List<Notification> toRemove = _notifications.Where(message => message.TimeRemaining <= 0).ToList();
            _notifications.RemoveAll(message => toRemove.Contains(message));
            return toRemove.Select(message => message.View).ToList();
        }

        public IEnumerable<Notification> Notifications => _notifications;
    }
}
